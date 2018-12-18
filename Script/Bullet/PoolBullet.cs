using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//총알이 Pool에서 In,Out할때 필요한 함수 정리
public class PoolBullet : NetworkBehaviour
{
    public void SetBulletActive()
    {
        DoSetBulletActive();
    }

    void DoSetBulletActive()
    {
        Component[] comps = GetComponents<Component>();

        for (int i = 0; i < comps.Length; i++)
        {
            if (comps[i] != this && comps[i].GetType() != typeof(NetworkIdentity) && comps[i].GetType() != typeof(BulletCtrl))
            {
                if (comps[i].GetType().IsSubclassOf(typeof(MonoBehaviour)))
                    ((MonoBehaviour)comps[i]).enabled = true;

                if (comps[i].GetType().IsSubclassOf(typeof(Collider)))
                    ((Collider)comps[i]).enabled = true;

                if (comps[i].GetType().IsSubclassOf(typeof(Renderer)))
                    ((Renderer)comps[i]).enabled = true;
            }
        }

    }
    public void SetBulletInactive()
    {
        DoSetBulletInactive();
    }

    void DoSetBulletInactive()
    {
        Component[] comps = GetComponents<Component>();

        for (int i = 0; i < comps.Length; i++)
        {
            if (comps[i] != this && comps[i].GetType() != typeof(NetworkIdentity) && 
                comps[i].GetType() != typeof(BulletCtrl) && comps[i].GetType() != typeof(SyncTransform))
            {
                if (comps[i].GetType().IsSubclassOf(typeof(MonoBehaviour)))
                    ((MonoBehaviour)comps[i]).enabled = false;

                if (comps[i].GetType().IsSubclassOf(typeof(Collider)))
                    ((Collider)comps[i]).enabled = false;

                if (comps[i].GetType().IsSubclassOf(typeof(Renderer)))
                    ((Renderer)comps[i]).enabled = false;
            }
        }
    }

}
