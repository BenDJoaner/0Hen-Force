using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyJoinPanel : MonoBehaviour
{

    public LHLobbyManager lobbyManager;
    public RectTransform lobbyPanel;
    public Button JoinBtn;
    public RectTransform HostTable;
    public Button Refrash;
    public InputField IpInput;

    /// <summary>
    /// 点击加入房间
    /// </summary>
    public void OnClickJoin()
    {
        OnJoinHostServer(IpInput.text);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearBtnList()
    {
        foreach (RectTransform chiled in HostTable)
        {
            Destroy(chiled.gameObject);
        }
    }

    public void OnLoadHostList(string name, string ip)
    {
        foreach (RectTransform chiled in HostTable)
        {
            Destroy(chiled.gameObject);
        }
        //后面再做列表=======================
        GameObject iteam = AssetConfig.GetPrefabByName("HostBtnIteam");
        GameObject HostBtn = Instantiate(iteam);
        HostBtn.transform.SetParent(HostTable, true);
        HostBtn.GetComponent<UIHostItem>().Init(name, ip, this);
    }

    public void OnJoinHostServer(string ip)
    {
        lobbyManager.ChangeTo(lobbyPanel);

        lobbyManager.networkAddress = ip;
        lobbyManager.StartClient();

        lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        lobbyManager.DisplayIsConnecting();
        lobbyManager.SetServerInfo("进入房间...", "房间 IP 地址：" + lobbyManager.networkAddress);
    }
}
