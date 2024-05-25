using System.Collections.Generic;
using UnityEngine;

public partial class PieceModel : Singleton<PieceModel>, IDestroy
{
    private int maxStep;
    public int MaxStep
    {
        get
        {
            return maxStep;
        }
        set
        {
            if (maxStep != value)
            {
                maxStep = value;
                LeftStep = maxStep;
            }
        }
    }

    private int leftStep;
    public int LeftStep
    {
        get
        {
            return leftStep;
        }
        set
        {
            if (leftStep != value)
            {
                leftStep = Mathf.Max(0, value);
                EventModel.Send(EventEnum.ChangeStep);
            }
        }
    }

    private List<Vector2Int> dragIndexs = new List<Vector2Int>();

    public void DragPiece(PieceInfo fromInfo, Vector2Int toIndex)
    {
        combo = 0;
        if (fromInfo.index.x != toIndex.x && fromInfo.index.y != toIndex.y)
        {
            MoveIndex(fromInfo, fromInfo.index);
        }
        else
        {
            if (fromInfo.index.x == toIndex.x && fromInfo.index.y == toIndex.y)
            {
                MoveIndex(fromInfo, fromInfo.index);
            }
            else
            {
                if (toIndex.x >= 0 && toIndex.x < infos.GetLength(0) &&
                    toIndex.y >= 0 && toIndex.y < infos.GetLength(1))
                {
                    Vector2Int fromIndex = fromInfo.index;
                    PieceInfo toInfo = infos[toIndex.x, toIndex.y];
                    MoveIndex(toInfo, fromIndex);
                    MoveIndex(fromInfo, toIndex);

                    dragIndexs.Add(fromIndex);
                    dragIndexs.Add(toIndex);

                    LeftStep--;
                }
                else
                {
                    MoveIndex(fromInfo, fromInfo.index);
                }
            }
        }
    }

    public void MoveIndex(PieceInfo piece, Vector2Int index)
    {
        piece.index = index;
        infos[piece.index.x, piece.index.y] = piece;
        EventModel.Send(EventEnum.MovePiece, piece);
    }
}