using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOrigin : MonoBehaviour {

    //對應prefeb
    public GameObject[] Weapons;
    //public GameObject TrapItem;
    public GameObject OilObject;
    public GameObject Bomb;
    public GameObject Sheild;
    public GameObject MissileBomb;
    public GameObject TNTBomb;
    //生成點
    public GameObject TrapOut;
    public GameObject MissileOut;
    Vector3 TrapOutPosition;
    Vector3 MissileOutPosition;
    //當前持有
    //public GameObject m_Weapon;
    public int m_Weapon;
    //佇列存放
    public int[] WeaponQueue = new int[2];
    int WeaponTimes;
    public int NowQueue;
    GameObject UT1;
    public Text Weapon1_UseTimes;
    public bool WeaponOn = false;
    //打雷作用目標
    int ThunderCheck = 1;
    //抽選延遲Flag
    public bool WeaponOpen = true;   //true可以發射
    public bool SlotOpen = true;
    //發射間隔Flag
    public bool SlowlyFire = true;
    //========武器編號==========
    int WeaponKind;
    //實體物件生成基準
    public GameObject TheCar;
    public WeaponObjectPool pool;
    //使用次數
    public int UsingTimes;
    //排名加權
    int RankCheck;
    int[] RankNumber;
    int[] Rank1 = new int[] { 3, 3, 3, 4, 5, 5, 5, 6, 6, 6 };
    int[] Rank2 = new int[] { 1, 1, 2, 2, 2, 2, 3, 4, 5, 5, 6, 6 };
    int[] Rank3 = new int[] { 1, 1, 2, 2, 2, 3, 4, 4, 5, 6, 7, 7 };
    int[] Rank4 = new int[] { 1, 1, 2, 2, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8 };
    RankDetect Rank;
    //敵車效果判斷
    GameObject[] go;
    GameObject[] gop;
    //爆炸及打滑動畫
    public Animator m_animation;
    //武器效果開關
    public bool itemOneSpeedUp;
    public bool oilEffect;
    public bool bombEffect;
    public bool flyEffect;
    //護盾判定
    public bool SheildOn = false;
    //爆炸、電擊、打滑特效
    ParticleSystem Explos;
    ParticleSystem Electric;
    ParticleSystem OilWater;
    ParticleSystem Rocket;
    public Material[] m_Material;
    AudioSource ExplosionSound;
    AudioSource SpeedUpSound;
    AudioSource ElectricSound;
    AudioSource OilSound;

    //public Image FinalLap;
    public bool itemPlay;

    //ResourceLoad
    GameObject OilItemObject;
    GameObject MissileObject;
    GameObject SheildObject;
    GameObject TNTItem;
    GameObject BombObject;

    private void Start()
    {
        //指定自身車輛
        TheCar = this.gameObject.transform.gameObject;
        //排名獲取
        Rank = this.gameObject.GetComponent<RankDetect>();
        //初始化武器
        //m_Weapon = Weapons[0];
        m_Weapon = 0;
        //敵車儲存
        go = GameObject.FindGameObjectsWithTag("AI");
        gop = GameObject.FindGameObjectsWithTag("Kart");
        //受害動畫
        m_animation = this.gameObject.transform.GetChild(0).GetComponent<Animator>();

        //特效
        Explos = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        Electric = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        OilWater = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(3).GetComponent<ParticleSystem>();
        Rocket = this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(4).GetComponent<ParticleSystem>();

        ExplosionSound = this.gameObject.transform.GetChild(2).gameObject.transform.GetChild(3).GetComponent<AudioSource>();
        SpeedUpSound = this.gameObject.transform.GetChild(2).gameObject.transform.GetChild(4).GetComponent<AudioSource>();
        ElectricSound = this.gameObject.transform.GetChild(2).gameObject.transform.GetChild(5).GetComponent<AudioSource>();
        OilSound = this.gameObject.transform.GetChild(2).gameObject.transform.GetChild(6).GetComponent<AudioSource>();

        ExplosionSound.volume = 0.6f;
        SpeedUpSound.volume = 1.0f;
        ElectricSound.volume = 0.6f;
        OilSound.volume = 0.8f;
        SpeedUpSound.loop = true;

        MissileObject = (GameObject)Resources.Load("Weapon/Rocket");
        OilItemObject =  (GameObject)Resources.Load("Weapon/LifePot");
        SheildObject = (GameObject)Resources.Load("Weapon/SphereGemLarge");
        TNTItem = (GameObject)Resources.Load("Weapon/Chest_closed");
        BombObject = (GameObject)Resources.Load("Weapon/bomb");
    }

    private void FixedUpdate()
    {
        //受害效果判定
        ItemEffectTime();

        //if(WeaponOn == true && this.gameObject.tag == "Kart")
        //{
        //    Weapon1_UseTimes.text = UsingTimes.ToString();
        //}

        //if(WeaponOn == true && this.gameObject.tag == "Kart" && WeaponQueue[0] == 0 && WeaponQueue[1] == 0)
        //{
        //    Weapon1_UseTimes.gameObject.SetActive(false);
        //}
    }


    //設定武器+使用次數
    public void WeaponItem()  
    {
        //判斷佇列是否為0
        //將結果存入正確佇列位置
        if (WeaponQueue[0] == 0)
        {
            WeaponOpen = false;
            WeaponQueue[0] = WeaponNumber();
            NowQueue = 0;
        }
        else if(WeaponQueue[0] != 0 && WeaponQueue[1] == 0)
        {
            WeaponQueue[1] = WeaponNumber();
            NowQueue = 1;
        }
        else
        {
            Debug.Log("Weapon Full");
            NowQueue = 2;
        }
        WeaponQueueSet(NowQueue);
      
    }

    //設定佇列1武器 + 使用次數
    void WeaponQueueSet(int NowQueue)
    {
        //m_Weapon = Weapons[WeaponQueue[0]];
        m_Weapon = WeaponQueue[0];
        if(NowQueue != 2)
        {
            UseTimes(NowQueue);
            if(NowQueue == 0)
            {
                UsingTimes = WeaponTimes;
            }
        } 
        itemPlay = true;
        Invoke("TimeLate", 1.0f);
    }

    //抽選完畢
    void TimeLate()
    {
        WeaponOpen = true;
        itemPlay = false;
    }

    //===========隨機取數(修改武器抽取範圍)、加權==============
    
    public int WeaponNumber()
    {
        //=========加權==========

        if (Rank.rank == 1)
        {
            RankCheck = 1;
        }
        else if (Rank.rank <= 3 && Rank.rank > 1)
        {
            RankCheck = 2;
        }
        else if (Rank.rank <= 6 && Rank.rank > 3)
        {
            RankCheck = 3;
        }
        else if (Rank.rank <= 8 && Rank.rank > 6)
        {
            RankCheck = 4;
        }
        switch (RankCheck)
        {
            case 1:
                RankNumber = Rank1;
                //Debug.Log(TheCar + " is Rank1!");
                break;
            case 2:
                RankNumber = Rank2;
                //Debug.Log(TheCar + " is Rank2!");
                break;
            case 3:
                RankNumber = Rank3;
                //Debug.Log(TheCar + " is Rank3!");
                break;
            case 4:
                RankNumber = Rank4;
                //Debug.Log(TheCar + " is Rank4!");
                break;

        }

        int Ra = Random.Range(1, RankNumber.Length);
        WeaponKind = RankNumber[Ra - 1];
        //WeaponKind = 7;
        //WeaponKind = Random.Range(2,2);

        //=========加權==========

        return WeaponKind;
    }

    //INPUT
    public void WeaponCalled(InputManager IM)
    {
        if (IM.bShift)
        {
            //防止一鍵連射
            if(SlowlyFire == true)
            {
                //等待UI跑抽取動畫
                if (WeaponOpen == true)
                {

                    UseWeapons();
                    SlowlyFire = false;
                    Invoke("SlowlyFireOff", 0.1f);

                    //調換佇列
                    if (UsingTimes == 0)
                    {
                        WeaponQueue[0] = WeaponQueue[1];
                        WeaponQueue[1] = 0;
                        NowQueue = 0;
                        WeaponQueueSet(NowQueue);
                        //Debug.Log("Change Weapon UseTimes = " + UsingTimes);
                    }

                }
                else
                {
                    //Debug.Log("Wait for weapon");
                }
            }
        }
    }

    //防止一鍵連射
    void SlowlyFireOff()
    {
        SlowlyFire = true;
    }

    //使用武器
    public void UseWeapons()
    {
        if (m_Weapon == 0)
        {
            //Debug.Log("dont have weapon.");
        }
        else
        {
            //Debug.Log(TheCar + " use the weapon " + m_Weapon);
            UseThisItem();
            WeaponDestroy();
        }
    }

    //使用後消失
    void WeaponDestroy()
    {
        if(UsingTimes <= 0)
        {
            m_Weapon = 0;

            UsingTimes = 0;
            
        }
    }

    //分別發動道具
    void UseThisItem()
    {
        switch (WeaponQueue[0])
        {
            //加速道具 OK
            case 1:
                SpeedUp();
                UsingTimes -= 1;
                //Debug.Log("Speed Up");
                break;
            //追蹤導彈 //特效
            case 2:
                
                WeaponMissile WPMS = MissileObject.gameObject.GetComponent<WeaponMissile>();
                WPMS.MissileOn(TheCar);
                MissileOutPosition = transform.TransformPoint(0, 0.9f, 3);
                MissileBomb = Instantiate(MissileObject, MissileOutPosition, this.transform.rotation);
                MissileBomb.SetActive(true);
                //Debug.Log("Fire!");
                UsingTimes -= 1;
                break;
            //地面漏油 ok
            case 3:

                TrapOutPosition = transform.TransformPoint(0, 0, -2.5f);   
                Instantiate(OilItemObject, TrapOutPosition, this.transform.rotation);
                UsingTimes -= 1;
                break;
            //防護罩  OK
            case 4:
                
                if(SheildOn == false)
                {
                    Sheild = Instantiate(SheildObject, TheCar.transform.position, TheCar.transform.rotation);
                    Sheild.SetActive(true);
                    Sheild.transform.parent = TheCar.transform;
                    SheildOn = true;
                    //Debug.Log("Defense!");
                    UsingTimes -= 1;
                    Invoke("UnSheild", 5.0f);
                }
                break;
            //TNT ok
            case 5:

                TrapOutPosition = transform.TransformPoint(0, 0, -2.5f);
                TNTBomb = Instantiate(TNTItem, TrapOutPosition, this.transform.rotation);
                TNTBomb.SetActive(true);
                //Debug.Log("TNT");
                UsingTimes -= 1;
                break;
            //投擲炸彈  //需要速度資訊
            case 6:

                MissileOutPosition = transform.TransformPoint(0, 0.9f, 3);
                Bomb = Instantiate(BombObject, MissileOutPosition, this.transform.rotation);
                Bomb.SetActive(true);
                //Debug.Log("Bomb!");
                UsingTimes -= 1;
                break;
            //全域落雷 //特效未正確作用
            case 7:
                ThunderChecking();
                //Debug.Log("Thunder!");
                UsingTimes -= 1;
                break;
            //飛天 OK
            case 8:
                FlyCar();
                //Debug.Log("Fly!");
                UsingTimes -= 1;
                break;
        }
    }

    //各自使用次數
    void UseTimes(int NowQueue)
    {
        switch (WeaponQueue[NowQueue])
        {
            //加速
            case 1:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //追蹤飛彈
            case 2:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //漏油
            case 3:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //防護罩
            case 4:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //TNT
            case 5:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //投擲炸彈
            case 6:
                WeaponTimes = 3;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //全域落雷
            case 7:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
            //飛天
            case 8:
                WeaponTimes = 1;
                //Debug.Log(WeaponTimes + "Times");
                break;
        }

        //UseTimeText();
        WeaponOn = true;

    }

    //void UseTimeText()
    //{
    //    if (this.gameObject.tag == "Kart" && NowQueue == 0)
    //    {
    //        UT1 = GameObject.Find("Canvas").transform.Find("PlayerInfo").transform.Find("Item").gameObject;
    //        Weapon1_UseTimes = UT1.gameObject.GetComponentInChildren<Text>();
    //        Weapon1_UseTimes.gameObject.SetActive(true);
    //    }
    //}


    //防禦確認
    public bool DefenceCheck(GameObject TheCar)
    {
        WeaponOrigin WO = TheCar.gameObject.GetComponent<WeaponOrigin>();
        return WO.SheildOn;
    }

    //關閉護盾
    void UnSheild()
    {
        SheildOn = false;
        Sheild.SetActive(false);
    }

    //=====狀態增減=====//

    //===============加速==============

    void SpeedUp()
    {
        //自身最大速度增加
        itemOneSpeedUp = true;
        SpeedUpSound.Play();
        //Debug.Log(itemOneSpeedUp);
        Invoke("SpeedUpDown", 3.0f);
    }
    void SpeedUpDown()
    {
        //速度恢復原狀
        itemOneSpeedUp = false;
        SpeedUpSound.Stop();
    }

    //===============落雷================

    void ThunderChecking()
    {


        for (int i = 0; i < go.Length; i++)
        {
            RankDetect TargetRank = go[i].gameObject.transform.GetComponent<RankDetect>();
            WeaponOrigin Target = go[i].gameObject.transform.GetComponent<WeaponOrigin>();
            //Debug.Log("ThunderCheck " +Target + go[i]+ThunderCheck);
            if (TargetRank.rank == 1 && Target.DefenceCheck(Target.TheCar) == false)
            {
                Target.Electric.Play();
                Target.ExplosionSound.Play();
                Target.bombEffect = true;
                //Debug.Log("ThunderCheckNumber"+go[i]+" " + Target.ThunderCheck +" " +ThunderCheck);

            }


        }

        RankDetect TargetKartRank = gop[0].gameObject.transform.GetComponent<RankDetect>();
        WeaponOrigin TargetKart = gop[0].gameObject.transform.GetComponent<WeaponOrigin>();
        if (TargetKartRank.rank == 1 && TargetKart.DefenceCheck(TargetKart.TheCar) == false)
        {
            TargetKart.Electric.Play();
            TargetKart.ExplosionSound.Play();
            TargetKart.bombEffect = true;
            //Debug.Log("ThunderCheckNumber" + go[0] + " " + TargetKart.ThunderCheck + " " + ThunderCheck);


        }


        //ThunderCheck += 1;
        ////Electric.Play();
        ////bombEffect = true;

        //for (int i = 0; i < go.Length; i++)
        //{
        //    WeaponOrigin Target = go[i].gameObject.transform.GetComponent<WeaponOrigin>();

        //    //Debug.Log("ThunderCheck " +Target + go[i]+ThunderCheck);
        //    if (Target.ThunderCheck != this.ThunderCheck && Target.DefenceCheck(Target.TheCar) == false)
        //    {
        //        Target.Electric.Play();
        //        Target.ExplosionSound.Play();
        //        Target.bombEffect = true;
        //        //Debug.Log("ThunderCheckNumber"+go[i]+" " + Target.ThunderCheck +" " +ThunderCheck);

        //    }


        //}
        //WeaponOrigin TargetKart = gop[0].gameObject.transform.GetComponent<WeaponOrigin>();
        //if (TargetKart.ThunderCheck != this.ThunderCheck && TargetKart.DefenceCheck(TargetKart.TheCar) == false)
        //{
        //    TargetKart.Electric.Play();
        //    TargetKart.ExplosionSound.Play();
        //    TargetKart.bombEffect = true;
        //    //Debug.Log("ThunderCheckNumber" + go[0] + " " + TargetKart.ThunderCheck + " " + ThunderCheck);


        //}
        //ThunderCheck -= 1;
    }

    void ThunderStop(GameObject TheCar)
    {
        m_animation = TheCar.gameObject.transform.GetChild(0).GetComponent<Animator>();
        m_animation.SetBool("OnExplosion", false);
    }

    //===============飛天=================

    void FlyCar()
    {
        flyEffect = true;
        Rocket.Play();
        SpeedUpSound.Play();
        Invoke("FlyLateDown", 5.0f);
    }

    //==========碰撞判定==========

    private void OnTriggerEnter(Collider other)
    {
        //武器碰撞
        if(other.tag == "WeaponBox")
        {
            WeaponItem();
            SlotOpen = false;
            Invoke("SlotOpenOff", 0.1f);
        }
        //油瓶碰撞
        if(other.tag == "Oil" && DefenceCheck(TheCar) == false)
        {
            OilWater.Play();
            OilSound.Play();
            oilEffect = true;
            other.gameObject.SetActive(false);

        }
        //炸彈碰撞
        if(other.tag == "Bomb" && DefenceCheck(TheCar) == false)
        {
            Explos.Play();
            ExplosionSound.Play();
            bombEffect = true;
          
        }
        
    }
    //播放動畫
    void ItemEffectTime()
    {
        //打滑動畫
        if (oilEffect == true)
        {
            m_animation.SetBool("OnSlide", true);
            Invoke("OilLateDown", 0.3f);
        }
        //炸飛動畫
        if (bombEffect == true)
        {
            
            m_animation.SetBool("OnExplosion", true);
            Invoke("BombLateDown", 0.8f);
            flyEffect = false;
        }
    }

    void OilLateDown()
    {
        m_animation.SetBool("OnSlide", false);
        oilEffect = false;
    }

    void BombLateDown()
    {
        m_animation.SetBool("OnExplosion", false);
        bombEffect = false;
    }

    void FlyLateDown()
    {
        Rocket.Stop();
        SpeedUpSound.Stop();
        flyEffect = false;
    }

    void SlotOpenOff()
    {
        SlotOpen = true;
    }
}
