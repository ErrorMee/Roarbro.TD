using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PieceLayer : WorldLayer<PieceUnit>
{
    private void DragEnter()
    {
        CreateSelect();
    }

    private void DragUpdate()
    {
        Vector3 worldPos = CameraModel.Instance.ScreenToWorldPos(InputModel.Instance.Touch0LastPos,
            transform.position.y);

        float viewX = selectUnit.info.GetViewX();
        float viewZ = selectUnit.info.GetViewZ();

        Vector3 unitPos = new Vector3(Mathf.Clamp(worldPos.x, viewX - 1f, viewX + 1f),
            0.02f, Mathf.Clamp(worldPos.z, viewZ - 1f, viewZ + 1f));

        Vector3 offset = unitPos - selectUnit.transform.localPosition;
        if (offset.sqrMagnitude <= moveStreshold)
        {
            selectUnit.transform.localPosition = unitPos;
        }
        else
        {
            selectUnit.transform.localPosition += offset.normalized * moveSpeed;
        }

        bool selectEnable = false;
        Vector3 selectPos = GridUtil.WorldToGridPos(unitPos, false);
        Vector2Int index = GridUtil.WorldToGridIndex(selectPos);
        if (GridUtil.InGrid(index.x, index.y))
        {
            if ((Mathf.Abs(index.x - selectUnit.info.index.x) +
                Mathf.Abs(index.y - selectUnit.info.index.y)) > 1)
            {
                select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey,
                    QualityConfigs.GetColor2(QualityEnum.SSR));
            }
            else
            {
                select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey,
                    QualityConfigs.GetColor(QualityEnum.N));
                selectEnable = true;
            }
        }
        else
        {
            select.meshRenderer.SetMPBColor(MatPropUtil.BaseColorKey,
                QualityConfigs.GetColor2(QualityEnum.SSR));
        }
        selectPos.y = 0;
        select.transform.localPosition = selectPos;

        if (InputModel.Instance.ReleasedThisFrame)
        {
            if (selectEnable == false)
            {
                select.gameObject.SetActive(false);
            }
            index = GridUtil.WorldToGridIndex(select.transform.localPosition);
            PieceModel.Instance.DragPiece(selectUnit.info, index);
            selectUnit = null;

            fsm.ChangeState(PieceLayerState.Move);
        }
    }
}