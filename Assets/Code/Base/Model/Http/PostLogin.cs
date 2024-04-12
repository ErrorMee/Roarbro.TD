using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PostLogin : PostBase
{
    public string custom;
}

public class PostLoginResponse: PostResponse
{
    public string token;
    public string account;
    public uint channel;
}
