using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBomb : MonoBehaviour
{

    //基本數值
    public GameObject Bomb;
    public GameObject BombOut;
    private Transform vT;
    private float m_fTime;    
    Vector3 vf;
    private Rigidbody m_RB;
    public GameObject UsingCar;
    //CarBase carBase;

    //抓取物件
    GameObject m_Target;
    GameObject m_TargetChild;
    GameObject m_TargetParent;

    //防自炸
    bool BombOpen = false;
    Collider BombCollider;


    //計算拋物線
    public float BombPower = 200;
    public float BombAngle = 70;
    public float BombGravity = -1;
    float d_Time = 0;

    Vector3 XSpeed;
    Vector3 GritySpeed = Vector3.zero;
    Vector3 CurrentAngle;
    Vector3 StartFoward;
    Vector3 StartSpeed;

    //特效
    ParticleSystem BombFire;
    ParticleSystem ExplosFire;
    MeshRenderer BombMesh;
    AudioSource BombFuse;
    AudioSource ExplosionSound;


    void Start()
    {
        //啟動設定
        m_RB = GetComponent<Rigidbody>();
        m_fTime = 0.0f;
        BombCollider = this.gameObject.GetComponent<Collider>();
        BombCollider.enabled = false;
        
        //拋物線數據設定
        XSpeed = Quaternion.Euler(new Vector3(0, BombAngle, 0)) * Vector3.up * BombPower;
        CurrentAngle = Vector3.zero;
        StartFoward = this.transform.forward;

        BombFire = gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        BombFire.Play();
        ExplosFire = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        BombFuse = this.gameObject.GetComponent<AudioSource>();
        BombFuse.Play();
        ExplosionSound = gameObject.transform.GetChild(1).GetComponent<AudioSource>();
    }

    private void Update()
    {
        //計算拋物線
        GritySpeed.y = BombGravity * (m_fTime += Time.fixedDeltaTime);
        transform.localPosition += (XSpeed + GritySpeed) * Time.fixedDeltaTime;
        CurrentAngle.y = Mathf.Atan((XSpeed.y + GritySpeed.y) / XSpeed.y) * Mathf.Rad2Deg;
        transform.eulerAngles = CurrentAngle;
        m_RB.velocity = (StartFoward) * 90.0f;

        m_fTime += Time.deltaTime;

        //防止自炸
        if (m_fTime > 1.0f)
        {
            BombOpen = true;
            BombCollider.enabled = true;
        }

        //爆炸時限
        if (m_fTime > 5.0f)
        {
            //pool.Recovery(this.gameObject);
            this.gameObject.SetActive(false);
        }
        
    }

    public void BombOn(GameObject TheCarObject)
    {
        UsingCar = TheCarObject;
    }



    //撞到車輛判定消失
    public void OnTriggerEnter(Collider other)
    {

        if (BombOpen == true)
        {
            if (other.tag == "AI" || other.tag == "Kart")
            {
                
                this.gameObject.SetActive(false);

            }
            else if (other.tag == "SceneObject" || other.tag == "track" || other.tag == "grass")
            {
                //炸到場景
                
                ExplosFire.Play();
                ExplosionSound.Play();
                BombMesh = this.gameObject.GetComponent<MeshRenderer>();
                BombMesh.enabled = false;
                Invoke("LateDown", 0.8f);
                //this.gameObject.SetActive(false);
                //Debug.Log("ShootSceneObject");
            }
        }

        

        
    }

    void LateDown()
    {
        //Animator animation = m_TargetChild.gameObject.GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }


}
