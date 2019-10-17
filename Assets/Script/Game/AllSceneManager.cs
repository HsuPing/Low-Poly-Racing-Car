using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AllSceneManager : MonoBehaviour {

    //倒數計時開關
    public bool StartTimerOn = false;
    //遊戲開始開關
    public bool StartGame = false;
    public bool EndGame = false;
    public int mTime = 3;
    public Image[] TimeNumber;
    //末圈提示圖
    public Image FinalLap;

    //起始導覽運鏡
    public GameObject StartViewCamera;
    Camera ViewCamera;
    Animator StCameraAnimation;
    public GameObject PlayerInfo;
    public GameObject MiniMap;
    RankDetect rankDetect;

    //背景音樂
    AudioSource BackgroundMusic;
    bool finalBool;

    AudioSource TimerSound;
    AudioSource TimerGoSound;
    AudioSource FinishLine;
    GameObject SceneAudioSource;

    ButtonControl buttonControl;

    private void Awake()
    {
        //起始導覽運鏡
        PlayerInfo = GameObject.Find("Canvas").transform.Find("PlayerInfo").gameObject;
        PlayerInfo.gameObject.SetActive(false);

        StartViewCamera = GameObject.Find("MainCamera").gameObject;

        SceneAudioSource = GameObject.Find("SceneSound").gameObject;
        TimerSound = SceneAudioSource.gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        TimerSound.volume = 0.5f;
        TimerGoSound = SceneAudioSource.gameObject.transform.GetChild(1).GetComponent<AudioSource>();
        TimerGoSound.volume = 0.5f;
        FinishLine = SceneAudioSource.gameObject.transform.GetChild(4).GetComponent<AudioSource>();
        FinishLine.volume = 0.5f;
        buttonControl = this.gameObject.GetComponent<ButtonControl>();
    }

    private void Start()
    {
        StCameraAnimation = StartViewCamera.GetComponent<Animator>();
        StCameraAnimation.SetBool("Start", true);

        rankDetect = GameObject.Find("PlayerCar").GetComponent<RankDetect>();
        MiniMap = GameObject.Find("Canvas").transform.Find("MiniMap").gameObject;

        BackgroundMusic = this.GetComponent<AudioSource>();
        BackgroundMusic.Stop();
        BackgroundMusic.pitch = 1;
        BackgroundMusic.volume = 0.25f;
    }

    void Update ()
    {
        //跑完導覽動畫開始倒數
        if (StartTimerOn == false && StartViewCamera.activeSelf != false && StCameraAnimation.GetCurrentAnimatorStateInfo(0).IsName("Over"))
        {
            TimerSound.Play();
            StCameraAnimation.enabled = false;
            PlayerInfo.gameObject.SetActive(true);
            TimeNumber[0].gameObject.SetActive(true);
            Invoke("UnText", 1.0f);
            InvokeRepeating("StartTimer", 1, 1);
            StartTimerOn = true;
        }
        //末圈提示
        if(rankDetect.lapCount == 3 && finalBool == false)
        {
            FinalLap.gameObject.SetActive(true);
            BackgroundMusic.pitch = 1.2f;
            Invoke("FinalLapOff", 1.0f);
            finalBool = true;
        }    

        if(rankDetect.lapCount == 4 && EndGame == false)
        {
            BackgroundMusic.pitch = 1;
            BackgroundMusic.volume = 0.1f;
            FinishLine.Play();
            EndGame = true;
            PlayerInfo.gameObject.SetActive(false);
            MiniMap.gameObject.SetActive(false);
        }

        if(StartGame && !BackgroundMusic.isPlaying && buttonControl.Pause != true)
        {
            BackgroundMusic.Play();
        }
    }

    void FinalLapOff()
    {
        FinalLap.gameObject.SetActive(false);
    }

    //倒數計時器
    void StartTimer()
    {
        mTime -= 1;

        switch (mTime)
        {
            case 2:
                
                TimeNumber[0] = TimeNumber[2];
                TimeNumber[0].gameObject.SetActive(true);
                //TimerSound.Play();
                Invoke("UnText", 1.0f);
                break;
            case 1:
                
                TimeNumber[0] = TimeNumber[1];
                TimeNumber[0].gameObject.SetActive(true);
                //TimerSound.Play();
                Invoke("UnText", 1.0f);
                break;
        }

        if(mTime == 0)
        {
            TimeNumber[0] = TimeNumber[4];
            TimeNumber[0].gameObject.SetActive(true);
            //TimerSound.pitch = 1.5f;
            //TimerSound.volume = 0.7f;
            //TimerSound.Play();
            Invoke("UnText", 1.0f);
            StartGame = true;
            CancelInvoke("StartTimer");
        }
    }

    //關閉倒數計時器顯示
    void UnText()
    {
        TimeNumber[0].gameObject.SetActive(false);
    }
}
