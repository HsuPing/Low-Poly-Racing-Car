using UnityEngine;

public class RankDetect: MonoBehaviour
{
    AllSceneManager allSceneManager;
    public int rank;
    public int checkPoint;
    public int lapCount;
    public float TimeRecord;
    public RankManager rankManager; //Public for AIFunction
    int maxRank;
    bool[] rankBool;

    void Start()
    {
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();
        rankManager = GameObject.Find("Main").GetComponent<RankManager>();
        checkPoint = 0;
        lapCount = 1;
        TimeRecord = 0;
        maxRank = rankManager.rankDetects.Length;
        rankBool = new bool[rankManager.rankDetects.Length];
    }

    private void FixedUpdate()
    {
      
            RankCal();
        

        if(allSceneManager.StartGame == true && lapCount < 4 )
        {
            TimeRecord += Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject == rankManager.checkPointList[checkPoint])
        {
            if (checkPoint+1 == rankManager.checkPointList.Length)
            {
                lapCount += 1;
                checkPoint = 0;
            }
            else checkPoint += 1;
        }
    }

    void RankCal()
    {
        for (int i = 0; i < rankManager.rankDetects.Length; i++)
        {

            if(lapCount > rankManager.rankDetects[i].lapCount)
            {
                rankBool[i] = true;
            }

            else if(lapCount == rankManager.rankDetects[i].lapCount && checkPoint > rankManager.rankDetects[i].checkPoint)
            {
                rankBool[i] = true;
            }

            else if (checkPoint == rankManager.rankDetects[i].checkPoint && lapCount == rankManager.rankDetects[i].lapCount)
            {
                float thisDistance = (this.gameObject.transform.position - rankManager.checkPointList[checkPoint].transform.position).magnitude;
                float otherDistance = (rankManager.rankDetects[i].gameObject.transform.position - rankManager.checkPointList[checkPoint].transform.position).magnitude;

                if (thisDistance < otherDistance)
                {
                    rankBool[i] = true;
                }
                else
                {
                    rankBool[i] = false;
                }
            }
        }

        int tureN = count(rankBool, true);
        rank = rankManager.rankDetects.Length - tureN;
   
    }

    public static int count(bool[] array, bool flag)
    {
        int value = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == flag) value++;
        }

        return value;
    }
}
