using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//NPC AI구현을 위한 FSM 메니저
public enum State
{
    CHASE = 0,
    ATTACK,
    DEAD
}
[ExecuteInEditMode]
public class FSMManager : MonoBehaviour {

    private bool _isinit = false;
    public State startState = State.CHASE;
    Dictionary<State, FSMState> _states = new Dictionary<State, FSMState>();

    [SerializeField]
    private State _currentState;
    public State CurrentState
    {
        get
        {
            return _currentState;
        }
    }

    private CartridgeManager _cm;
    public CartridgeManager CM { get { return _cm; } }

    private SphereScript _ss;
    public SphereScript SS { get { return _ss; } }

    private MeshRenderer _renderer;
    public MeshRenderer Renderer { get { return _renderer; } }

    [SerializeField]
    private GameObject _target;
    public GameObject Target { get { return _target; } set { _target = value; } }

    private GameObject[] _player;
    public GameObject[] Player { get { return _player; } }

    private Rigidbody _rigi;
    public Rigidbody Rigi { get { return _rigi; } }

    private Vector3 _nextVec = Vector3.zero;
    public Vector3 NextVec { get { return _nextVec; } }

    private void Awake()
    {
        if (NetworkServer.active)
        {
            State[] stateValues = (State[])System.Enum.GetValues(typeof(State));
            foreach (State s in stateValues)
            {
                System.Type FSMType = System.Type.GetType(s.ToString());
                FSMState state = (FSMState)GetComponent(FSMType);
                if (null == state)
                {
                    state = (FSMState)gameObject.AddComponent(FSMType);
                }

                _states.Add(s, state);
                state.enabled = false;
            }
            _rigi = GetComponent<Rigidbody>();
            _ss = GetComponent<SphereScript>();
            _renderer = GetComponent<MeshRenderer>();
        }
    }

    public void SetState(State newState)
    {
        if (_isinit)
        {
            _states[_currentState].enabled = false;
            _states[_currentState].EndState();
        }
        _currentState = newState;
        _states[_currentState].BeginState();
        _states[_currentState].enabled = true;
    }

    private void Start()
    {
        if (NetworkServer.active)
        {
            SetState(startState);
            _isinit = true;
            _player = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject a in _player)
            {
                if (a.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    _cm = a.GetComponent<CartridgeManager>();
                }
            }
        }

    }

    private void Update()
    {
        _player = GameObject.FindGameObjectsWithTag("Player");
        if (NetworkServer.active)
        {
            Vector3 tem = Vector3.zero;
            float distance = float.MaxValue;
            foreach (GameObject a in _player)
            {
                Vector3 Vec = a.transform.position - this.transform.position;
                if (distance > Vec.magnitude)
                {
                    distance = Vec.magnitude;
                    tem = Vec;
                    _target = a;
                }
            }
            _nextVec.Set(tem.x, 0, tem.z);
        }
    }
}
