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
            //�ڸ����ж�ȡ��ת���룬���Բ�������ť���¡�
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            m_Jump = Input.GetAxis("Jump") >= 0.01;
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        //��ȡ���롣
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // float h = Input.GetAxis("Horizontal");
        //�����в������ݸ��ַ����ƽű���
        m_Character.Move(h, m_Jump);
        m_Jump = false;
    }
}

