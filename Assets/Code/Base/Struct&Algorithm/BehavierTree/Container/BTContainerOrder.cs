
/// <summary>
/// 按顺序执行容器
/// </summary>
public class BTContainerOrder : BTContainer
{
    override protected void OnExecute()
    {
        for (int i = 0; i < children.Count; i++)
        {
            BTBase child = children[i];
            child.Execute();
            if (child.Status != BTStatus.Finish)
            {
                return;
            }
        }

        Finish();
    }
}