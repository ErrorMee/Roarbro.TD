using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFocus : ScrollFocus<EnemyConfig, EnemySFCell>
{
    private void OnValidate()
    {
        RectTransform rectTransform = transform as RectTransform;

        if (rectTransform != null)
        {
            GameObject cellPrefab = CellPrefab;
            if(cellPrefab != null)
            {
                RectTransform cellRectTransform = cellPrefab.transform as RectTransform;
                float cellSize = cellRectTransform.sizeDelta.x;

                var totalSize = rectTransform.sizeDelta.x + (cellSize + 8) * 2;
                cellInterval = cellSize / totalSize;
                scrollOffset = 0.5f;
            }
        }
    }
}