using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.Networking;

public class LHPlayerController : NetworkBehaviour
{
    //初始化参数（NetworkPlayer传入）
    private CharacterData _data;

    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .01f;
    private Transform m_GroundCheck;
    [FieldLabel("地面图层")]
    public LayerMask m_WhatIsGround;//地面层级
    public Animator m_Anim;
    private Rigidbody2D m_Rigidbody2D;

    //联网同步处理
    [SyncVar(hook = "OnMyFacing")]
    private bool m_FacingRight = true;
    [SyncVar(hook = "OnMyColor")]
    private Color _color;
    [SyncVar(hook = "OnMyRote")]
    private Quaternion selfRote;

    //特殊情况处理
    [HideInInspector]
    public float uncontrolTime = 0;
    [HideInInspector]
    public Common.AttackEffect state = (Common.AttackEffect)0;

    //普通变量
    private bool initDone;
    private bool m_Jump;
    [HideInInspector]
    private Vector2 _joystick;//摇杆输入的量
    private float jump_cold;

    LHNetworkPlayer _player;

    // Use this for initialization
    void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<LHNetworkPlayer>();
    }


    public void OnMyColor(Color color)
    {
        //TODO：角色颜色
        _color = color;
        // GetComponentInChildren<SpriteRenderer>().color = _color;
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
        m_FacingRight = Flag;
    }

    public void OnMyRote(Quaternion rote)
    {
        transform.rotation = rote;
        selfRote = rote;
    }

    public void DataInit(CharacterData data)
    {
        if (initDone || data == null) return;
        _data = data;
        m_Rigidbody2D.gravityScale = data.weight;
        m_Anim = data.OnGetAnimator();
        initDone = true;
    }

    void Update()
    {
        if (!initDone)
        {
            DataInit(_player._data);
        }
        if (!m_Jump && jump_cold <= 0)
        {
            m_Jump = CnInputManager.GetButtonDown("Jump");
        }

        if (jump_cold > 0)
            jump_cold -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (initDone)
        {
            if (!hasAuthority) return;
            _joystick.x = CnInputManager.GetAxis("Horizontal");
            _joystick.y = CnInputManager.GetAxis("Vertical");
            //动画
            Move(_joystick.x, m_Jump);
            CmdBoolAnim("Ground", GroundCheck());
        }
    }

    void Move(float move, bool jump)
    {
        if (!isLocalPlayer) return;
        if (GroundCheck() || _data.airContorl)
        {
            CmdNumAnim("Speed", Mathf.Abs(move));

            m_Rigidbody2D.velocity = new Vector2(move * _data.moveSpeed, m_Rigidbody2D.velocity.y);

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
        if (GroundCheck() && jump)
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, _data.jumpForce));
            m_Jump = false;
            jump_cold = 0.4f;
        }
    }

    //被作用不受控制的向某点靠近（远离）
    public void BeEffect_MoveTo(float effectTime, Transform target, float speed)
    {

    }

    //被作用受到力的作用
    public void BeEffect_AddForce(float effectTime, float force)
    {

    }

    //被作用移动受损
    public void BeEffect_MoveDamaged(float effectTime, float times, bool congeal)
    {

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
        if (!GetComponentInChildren<Animator>()) return;
        GetComponentInChildren<Animator>().SetBool(str, flag);
    }

    [ClientRpc]
    void RpcNumAnim(string str, float num)
    {
        if (!GetComponentInChildren<Animator>()) return;
        GetComponentInChildren<Animator>().SetFloat(str, num);
    }

    [ClientRpc]
    void RpcTriggerAnim(string str)
    {
        if (!GetComponentInChildren<Animator>()) return;
        GetComponentInChildren<Animator>().SetTrigger(str);
    }
}
