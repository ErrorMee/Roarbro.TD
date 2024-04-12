using UnityEngine;
using UnityEngine.UI;

public class SplineTest : MonoBehaviour
{
    public Transform bg;

    public Image[] bgs;

    public Image path1;
    public Image path2;
    public Image path3;

    void Start()
    {
        bgs = bg.GetComponentsInChildren<Image>();

        DrawPath();
    }

    private void DrawPath()
    {
        for (int i = 0; i < bgs.Length - 3; i++)
        {
            Vector3 pos0 = bgs[i + 0].rectTransform.localPosition;
            Vector3 pos1 = bgs[i + 1].rectTransform.localPosition;
            Vector3 pos2 = bgs[i + 2].rectTransform.localPosition;
            Vector3 pos3 = bgs[i + 3].rectTransform.localPosition;

            for (float p = 0; p <= 1f; p += 0.1f)
            {
                DrawPath1(pos0, pos1, pos2, pos3, p);
                DrawPath2(pos0, pos1, pos2, pos3, p);
                DrawPath3(pos0, pos1, pos2, pos3, p);
            }
        }

        path1.gameObject.SetActive(false);
        path2.gameObject.SetActive(false);
        path3.gameObject.SetActive(false);
    }

    private void DrawPath1(Vector3 pos0, Vector3 pos1, Vector3 pos2, Vector3 pos3, float progress)
    {
        GameObject path = Instantiate(path1.gameObject, path1.transform.parent);
        Vector2 pos = Spline.CatmullRom(pos0.XY(), pos1.XY(), pos2.XY(), pos3.XY(), progress);
        path.transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }

    private void DrawPath2(Vector3 pos0, Vector3 pos1, Vector3 pos2, Vector3 pos3, float progress)
    {
        GameObject path = Instantiate(path2.gameObject, path2.transform.parent);
        float y = Spline.CatmullRom(pos0.y, pos1.y, pos2.y, pos3.y, progress);
        path.transform.localPosition = new Vector3(Mathf.Lerp(pos1.x, pos2.x, progress), y, 0);
    }

    private void DrawPath3(Vector3 pos0, Vector3 pos1, Vector3 pos2, Vector3 pos3, float progress)
    {
        GameObject path = Instantiate(path3.gameObject, path3.transform.parent);
        Vector3 pos = Spline.CatmullRom(pos0, pos1, pos2, pos3, progress);
        path.transform.localPosition = new Vector3(pos.x, pos.y, 0);
    }
}
