using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class UICapture : MonoBehaviour
{
    private Keyboard keyboard;

    void Start()
    {
        keyboard = Keyboard.current;
    }

    void Update()
    {
        if (keyboard.pKey.wasReleasedThisFrame)
        {
            CaptureScreenshot();
        }
    }

    public void CaptureScreenshot()
    {
        // 指定截图的文件名和路径
        string screenshotName = "App/screenshot.png";

        // 调用CaptureScreenshot函数进行截图
        ScreenCapture.CaptureScreenshot(screenshotName);

        // 输出截图路径
        Debug.Log("Screenshot captured at: " + screenshotName);
    }
}
