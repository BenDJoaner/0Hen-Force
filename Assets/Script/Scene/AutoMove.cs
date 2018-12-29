using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    const float k_GroundedRadius = .2f;
    public LayerMask m_WhatIsGround;//地面层级
    public Transform GroundChecker;
    Rigidbody2D rigbody;
    int Aix_x = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
    }

    //没路检测
    bool OutWayCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundChecker.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        // print("掉头");
        return false;
    }

    void FixedUpdate()
    {
        if (!OutWayCheck()) Aix_x = -Aix_x;
        rigbody.velocity = new Vector2(Aix_x * 2, rigbody.velocity.y);
    }
}
