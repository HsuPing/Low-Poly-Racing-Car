using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class RankManager : MonoBehaviour
{
    public GameObject[] checkPointList;
    public RankDetect[] rankDetects;

    GameObject[] ai;
    GameObject[] kart;
    public GameObject[] cars;
    public int[] ranks;
    int playerCarNum;

    TimeSpan timeSpan;
    string timerText;

    //For UI
    public GameObject RankList;
    public GameObject RankHightlight;
    public Image[] HelmetImages;
    public Text[] TimeTexts;

    AllSceneManager allSceneManager;
    public GameManager gameManager;
    public SceneMain sceneMain;
    public bool playerChecked;
    public bool doRank;

    int num = 1;

    void Awake()
    {
        checkPointList = GameObject.FindGameObjectsWithTag("CheckPoint").OrderBy(go => go.name).ToArray();
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sceneMain = this.GetComponent<SceneMain>();
        RankList = GameObject.Find("Canvas").transform.Find("RankList").gameObject;
        RankHightlight = RankList.transform.Find("HighlightPanel").gameObject;
        HelmetImages = new Image[8];
        TimeTexts = new Text[8];

        for (int i = 1; i <= 8; i++)
        {
            HelmetImages[i-1] = RankList.transform.Find(i.ToString()).transform.Find("helmetImage").GetComponent<Image>();
            TimeTexts[i-1] = RankList.transform.Find(i.ToString()).transform.Find("TimeText").GetComponent<Text>();
        }
    }

    private void Start()
    {
        ai = GameObject.FindGameObjectsWithTag("AI");
        kart = GameObject.FindGameObjectsWithTag("Kart");
        cars = ai.Concat(kart).ToArray();

        rankDetects = new RankDetect[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            rankDetects[i] = cars[i].GetComponent<RankDetect>();
        }

        ranks = new int[cars.Length];
      
        for (playerCarNum = 0; playerCarNum < cars.Length; playerCarNum++)
        {
            if(rankDetects[playerCarNum] == GameObject.Find("PlayerCar").GetComponent<RankDetect>())
            {
                return;
            }
        }
      
        RankList.SetActive(false);
        num = 1;
    }

    void Update()
    {

        //if (allSceneManager.EndGame != true)
        //{
            RankOnTime();
        //}
        if(allSceneManager.EndGame == true) //&& doRank == false
        {
            RankList.SetActive(true);
            EndGameRankList();
        }
      
    }

    void RankOnTime()
    {
        for (int i = 0; i < ranks.Length; i++)
        {
            ranks[i] = cars[i].GetComponent<RankDetect>().rank;
        }
    }

    void EndGameRankList()
    {  
       for(int i = num; i <= cars.Length; i++)  //名次
        {
           for(int j = 0; j < cars.Length; j++)  
            {
                string AIname;
                int AINum;
                if (ranks[j] == i)
                {
                    
                    timeSpan = TimeSpan.FromSeconds(rankDetects[j].TimeRecord);
                    timerText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);

                    if(rankDetects[j] == rankDetects[playerCarNum] && playerChecked == false)
                    {
                        HelmetImages[i - 1].sprite = Resources.Load<Sprite>("UI/Helmets/hel" + ((int)gameManager.carType).ToString());
                        TimeTexts[i - 1].text = timerText;
                        float y = RankList.transform.Find(i.ToString()).transform.localPosition.y;
                        RankHightlight.transform.localPosition = new Vector2(0, y);
                        num = num + 1;
                        playerChecked = true;   
                    }

                    else
                    {
                        AIname = rankDetects[j].name;
                        AINum = int.Parse(AIname.Substring(AIname.Length - 1, 1));
                        HelmetImages[i - 1].sprite = Resources.Load<Sprite>("UI/Helmets/hel" + sceneMain.randomNum[AINum-1]);
                        if (rankDetects[j].lapCount < 4)
                        {
                            TimeTexts[i - 1].text = "--:--:--";
                        }
                        else
                        {
                            TimeTexts[i - 1].text = timerText;
                            num = num + 1;
                        }
                    }
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        for(int i = 0; i < checkPointList.Length; i++)
        {
            if (i + 1 == 56) return;
            Gizmos.DrawLine(checkPointList[i].transform.position, checkPointList[i + 1].transform.position);
        }
       
    }
}
