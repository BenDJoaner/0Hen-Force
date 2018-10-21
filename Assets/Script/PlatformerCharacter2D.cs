using System;
using UnityEngine;
using UnityEngine.Networking;


public class PlatformerCharacter2D : NetworkBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                   //玩家可以在x轴上行进的最快速度。
    [SerializeField] private float m_JumpForce = 400f;                 //玩家跳跃时增加的力量。
    [SerializeField] private bool m_AirControl = false;                //玩家是否可以在跳跃时转向;
    [SerializeField] private LayerMask m_WhatIsGround;                 //一个掩码，用于确定角色的基础
    [SerializeField] public bool controlabel = true;                   //可以控制

    private Transform m_GroundCheck;   //位置标记在哪里检查玩家是否接地。
    const float k_GroundedRadius = .2f; //重叠圆的半径以确定是否接地
    public bool m_Grounded;            //播放器是否接地。
    const float k_CeilingRadius = .01f; //重叠圆的半径，以确定玩家是否可以站起来
    private Animator m_Anim;           //引用玩家的动画构件。
    private Rigidbody2D m_Rigidbody2D;
    [SyncVar]
    private bool m_FacingRight = true; //用于确定玩家当前面对的方式。
    private float uncontrolTime = 0;
    private SpriteRenderer _render;

    [SyncVar(hook = "OnMyColor")]
    public Color _color;//角色发生特殊变化时候同步到各个客户端
    [SyncVar(hook = "OnMyRote")]
    public Quaternion selfRote;
    [SyncVar(hook = "OnWalkParticel")]
    public bool _walkParticel;

    private bool initDone;
    private float m_ForceTransSpeed;
    private Vector3 m_ForceTransDirect;
    public GameObject walkparticel;

    private void Awake()
    {
        //设置参考。
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        m_Grounded = false;

        //如果到达地面检查位置的飞行员击中任何指定为地面的物体，则该玩家停飞
        //这可以使用图层来完成，但Sample Assets不会覆盖您的项目设置。
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
        CmdBoolAnim("Ground", m_Grounded);

        //设置垂直动画
        CmdNumAnim("vSpeed", m_Rigidbody2D.velocity.y);

        if (!_render) _render = GetComponentInChildren<SpriteRenderer>();
        if (!m_Anim) m_Anim = GetComponentInChildren<Animator>();
    }

    public void OnMyColor(Color color)
    {
        _render.color = color;
    }

    public void OnMyRote(Quaternion rote)
    {
        transform.rotation = rote;
    }

    public void OnWalkParticel(bool flag)
    {
        walkparticel.SetActive(flag);
    }

    public void DataInit(float jum, float speed, int weihgt, bool flag)
    {
        if (initDone) return;
        m_JumpForce = jum;
        m_MaxSpeed = speed;
        m_AirControl = flag;
        m_Rigidbody2D.gravityScale = weihgt;
        initDone = true;
    }

    public void Move(float move, bool jump)
    {

        if (!initDone) return;
        //只有在接地或airControl打开时才能控制播放器
        if (controlabel)
        {
            if (!isLocalPlayer) return;  //判断是否是本地客户端
            if (m_Grounded || m_AirControl)
            {
                // Speed animator参数设置为水平输入的绝对值。
                CmdNumAnim("Speed", Mathf.Abs(move));
                //移动角色
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                _walkParticel = move != 0;

                //如果输入正在向右移动播放器并且播放器朝向左侧...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    //CmdFilp();
                    m_FacingRight = !m_FacingRight;
                    //transform.Rotate(new Vector3(0, 180, 0));
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    selfRote = transform.rotation;
                }
                //否则，如果输入正在移动，则播放器离开并且播放器正对着...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    //CmdFilp();
                    m_FacingRight = !m_FacingRight;
                    //transform.Rotate(new Vector3(0, 180, 0));
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    selfRote = transform.rotation;
                }
            }
            //如果玩家应该跳跃...
            if (m_Grounded && jump)
            {
                // 。
                m_Grounded = false;
                CmdBoolAnim("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
        else
        {
            uncontrolTime -= Time.deltaTime;
            if (m_ForceTransSpeed != 0)
            {
                transform.Translate(m_ForceTransDirect * m_ForceTransSpeed * Time.deltaTime);
            }

            if (uncontrolTime <= 0)
            {
                _color = Color.white;
                m_ForceTransSpeed = 0;
                controlabel = true;
            }
        }
    }

    /// <summary>
    /// 使玩家失去控制，并对其产生里的作用
    /// </summary>
    /// <param name="Radiu">作用力半径</param>
    /// <param name="time">作用时间</param>
    /// <param name="effectPoint">作用中心点</param>
    /// <param name="force">作用力（正数时向外弹，负数时向中心吸）</param>
    /// <param name="color">角色在次作用下的颜色</param>
    public void PlayerUncontrol(float forceTransSpeed, float Radiu, float time, Vector3 effectPoint, float force, Color color)
    {
        CmdPlayerUnControl(forceTransSpeed, Radiu, time, effectPoint, force, color);
    }
    [Command]
    void CmdPlayerUnControl(float forceTransSpeed, float Radiu, float time, Vector3 effectPoint, float force, Color color)
    {
        RpcPlayerUncontrol(forceTransSpeed, Radiu, time, effectPoint, force, color);
    }
    [ClientRpc]
    void RpcPlayerUncontrol(float forceTransSpeed, float Radiu, float time, Vector3 effectPoint, float force, Color color)
    {
        controlabel = false;//被击中flag
        uncontrolTime = time;//作用时间
        _color = color;//变色
        Radiu += 0.5f;//半径修正
        Vector3 uniteVector = transform.position - effectPoint;//获得作用中心到自己的方向向量
        uniteVector = uniteVector.normalized;//把方向向量变成单位向量
        m_ForceTransSpeed = forceTransSpeed;//强制位移的速度
        m_ForceTransDirect = uniteVector;//强制位移的方向
        m_Anim.SetTrigger("hit");
        //获得距离作用力中心的距离
        float distance = Vector3.Distance(effectPoint, transform.position);
        //算出作用力的比重，因为只有distance < Radiu的时候才会调用这个函数，所以不怕出现负数
        float effectWeight = (Radiu - distance) / Radiu;
        //计算最终作用力向量(力*比重*方向)
        Vector3 effectForce = force * effectWeight * uniteVector;
        //施力
        m_Rigidbody2D.velocity = effectForce;


    }

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
}

