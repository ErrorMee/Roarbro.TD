#if UNITY_EDITOR || UNITY_STANDALONE
#define PCINPUT
#endif

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputModel : SingletonBehaviour<InputModel>
{
#if PCINPUT
    private Mouse mouse;
    private double tapStartTime = 0;
    private Keyboard keyboard;
#else
    private Touch? touch0;
    private Touch? touch1;
    private int touchCount = 0;
#endif

    public Action<float> Zoom;
    public Action<float> Rotate;
    public Action<Vector3> Offset;

    [SerializeField]
    [Range(0.1f, 0.3f)]
    private float tapGap = 0.18f;
    public bool TappedThisFrame
    {
        private set; get;
    } = false;

    public bool PressedThisFrame
    {
        private set; get;
    } = false;

    public bool Presseding
    {
        private set; get;
    } = false;

    public bool ReleasedThisFrame
    {
        private set; get;
    } = false;

    public Vector2 Touch0LastPos
    {
        private set; get;
    } = Vector2.zero;

    private Vector2 Touch1LastPos = Vector2.zero;

    public bool IsOverEventSystemObj
    {
        get { return EventSystem.current.IsPointerOverGameObject(); }
    }

    private void OnEnable()
    {
#if PCINPUT
        mouse = Mouse.current;
        keyboard = Keyboard.current;
#endif
    }

    private void Start()
    {
#if PCINPUT
#else
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp += OnFingerUp;
        Touch.onFingerMove += OnFingerMove;
#endif
    }

    private void Update()
    {
        Vector3 move = Vector3.zero;
#if PCINPUT
        Vector2 mousePosCrt = mouse.position.ReadValue();
        
        Vector2 scroll = mouse.scroll.ReadValue();
        float zoomDelta = scroll.y * 0.001f;
        if (!Mathf.Approximately(zoomDelta, 0))
        {
            Zoom?.Invoke(zoomDelta);
        }
        
        if (mouse.rightButton.isPressed)
        {
            Vector2 lastOffset = Touch0LastPos - CanvasModel.Instance.phoneCenter;
            Vector2 crtOffset = mousePosCrt - CanvasModel.Instance.phoneCenter;
            float angleDelta = Vector2.SignedAngle(lastOffset, crtOffset);
            Rotate?.Invoke(angleDelta * crtOffset.magnitude / CanvasModel.Instance.phoneSize.y);
        }

        if (mouse.leftButton.wasPressedThisFrame)
        {
            tapStartTime = mouse.lastUpdateTime;
            Presseding = PressedThisFrame = true;
        }else if (mouse.leftButton.isPressed)
        {
            Vector2 posGap = mousePosCrt - Touch0LastPos;
            Vector3 moveDelta = new(posGap.x, 0, posGap.y);
            Offset?.Invoke(0.05f * CanvasModel.Instance.phoneToDesignRate * -moveDelta);
        }
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            double timePass = mouse.lastUpdateTime - tapStartTime;
            if (timePass <= tapGap)
            {
                TappedThisFrame = true;
            }
            ReleasedThisFrame = true; Presseding = false;
        }

        Touch0LastPos = mousePosCrt;
#endif
    }

    private void LateUpdate()
    {
        ReleasedThisFrame = PressedThisFrame = TappedThisFrame = false;
    }

#if !PCINPUT
    private void OnFingerDown(Finger finger)
    {
        if (touch0 != null && touch1 == null)
        {
            touch1 = finger.currentTouch;
            Touch1LastPos = (Vector2)(touch1?.screenPosition);
            touchCount++;
        }

        if (touch0 == null)
        {
            touch0 = finger.currentTouch;
            Touch0LastPos = (Vector2)(touch0?.screenPosition);
            touchCount++;
            Presseding = PressedThisFrame = true;
        }
    }

    private void OnFingerUp(Finger finger)
    {
        if (touch0?.touchId == finger.currentTouch.touchId)
        {
            touch0 = null;
            touchCount--;
            double timePass = finger.currentTouch.time - finger.currentTouch.startTime;
            if (timePass <= tapGap)
            {
                TappedThisFrame = true;
            }
            ReleasedThisFrame = true; Presseding = false;
        }

        if (touch1?.touchId == finger.currentTouch.touchId)
        {
            touch1 = null;
            touchCount--;
        }
    }

    private void OnFingerMove(Finger finger)
    {
        if (touch0?.touchId == finger.currentTouch.touchId)
        {
            touch0 = finger.currentTouch;

            if (touchCount < 2)
            {
                Vector2 posPass = (Vector2)(touch0?.screenPosition) - Touch0LastPos;
                Vector3 moveDelta = new Vector3(posPass.x, 0, posPass.y);
                Offset?.Invoke(-moveDelta * 0.05f * CanvasModel.Instance.phoneToDesignRate);
            }
            else
            {
                HandleTwoTouch();
            }
            Touch0LastPos = (Vector2)(touch0?.screenPosition);
        }

        if (touch1?.touchId == finger.currentTouch.touchId)
        {
            touch1 = finger.currentTouch;
            if (touchCount >= 2)
            {
                HandleTwoTouch();
            }
            Touch1LastPos = (Vector2)(touch1?.screenPosition);
        }
    }

    private void HandleTwoTouch()
    {
        Vector2 lastOffset = Touch0LastPos - Touch1LastPos;
        Vector2 crtOffset = (Vector2)touch0?.screenPosition - (Vector2)touch1?.screenPosition;

        float lastDis = lastOffset.magnitude;
        float crtDis = crtOffset.magnitude;
        float disOffset = crtDis - lastDis;

        float zoomDelta = disOffset * 0.002f * CanvasModel.Instance.phoneToDesignRate;
        if (!Mathf.Approximately(zoomDelta, 0))
        {
            Zoom?.Invoke(zoomDelta);
        }

        float angleDelta = Vector2.SignedAngle(lastOffset, crtOffset);

        if (!Mathf.Approximately(angleDelta, 0))
        {
            Rotate?.Invoke(angleDelta);
        }
    }
#endif
}