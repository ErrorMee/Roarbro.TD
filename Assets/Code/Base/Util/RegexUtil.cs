using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RegexUtil
{
    /// <summary>
    /// ��������ģʽ
    /// </summary>
    public const string Partern4Int = @"\d+";

    /// <summary>
    /// ���ֳ��Եȼ�ģʽ
    /// </summary>
    public const string Partern4IntMultLV = @"{L\d+V}";

    /// <summary>
    /// ����ĸ��д��������ĸ������12����ȫ��ĸģʽ
    /// </summary>
    public const string Partern4Az13 = @"^[A-Z]{1}[a-zA-Z]{1,12}$";
}