using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMode {NONE ,BATTLE, TIMING};
public enum MapSelect { NONE, MOUNTAIN}
public enum CarType { NONE = 0, HONOR, SPACE, KNIGHT, REPORT, DEAD, VIKING, PINK, ANGEL, DEVIL, DRUID, BLOODDRUID, ICEDRUID}

public class GameManager : MonoBehaviour {

    static GameManager instance;
    public PlayerMode playerMode;
    public MapSelect mapSelect;
    public CarType carType;

    public string intake_Top_Path;
    public string front_Wing_Path;
    public string rear_Wing_Path;

    public int intake_top_num;
    public int front_wing_num;
    public int rear_wing_num;

    public float[] carsSpeed = {0, 50, 52, 51, 45, 53, 55, 48, 47, 53, 52, 53, 55};
    public float[] carsAcc = {0, 0.25f, 0.23f, 0.24f, 0.3f, 0.24f, 0.2f, 0.27f, 0.28f, 0.23f, 0.29f, 0.27f, 0.25f};

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            name = "GameManager";
        }
        else if(this != instance)
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        //playerMode = PlayerMode.NONE;
        //mapSelect = MapSelect.NONE;
        //carType = CarType.NONE;
    }

}
