using UnityEngine;
using UnityEngine.UI;

public class BehaviorTreeTest : MonoBehaviour
{
    readonly BTContainerOrder btRoot = new();

    public enum SeeState
    {
        None,
        Small,
        Big,
    }

    SeeState seeState = SeeState.Small;

    public Toggle toggleNone;
    public Toggle toggleSmall;
    public Toggle toggleBig;


    void Start()
    {
        Application.targetFrameRate = 8;

        toggleNone.onValueChanged.AddListener(OnNone);
        toggleSmall.onValueChanged.AddListener(OnSmall);
        toggleBig.onValueChanged.AddListener(OnBig);

        BTLeafLog leafStart = new("Start");
        btRoot.AddChild(leafStart);

        BTContainerSelector btSelector = new();

        BTLeafLog leafLog = new("Fight", false)
        {
            check = () => { return seeState == SeeState.Small; }
        };
        btSelector.AddChild(leafLog);

        leafLog = new("Escape", false)
        {
            check = () => { return seeState == SeeState.Big; }
        };
        btSelector.AddChild(leafLog);

        btRoot.AddChild(btSelector);

        BTLeafLog leafEnd = new("End");
        btRoot.AddChild(leafEnd);
    }

    void OnNone(bool select)
    {
        if (select)
        {
            seeState = SeeState.None;
        }
    }

    void OnSmall(bool select)
    {
        if (select)
        {
            seeState = SeeState.Small;
        }
    }

    void OnBig(bool select)
    {
        if (select)
        {
            seeState = SeeState.Big;
        }
    }


    void Update()
    {
        if (btRoot.Status != BTStatus.Finish)
        {
            btRoot.Execute();
        }
    }
}
