using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(FSMManager))]
public class FSMState : MonoBehaviour
{

    protected FSMManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FSMManager>();
    }

    public virtual void BeginState()
    {
    }

    public virtual void EndState()
    {

    }
}