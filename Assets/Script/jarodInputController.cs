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
    // private float fingerCurrentX;
    private float fingerCurrentY;
    // private float fingerSegmentX;
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

        // fingerBeginX = 0;
        fingerBeginY = 0;
        // fingerCurrentX = 0;
        fingerCurrentY = 0;
        // fingerSegmentX = 0;
        fingerSegmentY = 0;

        fingerTouchState = FINGER_STATE_NULL;

    }
    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            print(Input.touchCount);
            Touch touchPos = Input.touchCount == 1 ? Input.GetTouch(0) : Input.GetTouch(1);
            if (fingerTouchState == FINGER_STATE_NULL)
            {
                fingerTouchState = FINGER_STATE_TOUCH;
                // fingerBeginX = touchPos.position.x;
                fingerBeginY = touchPos.position.y;
                print("Y:" + fingerBeginY);
            }

        }

        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            Touch touchPos = Input.touchCount == 1 ? Input.GetTouch(0) : Input.GetTouch(1);
            // fingerCurrentX = touchPos.position.x;
            fingerCurrentY = touchPos.position.y;
            // fingerSegmentX = fingerCurrentX - fingerBeginX;
            fingerSegmentY = fingerCurrentY - fingerBeginY;

        }


        if (fingerTouchState == FINGER_STATE_TOUCH)
        {
            // float fingerDistance = fingerSegmentX * fingerSegmentX + fingerSegmentY * fingerSegmentY;
            float fingerDistance = fingerSegmentY * fingerSegmentY;

            if (fingerDistance > (fingerActionSensitivity * fingerActionSensitivity))
            {
                toAddFingerAction();
            }
        }

        if (Input.touchCount <= 1)
        {
            fingerTouchState = FINGER_STATE_NULL;
            //CrossPlatformInputManager.SetButtonUp("Jump");
            _virtualButton.Release();
        }
    }

    private void toAddFingerAction()
    {

        fingerTouchState = FINGER_STATE_ADD;

        // if (Mathf.Abs(fingerSegmentX) > Mathf.Abs(fingerSegmentY))
        // {
        //     fingerSegmentY = 0;
        // }
        // else
        // {
        //     fingerSegmentX = 0;
        // }

        // if (fingerSegmentX == 0)
        // {
        if (fingerSegmentY > 0)
        {
            //Debug.Log("up");
            //CrossPlatformInputManager.SetButtonDown("Jump");
            _virtualButton.Press();
        }
        else
        {
            //Debug.Log("down");
        }
        // }
        //else if (fingerSegmentY == 0)
        //{
        //    if (fingerSegmentX > 0)
        //    {
        //        //Debug.Log("right");
        //    }
        //    else
        //    {
        //        //Debug.Log("left");
        //    }
        //}

    }
}