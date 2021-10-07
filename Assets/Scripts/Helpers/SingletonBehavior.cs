using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Create() {
        GameObject gameObject = new GameObject(typeof(T).Name);
        return gameObject.AddComponent<T>();
    }

    public static T Instance {
        get { return instance ??= Create(); }
    }

    protected static T instance;
}