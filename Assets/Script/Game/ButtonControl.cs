using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonControl : MonoBehaviour
{
    AllSceneManager allSceneManager;
    public EventSystem eventSystem;
    public GameObject RankList;
    public GameObject PauseMenu;
    AudioSource[] pauseStopAudios;
    public bool Pause;

    bool setbool;
    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();
        RankList = GameObject.Find("Canvas").transform.Find("RankList").gameObject;
        PauseMenu = GameObject.Find("Canvas").transform.Find("Pause").gameObject;
    }

    private void Start()
    {
        GameObject[] AudioGOs = GameObject.FindGameObjectsWithTag("Audio");
        pauseStopAudios = new AudioSource[AudioGOs.Length];
        for (int i = 0; i< AudioGOs.Length; i++)
        {
            pauseStopAudios[i] = AudioGOs[i].GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = true;
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(PauseMenu.transform.GetChild(2).gameObject);
            for (int i = 0; i < pauseStopAudios.Length; i++)
            {
                pauseStopAudios[i].Pause();
            }
        }

        if (allSceneManager.EndGame && setbool == false)
        {
            Invoke("SetSelected", 3);
            setbool = true;
        }
    }

    void SetSelected()
    {
        eventSystem.SetSelectedGameObject(RankList);
    }

    public void BacktoMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ContinueButton()
    {
        Pause = false;
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        for (int i = 0; i < pauseStopAudios.Length; i++)
        {
            pauseStopAudios[i].UnPause();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
