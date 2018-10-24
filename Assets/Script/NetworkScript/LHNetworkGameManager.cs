using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LHNetworkGameManager : NetworkBehaviour
{
    static public List<LHNetworkPlayer> sPlayer = new List<LHNetworkPlayer>();
    static public LHNetworkGameManager sInstance = null;
    public LHNetworkPlayer localPlayer;

    protected bool _running = true;
    CharacterConfig charListObj;
    public UISelectChar ui_select;

    bool hasLocalPlayer = false;
    void Awake()
    {
        sInstance = this;
        charListObj = GetComponent<CharacterConfig>();
    }

    void Start()
    {
        if (isServer)
        {
            StartCoroutine("TriggerSpawnCoroutine");
        }

        for (int i = 0; i < sPlayer.Count; ++i)
        {
            // sPlayer[i].Init();
        }
        ui_select.gameObject.SetActive(true);

    }

    [ServerCallback]
    void Update()
    {
        if (!hasLocalPlayer) return;
        if (!_running || sPlayer.Count == 0)
            return;

        //返回前厅(站位)
        if (false)
        {
            StartCoroutine(ReturnToLoby());
        }
    }

    public void Init(LHNetworkPlayer player)
    {
        localPlayer = player;
        ui_select.Init(charListObj, localPlayer);
        hasLocalPlayer = true;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

    }

    IEnumerator ReturnToLoby()
    {
        _running = false;
        yield return new WaitForSeconds(3.0f);
        LHLobbyManager.s_Singleton.ServerReturnToLobby();
    }

    //协同随机生成机关
    IEnumerator TriggerSpawnCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
    }
}
