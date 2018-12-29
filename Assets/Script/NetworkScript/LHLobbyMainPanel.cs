using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.UI;

public class LHLobbyMainPanel : MonoBehaviour
{
    public LHLobbyManager lobbyManager;
    public RectTransform SignUpPanel;
    public RectTransform JoinPanel;//加入游戏面板
    public Text playerName;

    public void OnEnable()
    {
        lobbyManager.topPanel.ToggleVisibility(true);
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }

    /// <summary>
    /// 点击创建房间
    /// </summary>
    public void OnClickHost()
    {
        lobbyManager.StartHost();
    }

    public void OnClickChangeName()
    {
        lobbyManager.ChangeTo(SignUpPanel);
        lobbyManager.backDelegate = lobbyManager.ChangetoMainMenu;
    }

    public void OnClickJoinButton()
    {
        lobbyManager.ChangeTo(JoinPanel);
        lobbyManager.UClient.InitSocket();
        lobbyManager.backDelegate = lobbyManager.ChangetoMainMenu;
        JoinPanel.GetComponent<LobbyJoinPanel>().IpInput.text = lobbyManager.networkAddress;
    }
}
