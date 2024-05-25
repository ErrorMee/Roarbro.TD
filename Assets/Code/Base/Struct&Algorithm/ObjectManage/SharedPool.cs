public class SharedPool<T> : Singleton<ObjectPool<T>> where T : new() 
{
    public static T Get()
    {
        return Instance.Get();
    }

    public static void Cache(T instance)
    {
        Instance.Cache(instance);
    }
}