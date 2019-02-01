using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CrossPlatformInput;
using CnControls;

public class jarodInputController : MonoBehaviour
{


    private float fingerActionSensitivity = Screen.width * 0.05f; //手指动作的敏感度，这里设定为 二十分之一的屏幕宽度.
                                                                  //
                                                                  // private float fingerBeginX;
    private float fingerBeginY;
    private float fingerCurrentY;
    private float fingerSegmentY;
    //
    private int fingerTouchState;
    //
    private int FINGER_STATE_NULL = 0;
    private int FINGER_STATE_TOUCH = 1;
    private int FINGER_STATE_ADD = 2;

    private VirtualButton _virtualButton;

    private void OnEnable()
    {
        _virtualButton = _virtualButton ?? new VirtualButton("Jump");
        CnInputManager.RegisterVirtualButton(_virtualButton);
    }

    private void OnDisable()
    {
        CnInputManager.UnregisterVirtualButton(_virtualButton);
    }
    // Use this for initialization
    void Start()
    {
        fingerActionSensitivity = Screen.width * 0.05f;
        fingerBeginY = 0;
        fingerCurrentY = 0;
        fingerSegmentY = 0;
        fingerTouchState = FINGER_STATE_NULL;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touchPos = Input.touchCount == 1 ? Input.GetTouch(0) : Input.GetTouch(1);
            if (fingerTouchState == FINGER_STATE_NULL)
            {
                fingerTouchState = FINGER_STATE_TOUCH;
                fingerBeginY = touchPos.position.y;
            }
        }
        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            Touch touchPos = Input.touchCount == 1 ? Input.GetTouch(0) : Input.GetTouch(1);
            fingerCurrentY = touchPos.position.y;
            fingerSegmentY = fingerCurrentY - fingerBeginY;
        }
        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            float fingerDistance = fingerSegmentY * fingerSegmentY;

            if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
            {
                toAddFingerAction();
            }
        }
        if (Input.touchCount <= 1)
        {
            fingerTouchState = FINGER_STATE_NULL;
            _virtualButton.Release();
        }
    }

    private void toAddFingerAction()
    {
        fingerTouchState = FINGER_STATE_ADD;
        if (fingerSegmentY > 0)
        {
            _virtualButton.Press();
        }
    }
}