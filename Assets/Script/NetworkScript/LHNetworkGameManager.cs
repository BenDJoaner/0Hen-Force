using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LHNetworkGameManager : NetworkBehaviour
{
    static public List<LHNetworkPlayer> sPlayer = new List<LHNetworkPlayer>();
    static public LHNetworkGameManager sInstance = null;
    public LHNetworkPlayer localPlayer;

    protected bool _running = true;
    CharacterConfig charListObj;
    public UISelectChar ui_select;
    public Slider team_1_Slider;
    public Slider team_2_Slider;
    public Text countDownText;
    public Transform Team_1_Pice;
    public Transform Team_2_Pice;
    GameObject[] pice_1_Arr;
    GameObject[] pice_2_Arr;
    bool hasLocalPlayer = false;
    public GameMode currentStep = (GameMode)1;

    public enum GameMode
    {
        step_1 = 1,
        step_2 = 2,
    }

    float countDown = 1;
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
        //返回前厅
        if (!_running || sPlayer.Count == 0)
        {
            StartCoroutine(ReturnToLoby());
        }

        if (currentStep == (GameMode)2)
        {

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
        Debug.Log("ReturnToLoby");
        _running = false;
        yield return new WaitForSeconds(3.0f);
        LHLobbyManager.s_Singleton.ServerReturnToLobby();
    }

    //协同随机生成机关
    IEnumerator TriggerSpawnCoroutine()
    {
        while (_running && currentStep == (GameMode)1)
        {
            Debug.Log("TriggerSpawnCoroutine");
            yield return new WaitForSeconds(Random.Range(1, 3));
        }
    }
}
