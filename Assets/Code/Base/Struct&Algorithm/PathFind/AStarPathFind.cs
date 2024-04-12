using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFind
{
	/// <summary>
	/// 待查列表
	/// </summary>
	private List<PathNode> open;

	/// <summary>
	/// 已查列表
	/// </summary>
	private List<PathNode> closed;

	/// <summary>
	/// 寻路网格
	/// </summary>
	private PathGrid grid;

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
	/// 路径
	/// </summary>
	public List<PathNode> Path
	{
		private set;
		get;
	}

	public HashSet<PathNode> Visited
	{
		private set;
		get;
	}

	/// <summary>
	/// 估价函数
	/// </summary>
	private Func<PathNode, float> Heuristic
	{
		set;
		get;
	}

	/// <summary>
	/// 直线代价
	/// </summary>
	static readonly float straightCost = 1.0f;
	/// <summary>
	/// 对角代价
	/// </summary>
	static readonly float diagonalCost = Mathf.Sqrt(2);

	/// <summary>
	/// 构造函数
	/// </summary>
	public AStarPathFind()
	{
		open = new List<PathNode>();
		closed = new List<PathNode>();
		Visited = new HashSet<PathNode>();
		Path = new List<PathNode>();
		Heuristic = Manhattan;//Diagonal Manhattan Euclidian
	}

	public void Clear()
	{
		open.Clear();
		closed.Clear();
		Visited.Clear();
		Path.Clear();
	}

	public bool FindPath(PathGrid grid)
	{
		this.grid = grid;
		Clear();

		if (grid.StartNode == null)
		{
			Debug.Log("StartNode == null");
			return false;
		}

		if (grid.EndNode == null)
		{
			Debug.Log("EndNode == null");
			return false;
		}

		StartNode = grid.StartNode;
		EndNode = grid.EndNode;
		
		StartNode.startCost = 0;
		StartNode.endCost = Heuristic(StartNode);
		return Search();
	}

	private bool Search()
	{
		PathNode crtNode = StartNode;
		while (crtNode != EndNode)
		{
			for (int d = 0; d < Direction8Ext.Dir8.Length; d++)
			{
				Direction8 dir = Direction8Ext.Dir8[d];
				Vector2Int dirOffset = dir.ToVector2Int();
				PathNode neighbor = grid.GetNode(dirOffset.x + crtNode.x, dirOffset.y + crtNode.y);

				if (neighbor != null)
				{
					if (dir.Is4())
					{
						Direction8 dirOppo = dir.Oppo();
						if (!neighbor.enterAble[dirOppo])
						{
							continue;
						}
					}
					else
					{
						Direction8 dirPre = dir.Prev();
						Direction8 dirPreOppo = dirPre.Oppo();
						if (!neighbor.enterAble[dirPreOppo])
						{
							continue;
						}

						Direction8 dirNex = dir.Next();
						Direction8 dirNexOppo = dirNex.Oppo();
						if (!neighbor.enterAble[dirNexOppo])
						{
							continue;
						}

						Vector2Int preOffset = dirPre.ToVector2Int();
						PathNode neighborPre = grid.GetNode(preOffset.x + crtNode.x, preOffset.y + crtNode.y);
						if (!neighborPre.enterAble[dirPreOppo])
						{
							continue;
						}

						Vector2Int nexOffset = dirNex.ToVector2Int();
						PathNode neighborNex = grid.GetNode(nexOffset.x + crtNode.x, nexOffset.y + crtNode.y);
						if (!neighborNex.enterAble[dirNexOppo])
						{
							continue;
						}
					}

					float startCost = crtNode.startCost + NeighborCost(crtNode, neighbor);
					float endCost = Heuristic(neighbor);
					float fullCost = startCost + endCost;

					if (IsOpenOrClosed(neighbor))
					{
						if (neighbor.FullCost > fullCost)
						{
							neighbor.startCost = startCost;
							neighbor.endCost = endCost;
							neighbor.center = crtNode;
						}
					}
					else
					{
						neighbor.startCost = startCost;
						neighbor.endCost = endCost;
						neighbor.center = crtNode;
						open.Add(neighbor);
					}
				}
			}

			for (int o = 0; o < open.Count; o++)
			{
				Visited.Add(open[o]);
			}

			closed.Add(crtNode);

			if (open.Count == 0)
			{
				Debug.Log("no path find");
				return false;
			}

			crtNode = ShiftMinCostOpenNode();
		}

		BuildPath();
		return true;
	}

	public float NeighborCost(PathNode fromNode, PathNode toNode)
	{
		float cost = straightCost;
		if (!(fromNode.x == toNode.x || fromNode.y == toNode.y))
		{
			cost = diagonalCost;
		}
		return cost * toNode.costMultiplier;
	}

	private void BuildPath()
	{
		PathNode node = EndNode;
		Path.Add(node);
		while (node != StartNode)
		{
			node = node.center;
			Path.Add(node);
		}
		Path.Reverse();
	}

	private PathNode ShiftMinCostOpenNode()
	{
		open.Sort(SortByCost);
		PathNode shift = open[0];
		open.RemoveAt(0);
		return shift;
	}

	private int SortByCost(PathNode a, PathNode b)
	{
		float v = a.FullCost - b.FullCost;
		if (v > 0)
		{
			return 1;
		}
		else if(v < 0)
		{
			return -1;
		}
		return 0;
	}

	private bool IsOpenOrClosed(PathNode node)
	{
		return IsOpen(node) || IsClosed(node);
	}

	private bool IsOpen(PathNode node)
	{
		return open.Contains(node);
	}

	private bool IsClosed(PathNode node)
	{
		return closed.Contains(node);
	}


	#region Heuristic
	/// <summary>
	/// 曼哈顿距离,直角距离
	/// </summary>
	/// <param name="node"></param>
	/// <returns></returns>
	private float Manhattan(PathNode node)
	{
		return Mathf.Abs(node.x - EndNode.x) * straightCost + Mathf.Abs(node.y - EndNode.y) * straightCost;
	}

	/// <summary>
	/// 欧几里德距离 勾股定理，直线距离
	/// </summary>
	/// <param name="node"></param>
	/// <returns></returns>
	private float Euclidian(PathNode node)
	{
		float dx = node.x - EndNode.x;
		float dy = node.y = EndNode.y;
		return Mathf.Sqrt(dx * dx + dy * dy) * straightCost;
	}

	/// <summary>
	/// 对角线距离 找到最小正方形
	/// </summary>
	/// <param name="node"></param>
	/// <returns></returns>
	private float Diagonal(PathNode node)
	{
		float dx = Mathf.Abs(node.x - EndNode.x);
		float dy = Mathf.Abs(node.y - EndNode.y);

		float square = Mathf.Min(dx, dy);
		float straight = dx + dy;

		return diagonalCost * square + straightCost * (straight - 2 * square);
	}
	#endregion
}
