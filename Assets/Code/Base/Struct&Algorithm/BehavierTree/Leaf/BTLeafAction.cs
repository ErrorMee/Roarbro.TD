using System;

public class BTLeafAction : BTLeaf
{
	private Action callFunc;

	public BTLeafAction(Action func) : base()
	{
		callFunc = func;
	}

	override protected void OnStart()
	{
		callFunc?.Invoke();
		callFunc = null;
		Finish();
	}
}