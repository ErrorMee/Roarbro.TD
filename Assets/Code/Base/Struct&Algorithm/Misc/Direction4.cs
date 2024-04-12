
public enum Direction4
{
    left = 0,
    forward,
    right,
    back,
}

public static class Direction4Ext
{
	public static Direction4 Oppo(this Direction4 direction)
	{
		return direction <= Direction4.forward ? direction + 2 : direction - 2;
	}
}
