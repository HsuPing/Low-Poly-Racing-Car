using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EquipmentMain : MonoBehaviour {

    GameManager gameManager;
    EventSystem eventSystem;

    GameObject MainEquipment;
    GameObject EquipmentChange;
    GameObject LoadingObject;

    CarType e_carType;

    Vector3 mainPosition = new Vector3(0, -0.45f, 3);
    Vector3 mainRotation = new Vector3(4, 190, 0);

    int carTypeNum;
    Text carNameText;
    string[] carName = new string [] {"光榮號", "太空號", "騎士號", "檢舉達人", "不死號", "海盜號", "粉紅豹", "天使號", "惡魔號", "德魯伊", "血色德魯伊", "寒冰德魯伊" };
    public GameObject[] carGOs;

    int speedValue;
    int accelerationValue;
    Slider speedValueSlider;
    Slider accelerationValueSlider;

    int intakeTopNum;
    int frontWingNum;
    int rearWingNum;

    string intakeTopPath;
    string frontWingPath;
    string rearWingPath;

    Mesh intakeTopMesh;
    Mesh frontWingMesh;
    Mesh rearWingMesh;

    bool b_rotation = false;
    public Image[] ArrowImage;

    AudioSource switchSound;
    public AudioSource switchButton;
    bool trueOnswitchButtonAudio;

    Color32 ArrowOrigin = new Color32(255, 70, 60, 255);
    Color32 ArrowChange = new Color32(255, 155, 60, 220);

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        MainEquipment = GameObject.Find("Canvas").transform.Find("MainEquipment").gameObject;
        EquipmentChange = GameObject.Find("Canvas").transform.Find("EquipmentChange").gameObject;
        LoadingObject = GameObject.Find("Canvas").transform.Find("LoadingImage").gameObject;
        carNameText = GameObject.Find("Canvas").transform.Find("CarNameImage").GetComponentInChildren<Text>();
        speedValueSlider = MainEquipment.transform.Find("AttributesBackground").transform.Find("SpeedValue").GetComponentInChildren<Slider>();
        accelerationValueSlider = MainEquipment.transform.Find("AttributesBackground").transform.Find("AccelerationValue").GetComponentInChildren<Slider>();
        switchSound = this.GetComponent<AudioSource>();
        switchButton.volume = 0.0f;
    }
    
    private void Start()
    {
        e_carType = CarType.HONOR;

        MainEquipment.SetActive(true);
        EquipmentChange.SetActive(false);
        LoadingObject.SetActive(false);

        eventSystem.SetSelectedGameObject(MainEquipment.transform.GetChild(0).gameObject);

        carTypeNum = 0;
        carNameText.text = carName[0];
        carGOs[0].transform.position = mainPosition;
        carGOs[0].transform.eulerAngles = mainRotation;
        speedValue = 6;
        accelerationValue = 6;
       
    }

    private void Update()
    {
        if(trueOnswitchButtonAudio == false)
        {
            Invoke("TurnOnSound", 0.2f);
            trueOnswitchButtonAudio = true;
        }

        if (MainEquipment.activeSelf)
        {
            MainEquipmentFuction();
        }
        else if (EquipmentChange.activeSelf)
        {
            EquipmentChangeFuction();
        }
       
    }

    void MainEquipmentFuction()
    {
        if (eventSystem.currentSelectedGameObject.name == "ConfirmFakeButton")
        {
            //旋轉目前目標
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //向左轉換一個物件
                switchSound.PlayOneShot(switchSound.clip);
                carTypeNum = carTypeNum - 1;
                if (carTypeNum == -1) carTypeNum = 11;
                CarNameDetect();
                ArrowImage[0].color = ArrowChange;
            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                //向右轉換一個物件
                switchSound.PlayOneShot(switchSound.clip);
                carTypeNum = carTypeNum + 1;
                if (carTypeNum == 12) carTypeNum = 0;
                CarNameDetect();
                ArrowImage[1].color = ArrowChange; 
            }
            else
            {
                ArrowImage[0].color = ArrowOrigin;
                ArrowImage[1].color = ArrowOrigin;
            }
            
            ValueSlideChange(speedValue, accelerationValue);
        }

        if(MainEquipment.activeSelf) carGOs[carTypeNum].transform.Rotate(0, 35 * Time.deltaTime, 0);
    }

    public Image[] EquipmentImages;

    void EquipmentChangeFuction()
    {
        EquipmentImages[0].sprite = Resources.Load<Sprite>("UI/Materials/" + intakeTopPath + intakeTopNum.ToString());
        EquipmentImages[1].sprite = Resources.Load<Sprite>("UI/Materials/" + frontWingPath + frontWingNum.ToString());
        EquipmentImages[2].sprite = Resources.Load<Sprite>("UI/Materials/" + rearWingPath + rearWingNum.ToString());

        if (eventSystem.currentSelectedGameObject.name == "IntakeTopButton")
        {
            b_rotation = false;
            carGOs[carTypeNum].transform.eulerAngles = new Vector3(8, 225, 15);

            if (Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                intakeTopNum -= 1;
                if (intakeTopNum == 0) intakeTopNum = 7;

                intakeTopMesh = Resources.Load<Mesh>("IntakeTop/" + intakeTopPath + intakeTopNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Intake_top").GetComponent<MeshFilter>().sharedMesh = intakeTopMesh;
            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                intakeTopNum += 1;
                if (intakeTopNum == 8) intakeTopNum = 1;

                intakeTopMesh = Resources.Load<Mesh>("IntakeTop/" + intakeTopPath + intakeTopNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Intake_top").GetComponent<MeshFilter>().sharedMesh = intakeTopMesh;
            }
        }

        else if(eventSystem.currentSelectedGameObject.name == "FrontWingButton")
        {
            b_rotation = false;
            carGOs[carTypeNum].transform.eulerAngles = new Vector3(8, 225, 15);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                frontWingNum -= 1;
                if (frontWingNum == 0) frontWingNum = 6;
                frontWingMesh = Resources.Load<Mesh>("FrontWing/" + frontWingPath + frontWingNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Front_Wing").GetComponent<MeshFilter>().sharedMesh = frontWingMesh;
            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                frontWingNum += 1;
                if (frontWingNum == 7) frontWingNum = 1;
                frontWingMesh = Resources.Load<Mesh>("FrontWing/" + frontWingPath + frontWingNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Front_Wing").GetComponent<MeshFilter>().sharedMesh = frontWingMesh;
            }
        }

        else if(eventSystem.currentSelectedGameObject.name == "RearWingButton")
        {
            b_rotation = false;
            carGOs[carTypeNum].transform.eulerAngles = new Vector3(8, 225, 15);
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                rearWingNum -= 1;
                if (rearWingNum == 0) rearWingNum = 9;
                rearWingMesh = Resources.Load<Mesh>("RearWing/" + rearWingPath + rearWingNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Rear_Wing").GetComponent<MeshFilter>().sharedMesh = rearWingMesh;
            }

            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                switchSound.PlayOneShot(switchSound.clip);
                rearWingNum += 1;
                if (rearWingNum == 10) rearWingNum = 1;
                rearWingMesh = Resources.Load<Mesh>("RearWing/" + rearWingPath + rearWingNum.ToString());
                carGOs[carTypeNum].transform.Find("Equipment/Rear_Wing").GetComponent<MeshFilter>().sharedMesh = rearWingMesh;
            }
        }

        else
        {
            if(b_rotation == false)
            {
                carGOs[carTypeNum].transform.eulerAngles = new Vector3(3f, 200f, 0f);
                b_rotation = true;
            }
        }
            if(b_rotation) carGOs[carTypeNum].transform.Rotate(0, 35 * Time.deltaTime, 0);
    }

    void CarNameDetect()
    {
        switch(carTypeNum + 1)
        {
            case 1:
                carGOs[11].SetActive(false);
                carGOs[1].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 6;
                accelerationValue = 6;

                break;  
            case 2:
                carGOs[2].SetActive(false);
                carGOs[0].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 8;
                accelerationValue = 4;
                break;
            case 3:
                carGOs[3].SetActive(false);
                carGOs[1].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 7;
                accelerationValue = 5;
                break;
            case 4:
                carGOs[4].SetActive(false);
                carGOs[2].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 1;
                accelerationValue = 11;
                break;
            case 5:
                carGOs[5].SetActive(false);
                carGOs[3].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 9;
                accelerationValue = 3;
                break;
            case 6:
                carGOs[6].SetActive(false);
                carGOs[4].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 11;
                accelerationValue = 1;
                break;
            case 7:
                carGOs[7].SetActive(false);
                carGOs[5].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 4;
                accelerationValue = 8;
                break;
            case 8:
                carGOs[8].SetActive(false);
                carGOs[6].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 3;
                accelerationValue = 9;
                break;
            case 9:
                carGOs[9].SetActive(false);
                carGOs[7].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 9;
                accelerationValue = 3;
                break;
            case 10:
                carGOs[10].SetActive(false);
                carGOs[8].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 8;
                accelerationValue = 10;
                break;
            case 11:
                carGOs[11].SetActive(false);
                carGOs[9].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 9;
                accelerationValue = 8;
                break;
            case 12:
                carGOs[0].SetActive(false);
                carGOs[10].SetActive(false);
                carGOs[carTypeNum].SetActive(true);
                carNameText.text = carName[carTypeNum];
                carGOs[carTypeNum].transform.position = mainPosition;
                carGOs[carTypeNum].transform.eulerAngles = mainRotation;
                speedValue = 11;
                accelerationValue = 6;
                break;
        }
    }
    
    void ValueSlideChange(int speedV, int accV)
    {
        speedValueSlider.value = speedV;
        accelerationValueSlider.value = accV;
    }

    //Button 
    public void BackMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void BackButton()
    {
        EquipmentChange.SetActive(false);
        MainEquipment.SetActive(true);
        GameObject firstChild = MainEquipment.transform.GetChild(0).gameObject;
        eventSystem.SetSelectedGameObject(firstChild);
        carGOs[carTypeNum].transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        carGOs[carTypeNum].transform.position = mainPosition;
        carGOs[carTypeNum].transform.eulerAngles = mainRotation;
    }

    public void ConfirmButton()
    {
        MainEquipment.SetActive(false);
        EquipmentChange.SetActive(true);
        GameObject firstChild = EquipmentChange.transform.GetChild(0).gameObject;
        eventSystem.SetSelectedGameObject(firstChild);

        carGOs[carTypeNum].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        carGOs[carTypeNum].transform.position = new Vector3(0.7f, -0.4f, 3);
        carGOs[carTypeNum].transform.eulerAngles = new Vector3(3f, 200f, 0f);

        e_carType = (CarType)carTypeNum + 1;

        intakeTopMesh = carGOs[carTypeNum].transform.Find("Equipment/Intake_top").GetComponent<MeshFilter>().sharedMesh;
        frontWingMesh = carGOs[carTypeNum].transform.Find("Equipment/Front_Wing").GetComponent<MeshFilter>().sharedMesh;
        rearWingMesh = carGOs[carTypeNum].transform.Find("Equipment/Rear_Wing").GetComponent<MeshFilter>().sharedMesh;

        string intakeTopMeshName = intakeTopMesh.name;
        string frontWingMeshName = frontWingMesh.name;
        string rearWingMeshName = rearWingMesh.name;

        intakeTopNum = int.Parse(intakeTopMeshName.Substring(intakeTopMeshName.Length - 1, 1));
        frontWingNum = int.Parse(frontWingMeshName.Substring(frontWingMeshName.Length - 1, 1));
        rearWingNum = int.Parse(rearWingMeshName.Substring(rearWingMeshName.Length - 1, 1));

        PathSearch(carTypeNum + 1);
    }

    void PathSearch(int i)
    {
        switch(i)
        {
            case 1:
                intakeTopPath = "White/White_Engine_Intake_top_";
                frontWingPath = "Gray/Gray_Front_Wing_";
                rearWingPath = "Gray/Gray_Rear_Wing_";
            break;

            case 2:
                intakeTopPath = "Blue/Neonblue_Engine_Intake_top_";
                frontWingPath = "Blue/Neonblue_Front_Wing_";
                rearWingPath = "Blue/Neonblue_Rear_Wing_";
                break;

            case 3:
                intakeTopPath = "Black/Black_Engine_Intake_top_";
                frontWingPath = "Black/Black_Front_Wing_";
                rearWingPath = "Black/Black_Rear_Wing_";
                break;

            case 4:
                intakeTopPath = "Gray/Gray_Engine_Intake_top_";
                frontWingPath = "Gray/Gray_Front_Wing_";
                rearWingPath = "Gray/Gray_Rear_Wing_";
                break;

            case 5:
                intakeTopPath = "Black/Black_Engine_Intake_top_";
                frontWingPath = "Black/Black_Front_Wing_";
                rearWingPath = "Black/Black_Rear_Wing_";
                break;

            case 6:
                intakeTopPath = "Blood/Bloodred_Engine_Intake_top_";
                frontWingPath = "Blood/Bloodred_Front_Wing_";
                rearWingPath = "Blood/Bloodred_Rear_Wing_";
                break;

            case 7:
                intakeTopPath = "Pink/Pink_Engine_Intake_top_";
                frontWingPath = "Pink/Pink_Front_Wing_";
                rearWingPath = "Pink/Pink_Rear_Wing_";
                break;

            case 8:
                intakeTopPath = "Yellow/Yellow_Engine_Intake_top_";
                frontWingPath = "Yellow/Yellow_Front_Wing_";
                rearWingPath = "Yellow/Yellow_Rear_Wing_";
                break;

            case 9:
                intakeTopPath = "Red/Flame_Engine_Intake_top_";
                frontWingPath = "Red/Flame_Front_Wing_";
                rearWingPath = "Red/Flame_Rear_Wing_";
                break;

            case 10:
                intakeTopPath = "Blood/Bloodred_Engine_Intake_top_";
                frontWingPath = "Blood/Bloodred_Front_Wing_";
                rearWingPath = "Blood/Bloodred_Rear_Wing_";
                break;

            case 11:
                intakeTopPath = "Blood/Bloodred_Engine_Intake_top_";
                frontWingPath = "Blood/Bloodred_Front_Wing_";
                rearWingPath = "Blood/Bloodred_Rear_Wing_";
                break;

            case 12:
                intakeTopPath = "White/White_Engine_Intake_top_";
                frontWingPath = "White/White_Front_Wing_";
                rearWingPath = "White/White_Rear_Wing_";
                break;
        }
    }

    public void StartButton()
    {
        carGOs[carTypeNum].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        carGOs[carTypeNum].transform.position = new Vector3(0.7f, -0.4f, 3);
        carGOs[carTypeNum].transform.eulerAngles = new Vector3(3f, 200f, 0f);

        gameManager.carType = e_carType;
        gameManager.intake_top_num = intakeTopNum;
        gameManager.front_wing_num = frontWingNum;
        gameManager.rear_wing_num = rearWingNum;

        gameManager.intake_Top_Path = intakeTopPath;
        gameManager.front_Wing_Path = frontWingPath;
        gameManager.rear_Wing_Path = rearWingPath;

        EquipmentChange.SetActive(false);
        LoadingObject.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        StartCoroutine(DisplayLoadingScreen(2)); 
    }

    public Slider loadingSlider;

    IEnumerator DisplayLoadingScreen(int sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            loadingSlider.value = async.progress;
            yield return null;
        }
    }

    public void TurnOnSound()
    {
        switchButton.volume = 0.6f;
    }
}
