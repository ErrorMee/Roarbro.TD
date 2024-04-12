using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindTest : MonoBehaviour
{
	public int cellSize = 8;

	public PathFindCell cellObj;

	private PathGrid grid;
	AStarPathFind aStar = new AStarPathFind();

	private List<List<PathFindCell>> items;

	private void Start()
	{
		cellObj.gameObject.SetActive(false);

		grid = new PathGrid(cellSize, cellSize);
		grid.SetStartNode(0, 0);
		grid.SetEndNode(cellSize - 1, cellSize - 1);

		DrawGrid(grid);
		FindPath();
	}

	private void DrawGrid(PathGrid grid)
	{
		items = new List<List<PathFindCell>>();
		for (int i = 0; i < grid.Column; i++)
		{
			items.Add(new List<PathFindCell>());
			for (int j = 0; j < grid.Row; j++)
			{
				PathNode node = grid.GetNode(i, j);
				PathFindCell itenView = Instantiate(cellObj, this.gameObject.transform);
				itenView.gameObject.SetActive(true);
				itenView.SetData(node, grid, ClickCallBack);
				items[i].Add(itenView);
			}
		}
	}

	private void ClickCallBack(PathNode node)
	{
		grid.SetEnterAble(node.x, node.y, !node.enterAble[Direction8.left]);

		for (int i = 0; i < grid.Column; i++)
		{
			for (int j = 0; j < grid.Row; j++)
			{
				items[i][j].SetData(grid.GetNode(i,j), grid, ClickCallBack);
			}
		}

		FindPath();
	}

	private void FindPath()
	{
		if (aStar.FindPath(grid))
		{
			ShowVisited(aStar);
			ShowPath(aStar);
		}
	}

	private void ShowVisited(AStarPathFind aStar)
	{
		HashSet<PathNode> visited = aStar.Visited;
		foreach (PathNode node in visited)
		{
			items[node.x][node.y].ShowVisited(true);
		}
	}

	private void ShowPath(AStarPathFind aStar)
	{
		List<PathNode> path = aStar.Path;
		for (int i = 0; i < path.Count; i++)
		{
			PathNode node = path[i];
			items[node.x][node.y].ShowPath(true);
		}
	}
}
