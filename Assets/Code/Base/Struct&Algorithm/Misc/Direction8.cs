using System.Collections.Generic;
using UnityEngine;

public enum Direction8 : byte
{
    left = 0,
    leftFwd,
    forward,
    rightFwd,
    right,
    rightBack,
    back,
    leftBack,
}

public static class Direction8Ext
{
	public static Direction8[] Dir2 = new Direction8[] {
		Direction8.left, Direction8.forward};

	public static Direction8[] Dir4 = new Direction8[] { 
		Direction8.left, Direction8.forward,
		Direction8.right, Direction8.back};

	public static Direction8[] Dir8 = new Direction8[] { 
		Direction8.left, Direction8.leftFwd, Direction8.forward, Direction8.rightFwd, 
		Direction8.right, Direction8.rightBack, Direction8.back, Direction8.leftBack };

	public static Dictionary<Direction8, Vector3> DicVector3 = new Dictionary<Direction8, Vector3>
	{
		[Direction8.left] = Vector3.left, [Direction8.leftFwd] = new Vector3(-1, 0, 1),
		[Direction8.forward] = Vector3.forward, [Direction8.rightFwd] = new Vector3(1, 0, 1),
		[Direction8.right] = Vector3.right, [Direction8.rightBack] = new Vector3(1, 0, -1),
		[Direction8.back] = Vector3.back, [Direction8.leftBack] = new Vector3(-1, 0, -1),
	};

	/// <summary>
	/// 反向
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public static Direction8 Oppo(this Direction8 direction)
	{
		return (byte)direction < 4 ? (direction + 4) : (direction - 4);
	}

	/// <summary>
	/// 上一个
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public static Direction8 Prev(this Direction8 direction)
	{
		return direction == Direction8.left ? Direction8.leftBack : (direction - 1);
	}

	/// <summary>
	/// 下一个
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public static Direction8 Next(this Direction8 direction)
	{
		return direction == Direction8.leftBack ? Direction8.left : (direction + 1);
	}

	public static bool Is4(this Direction8 direction)
	{
		return (int)direction % 2 == 0;
	}

	public static Vector3 ToVector3(this Direction8 direction)
	{
		return DicVector3[direction];
	}

	public static Vector2Int ToVector2Int(this Direction8 direction)
	{
		Vector3 vector3 = direction.ToVector3();
		return new Vector2Int((int)vector3.x, (int)vector3.z);
	}

	public static Vector3 Offset(this Direction8 direction, float scale, Vector3 source)
	{
		return direction.ToVector3() * scale + source;
	}

	public static float Get4Angle(this Direction8 direction)
	{
		switch (direction)
		{
			case Direction8.left:
				return 90;
			case Direction8.forward:
				return 180;
			case Direction8.right:
				return -90;
			case Direction8.back:
				return 0;
		}
		return 0;
	}

	private static float[] Sins90 = new float[] {
		Mathf.Sin(Mathf.PI * 0.5f), Mathf.Sin(Mathf.PI * 1.0f),
		Mathf.Sin(Mathf.PI * 1.5f), Mathf.Sin(Mathf.PI * 2)};

	private static float[] Coss90 = new float[] {
		Mathf.Cos(Mathf.PI * 0.5f), Mathf.Cos(Mathf.PI * 1.0f), 
		Mathf.Cos(Mathf.PI * 1.5f), Mathf.Cos(Mathf.PI * 2)};

	public static List<Vector2> Angle90Rotates = new List<Vector2>
		{
			new Vector2(Sins90[0], Coss90[0]),
			new Vector2(Sins90[1], Coss90[1]),
			new Vector2(Sins90[2], Coss90[2]),
			new Vector2(Sins90[3], Coss90[3])
		};

	public static Vector2 Get4AngleRotate(this Direction8 direction)
	{
		switch (direction)
		{
			case Direction8.left:
				return Angle90Rotates[2];
			case Direction8.forward:
				return Angle90Rotates[1];
			case Direction8.right:
				return Angle90Rotates[0];
			case Direction8.back:
				return Angle90Rotates[3];
		}
		return Angle90Rotates[3];
	}
}