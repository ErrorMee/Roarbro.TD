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
        // ָ����ͼ���ļ�����·��
        string screenshotName = "App/screenshot.png";

        // ����CaptureScreenshot�������н�ͼ
        ScreenCapture.CaptureScreenshot(screenshotName);

        // �����ͼ·��
        Debug.Log("Screenshot captured at: " + screenshotName);
    }
}
