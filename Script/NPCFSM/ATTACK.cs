using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ATTACK : FSMState
{
    GameObject bullet;
    float lastShotTime;
    float fireRate = 0.1f;

    public override void BeginState()
    {
        base.BeginState();
    }

    public override void EndState()
    {
        base.EndState();
    }


    private void Update()
    {
        if (NetworkServer.active)
        {
            if ((_manager.Target.transform.position - this.transform.position).magnitude > 40)
            {
                _manager.SetState(State.CHASE);
            }

            if (Time.timeSinceLevelLoad - lastShotTime > fireRate)
            {
                bullet = StartFire();
                lastShotTime = Time.timeSinceLevelLoad;
            }

            if (bullet != null)
            {
                if (bullet.GetComponent<BulletPhysic>().velocity != Vector3.zero)
                {
                    bullet.GetComponent<SyncTransform>().SendTransform();
                    bullet = null;
                }

            }
            if (_manager.SS.HP <= 0)
            {
                _manager.SetState(State.DEAD);
            }
        }

    }

    // 플레이어의 위치를 기반으로 역탄도 계산
    float CalcShoot(Vector3 target)
    {
        Vector3 shootPos = new Vector3(this.transform.position.x, this.transform.position.y +1, this.transform.position.z);
        Vector3 _target = new Vector3(target.x, this.transform.position.y + 1, target.z);
        float x = (_target - shootPos).magnitude;
        float vel = 30.0f;
        float c = ((9.81f * x * x) / (vel * vel)) / 2;
        float s2 = (x - (Mathf.Sqrt((x * x) - (4 * c * (c + (target.y - shootPos.y)))))) / (2 * c);
        return Mathf.Rad2Deg * Mathf.Atan(s2);
    }

    GameObject StartFire()
    {
        GameObject bullet;
        Vector3 targetVec = new Vector3(_manager.Target.transform.position.x, this.transform.position.y, _manager.Target.transform.position.z);
        Vector3 a = targetVec - transform.position;
        Vector3 v = a - transform.forward;
        Quaternion ro = Quaternion.Euler(-CalcShoot(_manager.Target.transform.position), Mathf.Atan2(v.x,v.z) * Mathf.Rad2Deg, 0);
        Vector3 shootPos = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        _manager.CM.InstantiateFromPool(shootPos, Quaternion.identity, out bullet);
        bullet.GetComponent<PoolBullet>().SetBulletActive();
        bullet.GetComponent<BulletPhysic>().velocity = Vector3.zero;
        bullet.GetComponent<BulletPhysic>().velocity = ro * Vector3.forward * 30;
        return bullet;
    }
}


