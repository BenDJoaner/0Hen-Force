using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
public class LHNetworkPlayer : NetworkBehaviour
{
    //网络同步
    [SyncVar]
    public string playerName;//玩家名字
    [SyncVar]
    public int team;

    private bool luachFlag;
    private float preAttackTime;
    public CharacterData data;
    public GameObject pCanva;//画布控制脚本
    float _shootingTimer;

    LHNetworkGameManager manager;
    //很难控制什么时候调用Init（网络令对象产生非确定性的顺序）
    //所以我们从多个位置调用init（取决于首先创建的宇宙飞船和管理器之间的内容）。
    protected bool _wasInit = false;

    void Awake()
    {
        //在游戏管理器中注册，这将允许循环。
        //LHNetworkGameManager.sPlayer.Add(this);
        //Invoke("Init", 0.1f);
    }

    private void Start()
    {
        manager = LHNetworkGameManager.sInstance;

        pCanva = Instantiate(AssetConfig.GetPrefabByName("PlayerCanvas"), transform.position, Quaternion.identity);
        pCanva.GetComponent<PlayerCanvaFollow>().OnSetTargetTransform(gameObject.transform);
        pCanva.GetComponent<PlayerCanvaFollow>().OnSetPlayerName(playerName);
        switch (team)
        {
            case 1:
                pCanva.GetComponent<PlayerCanvaFollow>().OnSetColor(new Color(52.0f / 255.0f, 114.0f / 255.0f, 161.0f / 255.0f, 1.0f));
                break;
            default:
                pCanva.GetComponent<PlayerCanvaFollow>().OnSetColor(new Color(161.0f / 255.0f, 92.0f / 255.0f, 52.0f / 255.0f, 1.0f));
                break;
        }

        if (isLocalPlayer)
        {
            manager.Init(this);
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<PlatformEffector2D>().enabled = true;
            GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraControl>().Player = gameObject;
        }
    }

    public void Init(CharacterData data)
    {
        if (_wasInit||data==null)
            return;
        //设置属性，动画和武=====START==========
        Instantiate(data.gameObject, transform.position, Quaternion.identity).transform.SetParent(gameObject.transform, true);       //生成Char
        GetComponent<PlatformerCharacter2D>().DataInit(
            data.jumpForce,
            data.moveSpeed,
            data.weight,
            data.airContorl);
        //设置属性，动画和武=====END==========
        Instantiate(AssetConfig.GetPrefabByName("KillParticle"), transform.position, Quaternion.identity);
        _wasInit = true;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (!_wasInit) return;
        //if (CrossPlatformInputManager.GetButtonDown("Submit") && _shootingTimer <= 0)
        //{
        //    _shootingTimer = data.attackSpeed;
        //    luachFlag = true;
        //    preAttackTime = data.preAttackTime;
        //}

        if (_shootingTimer > 0)
            _shootingTimer -= Time.deltaTime;

        if (transform.position.y < -20 || transform.position.x < -30 || transform.position.x > 30) transform.position = new Vector3(0, 0, 0);

        if (luachFlag)
        {
            preAttackTime -= Time.deltaTime;
            if (preAttackTime <= 0)
            {
                luachFlag = false;
                preAttackTime = 0;
                //foreach (Transform trans in GetComponentInChildren<CharacterData>().m_AttackCheck)
                //{
                //    CmdFire(trans.position, transform.localEulerAngles, data.luanchForce);
                //}
            }
        }
    }

    [Command]
    public void CmdFire(Vector3 AttackPos, Vector3 Rote, float force)
    {
        //Rigidbody2D Bullet = Instantiate(data.bullet, AttackPos, Quaternion.identity) as Rigidbody2D;
        //Bullet.transform.localEulerAngles = Rote;
        //Bullet.gameObject.GetComponent<GaneralLuachScript>()._teamFlag = team;
        //Bullet.velocity = (AttackPos - transform.position) * force;
        //NetworkServer.Spawn(Bullet.gameObject);
        //RpcFireAnim();
    }

    [ClientRpc]
    public void RpcFireAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("attack");
    }
}
