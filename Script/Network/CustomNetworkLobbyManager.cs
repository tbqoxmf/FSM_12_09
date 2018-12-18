using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class CustomNetworkLobbyManager : NetworkLobbyManager 
{
	public override void OnLobbyStartHost()
	{
		Debug.Log("OnLobbyStartHost");
	}

	public override void OnLobbyStopHost()
	{
		Debug.Log("OnLobbyStopHost");
	}

	public override void OnLobbyStartServer()
	{
		Debug.Log("OnLobbyStartServer");
	}

	public override void OnLobbyServerConnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyServerConnect " + conn.connectionId);
	}

	public override void OnLobbyServerDisconnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyServerDisonnect " + conn.connectionId);
	}

	public override void OnLobbyServerSceneChanged(string sceneName)
	{
		Debug.Log("OnLobbyServerSceneChanged " + sceneName);
	}

	public override GameObject OnLobbyServerCreateLobbyPlayer
		(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerCreateLobbyPlayer " + 
			conn.connectionId + ":" + playerController);
		return base.OnLobbyServerCreateLobbyPlayer(conn, playerController);
	}

	public override GameObject OnLobbyServerCreateGamePlayer
		(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerCreateGamePlayer " + 
			conn.connectionId + ":" + playerController);
		return base.OnLobbyServerCreateGamePlayer(conn, playerController);
	}

	public override void OnLobbyServerPlayerRemoved
		(NetworkConnection conn, short playerController)
	{
		Debug.Log("OnLobbyServerPlayerRemoved " + 
			conn.connectionId + ":" + playerController);
	}

	public override bool OnLobbyServerSceneLoadedForPlayer
		(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		Debug.Log("OnLobbyServerSceneLoadedForPlayer " + 
			lobbyPlayer.name + ":" + gamePlayer.name);
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
	}

	public override void OnLobbyServerPlayersReady()
	{
		Debug.Log("OnLobbyServerPlayersReady");
		base.OnLobbyServerPlayersReady();
	}

	// 以下はクライアント側

	public override void OnLobbyClientEnter()
	{
		Debug.Log("OnLobbyClientEnter");
		base.OnLobbyClientEnter();
	}

	public override void OnLobbyClientExit()
	{
		Debug.Log("OnLobbyClientExit");
		base.OnLobbyClientExit();
	}

	public override void OnLobbyClientConnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyClientConnect " + conn.connectionId);
		base.OnLobbyClientConnect(conn);
	}

	public override void OnLobbyClientDisconnect(NetworkConnection conn)
	{
		Debug.Log("OnLobbyClientDisconnect " + conn.connectionId);
		base.OnLobbyClientDisconnect(conn);
	}

	public override void OnLobbyStartClient(NetworkClient client)
	{
		Debug.Log("OnLobbyStartClient");
		base.OnLobbyStartClient(client);
	}

	public override void OnLobbyStopClient()
	{
		Debug.Log("OnLobbyStopClient");
		base.OnLobbyStopClient();
	}

	public override void OnLobbyClientSceneChanged(NetworkConnection conn)
	{
		base.OnLobbyClientSceneChanged(conn);
		Debug.Log("OnLobbyClientSceneChanged " + conn.connectionId);
        if (NetworkServer.active)
        {
            GameObject npc = NetworkManager.singleton.spawnPrefabs[0];
            Vector3 spwanpoint = new Vector3(0, 0, -100);
            NetworkServer.Spawn(Instantiate(npc, spwanpoint, Quaternion.identity));
        }
	}

	public override void OnLobbyClientAddPlayerFailed()
	{
		Debug.Log("OnLobbyClientAddPlayerFailed");
	}

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("OnStartServer");
    }
    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
        Debug.Log("OnStartClient");
    }
}

