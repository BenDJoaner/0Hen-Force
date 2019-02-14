using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CnControls;
using UnityEngine.Networking;

public class LHPlayerController : NetworkBehaviour
{
    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .01f;
    
    [FieldLabel("地面图层")]
    public LayerMask m_WhatIsGround;//地面层级
    public Animator m_Anim;

    [SyncVar(hook = "OnMyFacing")]
    private bool m_FacingRight = true;
    [SyncVar(hook = "OnMyColor")]
    private Color _color;
    [SyncVar(hook = "OnMyRote")]
    [HideInInspector]
    public float uncontrolTime = 0;
    [HideInInspector]
    public Common.AttackEffect state = (Common.AttackEffect)0;
    private Rigidbody2D m_Rigidbody2D;
    private bool initDone;
    private bool m_Jump;
    private Transform m_GroundCheck;
    private Vector2 _joystick;//摇杆输入的量
    private float jump_cold;
    private GameObject m_PointerCheck;
    private Quaternion selfRote;
    CharacterData _data;
    //攻击控制相关
    bool m_attack;
    Parabola parabola;
    LHNetworkPlayer _player;

    public bool InitDone { get => initDone; set => initDone = value; }

    // Use this for initialization
    void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        _player = GetComponent<LHNetworkPlayer>();
        m_PointerCheck = transform.Find("PointerCheck").gameObject;
        parabola = m_PointerCheck.GetComponent<Parabola>();
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
        if (InitDone || data == null) return;
        _data = data;
        m_Rigidbody2D.gravityScale = data.weight;
        m_Anim = data.OnGetAnimator();
        if (data.shooterModule && !data.autoAim)
        {
            m_PointerCheck.SetActive(true);
            parabola.shootForce = data.luanchForce;
            if (!data.aimContorlable)
            {
                m_PointerCheck.transform.eulerAngles = new Vector3(0, 0, data.luanchAngle);
            }
        }
        else
        {
            m_PointerCheck.SetActive(false);
        }
        InitDone = true;
    }

    void Update()
    {
        if (!InitDone)
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
        if (InitDone)
        {
            if (!hasAuthority) return;
            _joystick.x = CnInputManager.GetAxis("Horizontal");
            _joystick.y = CnInputManager.GetAxis("Vertical");
            //动画
            Move(_joystick.x, m_Jump);
            CmdBoolAnim("Ground", GroundCheck());
        }

        if (InitDone)
        {
            _joystick.x = CnInputManager.GetAxis("Horizontal");
            _joystick.y = CnInputManager.GetAxis("Vertical");
            m_attack = CnInputManager.GetButtonDown("attack");

            AimFunc();

            if (m_attack)
            {
                _player.setLunchFlag();
                GetComponentInChildren<Animator>().SetTrigger("attack");
            }
            else
            {

            }
        }
    }

    public void OnFire(){
        LuanchPos[] posArr = GetComponentsInChildren<LuanchPos>();
    }
    void AimFunc()
    {
        if (!_data.aimContorlable) return;
        if (_joystick == new Vector2(0, 0)) return;
        float angle = Mathf.Atan2(_joystick.y, _joystick.x) * 180f / Mathf.PI;
        m_PointerCheck.transform.eulerAngles = new Vector3(0, 0, angle);
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
