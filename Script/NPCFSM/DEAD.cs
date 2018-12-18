using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEAD : FSMState {
    float timer;

    public override void BeginState()
    { 
        base.BeginState();
        timer = Time.time;
        _manager.Renderer.enabled = false;
    }

    public override void EndState()
    {
        base.EndState();
        
        _manager.Renderer.enabled = true;
    }

    private void Update()
    {
        if(Time.time - timer > 5)
        {
            _manager.SS.Respawn();
            _manager.SetState(State.CHASE);
        }
    }
}
