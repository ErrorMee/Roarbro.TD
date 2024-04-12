
public class ConfigInfo<T> where T : Config
{
    public T config;

    public int instID;
    static int lastID = 0;

    public ConfigInfo()
    {
        instID = lastID++;
    }

}
