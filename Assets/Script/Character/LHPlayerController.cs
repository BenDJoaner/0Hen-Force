using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.Networking;

public class LHPlayerController : NetworkBehaviour
{
    //初始化参数（NetworkPlayer传入）
    private float m_MaxSpeed;
    private float m_JumpForce;
    private bool m_AirControl;

    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .01f;
    private Transform m_GroundCheck;
    public LayerMask m_WhatIsGround;//地面层级
    private Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;

    //联网同步处理
    [SyncVar(hook = "OnMyFacing")]
    private bool m_FacingRight = true;
    [SyncVar(hook = "OnMyColor")]
    public Color _color;
    [SyncVar(hook = "OnMyRote")]
    public Quaternion selfRote;

    //特殊情况处理
    [HideInInspector]
    public float uncontrolTime = 0;
    [HideInInspector]
    public bool controlabel = false;

    //普通变量
    private bool m_Grounded;
    private bool initDone;
    private bool m_Jump;
    private float x;
    private float y;
    // Use this for initialization
    void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void OnMyColor(Color color)
    {
        //TODO：设置名字颜色
    }

    public void OnMyFacing(bool Flag)
    {
        if (Flag)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            selfRote = transform.rotation;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            selfRote = transform.rotation;
        }
    }

    public void OnMyRote(Quaternion rote)
    {
        transform.rotation = rote;
    }

    public void DataInit(CharacterData data)
    {
        if (initDone) return;
        m_JumpForce = data.jumpForce;
        m_MaxSpeed = data.moveSpeed;
        m_AirControl = data.airContorl;
        m_Rigidbody2D.gravityScale = data.weight;
        m_Anim = data.OnGetAnimator();
        initDone = true;
    }

    void Update()
    {
        if (!m_Jump)
            m_Jump = CnInputManager.GetButtonDown("Jump");
    }

    void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        m_Grounded = GroundCheck();

        x = CnInputManager.GetAxis("Horizontal");
        y = CnInputManager.GetAxis("Vertical");

        if (initDone && controlabel)
        {
            Move(x, m_Jump);
        }
        else
        {
            //TODO：失控时候的状态
        }

        //动画
        CmdBoolAnim("Ground", m_Grounded);
        CmdNumAnim("vSpeed", m_Rigidbody2D.velocity.y);
    }

    void Move(float move, bool jump)
    {
        if (!isLocalPlayer) return;
        if (m_Grounded || m_AirControl)
        {
            CmdNumAnim("Speed", Mathf.Abs(move));

            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            //==================翻转角色测试==================
            if (move > 0 && !m_FacingRight)
            {
                m_FacingRight = !m_FacingRight;
            }
            else if (move < 0 && m_FacingRight)
            {
                m_FacingRight = !m_FacingRight;

            }
        }
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            CmdBoolAnim("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    //落地检测
    bool GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    //以下函数用于同步动画控制
    [Command]
    void CmdBoolAnim(string str, bool flag)
    {
        RpcBoolAnim(str, flag);
    }

    [Command]
    void CmdNumAnim(string str, float num)
    {
        RpcNumAnim(str, num);
    }

    [Command]
    void CmdTriggerAnim(string str)
    {
        RpcTriggerAnim(str);
    }

    [ClientRpc]
    void RpcBoolAnim(string str, bool flag)
    {
        if (!m_Anim) return;
        m_Anim.SetBool(str, flag);
    }

    [ClientRpc]
    void RpcNumAnim(string str, float num)
    {
        if (!m_Anim) return;
        m_Anim.SetFloat(str, num);
    }

    [ClientRpc]
    void RpcTriggerAnim(string str)
    {
        if (!m_Anim) return;
        m_Anim.SetTrigger(str);
    }
}
