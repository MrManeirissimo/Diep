using UnityEngine;

public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T> {
    public static T Create() {
        GameObject gameObject = new GameObject(typeof(T).Name);
        return gameObject.AddComponent<T>();
    }

    public static T Instance {
        get { return instance ??= Create(); }
    }

    protected static T instance;

    public virtual void OnEnable() => instance = (T)this;
    public virtual void OnDisable() => instance = (instance == (T)this ? null : instance);
}