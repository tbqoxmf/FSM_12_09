using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Npc의 총알을 초기화
public class NPCCartridgeManager : NetworkBehaviour {
    
    public GameObject Bullet;
    public int poolSize = 2;
    bool initList = false;


    public bool AutoResize = true;

    void Start()
    {
        if (!isServer && isLocalPlayer)
        {
            for (int i = 0; i < poolSize; i++)
            {
                CmdInstantiatePrefab();
            }
        }
    }

    private void Update()
    {

        if (!initList && isServer && isLocalPlayer)
        {
            if (NetworkServer.connections[0].isReady)
            {
                for (int i = 0; i < poolSize; i++)
                {
                    CmdInstantiatePrefab();
                }
                initList = true;
            }

        }
        if (!initList && isServer && transform.tag == "NPC")
        {
            if (NetworkServer.connections[0].isReady)
            {
                for (int i = 0; i < poolSize; i++)
                {
                    CmdInstantiatePrefab();
                }
                initList = true;
            }
        }
    }

    [Command]
    void CmdInstantiatePrefab()
    {
        GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
        obj.GetComponent<PoolBullet>().SetBulletInactive();
        if (transform.tag == "Player")
            NetworkServer.SpawnWithClientAuthority(obj, connectionToClient);
        else
            NetworkServer.SpawnWithClientAuthority(obj, NetworkServer.connections[0]);
    }
}
