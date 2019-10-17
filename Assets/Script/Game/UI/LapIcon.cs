using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapIcon : MonoBehaviour {

    Sprite[] NowLapImage = new Sprite[3];
    RankDetect m_Lap;
    Image IconObject;
    
	void Start ()
    {
		m_Lap = GameObject.Find("PlayerCar").GetComponent<RankDetect>();
        IconObject = this.gameObject.transform.GetChild(0).GetComponent<Image>();
        NowLapImage[0] = Resources.Load<Sprite>("UI/1");
        NowLapImage[1] = Resources.Load<Sprite>("UI/2");
        NowLapImage[2] = Resources.Load<Sprite>("UI/3");
    }
	
	void Update ()
    {
        int i = m_Lap.lapCount - 1;
        if (i > 2) i = 2;
        IconObject.sprite = NowLapImage[i];
    }
}
