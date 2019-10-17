using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankImage : MonoBehaviour {

    RankDetect CarRank;
    int NowRank;
    public Sprite[] RankIcon = new Sprite[8];
    public Sprite[] m_RankIcon;
    public Sprite m_Icon;
    public Image IconObject;

	void Start ()
    {
		CarRank = GameObject.Find("PlayerCar").GetComponent<RankDetect>();
        
        IconObject = this.gameObject.GetComponent<Image>();

        RankIcon[0] = Resources.Load<Sprite>("UI/Rank/1st");
        RankIcon[1] = Resources.Load<Sprite>("UI/Rank/2nd");
        RankIcon[2] = Resources.Load<Sprite>("UI/Rank/3rd");
        RankIcon[3] = Resources.Load<Sprite>("UI/Rank/4th");
        RankIcon[4] = Resources.Load<Sprite>("UI/Rank/5th");
        RankIcon[5] = Resources.Load<Sprite>("UI/Rank/6th");
        RankIcon[6] = Resources.Load<Sprite>("UI/Rank/7th");
        RankIcon[7] = Resources.Load<Sprite>("UI/Rank/8th");
    }
	
	void Update ()
    {
        IconObject.sprite = RankIcon[CarRank.rank - 1];
	}
}
