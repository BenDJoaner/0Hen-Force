using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
//using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(LHPlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
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
    PlayerCanvaFollow infoCanva;
    LHPlayerController controller;

    protected bool _wasInit = false;

    void Awake()
    {
        //在游戏管理器中注册，这将允许循环。
        LHNetworkGameManager.sPlayer.Add(this);
        controller = GetComponent<LHPlayerController>();
    }

    private void Start()
    {
        manager = LHNetworkGameManager.sInstance;

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
            Destroy(GetComponent<CircleCollider2D>());
        }
    }

    public void Init(CharacterData data)
    {
        if (_wasInit || data == null)
            return;
        //生成玩家对象
        Instantiate(data.gameObject, transform.position, Quaternion.identity).transform.SetParent(gameObject.transform, true);       //生成Char
        //传送数据
        controller.DataInit(data);
        _wasInit = true;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (!_wasInit) return;

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
            }
        }
    }
}
