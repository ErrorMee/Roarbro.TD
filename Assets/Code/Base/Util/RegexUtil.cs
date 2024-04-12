using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RegexUtil
{
    /// <summary>
    /// 所有数字模式
    /// </summary>
    public const string Partern4Int = @"\d+";

    /// <summary>
    /// 数字乘以等级模式
    /// </summary>
    public const string Partern4IntMultLV = @"{L\d+V}";

    /// <summary>
    /// 首字母大写，其他字母不超过12个的全字母模式
    /// </summary>
    public const string Partern4Az13 = @"^[A-Z]{1}[a-zA-Z]{1,12}$";
}