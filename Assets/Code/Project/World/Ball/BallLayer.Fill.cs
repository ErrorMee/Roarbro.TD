using System;
using System.Collections.Generic;
using UnityEngine;

public partial class BallLayer : WorldLayer<BallUnit>
{
    Dictionary<int, List<BallUnit>> sortedReuseBalls = new Dictionary<int, List<BallUnit>>();

    static Vector3 fillSpeed = new Vector3(0.125f, 0.125f, 0.125f);

    private void FillEnter()
    {
        BallModel.Instance.Fill();

        for (int i = 0; i < removeBalls.Count; i++)
        {
            BallUnit reuseUnit = removeBalls[i];

            if (!sortedReuseBalls.TryGetValue(reuseUnit.info.index.x, out List<BallUnit> columnUnits))
            {
                columnUnits = new List<BallUnit>();
                sortedReuseBalls.Add(reuseUnit.info.index.x, columnUnits);
            }

            columnUnits.Add(reuseUnit);
        }

        foreach (var item in sortedReuseBalls)
        {
            item.Value.Sort((A, B) => { return B.info.index.y - A.info.index.y; });
        }

        foreach (var item in sortedReuseBalls)
        {
            for (int y = 0; y < item.Value.Count; y++)
            {
                BallUnit reuseUnit = item.Value[y];
                reuseUnit.UpdateShow();
                reuseUnit.transform.localScale = Vector3.one;
                reuseUnit.transform.localPosition = new Vector3(reuseUnit.info.GetViewX(),
                0, -GridUtil.YRadiusCount - 1 - y);
            }
        }

        foreach (var item in sortedReuseBalls)
        {
            item.Value.Clear();
        }
    }

    private void FillUpdate()
    {
        BallUnit reuseUnit = null;
        for (int i = 0; i < removeBalls.Count; i++)
        {
            reuseUnit = removeBalls[i];
            reuseUnit.transform.localScale += fillSpeed;
        }

        if (reuseUnit.transform.localScale.x >= 1 ||
                    reuseUnit.transform.localScale.z >= 1)
        {
            for (int i = 0; i < removeBalls.Count; i++)
            {
                reuseUnit = removeBalls[i];
                reuseUnit.transform.localScale = Vector3.one;
            }

            removeBalls.Clear();
            fsm.ChangeState(BallLayerState.Move);
        }
    }
}