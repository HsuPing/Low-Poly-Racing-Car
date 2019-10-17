using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {

    private const float MaxSpeedAngle = -20;
    private const float ZeroSpeedAngle = 210;

    private Transform PointerTransform;
    private Transform SpeedLabelTemplateTransform;

    private float Speed;
    private float SpeedMax;

    PlayerCar playerCar;

    void Start()
    {
        playerCar = GameObject.FindGameObjectWithTag("Kart").GetComponent<PlayerCar>();
        PointerTransform = transform.Find("Pointer");
        SpeedLabelTemplateTransform = transform.Find("SpeedLabelTemplate");
        SpeedLabelTemplateTransform.gameObject.SetActive(false);
        Speed = 0f; //速度最小值
        SpeedMax = 180f; //速度最大值
        CreateSpeedLabel();
    }

    private void Update ()
    {
        Speed = playerCar.velocity * 2;
        PointerTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
	}

    private void CreateSpeedLabel() //時速器條碼
    {
        int LabelAmount = 9; //速度碼的數量, 0除外
        float TotalAngleSize = ZeroSpeedAngle - MaxSpeedAngle;

        for (int i = 0; i <= LabelAmount; i++)
        {
            Transform SpeedLabelTransform = Instantiate(SpeedLabelTemplateTransform, transform); //生成速度碼欄位
            float LabelSpeedNormalized = (float)i / LabelAmount;
            float SpeedLabelAngle = ZeroSpeedAngle - LabelSpeedNormalized * TotalAngleSize; //計算出速度碼編排的位置
            SpeedLabelTransform.eulerAngles = new Vector3(0, 0, SpeedLabelAngle);
            SpeedLabelTransform.Find("SpeedText").GetComponent<Text>().text = Mathf.RoundToInt(LabelSpeedNormalized * SpeedMax).ToString();
            //上面是四捨五入後，計算出速度碼，並依序排列
            SpeedLabelTransform.Find("SpeedText").eulerAngles = Vector3.zero; //將速度碼角度調正
            SpeedLabelTransform.gameObject.SetActive(true);
        }
    }

    private float GetSpeedRotation() //指針旋轉的角度
    {
        float TotalAngleSize = ZeroSpeedAngle - MaxSpeedAngle;  //230
        float SpeedNormalized = Speed / SpeedMax;   //比例
        return ZeroSpeedAngle - SpeedNormalized * TotalAngleSize;
    }
}
