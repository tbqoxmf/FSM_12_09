using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace MsgHandler
{
    public static class NetworkMessageHandler
    {

        public static short MyNewMsg = MsgType.Highest + 1;
        public class MovementMessage : MessageBase
        {
            public string ObjectID;
            public Vector3 position;
            public Vector3 velocity;
            public int RTT;
        }

        public static short MyNewMsg2 = MsgType.Highest + 2;
        public class InactiveMessage : MessageBase
        {
            public string ObjectID;
        }

        public static short MyNewMsg3 = MsgType.Highest + 3;
        public class ChangeHPMessage : MessageBase
        {
            public string ObjectID;
            public int HP;
        }

    }
}