/// <summary>
/// 选择执行容器
/// </summary>
public class BTContainerSelector: BTContainer
{
    override protected void OnExecute()
    {
        bool selected = false;
        for (int i = 0; i < children.Count; i++)
        {
            BTBase child = children[i];

            if (selected == false)
            {
                child.Execute();
                if (child.Status != BTStatus.Wait && child.Status != BTStatus.Finish)
                {
                    selected = true;
                }
            }
            else
            {
                child.Finish();
            }
        }
        if (!selected)
        {
            Finish();
        }
    }
}