using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMissile : MonoBehaviour
{
    //生成物件
    public GameObject MissileBomb;
    public GameObject MissileOut;
    //移動&一定時間後消失
    private Rigidbody m_RB;
    private Transform vT;
    private float m_fTime;
    Vector3 vf;

    //防止自炸
    bool MissileOpen = false;
    //追蹤目標
    //public GameObject MissileTarget;
    public GameObject Enemy = null;
    public bool StopFind = false;
    GameObject[] gos;
    public GameObject closest;
    Collider missionCol;
    Vector3 v_distance;
    float m_distance;

    public RankManager rankManager;
    public RankDetect rankDetect;
    int checkPoint;  //查找目標點rankManager.checkPointList[checkPoint]
    GameObject WayPoint;  //目標checkPoint
    ParticleSystem Smoke;
    AudioSource MissileSound;

    public GameObject UsingCar;

    void Start ()
    {
        //查找發射飛彈的車輛目前的checkPoint


        //煙霧特效
        Smoke = gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        Smoke.Play();
        MissileSound = this.gameObject.GetComponent<AudioSource>();
        MissileSound.Play();
        //數值初始化
        missionCol = this.GetComponent<BoxCollider>();
        m_fTime = 0.0f;
        missionCol.enabled = false;
        //如果有找到名次比自己前面的敵人便追蹤

    }

    void FixedUpdate ()
    {
        if (StopFind == true)
        {
            rankDetect = UsingCar.GetComponent<RankDetect>();
            rankManager = GameObject.Find("Main").GetComponent<RankManager>();
            if (rankDetect.checkPoint == rankManager.checkPointList.Length) checkPoint = 0;
            else checkPoint = rankDetect.checkPoint;
            if (FindEnemy() != null) Enemy = FindEnemy();
            StopFind = false;
        }
        m_fTime += Time.deltaTime;
        PathTracking();  //使炸彈與地面保持一定距離，偵測地面法向量決定飛彈旋轉
        //設定飛彈速度
        this.transform.position += this.transform.forward * 120.0f * Time.deltaTime;
       
        //決定目標與目標更換判斷
        WayPoint = rankManager.checkPointList[checkPoint];
        float distance = (this.transform.position - rankManager.checkPointList[checkPoint].transform.position).magnitude;
        if (distance < 20)
        {
            checkPoint += 1;
            if (checkPoint+1 == rankManager.checkPointList.Length) checkPoint = 0;
        }

        Quaternion targetRotation = Quaternion.LookRotation(WayPoint.transform.position - transform.position);

        //尋找並追蹤前方敵人
        if (Enemy != null)
        {
            m_distance = Vector3.Distance(this.transform.position, Enemy.transform.position);
            if(m_distance > 100)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
            }
            else if (m_distance <= 100)
            {
                this.transform.LookAt(Enemy.transform);
            }
        }
       else  transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

        //防止自炸
        if (m_fTime > 0.3f)
        {
            MissileOpen = true;
            missionCol.enabled = true;
        }

        //超過五秒消失
        if (m_fTime > 5.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

    GameObject FindEnemy()
    {
        rankDetect = UsingCar.GetComponent<RankDetect>();

        int thisRank = rankDetect.rank;
        int targetRank = thisRank - 1;

        for (int i = 0; i < rankManager.cars.Length; i++)
        {
            if (rankManager.ranks[i] == targetRank)
                closest = rankManager.cars[i];
        }
        return closest;
    }

    void PathTracking()
    {
        RaycastHit hit;
       if(Physics.Raycast(this.transform.position + this.transform.forward, Vector3.down, out hit, 5, 1 << 16 | 1 << 8))
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, hit.point.y + 1f, this.transform.position.z), 0.5f);
            Quaternion _newRot = Quaternion.LookRotation(Vector3.Cross(this.transform.right, hit.normal).normalized, hit.normal);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _newRot, 0.5f);
        }
    }

    //撞到車輛判定消失
    public void OnTriggerEnter(Collider other)
    {
        if(MissileOpen == true)
        {
            if (other.tag == "AI" || other.tag == "Kart")
            {
                    this.gameObject.SetActive(false);
                    //Debug.Log("ShootEnemy");
            }

            if (other.tag == "SceneObject")
            {
                this.gameObject.SetActive(false);
                //Debug.Log("ShootSceneObject");
            }
        }
    }

    public void MissileOn(GameObject TheCarObject)
    {
        UsingCar = TheCarObject;
        StopFind = true;
    }
}
