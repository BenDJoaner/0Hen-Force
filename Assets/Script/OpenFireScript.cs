using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OpenFireScript : NetworkBehaviour
{

    public int WeaponIndex;
    public Transform BulletPos;
    public string PlayerName;
    float timecount;
    GameObject PlayerCanva;
    protected Collider _collider;
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    //private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
    private bool m_Grounded;            // Whether or not the player is grounded.

    // Use this for initialization
    void Start()
    {
        PlayerCanva = Instantiate(AssetConfig.GetPrefabByName("PlayerCanvas"), transform.position, Quaternion.identity);
        PlayerCanva.GetComponent<PlayerCanvaFollow>().OnSetTargetTransform(gameObject.transform);
        PlayerCanva.GetComponent<PlayerCanvaFollow>().OnSetPlayerName(PlayerName);
        _collider = GetComponent<Collider>();
        _collider.enabled = isServer;
    }

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        //m_CeilingCheck = transform.Find("CeilingCheck");
    }

    public override void OnStartLocalPlayer()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetLocalPlayer(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if ((Input.GetAxis("X") >= 1 || Input.GetKey(KeyCode.J)) && timecount <= 0)
        {
            CmdFire();
            timecount = 0.5f;
        }

        if (timecount > 0)
        {
            timecount -= Time.deltaTime;
        }

        if (transform.position.y < -12)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            transform.position = new Vector3(0, 0, 0);
        }
    }

    [Command]
    void CmdFire()
    {
        switch (WeaponIndex)
        {
            case 0:
                GameObject temp = Instantiate(AssetConfig.GetPrefabByName("Rocky"), BulletPos.position, Quaternion.identity);
                temp.transform.localEulerAngles = transform.localEulerAngles; ;
                temp.GetComponent<Rigidbody2D>().AddForce(transform.right * 700);
                GetComponent<Rigidbody2D>().AddForce(transform.right * -500);
                Destroy(temp, 5.0f);
                // Spawn the bullet on the Clients 
                NetworkServer.Spawn(temp);
                break;
            case 1://毒液
                m_Grounded = false;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                        m_Grounded = true;
                }
                if (m_Grounded)
                {
                    GameObject temp1 = Instantiate(AssetConfig.GetPrefabByName("AlienButtlet"), BulletPos.position, Quaternion.identity);
                    temp1.GetComponent<Rigidbody2D>().AddForce(Vector2.down * 300);
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * 200);
                    Destroy(temp1, 5.0f);
                    // Spawn the bullet on the Clients 
                    NetworkServer.Spawn(temp1);
                }
                break;
            case 2:
                break;
        }
    }
}
