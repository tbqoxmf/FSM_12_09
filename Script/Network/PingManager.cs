using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Ping을 표시하기 위한 클레스
public class PingManager : NetworkBehaviour 
{
    public static int ping;
    public Text textRTT;
	void Start() {
        textRTT = GameObject.FindGameObjectWithTag("PingText").GetComponent<Text>();
	}

	void Update()
	{
        if(isLocalPlayer)
        {
            ping = NetworkManager.singleton.client.GetRTT();
            textRTT.text = ping.ToString() + "ms";
        }
	}

}

