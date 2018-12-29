using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LHLobbyManager : NetworkLobbyManager
{
    static short MsgKicked = MsgType.Highest + 1;
    static public LHLobbyManager s_Singleton;

    [Header("灵痕大乱斗Lobby")]
    [Tooltip("所有角色就位后的倒数")]
    public float prematchCountdown = 5.0f;

    [Space]
    //public Text statusInfo;//本机状态
    //public Text hostInfo;//主机状态
    [HideInInspector]
    public UdpClient UClient;
    [HideInInspector]
    public UdpServer UServer;

    [Space]
    protected RectTransform currentPanel;//当前页面
    public RectTransform mainMenuPanel;//连接大厅
    public RectTransform lobbyPanel;//选角色大厅
    public RectTransform SignUpPanel;//注册面板
    //public RectTransform JoinPanel;//加入游戏面板
    //public Button backButton;//返回按钮

    [Space]
    public LobbyTopPanel topPanel;
    public LobbyInfoPanel infoPanel;
    public LobbyJoinPanel joinPanel;
    public LobbyCountdownPanel countdownPanel;

    //用于在玩家退出房间时候断开连接
    [HideInInspector]
    public bool _isMatchmaking = false;
    protected bool _disconnectServer = false;

    protected LHNetworkLobbyHook _lobbyHooks;
    //protected UDPRadiate _UDP;

    /*======================================
     * 来自NetworkManager的Client numPlayers始终为0，
     * 因此我们计算（通过LHLobbyPlayer中的连接/销毁）玩家的数量，
     * 以便即使客户端知道有多少玩家。
    =======================================*/

    [HideInInspector]
    public int _playerNumber = 0;

    IPAddress ivp4;
    //目前匹配ID
    protected ulong _currentMatchID;

    private void Start()
    {

        s_Singleton = this;
        _lobbyHooks = GetComponent<LHNetworkLobbyHook>();
        // _UDP = GetComponent<UDPRadiate>();
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            SignUpPanel.gameObject.SetActive(false);
            currentPanel = mainMenuPanel;
        }
        else
        {
            currentPanel = SignUpPanel;
            SignUpPanel.gameObject.SetActive(true);
        }

        UClient = GetComponent<UdpClient>();
        UServer = GetComponent<UdpServer>();

        // _UDP.mb_StartRecvUdpData();//开始接收
        //backButton.gameObject.SetActive(false);
        GetComponent<Canvas>().enabled = true;
        SetServerInfo("离线", "获取 IP 中......");
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());   //Dns.GetHostName()获取本机名Dns.GetHostAddresses()根据本机名获取ip地址组
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ivp4 = ip;
            }
        }
    }

    /// <summary>
    /// 跳转到页面
    /// </summary>
    /// <param name="newPanel">New panel.</param>
    public void ChangeTo(RectTransform newPanel)
    {
        networkAddress = ivp4.ToString();
        if (currentPanel != null)
        {
            // currentPanel.gameObject.SetActive(false);
            if (currentPanel.GetComponent<Animator>())
                currentPanel.GetComponent<Animator>().SetTrigger("close");
            if (currentPanel == mainMenuPanel)
                currentPanel.gameObject.SetActive(false);
        }

        if (newPanel != null)
        {
            newPanel.gameObject.SetActive(false);
            newPanel.gameObject.SetActive(true);
        }

        currentPanel = newPanel;

        if (currentPanel != mainMenuPanel)
        {
            //backButton.gameObject.SetActive(true);
        }
        else
        {
            //backButton.gameObject.SetActive(false);

            SetServerInfo("离线", "获取 IP 中......");
            _isMatchmaking = false;
            UClient.SocketQuit();
            UServer.SocketQuit();
        }
    }

    private void ClosePanel()
    {
        currentPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置显示主机和客户端信息
    /// </summary>
    /// <param name="status">Status.</param>
    /// <param name="host">Host.</param>
    public void SetServerInfo(string status, string host)
    {
        //statusInfo.text = status;
        //hostInfo.text = host;
        lobbyPanel.GetComponent<LHLobbyPlayerList>().networkAddress.text = host;
    }

    /// <summary>
    /// 停止客户端回调
    /// </summary>
    public void StopClientClbk()
    {
        StopClient();

        if (_isMatchmaking)
        {
            StopMatchMaker();
        }

        ChangeTo(mainMenuPanel);
    }

    public void ChangetoMainMenu()
    {
        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// 开启房间
    /// </summary>
    public override void OnStartHost()
    {
        base.OnStartHost();
        //print("开启房间");
        ChangeTo(lobbyPanel);
        backDelegate = StopHostClbk;
        SetServerInfo("房主", "房间 IP：" + networkAddress);
        UServer.InitSocket();

    }

    //停止主机回调
    public void StopHostClbk()
    {
        //print("停止主机回调");
        if (_isMatchmaking)
        {
            matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
            _disconnectServer = true;
        }
        else
        {
            StopHost();
        }
        ChangeTo(mainMenuPanel);
    }

    /// <summary>
    /// OnStartHost后客户端连接
    /// </summary>
    /// <param name="conn">Conn.</param>
    public override void OnClientConnect(NetworkConnection conn)
    {
        //print("OnStartHost后客户端连接");
        base.OnClientConnect(conn);

        infoPanel.gameObject.SetActive(false);

        conn.RegisterHandler(MsgKicked, KickedMessageHandler);

        if (!NetworkServer.active)
        {//only to do on pure client (not self hosting client)
            ChangeTo(lobbyPanel);
            backDelegate = StopClientClbk;
            SetServerInfo("客户", "房间 IP：" + networkAddress);
        }
    }

    /// <summary>
    /// 踢出房间消息接收器
    /// </summary>
    /// <param name="netMsg">Net message.</param>
    public void KickedMessageHandler(NetworkMessage netMsg)
    {
        //print("踢出房间消息接收器");
        infoPanel.Display("被房主踢出", "关闭", null);
        netMsg.conn.Disconnect();
    }

    public void SetPlayerName(Text tex)
    {
        if (tex.text == "" || tex.text == null)
        {
            infoPanel.Display("不能为空", "知道了", null);
            return;
        }
        PlayerPrefs.SetString("PlayerName", tex.text);
        ChangeTo(mainMenuPanel);
    }

    //如果我们没有足够的玩家，我们想禁用按钮JOIN
    //但OnLobbyClientConnect未在主机玩家上调用。 所以我们重写lobbyPlayer的创建
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        //print("服务器创建Lobby玩家");
        GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

        LHLobbyPlayer newPlayer = obj.GetComponent<LHLobbyPlayer>();
        newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);

        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            LHLobbyPlayer p = lobbySlots[i] as LHLobbyPlayer;

            if (p != null)
            {
                p.RpcUpdateRemoveButton();
                p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
            }
        }

        return obj;
    }

    class KickMsg : MessageBase { }
    public void KickPlayer(NetworkConnection conn)
    {
        //print("剔出玩家");
        conn.Send(MsgKicked, new KickMsg());
    }

    //允许处理（+）按钮添加/删除玩家
    public void OnPlayersNumberModified(int count)
    {
        //print("修改玩家个数和排序");
        _playerNumber += count;

        int localPlayerCount = 0;
        foreach (PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        //print("客户端连接");
        infoPanel.Display("连接已断开", "返回", null);
        base.OnClientDisconnect(conn);
        ChangeTo(mainMenuPanel);
    }

    public override void OnClientError(NetworkConnection conn, int errorCode)
    {
        Debug.LogError("客户端错误");
        ChangeTo(mainMenuPanel);
        infoPanel.Display("客户端错误 : " + (errorCode == 6 ? "超时" : errorCode.ToString()), "关闭", null);
    }

    /// <summary>
    /// 玩家准备方法
    /// </summary>
    public override void OnLobbyServerPlayersReady()
    {
        //print("Lobby服务器玩家准确情况");
        bool allready = true;
        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null)
                allready &= lobbySlots[i].readyToBegin;
        }

        if (allready)
        {
            UServer.SocketQuit();
            StartCoroutine(ServerCountdownCoroutine());
            // infoPanel.Display("注意！有一支队伍的人数为空，确定这样进入游戏吗？", "取消", () => { this.ServerChangeScene(playScene); });
        }
    }

    public IEnumerator ServerCountdownCoroutine()
    {
        //print("服务器倒数开始");
        float remainingTime = prematchCountdown;
        int floorTime = Mathf.FloorToInt(remainingTime);

        while (remainingTime > 0)
        {
            yield return null;

            remainingTime -= Time.deltaTime;
            int newFloorTime = Mathf.FloorToInt(remainingTime);

            if (newFloorTime != floorTime)
            {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                floorTime = newFloorTime;

                for (int i = 0; i < lobbySlots.Length; ++i)
                {
                    if (lobbySlots[i] != null)
                    {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                        (lobbySlots[i] as LHLobbyPlayer).RpcUpdateCountdown(floorTime);
                    }
                }
            }
        }

        for (int i = 0; i < lobbySlots.Length; ++i)
        {
            if (lobbySlots[i] != null)
            {
                (lobbySlots[i] as LHLobbyPlayer).RpcUpdateCountdown(0);
            }
        }

        ServerChangeScene(playScene);
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //print("将大厅玩家的状态数据应用到游戏玩家");
        //该挂钩允许您将大厅玩家的状态数据应用到游戏玩家
        //只是子类“LobbyHook”并将其添加到大厅对象。

        if (_lobbyHooks)
            _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

        return true;
    }

    /// <summary>
    /// 前厅客户端场景变化
    /// </summary>
    /// <param name="conn">Conn.</param>
    public override void OnLobbyClientSceneChanged(NetworkConnection conn)
    {
        //print("检测到场景变化");
        if (SceneManager.GetSceneAt(0).name == lobbyScene)
        {
            if (topPanel.isInGame)
            {
                ChangeTo(lobbyPanel);
                if (_isMatchmaking)
                {
                    if (conn.playerControllers[0].unetView.isServer)
                    {
                        backDelegate = StopHostClbk;
                    }
                    else
                    {
                        backDelegate = StopClientClbk;
                    }
                }
                else
                {
                    if (conn.playerControllers[0].unetView.isClient)
                    {
                        backDelegate = StopHostClbk;
                    }
                    else
                    {
                        backDelegate = StopClientClbk;
                    }
                }
            }
            else
            {
                ChangeTo(mainMenuPanel);
            }

            topPanel.ToggleVisibility(true);
            topPanel.isInGame = false;
        }
        else
        {
            ChangeTo(null);
            Destroy(GameObject.Find("MainMenuUI(Clone)"));
            //backDelegate = StopGameClbk;
            topPanel.isInGame = true;
            topPanel.ToggleVisibility(false);
        }
    }


    /// <summary>
    /// 显示连接面板
    /// </summary>
    public void DisplayIsConnecting()
    {
        UClient.SocketQuit();
        var _this = this;
        infoPanel.Display("等待房间响应...", "取消", () => { _this.backDelegate(); });
    }

    //===============返回按钮==================
    public delegate void BackButtonDelegate();
    public BackButtonDelegate backDelegate;
    public void GoBackButton()
    {
        backDelegate();
        topPanel.isInGame = false;
    }
    //===============返回按钮==================
}