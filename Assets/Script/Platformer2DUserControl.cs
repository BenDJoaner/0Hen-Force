using System;
using UnityEngine;
using UnityEngine.Networking;
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
            // m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            m_Jump = Input.GetAxis("Jump") >= 0.01;
        }
    }

    private void FixedUpdate()
    {
        // float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // m_Character.Move(h, m_Jump);
        m_Jump = false;
    }
}

