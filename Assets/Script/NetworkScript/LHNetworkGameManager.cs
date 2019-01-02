using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LHNetworkGameManager : NetworkBehaviour
{
    static public List<LHNetworkPlayer> sPlayer = new List<LHNetworkPlayer>();
    // static public LHNetworkGameManager sInstance = null;
    public LHNetworkPlayer localPlayer;
    public UISelectChar ui_select;
    public UIOnlineMain ui_main;
    public BronTransConfig bronPoint;
    Common.GameMode currentStep = (Common.GameMode)1;

    List<EnegySprite> SpritArr = new List<EnegySprite>();
    bool _running = true;
    bool hasLocalPlayer = false;

    CharacterConfig charListObj;

    public float team_1_Num;
    public float team_2_Num;
    public float countDown = 0;

    void Awake()
    {
        // sInstance = this;
        charListObj = GetComponent<CharacterConfig>();
    }

    void Start()
    {
        if (isServer)
        {
            StartCoroutine("TriggerSpawnCoroutine");
        }

        // for (int i = 0; i < sPlayer.Count; ++i)
        // {
        //     // sPlayer[i].Init();
        // }
        ui_select.gameObject.SetActive(true);
        ui_main.OnModeChange(1);
    }

    [ServerCallback]
    void Update()
    {
        if (!hasLocalPlayer) return;
        //返回前厅
        if (!_running)
        {
            StartCoroutine(ReturnToLoby());
        }
        else
        {
            if ((team_2_Num >= Common.Step_1_Sum || team_1_Num >= Common.Step_1_Sum) && currentStep == (Common.GameMode)1)
            {
                currentStep = (Common.GameMode)2;
                DestroyAllSprite();
                ui_main.OnModeChange(2);
                countDown = Common.timeList[2];

                /*用于测试的数据： */
                team_1_Num = 6;
                team_2_Num = 6;
            }
            if (currentStep == (Common.GameMode)2)
            {
                if (countDown > 0)
                {
                    countDown -= Time.deltaTime;
                    ui_main.SetTime(countDown);
                    if (team_1_Num <= 0 || team_2_Num <= 0)
                    {
                        _running = false;
                    }
                    else
                    {
                        team_1_Num -= Time.deltaTime * Common.decTime_1;
                        team_2_Num -= Time.deltaTime * Common.decTime_1;
                    }
                    ui_main.OnAddEnegy(1, team_1_Num);
                    ui_main.OnAddEnegy(2, team_2_Num);
                }
                else
                {

                }

            }
        }

    }

    void BattleResult()
    {

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

    void DestroyAllSprite()
    {
        foreach (EnegySprite _sprite in SpritArr)
        {
            GameObject.Destroy(_sprite.gameObject);
        }
    }

    public void OnUseEnegy(LHNetworkPlayer player)
    {
        int teamIndex = player.team;
        switch (teamIndex)
        {
            case 1:
                team_1_Num += player.GetEnegySprite().EnenyNum;
                ui_main.OnAddEnegy(teamIndex, team_1_Num);
                break;
            case 2:
                team_2_Num += player.GetEnegySprite().EnenyNum;
                ui_main.OnAddEnegy(teamIndex, team_2_Num);
                break;
        }
        SpritArr.Remove(player.GetEnegySprite());
    }

    IEnumerator ReturnToLoby()
    {
        // Debug.Log("ReturnToLoby");
        _running = false;
        yield return new WaitForSeconds(10.0f);
        LHLobbyManager.s_Singleton.ServerReturnToLobby();
    }

    //协同随机生成机关
    IEnumerator TriggerSpawnCoroutine()
    {
        while (_running && currentStep == (Common.GameMode)1)
        {
            if (SpritArr.Count < 5)
            {
                // print("生成碎片，目前总数：" + SpritArr.Count);
                Vector2 pos = bronPoint.GetRandomTrans().position;
                GameObject _sprite = Instantiate(AssetConfig.GetPrefabByName("sprite"), pos, Quaternion.identity);
                SpritArr.Add(_sprite.GetComponent<EnegySprite>());
            }
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }
}
