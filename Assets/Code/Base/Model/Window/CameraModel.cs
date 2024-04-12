using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModel : SingletonBehaviour<CameraModel>
{
    Transform root;
    /// <summary>
	/// 控制摄像机的旋转
	/// </summary>
	Transform swivel;
    /// <summary>
    /// 控制摄像机的距离
    /// </summary>
    Transform stick;

    private new Camera camera;

    public Rect InitViewRect
    {
        private set;
        get;
    }

    public Material materialCamera;

    public MeshRenderer backGround;
    readonly Dictionary<BGEnum, Material> bgMats = new();

    protected override void Awake()
    {
        root = transform.Find("Root");
        backGround = root.Find("BackGround").GetComponent<MeshRenderer>();
        swivel = root.Find("Swivel");
        stick = swivel.GetChild(0);
        camera = stick.GetChild(0).GetComponent<Camera>();

        Vector3 leftDown = ScreenToWorldPos(Vector2.zero, 0);
        Vector3 rightUp = ScreenToWorldPos(new Vector2(Screen.width, Screen.height), 0);
        InitViewRect = new Rect(leftDown.x, leftDown.z, rightUp.x - leftDown.x, rightUp.z - leftDown.z);
    }

    public Vector3 ScreenToWorldPos(Vector2 screenPos, float height)
    {
        Ray ray = camera.ScreenPointToRay(screenPos);
        Vector3 worldPos = RayUtil.FixedY(ray, height);
        return worldPos;
    }

    bool hasTarget = false;
    Transform _target;
    public Transform Target
    {
        set
        {
            _target = value;
            hasTarget = _target != null;
        }
    }

    void LateUpdate()
    {
        if (hasTarget)
        {
            transform.position = new Vector3(_target.position.x, 0, _target.position.z);
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    public MeshRenderer ShowBackGround(BGEnum bgEnum)
    {
        if (bgEnum != BGEnum.None)
        {
            if (bgEnum != BGEnum.Reuse)
            {
                if (!bgMats.TryGetValue(bgEnum, out Material material))
                {
                    material = AddressModel.LoadMaterial(Address.BgMaterial("SDF_BG_" + bgEnum.ToString()));
                    bgMats.Add(bgEnum, material);
                }
                backGround.transform.localScale = new Vector3(InitViewRect.width, 0, InitViewRect.height) * 1.1f;
                backGround.sharedMaterial = material;
                backGround.gameObject.SetActive(true);
            }
        }
        else
        {
            backGround.gameObject.SetActive(false);
        }
        return backGround;
    }

    public void Shake(Vector3 maxShake)
    {
        ShakeUtil.Play(root, maxShake, true, 1, 4, 30);
    }

    public void CullingMaskAll()
    {
        camera.cullingMask = -1;
    }

    public void CullingMaskJust(LayerEnum layer)
    {
        camera.cullingMask = (1 << (int)layer);
    }

    public void Vibrate(int milliseconds)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            //var s = AndroidUtil.service.GetStatic<string>("VIBRATOR_SERVICE");
            //AndroidJavaObject vibrator = AndroidUtil.currentActivity.Call<AndroidJavaObject>("getSystemService", s);
            //if (vibrator.Call<bool>("hasVibrator"))
            //{
            //    vibrator.Call("vibrate", milliseconds);
            //}
            Handheld.Vibrate();
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Handheld.Vibrate();
            StartCoroutine(StopVibration(milliseconds * 0.001f));
        }
        ShakeUtil.Play(root, Vector3.one * 0.1f, true, 1.5f, 5, 10);
    }

    private IEnumerator StopVibration(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Handheld.Vibrate();
    }
}