
public class LoginModel : Singleton<LoginModel>, IDestroy
{
    /// <summary>
    /// 渠道号，和登录SDK相关
    /// </summary>
    private uint m_Channel = 10000;
    /// <summary>
    /// 令牌由SDK和PHP服务器提供
    /// </summary>
    private string m_Token = "TUZjl8grRflMHhLN";


    public void SetChannel(uint channel)
    {
        m_Channel = channel;
    }

    public uint GetChannel()
    {
        return m_Channel;
    }

    public void SetToken(string token)
    {
        m_Token = token;
    }

    public string GetToken()
    {
        return m_Token;
    }

	/// <summary>
	/// 账号由SDK提供
	/// </summary>
	public string Account
	{
		set;
		get;
	}

	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName
	{
		set;
		get;
	}
}