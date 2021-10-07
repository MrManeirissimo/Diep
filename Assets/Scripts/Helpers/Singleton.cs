
public abstract class Singleton<T> where T : Singleton<T> {
    public static T Instance { get => instance; protected set => instance = value; }

    protected static T instance;
}