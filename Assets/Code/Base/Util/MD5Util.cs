using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class MD5Util
{
    public static string StringToMD5Hash(string inputString)
    {
		return BytesToMD5Hash(Encoding.Default.GetBytes(inputString));
    }

	public static string BytesToMD5Hash(byte[] bytes)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		byte[] encryptedBytes = md5.ComputeHash(bytes);
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < encryptedBytes.Length; i++)
		{
			sb.AppendFormat("{0:x2}", encryptedBytes[i]);
		}
		return sb.ToString();
	}
}
