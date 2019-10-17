using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarShaderChange : MonoBehaviour {

    public Material M1,M2;
    WeaponOrigin WO;
    int CarTypeNum = 0;

    private void Awake()
    {
        WO = this.gameObject.transform.parent.gameObject.GetComponent<WeaponOrigin>();
    }

    void Start ()
    {
        if (this.transform.parent.tag == "AI")
        {
            CarTypeNum = this.transform.parent.GetComponent<AICar>().CarTypeNum;
        }
        else if(this.transform.parent.tag == "Kart")
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            CarTypeNum = (int)gameManager.carType;
        }

        M1 = (Material)Resources.Load("Material/Car/" + CarTypeNum.ToString()+ "/Kart_BlinnPhong_" + CarTypeNum.ToString());
        M2 = (Material)Resources.Load("Material/Car/" + CarTypeNum.ToString() + "/ShaderTest_" + CarTypeNum.ToString());
    }
	
	void Update () {
		if(WO.itemOneSpeedUp == true || WO.flyEffect ==true)
        {
            for(int i=0; i<=5; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = M2;
            }
        }
        else
        {
            for (int i = 0; i <= 5; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().material = M1;
            }
        }
	}
}
