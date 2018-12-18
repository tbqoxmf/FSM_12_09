using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkIDManager;
using MsgHandler;

// 총알의 위치 동기화, 캐릭터의 HP를 동기화 하기위핸 메세지 등록
public class SyncManager : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        RegisterNetworkMessages();
    }

    public void RegisterNetworkMessages()
    {
        if (NetworkServer.active)
        {
            NetworkServer.RegisterHandler(NetworkMessageHandler.MyNewMsg, OnReceiveSyncMsg);
            NetworkServer.RegisterHandler(NetworkMessageHandler.MyNewMsg2, OnReceiveInactiveMsg);
            NetworkServer.RegisterHandler(NetworkMessageHandler.MyNewMsg3, OnReceiveHPMsg);
        }
        else
        {
            NetworkLobbyManager.singleton.client.RegisterHandler(NetworkMessageHandler.MyNewMsg, OnReceiveSyncMsg);
            NetworkLobbyManager.singleton.client.RegisterHandler(NetworkMessageHandler.MyNewMsg2, OnReceiveInactiveMsg);
            NetworkLobbyManager.singleton.client.RegisterHandler(NetworkMessageHandler.MyNewMsg3, OnReceiveHPMsg);
        }
    }

    void OnReceiveSyncMsg(NetworkMessage _msg)
    {
        NetworkMessageHandler.MovementMessage msg = _msg.ReadMessage<NetworkMessageHandler.MovementMessage>();
        if (NetworkServer.active)
        {
            NetworkServer.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<SyncTransform>().ReceiveMovementMessage(msg.position, msg.velocity, msg.RTT);
            NetworkServer.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<PoolBullet>().SetBulletActive();
            NetworkServer.SendToAll(NetworkMessageHandler.MyNewMsg, msg);
        }
        else
        {
            if (ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<NetworkIdentity>().hasAuthority) return;

            ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<SyncTransform>().ReceiveMovementMessage(msg.position, msg.velocity, msg.RTT);
            ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<PoolBullet>().SetBulletActive();

        }
    }
    void OnReceiveInactiveMsg(NetworkMessage _msg)
    {
        NetworkMessageHandler.InactiveMessage msg = _msg.ReadMessage<NetworkMessageHandler.InactiveMessage>();
        if (NetworkServer.active)
        {
            NetworkServer.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<PoolBullet>().SetBulletInactive();
            NetworkServer.SendToAll(NetworkMessageHandler.MyNewMsg2, msg);
        }
        else
        {
            if (!ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<NetworkIdentity>().hasAuthority)
            {
                ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<PoolBullet>().SetBulletInactive();
            }
        }
    }
    void OnReceiveHPMsg(NetworkMessage _msg)
    {
        NetworkMessageHandler.ChangeHPMessage msg = _msg.ReadMessage<NetworkMessageHandler.ChangeHPMessage>();
        ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<SphereScript>().HP = msg.HP;
        ClientScene.FindLocalObject(Manager.Instance.GetNetIDFromConnectedNetID(msg.ObjectID)).GetComponent<SphereScript>().UpdateHp();

    }
}
