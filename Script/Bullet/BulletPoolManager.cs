using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletPoolManager : NetworkBehaviour {
    
    public GameObject Bullet;
    public int poolSize = 2;

    public List<GameObject> BulletList;
    List<GameObject> AbleList;

    public bool AutoResize = true;

    GameObject ClientSpawnHandler(Vector3 position, NetworkHash128 assetId)
    {
        var go = InstantiatePrefab();
        return go;
    }

    void ClientUnSpawnHandler(GameObject spawned)
    {
        spawned.GetComponent<PoolBullet>().SetBulletInactive();
    }

    void Awake() {
        
        ClientScene.RegisterSpawnHandler(Bullet.GetComponent<NetworkIdentity>().assetId, ClientSpawnHandler, ClientUnSpawnHandler);
    }

    void Start() {
        if (NetworkServer.active)
        {
            BulletList = new List<GameObject>();
            AbleList = new List<GameObject>();

            for(int i = 0; i<poolSize; i++)
            {
                BulletList.Add(InstantiatePrefab());
                AbleList.Add(BulletList[BulletList.Count - 1]);
            }
        }
    }

    GameObject InstantiatePrefab()
    {
        GameObject obj = Instantiate(Bullet, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        PoolBullet nhobj = obj.AddComponent<PoolBullet>();
        nhobj.SetBulletInactive();
        if (isServer)
            NetworkServer.Spawn(obj);

        return obj;
    }

    public bool InstantiateFromPool(Vector3 pos, Quaternion rot, out GameObject obj)
    {
        if (!isServer) { obj = null; return false; }

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
            obj = AbleList[lastIndex];
            AbleList.RemoveAt(lastIndex);
            return true;
        }

        if (AutoResize)
        {
            GameObject g = InstantiatePrefab();
            g.transform.position = pos;
            g.transform.rotation = rot;
            BulletList.Add(g);
            obj = g;
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
        if (!isServer) return;

        AbleList.Add(obj);
    }

}
