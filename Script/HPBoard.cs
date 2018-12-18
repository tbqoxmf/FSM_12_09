using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HP바를 표시하고 정렬
public class HPBoard : MonoBehaviour {

    GameObject cam;
    Slider HpBar;
	// Use this for initialization
	void Start () {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        HpBar = transform.GetChild(0).GetComponent<Slider>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.root.position;
        pos.y += 0.7f;
        transform.position = pos;
        transform.LookAt(cam.transform.position);
	}

    public void SetHp(int hp)
    {
        HpBar.value = hp;
    }
}
