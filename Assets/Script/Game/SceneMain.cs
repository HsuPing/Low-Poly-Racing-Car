using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain : MonoBehaviour {

    GameManager gameManager;
    public List<int> carNum = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
    Vector3[] startPosition = new[] { new Vector3(13.8f, 1.12f, 3f), new Vector3(12.6f, 1.12f, 10f), new Vector3(11.7f, 1.12f, 16f), new Vector3(4.2f, 1.12f, -3f), new Vector3(3.2f, 1.12f, 3f), new Vector3(2.3f, 1.12f, 10f), new Vector3(14.8f, 1.12f, -3f) };
    public int[] randomNum = new int[7];

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        BuildPlayer();
        BuildAI();
    }

    void BuildPlayer()
    {
        //製造玩家模型
        GameObject playerCar = Instantiate(Resources.Load("Cars/Car" + ((int)gameManager.carType).ToString()) as GameObject);
        playerCar.name = "PlayerCar";
        playerCar.tag = "Kart";
        playerCar.transform.position = new Vector3(1.3f, 1.12f, 16f); 
        playerCar.transform.eulerAngles = new Vector3(0, 90, 0);
        playerCar.transform.Find("Equipment/Intake_top").GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("IntakeTop/" + gameManager.intake_Top_Path + gameManager.intake_top_num.ToString());
        playerCar.transform.Find("Equipment/Front_Wing").GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("FrontWing/" + gameManager.front_Wing_Path + gameManager.front_wing_num.ToString());
        playerCar.transform.Find("Equipment/Rear_Wing").GetComponent<MeshFilter>().sharedMesh = Resources.Load<Mesh>("RearWing/" + gameManager.rear_Wing_Path + gameManager.rear_wing_num.ToString());

        playerCar.AddComponent<PlayerCar>();
        playerCar.GetComponent<PlayerCar>().speedValue = gameManager.carsSpeed[(int)gameManager.carType];
        playerCar.GetComponent<PlayerCar>().accValue = gameManager.carsAcc[(int)gameManager.carType];

        playerCar.AddComponent<AudioListener>();
    }

    void BuildAI()
    {
        for(int i = 0; i < carNum.Count; i++)
        {
            if(carNum[i] == (int)gameManager.carType)
            {
                carNum.Remove(carNum[i]);
            }
        }

        for (int j = 0; j <= 6; j++)
        {
            randomNum[j] = carNum[Random.Range(0, carNum.Count)];
            carNum.Remove(randomNum[j]);
        }

        for(int k = 0; k <=6; k++)
        {
            GameObject AICar = Instantiate(Resources.Load("Cars/Car" + randomNum[k].ToString()) as GameObject);
            AICar.name = "AI"+(k+1).ToString();
            AICar.tag = "AI";
            AICar.transform.position = startPosition[k];
            AICar.transform.eulerAngles = new Vector3(0, 90, 0);
            AICar.AddComponent<AICar>();
            AICar.GetComponent<AICar>().speedValue = gameManager.carsSpeed[randomNum[k]];
            AICar.GetComponent<AICar>().accValue = gameManager.carsAcc[randomNum[k]];
            AICar.GetComponent<AICar>().CarTypeNum = randomNum[k];
        }
    }
}
