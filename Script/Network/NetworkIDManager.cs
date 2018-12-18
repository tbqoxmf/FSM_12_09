using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace NetworkIDManager
{
    public class Manager 
    {
        private static Manager _instance;
        public Dictionary<string, NetworkInstanceId> ConnectedNetID { get; set; }

        public int NumberConnectedPlayers { get; private set; }

        private Manager()
        {
            if (_instance != null)
            {
                return;
            }

            ConnectedNetID = new Dictionary<string, NetworkInstanceId>();
            NumberConnectedPlayers = 0;

            _instance = this;
        }

        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                {
                    new Manager();
                }

                return _instance;
            }
        }

        public void AddNetIdToConnectedNetID(string _playerID, NetworkInstanceId netID)
        {
            if (!ConnectedNetID.ContainsKey(_playerID))
            {
                ConnectedNetID.Add(_playerID, netID);
                NumberConnectedPlayers++;
            }
        }

        public void RemoveNetIDFromConnectedNetID(string _playerID)
        {
            if (ConnectedNetID.ContainsKey(_playerID))
            {
                ConnectedNetID.Remove(_playerID);
                NumberConnectedPlayers--;
            }
        }

        public NetworkInstanceId[] GetConnectedNetID()
        {
            return ConnectedNetID.Values.ToArray();
        }

        public NetworkInstanceId GetNetIDFromConnectedNetID(string _playerID)
        {
            if (ConnectedNetID.ContainsKey(_playerID))
            {
                return ConnectedNetID[_playerID];
            }

            return NetworkInstanceId.Invalid;
        }       
    }
}