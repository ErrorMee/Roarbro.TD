using System;

public class PostData<T> where T: PostBase
{
    public uint channel;
    public T data;
}

[Serializable]
public class PostBase
{
    public string token;
    public string account;
    public string sign;
}

public class PostResponse
{
    public int state;
}

public class PostKeyValue
{
    public string key;
    public string value;

    public PostKeyValue(string _key, string _value)
    {
        key = _key;
        value = _value;
    }

    public override string ToString()
    {
        return key + ":" + value;
    }
}
