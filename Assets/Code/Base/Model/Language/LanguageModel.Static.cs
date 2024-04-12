
public partial class LanguageModel
{
    public static string Get(int id)
    {
        LanguageConfig languageConfig = Instance.languageDic[id];
        return languageConfig.GetLanguage(Instance.LanguageCrt);
    }
}