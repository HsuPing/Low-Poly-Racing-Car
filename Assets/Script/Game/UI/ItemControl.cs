using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Reel
{
    public Image[] ImageArray;
}

public class ItemControl : MonoBehaviour
{
    public GameObject Reel1 = null;
    public GameObject Reel2 = null;
    Reel[] ReelArray = null;  
    public Animator ani1;
    public Animator ani2;
    public Rank _RankParameter;
    public WeaponOrigin weaponOrigin;
    
    public GameObject Weapon1_Icon;
    public GameObject Weapon2_Icon;
    public GameObject IconButtom;
    bool firstFull;

    Text Weapon1_UseTimes;

    void Start()
    {
        weaponOrigin = GameObject.Find("PlayerCar").GetComponent<WeaponOrigin>();
        _RankParameter = (Rank)Resources.Load("ScriptableObject/RankParameter");
        Weapon1_UseTimes = GetComponentInChildren<Text>();
        ReelArray = new Reel[2];
        ReelArray[0].ImageArray = Reel1.GetComponentsInChildren<Image>();
        ReelArray[1].ImageArray = Reel2.GetComponentsInChildren<Image>();
        IconButtom = this.gameObject.transform.GetChild(0).gameObject;
        Weapon1_Icon = this.gameObject.transform.GetChild(1).gameObject;
        Weapon2_Icon = this.gameObject.transform.GetChild(2).gameObject;
        IconButtom.SetActive(false);
        Weapon1_Icon.SetActive(false);
        Weapon2_Icon.SetActive(false);
        Weapon1_UseTimes.gameObject.SetActive(false);
    }

    void Update()
    {

        if (weaponOrigin.WeaponQueue[0] == 0)
        {
            Weapon1_Icon.SetActive(false);
            IconButtom.SetActive(false);
            firstFull = false;
        }

        if (weaponOrigin.itemPlay == true && weaponOrigin.WeaponQueue[0] != 0 && firstFull == false)
        {
            Weapon1_Icon.SetActive(true);
            IconButtom.SetActive(true);
            ani1.Play("Roll");
        }

        else if (weaponOrigin.itemPlay == false && weaponOrigin.WeaponQueue[0] != 0 && firstFull == false)
        {
            firstFull = true;
            GetItem(0, weaponOrigin.WeaponQueue[0]);
        }

        if (weaponOrigin.WeaponQueue[1] == 0) Weapon2_Icon.SetActive(false);
        else if (weaponOrigin.itemPlay == true && weaponOrigin.WeaponQueue[1] != 0 && firstFull == true)
        {
            Weapon2_Icon.SetActive(true);
            ani2.Play("Roll");
        }

        else if (weaponOrigin.itemPlay == false && weaponOrigin.WeaponQueue[1] != 0 && firstFull == true)
        {
            GetItem(1, weaponOrigin.WeaponQueue[1]);
        }

        if(weaponOrigin.WeaponQueue[0] != 0)
        {
            GetItem(0, weaponOrigin.WeaponQueue[0]);
        }

        WeaponUI();
    }

    public void GetItem(int ReelIndex, int Rank)
    {
        ReelArray[ReelIndex].ImageArray[0].sprite = _RankParameter.RankInformations[Rank]._Sprite;
    }
   
    void WeaponUI()
    {
        if (weaponOrigin.WeaponOn == true)
        {
            Weapon1_UseTimes.gameObject.SetActive(true);
            Weapon1_UseTimes.text = weaponOrigin.UsingTimes.ToString();
        }

        if (weaponOrigin.WeaponOn == true  && weaponOrigin.WeaponQueue[0] == 0 && weaponOrigin.WeaponQueue[1] == 0)
        {
            Weapon1_UseTimes.gameObject.SetActive(false);
        }
    }

    
}

