using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using NetworkIDManager;
using MsgHandler;

// HP를 동기화하거나 위치를 동기화시 필요한 함수 정리(Send, Receive)
public class SyncTransform : NetworkBehaviour
{
    public string objectID;

    // Use this for initialization
    void Start()
    {
        objectID = transform.name + GetComponent<NetworkIdentity>().netId.ToString();
        transform.name = objectID;
        Manager.Instance.AddNetIdToConnectedNetID(objectID, GetComponent<NetworkIdentity>().netId);
    }


    public void ReceiveMovementMessage(Vector3 position, Vector3 velocity, int RTT)
    {
        if (isServer)
        {       
            this.transform.position = GetComponent<BulletPhysic>().GetNextPos(position, velocity, RTT*0.001f/2);
            this.GetComponent<BulletPhysic>().velocity = this.GetComponent<BulletPhysic>().GetNextVel(velocity, RTT * 0.001f / 2);
        }
        else
        {
            this.transform.position = GetComponent<BulletPhysic>().GetNextPos(position, velocity, (NetworkManager.singleton.client.GetRTT() + RTT) * 0.001f / 2);
            this.GetComponent<BulletPhysic>().velocity = this.GetComponent<BulletPhysic>().GetNextVel(velocity, (NetworkManager.singleton.client.GetRTT() + RTT) * 0.001f / 2);
        }
    }

    public void SendTransform()
    {
        
        if (isServer)
        {
            NetworkMessageHandler.MovementMessage msg = new NetworkMessageHandler.MovementMessage()
            {
                ObjectID = objectID,
                position = this.transform.position,
                velocity = this.GetComponent<BulletPhysic>().velocity,
                RTT = 0
            };
            NetworkServer.SendToAll(NetworkMessageHandler.MyNewMsg, msg);
        }

        else
        {
            NetworkMessageHandler.MovementMessage msg = new NetworkMessageHandler.MovementMessage()
            {
                ObjectID = objectID,
                position = this.transform.position,
                velocity = this.GetComponent<BulletPhysic>().velocity,
                RTT = NetworkManager.singleton.client.GetRTT()
            };
            NetworkLobbyManager.singleton.client.Send(NetworkMessageHandler.MyNewMsg, msg);
        }
    }

    public void SendInactive()
    {
        NetworkMessageHandler.InactiveMessage msg = new NetworkMessageHandler.InactiveMessage()
        {
            ObjectID = objectID
        };
        if (isServer)
            NetworkServer.SendToAll(NetworkMessageHandler.MyNewMsg2, msg);
        else
            NetworkLobbyManager.singleton.client.Send(NetworkMessageHandler.MyNewMsg2, msg);
    }

    public void SendHP()
    {
        NetworkMessageHandler.ChangeHPMessage msg = new NetworkMessageHandler.ChangeHPMessage()
        {
            ObjectID = objectID,
            HP = GetComponent<SphereScript>().HP
        };
        if (isServer)
            NetworkServer.SendToAll(NetworkMessageHandler.MyNewMsg3, msg);
        else
            NetworkLobbyManager.singleton.client.Send(NetworkMessageHandler.MyNewMsg3, msg);

    }
}

    
