using System;
using UnityEngine;
using UnityEngine.Networking;


public class PlatformerCharacter2D : NetworkBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                   //��ҿ�����x�����н�������ٶȡ�
    [SerializeField] private float m_JumpForce = 400f;                 //�����Ծʱ���ӵ�������
    [SerializeField] private bool m_AirControl = false;                //����Ƿ��������Ծʱת��;
    [SerializeField] private LayerMask m_WhatIsGround;                 //һ�����룬����ȷ����ɫ�Ļ���
    [SerializeField] public bool controlabel = true;                   //���Կ���

    private Transform m_GroundCheck;   //λ�ñ��������������Ƿ�ӵء�
    const float k_GroundedRadius = .2f; //�ص�Բ�İ뾶��ȷ���Ƿ�ӵ�
    public bool m_Grounded;            //�������Ƿ�ӵء�
    const float k_CeilingRadius = .01f; //�ص�Բ�İ뾶����ȷ������Ƿ����վ����
    private Animator m_Anim;           //������ҵĶ���������
    private Rigidbody2D m_Rigidbody2D;
    [SyncVar]
    private bool m_FacingRight = true; //����ȷ����ҵ�ǰ��Եķ�ʽ��
    private float uncontrolTime = 0;
    private SpriteRenderer _render;

    [SyncVar(hook = "OnMyColor")]
    public Color _color;//��ɫ��������仯ʱ��ͬ���������ͻ���
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
        //���òο���
        m_GroundCheck = transform.Find("GroundCheck");
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        m_Grounded = false;

        //������������λ�õķ���Ա�����κ�ָ��Ϊ��������壬������ͣ��
        //�����ʹ��ͼ������ɣ���Sample Assets���Ḳ��������Ŀ���á�
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
            }
        }
        CmdBoolAnim("Ground", m_Grounded);

        //���ô�ֱ����
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
        //ֻ���ڽӵػ�airControl��ʱ���ܿ��Ʋ�����
        if (controlabel)
        {
            if (!isLocalPlayer) return;  //�ж��Ƿ��Ǳ��ؿͻ���
            if (m_Grounded || m_AirControl)
            {
                // Speed animator��������Ϊˮƽ����ľ���ֵ��
                CmdNumAnim("Speed", Mathf.Abs(move));
                //�ƶ���ɫ
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                _walkParticel = move != 0;

                //����������������ƶ����������Ҳ������������...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    //CmdFilp();
                    m_FacingRight = !m_FacingRight;
                    //transform.Rotate(new Vector3(0, 180, 0));
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    selfRote = transform.rotation;
                }
                //����������������ƶ����򲥷����뿪���Ҳ�����������...
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
            //������Ӧ����Ծ...
            if (m_Grounded && jump)
            {
                // ��
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
    /// ʹ���ʧȥ���ƣ�����������������
    /// </summary>
    /// <param name="Radiu">�������뾶</param>
    /// <param name="time">����ʱ��</param>
    /// <param name="effectPoint">�������ĵ�</param>
    /// <param name="force">������������ʱ���ⵯ������ʱ����������</param>
    /// <param name="color">��ɫ�ڴ������µ���ɫ</param>
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
        controlabel = false;//������flag
        uncontrolTime = time;//����ʱ��
        _color = color;//��ɫ
        Radiu += 0.5f;//�뾶����
        Vector3 uniteVector = transform.position - effectPoint;//����������ĵ��Լ��ķ�������
        uniteVector = uniteVector.normalized;//�ѷ���������ɵ�λ����
        m_ForceTransSpeed = forceTransSpeed;//ǿ��λ�Ƶ��ٶ�
        m_ForceTransDirect = uniteVector;//ǿ��λ�Ƶķ���
        m_Anim.SetTrigger("hit");
        //��þ������������ĵľ���
        float distance = Vector3.Distance(effectPoint, transform.position);
        //����������ı��أ���Ϊֻ��distance < Radiu��ʱ��Ż����������������Բ��³��ָ���
        float effectWeight = (Radiu - distance) / Radiu;
        //������������������(��*����*����)
        Vector3 effectForce = force * effectWeight * uniteVector;
        //ʩ��
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

