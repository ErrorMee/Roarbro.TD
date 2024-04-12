using System.Collections.Generic;

public class PathNode
{
	public int x;
	public int y;

	/// <summary>
	/// 总代价：该点到起点和终点两段代价总和
	/// </summary>
	public float FullCost
	{
		get
		{
			return startCost + endCost;
		}
	}
	/// <summary>
	/// 起点到当前点的代价
	/// </summary>
	public float startCost;
	/// <summary>
	/// 从当前点到终点的估计代价
	/// </summary>
	public float endCost;

	/// <summary>
	/// 四方向行走
	/// </summary>
	public Dictionary<Direction8, bool> enterAble =
		new Dictionary<Direction8, bool>()
		{
			[Direction8.left] = true,
			[Direction8.forward] = true,
			[Direction8.right] = true,
			[Direction8.back] = true
		};

	public PathNode center;
	public float costMultiplier = 1.0f;

	public PathNode(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public void Clear()
	{
		startCost = 0;
		endCost = 0;
		center = null;
	}
}
