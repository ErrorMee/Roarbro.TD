using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : BattleLayer<PieceUnit>
{
    Dictionary<int, List<PieceUnit>> sortedReusePieces = new Dictionary<int, List<PieceUnit>>();

    static Vector3 fillSpeed = new Vector3(0.1f, 0.1f, 0.1f);

    private void FillEnter()
    {
        PieceModel.Instance.Fill();

        for (int i = 0; i < removePieces.Count; i++)
        {
            PieceUnit reuseUnit = removePieces[i];

            if (!sortedReusePieces.TryGetValue(reuseUnit.info.index.x, out List<PieceUnit> columnUnits))
            {
                columnUnits = new List<PieceUnit>();
                sortedReusePieces.Add(reuseUnit.info.index.x, columnUnits);
            }

            columnUnits.Add(reuseUnit);
        }

        foreach (var item in sortedReusePieces)
        {
            item.Value.Sort((A, B) => { return B.info.index.y - A.info.index.y; });
        }

        foreach (var item in sortedReusePieces)
        {
            for (int y = 0; y < item.Value.Count; y++)
            {
                PieceUnit reuseUnit = item.Value[y];
                reuseUnit.UpdateShow();
                reuseUnit.transform.localScale = Vector3.one;
                reuseUnit.transform.localPosition = new Vector3(reuseUnit.info.GetViewX(),
                0, -GridUtil.YRadiusCount - 1 - y);
            }
        }

        foreach (var item in sortedReusePieces)
        {
            item.Value.Clear();
        }
    }

    private void FillExit()
    {
    }

    private void FillUpdate()
    {
        PieceUnit reuseUnit = null;
        for (int i = 0; i < removePieces.Count; i++)
        {
            reuseUnit = removePieces[i];
            reuseUnit.transform.localScale += fillSpeed;
        }

        if (reuseUnit.transform.localScale.x >= 1 ||
                    reuseUnit.transform.localScale.z >= 1)
        {
            for (int i = 0; i < removePieces.Count; i++)
            {
                reuseUnit = removePieces[i];
                reuseUnit.transform.localScale = Vector3.one;
            }

            removePieces.Clear();
            ChangeState(PieceLayerState.Move);
        }
    }
}