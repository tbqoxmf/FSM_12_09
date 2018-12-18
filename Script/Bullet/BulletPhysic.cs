using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RigidBody를 대신하여 중력 물리를 구현
public class BulletPhysic : MonoBehaviour {

    float GA = 9.81f;
    Vector3 gravite = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    private void Start()
    {
        gravite = Vector3.down * GA;
    }

    private void FixedUpdate()
    {
        velocity = velocity + (gravite * Time.fixedDeltaTime);
        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    // time초 뒤에 총알의 Velocity 계산
    public Vector3 GetNextVel(Vector3 currentVel, float time)
    {
        Vector3 nextVel = currentVel + (gravite * time);
        return nextVel;
    }

    // time초 뒤에 총알의 Position 계산
    public Vector3 GetNextPos(Vector3 currentPos, Vector3 currentVel, float time)
    {
        Vector3 pos = currentPos;
        pos = new Vector3(pos.x + (currentVel.x * time), pos.y +((currentVel.y * time) - (GA * time * time / 2)), pos.z + (currentVel.z * time));
        return pos;
    }
}
