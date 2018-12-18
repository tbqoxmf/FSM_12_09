using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//총알이 스폰될때 필요한 핸들러 등록
public class SpawnManager : MonoBehaviour {

    public GameObject Bullet;

    GameObject ClientSpawnHandler(Vector3 position, NetworkHash128 assetId)
    {
        var go = Instantiate(Bullet);
        go.GetComponent<PoolBullet>().SetBulletInactive();
        return go;
    }

    void ClientUnSpawnHandler(GameObject spawned)
    {
        spawned.GetComponent<PoolBullet>().SetBulletInactive();
    }
    private void Awake()
    {
        ClientScene.RegisterSpawnHandler(Bullet.GetComponent<NetworkIdentity>().assetId, ClientSpawnHandler, ClientUnSpawnHandler);
    }
}
