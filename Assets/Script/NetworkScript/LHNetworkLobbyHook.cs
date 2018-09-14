using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class LHNetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LHLobbyPlayer lobby = lobbyPlayer.GetComponent<LHLobbyPlayer>();
        LHNetworkPlayer player = gamePlayer.GetComponent<LHNetworkPlayer>();

        player.name = lobby.name;
        player.playerName = lobby.playerName;
        player.team = lobby.playerTeam;
        //player.charactor = lobby.playerCharactor;
    }
}
