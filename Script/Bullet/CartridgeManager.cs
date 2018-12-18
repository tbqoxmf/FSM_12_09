using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// 총알의 Pool을 초기화, 관리
public class CartridgeManager : NetworkBehaviour {
    
    public GameObject Bullet;
    public int poolSize = 2;
    bool initList = false;


    public List<GameObject> BulletList;
    List<GameObject> AbleList;

    public bool AutoResize = true;

    void Awake()
    {
        BulletList = new List<GameObject>();
        AbleList = new List<GameObject>();
    }

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

    // Pool에서 총알을 가져오는 함수
    public bool InstantiateFromPool(Vector3 pos, Quaternion rot, out GameObject obj)
    {
        if (!isLocalPlayer) { obj = null; return false; }

        int lastIndex = AbleList.Count - 1;
        if (AbleList.Count > 0)
        {
            if (AbleList[lastIndex] == null)
            {
                Debug.LogError("AvailableObject is missing?");
                obj = null;
                return false;
            }

            AbleList[lastIndex].transform.position = pos;
            AbleList[lastIndex].transform.rotation = rot;
            AbleList[lastIndex].GetComponent<PoolBullet>().SetBulletActive();
            obj = AbleList[lastIndex];
            AbleList.RemoveAt(lastIndex);
            return true;
        }

        if (AutoResize)
        {
            CmdInstantiatePrefab();
            AbleList[lastIndex].transform.position = pos;
            AbleList[lastIndex].transform.rotation = rot;
            AbleList[lastIndex].GetComponent<PoolBullet>().SetBulletActive();
            obj = AbleList[lastIndex];
            AbleList.RemoveAt(lastIndex);
            return true;
        }
        else
        {
            obj = null;
            return false;
        }
    }

    public void AddToAbleList(GameObject obj)
    {      
        AbleList.Add(obj);
    }
    public void AddToBulletList(GameObject obj)
    {
        BulletList.Add(obj);
    }


}
