using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//List of players in the lobby
public class LHLobbyPlayerList : MonoBehaviour
{
    public static LHLobbyPlayerList _instance = null;
    public Text networkAddress;
    public RectTransform Team_1_playerListContentTransform;
    public RectTransform Team_2_playerListContentTransform;
    public Button teamChange_1;
    public Button teamChange_2;


    protected VerticalLayoutGroup _Team_1_layout;
    protected VerticalLayoutGroup _Team_2_layout;

    public List<LHLobbyPlayer> team_1 = new List<LHLobbyPlayer>();
    public List<LHLobbyPlayer> team_2 = new List<LHLobbyPlayer>();


    public void OnEnable()
    {
        _instance = this;
        _Team_1_layout = Team_1_playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        _Team_2_layout = Team_2_playerListContentTransform.GetComponent<VerticalLayoutGroup>();
    }

    /// <summary>
    /// 添加玩家，lobby玩家调用自行添加
    /// </summary>
    /// <param name="player"></param>
    public void AddPlayer(LHLobbyPlayer player)
    {
        if (team_1.Contains(player) || team_2.Contains(player))
            return;

        if (team_1.Count <= team_2.Count && team_1.Count < 4)
        {
            team_1.Add(player);
            player.transform.SetParent(Team_1_playerListContentTransform, false);
            player.teamPos = team_1.IndexOf(player);
            player.playerTeam = 1;
        }
        else
        {
            team_2.Add(player);
            player.transform.SetParent(Team_2_playerListContentTransform, false);
            player.teamPos = team_2.IndexOf(player);
            player.playerTeam = 2;
        }

        PlayerListModified();
    }

    public void PlayerListModified()
    {
        foreach (LHLobbyPlayer p in team_1)
        {
            p.OnPlayerListChanged(team_1.IndexOf(p));
        }

        foreach (LHLobbyPlayer p in team_2)
        {
            p.OnPlayerListChanged(team_2.IndexOf(p));
        }
    }

    public void RemovePlayer(int teamIdx, LHLobbyPlayer player)
    {
        switch (teamIdx)
        {
            case 1:
                team_1.Remove(player);
                break;
            case 2:
                team_2.Remove(player);
                break;
        }
        PlayerListModified();
    }

    public int OnGetTeamNumByIndex(int index){
        switch(index){
            case 2:
                return team_2.Count;
            default:
                return team_2.Count;
        }
    }

    public void TeamChange(int OriginTeam, int idx)
    {
        LHLobbyPlayer player = new LHLobbyPlayer();
        switch (OriginTeam)
        {
            case 1:
                player = team_1[idx];
                if (team_2.Contains(player) || team_2.Count > 3)
                    return;
                player.transform.SetParent(Team_2_playerListContentTransform, false);
                player.playerTeam = 2;
                team_2.Add(player);
                RemovePlayer(OriginTeam, player);//去掉原来位置上的玩家
                player.teamPos = team_2.IndexOf(player);
                break;
            case 2:
                player = team_2[idx];
                if (team_1.Contains(player) || team_1.Count > 3)
                    return;
                player.transform.SetParent(Team_1_playerListContentTransform, false);
                player.playerTeam = 1;
                team_1.Add(player);
                RemovePlayer(OriginTeam, player);//去掉原来位置上的玩家
                player.teamPos = team_1.IndexOf(player);
                break;
        }

        PlayerListModified();
    }
}
