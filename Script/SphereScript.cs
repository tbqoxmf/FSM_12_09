using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using NetworkIDManager;

// 캐릭터의 전반적인 움직임을 담당(이동, 사격, 상태)
public class SphereScript : NetworkBehaviour
{
    public int HP;

    public GameObject bullet;
    GameObject Camera;
    Quaternion ro; 
    Rigidbody rig;
    Vector3 movement;
    public float speed;
    public float jumpPower;

    CartridgeManager cartridgeManager;
    public float fireRate = 1.0f;
    float lastShotTime;

    NetworkStartPosition[] spawnPoints;

    

    public static SphereScript ownerObj { get; private set; }

    void Awake()
    {
        if (transform.tag == "Player")
        {
            rig = GetComponent<Rigidbody>();
            Camera = GameObject.FindGameObjectWithTag("MainCamera");
            cartridgeManager = GetComponent<CartridgeManager>();
        }
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            ownerObj = this;            
            GetComponent<MeshRenderer>().material.color = Color.red;      
        }
        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        HP = 100;
    }

	void Update()
	{
        if (isLocalPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                if (Time.timeSinceLevelLoad - lastShotTime > fireRate)
                {
                    bullet = StartFire(Camera.transform.position, Camera.transform.rotation.eulerAngles.x, Camera.transform.rotation.eulerAngles.y);
                    lastShotTime = Time.timeSinceLevelLoad;
                }
            }
            if (bullet != null)
            {
                if (bullet.GetComponent<BulletPhysic>().velocity != Vector3.zero)
                {
                    bullet.GetComponent<SyncTransform>().SendTransform();
                    bullet = null;
                }
            }
            if(HP <= 0)
            {
                Respawn();
            }
        }         
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(h, v);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(Vector3.up * 10, ForceMode.Impulse);
            }
        }
    }

    void Move(float h, float v)
    {
        ro = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        movement.Set(h, 0, v);
        movement = ro * movement;
        movement = movement.normalized * speed;

        rig.AddForce(movement);
    }

    GameObject StartFire(Vector3 CameraPos, float anglesX, float anglesY)
    {
        GameObject bullet;
        ro = Quaternion.Euler(anglesX - 20, anglesY, 0);
        Vector3 shootPos = CameraPos + ((transform.position - CameraPos));
        shootPos = new Vector3(shootPos.x, shootPos.y + 1, shootPos.z);
        cartridgeManager.InstantiateFromPool(shootPos, Quaternion.identity, out bullet);
        bullet.GetComponent<PoolBullet>().SetBulletActive();
        bullet.GetComponent<BulletPhysic>().velocity = Vector3.zero;
        bullet.GetComponent<BulletPhysic>().velocity = ro * Vector3.forward * 30;
        return bullet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        HP -= 10;
        GetComponent<SyncTransform>().SendHP();
        UpdateHp();
    }

    public void UpdateHp()
    {
        transform.GetChild(0).GetComponent<HPBoard>().SetHp(HP);
    }

    public void Respawn()
    {
        Vector3 spawnPoint = Vector3.zero;
        if(spawnPoints != null && spawnPoints.Length >0)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }
        transform.position = spawnPoint;
        HP = 100;
        GetComponent<SyncTransform>().SendHP();
        UpdateHp();
    }
}

