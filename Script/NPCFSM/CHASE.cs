using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CHASE : FSMState
{

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
            if ((_manager.Target.transform.position - this.transform.position).magnitude < 40)
            {
                _manager.SetState(State.ATTACK);
            }
            _manager.Rigi.AddForce(_manager.NextVec.normalized * 10);
            if(_manager.SS.HP <= 0)
            {
                _manager.SetState(State.DEAD);
            }
        }

    }
}
