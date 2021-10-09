using System.Collections.Generic;
using System;

using UnityEngine;

public class PoolingSystem : SingletonBehavior<PoolingSystem> {
    public List<Pool> PoolConfiguration { get => poolConfiguration; }

    [Serializable] public class Pool {
        public string Tag => tag;
        public ushort Size => size;
        public GameObject Prefab => prefab;

        [SerializeField] string tag;
        [SerializeField] ushort size;
        [SerializeField] GameObject prefab;
    }

    [SerializeField] List<Pool> poolConfiguration;
    Dictionary<string, Queue<GameObject>> poolInstances;
    Dictionary<string, Transform> childByName = new Dictionary<string, Transform>();
    

    private void Awake() {
        poolInstances = new Dictionary<string, Queue<GameObject>>();

        if (poolConfiguration == null)
            poolConfiguration = new List<Pool>();

        foreach (var config in poolConfiguration) {
            {
                GameObject obj = new GameObject(config.Tag);
                obj.transform.parent = transform;
                childByName.Add(config.Tag, obj.transform);
            }

            {
                poolInstances.Add(config.Tag, new Queue<GameObject>());

                for (int i = 0; i < config.Size; i++) {
                    GameObject instance = Instantiate(config.Prefab);
                    Return(config.Tag, instance);
                }
                
            }
        }
    }

    void DisableInstance(string tag, GameObject instance) {
        instance.transform.parent = childByName[tag];
        instance.transform.position = Vector3.zero;
        instance.transform.rotation = Quaternion.Euler(Vector3.zero);
        instance.SetActive(false);
    }

    public void Return(string tag, GameObject gameObject) {
        DisableInstance(tag, gameObject);
        poolInstances[tag].Enqueue(gameObject);
    }
    public GameObject Request(string tag) {
        GameObject instance = poolInstances[tag].Dequeue();
        instance.transform.parent = null;

        return instance;
    }
}