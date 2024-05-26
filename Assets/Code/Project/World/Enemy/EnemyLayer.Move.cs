
using UnityEngine;

public partial class EnemyLayer : WorldLayer<EnemyUnit>
{
    private void MoveEnter()
    {
        units = new EnemyUnit[GridUtil.XCount, GridUtil.YCount];

        for (int y = 0; y < GridUtil.YCount; y++)
        {
            for (int x = 0; x < GridUtil.XCount; x++)
            {
                EnemyInfo info = EnemyModel.Instance.infos[x, y];
                if (info.config.id > 0)
                {
                    EnemyUnit unit = CreateUnit();
                    units[x, y] = unit;
                    unit.info = info;
                    unit.UpdateShow();
                    unit.transform.localPosition = new Vector3(x - GridUtil.XRadiusCount, 0,
                        y + GridUtil.FightStartY);
                    unit.fsm.ChangeState(EnemyState.Fight);
                }
            }
        }
    }

    private void MoveUpdate()
    {
    }
}