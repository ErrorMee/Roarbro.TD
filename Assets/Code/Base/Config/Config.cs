using System;

[Serializable]
public class Config : IComparable
{
    public bool use = true;
    public int id = -1;
    public int name;
    public bool sortUp = true;

    public string LanguageName()
    {
        return LanguageModel.Get(name);
    }

    public int CompareTo(object obj)
    {
        return id - ((Config)obj).id;
    }

    public T GetElement<T>(T[] array, int index)
    {
        if (!sortUp)
        {
            index = array.Length - index - 1;
        }
        return array[index];
    }
}
