#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
#define USE_KEYBOARD
#endif

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour
{
    private Graphic padImage;

    [SerializeField] Graphic pointer;
    [SerializeField] Graphic bg;

    private Material bgMaterial;

    private float circleRadius;

    private bool steering;
    [SerializeField] JoyStick student;

    public Vector3 Direction
    {
        get; private set;
    } = Vector3.zero;

    public float Strength
    {
        get; private set;
    } = 0;

    public bool ManualOP
    {
        get; private set;
    } = false;

    private void Awake()
    {
        padImage = GetComponent<Image>();
        circleRadius = (bg.rectTransform.sizeDelta.x - 166) * 0.5f;
        bg.material = bgMaterial = new Material(bg.material);
#if USE_KEYBOARD
        keyboard = Keyboard.current;
#endif
    }

    void Start()
    {
        ClickListener.Add(padImage.transform).onEventDown = OnDownPad;
        DragListener.Add(padImage.transform).onEventDrag = OnDragPad;
        ClickListener.Add(padImage.transform).onEventUp = OnUpPad;
    }

    private void OnDownPad(PointerEventData eventData)
    {
        Vector3 canvasPos = CanvasModel.Instance.ScreenToCanvasPos(eventData.position);
        Vector3 pointTo = canvasPos - transform.localPosition;

        float pointToLen = pointTo.MagnitudeXY();

        if (pointToLen > circleRadius)
        {
            Vector3 pointToNormal = pointTo.normalized;
            bg.rectTransform.localPosition = pointTo - pointToNormal * circleRadius;
        }
        steering = true;
        ManualOP = true;
        Steer(canvasPos);
    }

    private void OnDragPad(PointerEventData eventData)
    {
        Steer(CanvasModel.Instance.ScreenToCanvasPos(eventData.position));
    }

    public void OnUpPad(PointerEventData eventData = null)
    {
        ManualOP = false;
        steering = false;
        pointer.rectTransform.localPosition = Vector3.zero;
        bg.transform.localPosition = Vector3.zero;
        Direction = Vector3.zero;
        Strength = 0;
        ShowBG(0);
        if (student != null && student.steering == false)
        {
            student.OnUpPad();
        }
    }

    public void Steer(Vector3 canvasPos)
    {
        Vector3 canvasPosLocal = canvasPos - transform.localPosition;

        Vector3 eventToCircle = canvasPosLocal - bg.transform.localPosition;
        Vector3 eventToCircleNormal = eventToCircle.normalized;
        float oriLen = eventToCircle.MagnitudeXY();
        
        if (oriLen > circleRadius)
        {
            Strength = 1;
            pointer.rectTransform.localPosition = (eventToCircleNormal * circleRadius) + bg.transform.localPosition;
        }
        else
        {
            Strength = oriLen / circleRadius;
            pointer.rectTransform.localPosition = canvasPosLocal;
        }
        
        Direction = new(eventToCircleNormal.x, 0, eventToCircleNormal.y);

        float signAngle = -Vector2.SignedAngle(Vector2.up, eventToCircleNormal);
        ShowBG(signAngle);

        if (student != null && student.steering == false)
        {
            student.Steer(canvasPos + student.transform.localPosition - transform.localPosition);
        }
    }

    private void ShowBG(float signAngle)
    {
        bgMaterial.SetFloat(MatPropUtil.ProgressKey, Strength);
    }

    private void OnDisable()
    {
        OnUpPad();
#if USE_KEYBOARD
        boardActive = false;
#endif
    }

#if USE_KEYBOARD
    [SerializeField] bool WASD = true;
    private Keyboard keyboard;
    private bool boardActive = false;
    private void Update()
    {
        Vector3 canvasPos = transform.localPosition;

        if (WASD ? keyboard.wKey.isPressed : keyboard.upArrowKey.isPressed)
        {
            boardActive = steering = true;
            canvasPos.y += circleRadius;
        }

        if (WASD ? keyboard.sKey.isPressed : keyboard.downArrowKey.isPressed)
        {
            boardActive = steering = true;
            canvasPos.y -= circleRadius;
        }

        if (WASD ? keyboard.aKey.isPressed : keyboard.leftArrowKey.isPressed)
        {
            boardActive = steering = true;
            canvasPos.x -= circleRadius;
        }

        if (WASD ? keyboard.dKey.isPressed : keyboard.rightArrowKey.isPressed)
        {
            boardActive = steering = true;
            canvasPos.x += circleRadius;
        }

        if (boardActive)
        {
            if (WASD ? (keyboard.wKey.wasReleasedThisFrame || keyboard.sKey.wasReleasedThisFrame ||
           keyboard.aKey.wasReleasedThisFrame || keyboard.dKey.wasReleasedThisFrame)
           : (keyboard.upArrowKey.wasReleasedThisFrame || keyboard.downArrowKey.wasReleasedThisFrame ||
           keyboard.leftArrowKey.wasReleasedThisFrame || keyboard.rightArrowKey.wasReleasedThisFrame))
            {
                OnUpPad();
                boardActive = false;
            }

            if (steering)
            {
                ManualOP = true;
                Steer(canvasPos);
            }
        }
    }
#endif
}