using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//총알 생성시 Pool에 저장과 폐기 총알을 다시 Pool로 되돌림
public class BulletCtrl : NetworkBehaviour
{
    CartridgeManager cartridgeManager;
    bool endAddList = false;

    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<NetworkIdentity>().hasAuthority)
        {
            GetComponent<PoolBullet>().SetBulletInactive();
            cartridgeManager.AddToAbleList(gameObject);   
            GetComponent<SyncTransform>().SendInactive();
        }   
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        while (!endAddList)
        {
            if (cartridgeManager == null)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject p in players)
                {
                    if (p.GetComponent<NetworkIdentity>().isLocalPlayer)
                    {
                        cartridgeManager = p.GetComponent<CartridgeManager>();
                    }
                }
            }
            else if (GetComponent<NetworkIdentity>().hasAuthority)
            {
                cartridgeManager.AddToAbleList(gameObject);
                cartridgeManager.AddToBulletList(gameObject);
                endAddList = true;
            }
        }

    }

}
