using UnityEngine;

public partial class PieceLayer : WorldLayer<PieceUnit>
{
    private void FightEnter()
    {
        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                PieceUnit unit = units[x, y];
                unit.fighting = true;
            }
        }
    }

    private void FightUpdate()
    {
        
    }
}