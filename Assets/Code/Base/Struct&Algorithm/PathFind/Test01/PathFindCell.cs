using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFindCell : MonoBehaviour
{
	public Color defult;
	public Color obstacle;
	public Color start;
	public Color end;

	public Color path;
	public Color visited;

	public PathNode node;
	PathGrid grid;

	public Image image;

	public Image point;

	public Text text;

	Action<PathNode> clickCallBack;

	private void Awake()
	{
		ClickListener.Add(transform).onClick = OnClick;
	}

	private void OnClick()
	{
		clickCallBack?.Invoke(node);
	}

	public void SetData(PathNode node, PathGrid grid, Action<PathNode> clickCallBack)
	{
		this.node = node;
		this.grid = grid;
		this.clickCallBack = clickCallBack;
		if (this.node.enterAble[Direction8.left])
		{
			image.color = defult;

			if (this.grid.StartNode == this.node)
			{
				image.color = start;
			}
			if (this.grid.EndNode == this.node)
			{
				image.color = end;
			}
		}
		else
		{
			image.color = obstacle;
		}
		ShowVisited(false);
		ShowPath(false);
		text.text = node.x + " " + node.y;
	}

	public void ShowVisited(bool value)
	{
		point.gameObject.SetActive(value);
		if (value)
		{
			point.color = visited;
		}
	}

	public void ShowPath(bool value)
	{
		if (value)
		{
			point.color = path;
		}
	}
}
