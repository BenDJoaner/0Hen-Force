using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
//using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : NetworkBehaviour
{
    private PlatformerCharacter2D m_Character;
    private bool m_Jump;


    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
    }

    [ClientCallback]
    private void Update()
    {
        if (!m_Jump)
        {
            //在更新中读取跳转输入，所以不会错过按钮按下。
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            m_Jump = Input.GetAxis("Jump") >= 0.01;
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        //读取输入。
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // float h = Input.GetAxis("Horizontal");
        //将所有参数传递给字符控制脚本。
        m_Character.Move(h, m_Jump);
        m_Jump = false;
    }
}

