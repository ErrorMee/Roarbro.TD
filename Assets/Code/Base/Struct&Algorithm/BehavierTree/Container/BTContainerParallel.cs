using UnityEngine;
/// <summary>
/// 并发执行容器
/// </summary>
public class BTContainerParallel : BTContainer
{
    override protected void OnExecute()
    {
        BTStatus statusTemp = BTStatus.Finish;
        for (int i = 0; i < children.Count; i++)
        {
            BTBase child = children[i];
            child.Execute();
            if (child.Status != BTStatus.Finish)
            {
                statusTemp = child.Status;
            }
        }

        if (statusTemp == BTStatus.Finish)
        {
            Finish();
        }
    }
}