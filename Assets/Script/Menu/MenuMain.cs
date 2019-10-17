using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuMain : MonoBehaviour
{
    EventSystem eventSystem;
    GameManager gameManager;

    GameObject StartMenu;           //開始頁面
    GameObject MapMenu;             //地圖頁面
    GameObject InstructionsMenu;    //操作介紹頁面
   
    GameObject BluePanel;
    GameObject WhitePanel; 

    //bool InstructionsMenubool;
    //bool buttonAudioVolume;
    bool trueOnswitchButtonAudio;

    AudioSource switchButtonAudio;

    void Awake()
    {
        Cursor.visible = false;  //隱藏滑鼠
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        eventSystem = GameObject.Find("EventSystem").gameObject.GetComponent<EventSystem>();

        StartMenu = GameObject.Find("StartMenu").gameObject;
        MapMenu = GameObject.Find("MapMenu").gameObject;

        InstructionsMenu = GameObject.Find("InstructionsMenu").gameObject;
       
        switchButtonAudio = this.GetComponent<AudioSource>();
        switchButtonAudio.volume = 0.0f;

        BluePanel = this.transform.GetChild(0).gameObject;
        WhitePanel = this.transform.GetChild(1).gameObject;
    }


    private void Start()
    {
        StartMenu.SetActive(true);
        MapMenu.SetActive(false);
        InstructionsMenu.SetActive(false);
        
        BluePanel.SetActive(false);
        WhitePanel.SetActive(false);
       
        eventSystem.SetSelectedGameObject(StartMenu.transform.GetChild(0).gameObject);
    }

    private void Update()
    {
        if(trueOnswitchButtonAudio == false)
        {
            Invoke("TurnOnSound",0.2f);
            trueOnswitchButtonAudio = true;
        }
    }

    public void StartToMapMenu()
    {
        BluePanel.SetActive(true);
        WhitePanel.SetActive(true);
        MapMenu.SetActive(true);
        StartMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(MapMenu.transform.GetChild(0).gameObject);
    }

    public void Instruction()
    {
        //操作介紹
        //eventSystem.SetSelectedGameObject(//按鈕物件);
        InstructionsMenu.SetActive(true);
        BluePanel.SetActive(true);
        WhitePanel.SetActive(true);
        StartMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(InstructionsMenu.transform.GetChild(0).gameObject);
    }

    public void BackButton()
    {
        StartMenu.SetActive(true);
        MapMenu.SetActive(false);
        BluePanel.SetActive(false);
        WhitePanel.SetActive(false);
        InstructionsMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(StartMenu.transform.GetChild(0).gameObject);
    }

    public void ConfirmButton()
    {
        gameManager.mapSelect = MapSelect.MOUNTAIN;
        SceneManager.LoadScene("Equipment");
        switchButtonAudio.volume = 0;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void TurnOnSound()
    {
        switchButtonAudio.volume = 0.6f;
    }
   
}

