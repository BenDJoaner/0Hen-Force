using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LHLobbyPlayer : NetworkLobbyPlayer
{
    public Text myNameText;
    public Image ReadyIcon;

    public int teamPos = -1;

    public Button readyButton;

    public Image CharIcon;
    public bool lockedChar = false;

    //OnMyName函数将在服务器更改playerName的值时在客户端上调用
    [SyncVar(hook = "OnMyName")]
    public string playerName = "";
    [SyncVar(hook = "OnMyTeam")]
    public int playerTeam = 0;
    

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        if (LHLobbyManager.s_Singleton != null) LHLobbyManager.s_Singleton.OnPlayersNumberModified(1);

        LHLobbyPlayerList._instance.AddPlayer(this);
        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
        }
        //在UI上设置玩家数据。 值是SyncVar，所以玩家
        //将使用服务器上的正确值创建
        OnMyName(playerName);
        OnMyTeam(playerTeam);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //if we return from a game, color of text can still be the one for "Ready"
        // readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

        SetupLocalPlayer();
    }

    public void OnMyName(string str)
    {
        playerName = str;
        myNameText.text = playerName;
    }


    public void OnMyTeam(int other)
    {
        playerTeam = other;
        myNameText.color = other == 1 ?
            new Color(52.0f / 255.0f, 114.0f / 255.0f, 161.0f / 255.0f, 1.0f)
            : new Color(161.0f / 255.0f, 92.0f / 255.0f, 52.0f / 255.0f, 1.0f);
    }

    public void ToggleJoinButton(bool enabled)
    {
        readyButton.gameObject.SetActive(enabled);
    }

    void SetupOtherPlayer()
    {
        readyButton.transform.GetChild(0).GetComponent<Text>().text = "";
        readyButton.interactable = false;

        OnClientReady(false);
    }

    public void OnPlayerListChanged(int idx)
    {
        teamPos = idx;
    }

    public override void OnClientReady(bool readyState)
    {
        if (readyState)
        {
            Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
            // textComponent.text = "就绪";
            ReadyIcon.color = Color.green;
            readyButton.GetComponent<Image>().color = new Color(108.0f / 255.0f, 217.0f / 255.0f, 136.0f / 255.0f, 0.5f);
            readyButton.interactable = false;
        }
        else
        {
            Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
            // textComponent.text = isLocalPlayer ? "准备" : "等待...";
            ReadyIcon.color = Color.gray;
            readyButton.interactable = isLocalPlayer;
        }
    }


    [ClientRpc]
    public void RpcUpdateRemoveButton()
    {
        CheckRemoveButton();
    }

    //这根据是否是唯一的本地玩家启用/禁用删除按钮
    public void CheckRemoveButton()
    {
        if (!isLocalPlayer)
            return;

        int localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

    }

    void SetupLocalPlayer()
    {
        CheckRemoveButton();

        readyButton.transform.GetChild(0).GetComponent<Text>().text = "";
        readyButton.interactable = true;
        LHLobbyPlayerList._instance.teamChange_1.onClick.RemoveAllListeners();
        LHLobbyPlayerList._instance.teamChange_1.onClick.AddListener(OnTeamClicked);
        LHLobbyPlayerList._instance.teamChange_2.onClick.RemoveAllListeners();
        LHLobbyPlayerList._instance.teamChange_2.onClick.AddListener(OnTeamClicked);
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(OnReadyClicked);

        //当OnClientEnterLobby被调用时，Loval PlayerController尚未创建，所以我们需要重做这里来禁用
        //添加按钮，如果我们到达maxLocalPlayer。 我们传递0，因为它已经计入OnClientEnterLobby
        if (LHLobbyManager.s_Singleton != null) LHLobbyManager.s_Singleton.OnPlayersNumberModified(0);
        OnSetName();

        LHLobbyPlayerList._instance.teamChange_1.gameObject.SetActive(playerTeam == 1);
        LHLobbyPlayerList._instance.teamChange_2.gameObject.SetActive(playerTeam == 2);
    }

    public void OnSetName()
    {
        CmdSetName(PlayerPrefs.GetString("PlayerName"));
    }

    public void OnTeamClicked()
    {
        if (!isLocalPlayer) return;
        
        if(LHLobbyPlayerList._instance.OnGetTeamNumByIndex(playerTeam)>3){
            LHLobbyManager.s_Singleton.infoPanel.Display("队伍满人，无法加入", "关闭", null);
            return;
        }

        CmdTeamChange(playerTeam, teamPos);
        LHLobbyPlayerList._instance.teamChange_1.gameObject.SetActive(playerTeam == 2);
        LHLobbyPlayerList._instance.teamChange_2.gameObject.SetActive(playerTeam == 1);
    }

    public void OnReadyClicked()
    {
        lockedChar = true;
        LHLobbyPlayerList._instance.teamChange_1.gameObject.SetActive(false);
        LHLobbyPlayerList._instance.teamChange_2.gameObject.SetActive(false);
        SendReadyToBeginMessage();
    }


    public void OnRemovePlayerClick()
    {
        if (isLocalPlayer)
        {
            RemovePlayer();
        }
        else if (isServer)
            LHLobbyManager.s_Singleton.KickPlayer(connectionToClient);
    }

    [Command]
    public void CmdSetName(string str)
    {
        playerName = str;
    }

    [Command]
    public void CmdTeamChange(int team, int index)
    {
        RpcTeamChange(team, index);
    }

    [ClientRpc]
    public void RpcTeamChange(int team, int index)
    {
        LHLobbyPlayerList._instance.TeamChange(team, index);
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        LHLobbyManager.s_Singleton.countdownPanel.UIText.text = "" + countdown + "";
        LHLobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
    }

    //销毁时清理（当客户端被踢出或断开连接时）
    public void OnDestroy()
    {
        LHLobbyPlayerList._instance.RemovePlayer(playerTeam, this);
        if (LHLobbyManager.s_Singleton != null) LHLobbyManager.s_Singleton.OnPlayersNumberModified(-1);
    }
}