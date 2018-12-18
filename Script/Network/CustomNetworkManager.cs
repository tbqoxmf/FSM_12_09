using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager 
{
	public NetworkDiscovery discovery;

	void Start() { Debug.Log("CustomNetworkManager start."); }

    // NetworkDsicovery
    public void OnReceivedBroadcast(string address, string msg)
	{
		Debug.Log("OnReceivedBroadcast " + address + ":" + msg);
	}


	public override NetworkClient StartHost()
	{
		Debug.Log("StartHost");
		return base.StartHost();
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		Debug.Log ("OnServerConnect " + conn.connectionId);
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		Debug.Log("OnClientConnect " + conn.connectionId);
	}

	public override void OnClientDisconnect(NetworkConnection conn)
	{
		base.OnClientDisconnect(conn);
		Debug.Log("OnClientDisconnect " + conn.connectionId);
	}

	public override void OnStartClient(NetworkClient client)
	{
		base.OnStartClient(client);
		Debug.Log("OnStartClient");
        
	}


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("OnServerAddPlayer " + conn.connectionId + ":" + playerControllerId);
		base.OnServerAddPlayer(conn, playerControllerId);
	}

	public override void OnServerSceneChanged(string sceneName)
	{
		Debug.Log("OnServerSceneChanged " + sceneName);
		base.OnServerSceneChanged(sceneName);
	}

	public override void OnClientSceneChanged(NetworkConnection conn)
	{
		Debug.Log("OnClientSceneChanged " + conn.connectionId);
		base.OnClientSceneChanged(conn);
	}
}
