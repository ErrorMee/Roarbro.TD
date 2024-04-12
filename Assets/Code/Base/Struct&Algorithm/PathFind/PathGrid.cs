using System.Collections.Generic;

/// <summary>
/// 寻路网格
/// </summary>
public class PathGrid
{
	/// <summary>
	/// 起点
	/// </summary>
	public PathNode StartNode
	{
		private set;
		get;
	}
	/// <summary>
	/// 终点
	/// </summary>
	public PathNode EndNode
	{
		private set;
		get;
	}
	/// <summary>
	/// 所有点
	/// </summary>
	private List<List<PathNode>> nodes;
	/// <summary>
	/// 列
	/// </summary>
	public int Column
	{
		private set;
		get;
	}
	/// <summary>
	/// 行
	/// </summary>
	public int Row
	{
		private set;
		get;
	}
	/// <summary>
	/// 构造
	/// </summary>
	/// <param name="_column">列（x）</param>
	/// <param name="_row">行（y）</param>
	public PathGrid(int _column, int _row)
	{
		Column = _column;
		Row = _row;
		nodes = new List<List<PathNode>>();
		for (int i = 0; i < Column; i++)
		{
			nodes.Add(new List<PathNode>());
			for (int j = 0; j < Row; j++)
			{
				nodes[i].Add(new PathNode(i, j));
			}
		}
	}

	/// <summary>
	/// 获取节点
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public PathNode GetNode(int x, int y)
	{
		if (x < 0 || nodes.Count < (x + 1))
		{
			return null;
		}
		if (y < 0 || nodes[0].Count < (y + 1))
		{
			return null;
		}
		return nodes[x][y];
	}

	/// <summary>
	/// 设置起点
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void SetStartNode(int x, int y)
	{
		StartNode = GetNode(x, y);
	}

	/// <summary>
	/// 设置终点
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void SetEndNode(int x, int y)
	{
		EndNode = GetNode(x, y);
	}

	/// <summary>
	/// 设置是否可行走
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="enterAble"></param>
	public void SetEnterAble(int x, int y, bool enterAble)
	{
		SetEnterAble(x, y, enterAble, enterAble, enterAble, enterAble);
	}

	public void SetEnterAble(int x, int y, bool left, bool forward, bool right, bool back)
	{
		PathNode node = GetNode(x, y);
		if (node != null)
		{
			node.enterAble[Direction8.left] = left;
			node.enterAble[Direction8.forward] = forward;
			node.enterAble[Direction8.right] = right;
			node.enterAble[Direction8.back] = back;
		}
	}

	public void Clear()
	{
		for (int i = 0; i < Column; i++)
		{
			for (int j = 0; j < Row; j++)
			{
				PathNode node = nodes[i][j];
				node.Clear();
			}
		}
		StartNode = null;
		EndNode = null;
	}
}
