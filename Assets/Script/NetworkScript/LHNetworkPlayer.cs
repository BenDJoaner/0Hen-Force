using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(LHPlayerController))]
[AddComponentMenu("0HenTool/LHNetworkPlayer")]
public class LHNetworkPlayer : NetworkBehaviour
{
    //网络同步
    [SyncVar]
    public string playerName;//玩家名字
    [SyncVar]
    public int team;

    private bool luachFlag;
    private float preAttackTime;
    public CharacterData _data;
    public GameObject pCanva;//画布控制脚本
    float _shootingTimer;
    public bool controlable;
    private GameObject m_PointerCheck;
    private GameObject hero;

    LHNetworkGameManager manager;
    PlayerCanvaFollow infoCanva;

    protected bool _wasInit = false;
    EnegySprite _carrySprite;

    void Awake()
    {
        //在游戏管理器中注册，这将允许循环。
        // LHNetworkGameManager.sPlayer.Add(this);
        m_PointerCheck = transform.Find("PointerCheck").gameObject;
    }

    private void Start()
    {
        // manager = LHNetworkGameManager.sInstance;

        pCanva = Instantiate(AssetConfig.GetPrefabByName("PlayerCanvas"), transform.position, Quaternion.identity);
        infoCanva = pCanva.GetComponent<PlayerCanvaFollow>();
        infoCanva.OnSetTargetTransform(gameObject.transform);
        infoCanva.OnSetPlayerName(playerName);
        switch (team)
        {
            case 1:
                infoCanva.OnSetColor(new Color(52.0f / 255.0f, 114.0f / 255.0f, 161.0f / 255.0f, 1.0f));
                break;
            default:
                infoCanva.OnSetColor(new Color(161.0f / 255.0f, 92.0f / 255.0f, 52.0f / 255.0f, 1.0f));
                break;
        }
    }

    public void Init(CharacterData data)
    {
        if (_wasInit || data == null)
            return;
        //生成玩家对象
        hero = Instantiate(data.gameObject, transform.position, Quaternion.identity);       //生成Char
        hero.transform.SetParent(gameObject.transform, true);
        //传送数据
        this._data = data;
        _wasInit = true;
        GetComponent<LHPlayerController>().enabled = true;
        GetComponent<Rigidbody2D>().WakeUp();
    }

    [ClientCallback]
    private void Update()
    {
        if (!manager && GameObject.FindGameObjectWithTag("GameManager"))
        {
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LHNetworkGameManager>();
            if (isLocalPlayer)
            {
                manager.Init(this);
                GetComponent<CircleCollider2D>().enabled = true;
                // GetComponent<PlatformEffector2D>().enabled = true;
                GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraControl>().Player = gameObject;
            }
            else
            {
                // Destroy(GetComponent<PlatformEffector2D>());
                GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        if (!isLocalPlayer) return;
        if (!_wasInit) return;

        if (_shootingTimer > 0)
            _shootingTimer -= Time.deltaTime;

        if (transform.position.y < -20 || transform.position.x < -30 || transform.position.x > 30) ResetPlayer();

        if (luachFlag)
        {
            preAttackTime -= Time.deltaTime;
            if (preAttackTime <= 0)
            {
                luachFlag = false;
                preAttackTime = 0;
            }
        }

        //临时
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        manager.ui_select.gameObject.SetActive(true);
        _wasInit = false;
        _data = null;
        transform.position = new Vector3(0, 0, 0);
        GameObject.Destroy(hero);
        transform.eulerAngles = new Vector3(0, 0, 0);
        GetComponent<LHPlayerController>().enabled = false;
        GetComponent<LHAttackController>().InitDone = false;
        GetComponent<Rigidbody2D>().Sleep();
    }

    public bool CarrySprite(EnegySprite obj)
    {
        if (_carrySprite)
        {
            return true;
        }
        else
        {
            if (obj) _carrySprite = obj;
            return false;
        }
    }

    public EnegySprite GetEnegySprite()
    {
        return _carrySprite;
    }

}
