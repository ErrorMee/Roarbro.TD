using UnityEngine;

public partial class PieceLayer : WorldLayer<PieceUnit>
{
    private void FightEnter()
    {
        PieceModel.Instance.fighting = true;
    }

    private void FightUpdate()
    {
        
    }
}