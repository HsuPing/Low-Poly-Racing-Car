using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIFunction {

    GameObject carAI;
    GameObject carPlay;

    public GameObject targetObject;      //目標物
    public GameObject nextTargetObject;
    public GameObject previousTargetObject;//下個目標

    Vector3 targetPoint;                 //目標物位置
    Vector3 previousTargetPoint;         //上個物標物件位置
    Vector3 nextTargetPoint;             //下個目標物位置

    Vector3 vectorBwTarget;
    Vector3 nextVectorArea;

    public float disBwPlayer;
    public float dotBwTarget;     

    public float disBwTarget;            //與目標距離
    public float dotWithTarget;          //與目標內積，判斷前後
    public float disBwNextTarget;        //與下個目標物距離
    public float dotTargetWithNextTarget;//與下個目標物內積

    public float dotBwTargetArea;
    Vector3 crossBwTargetArea;
    public float crossYBwTargetArea;
    Vector3 crossBwNextTarget;
    public float crossBwNextTargetY;
    //--------------------------
    Vector3 crossWithTarget;             //與目標物外積，判斷左右
    Vector3 crossWithNextTarget;         //與下個目標物外積，判斷左右
    public float crossY;
    public float crossNextY;

    float shootTime = 0;
    RaycastHit frontHit;
    RaycastHit checkPointHit;
    bool carDetect;
    bool checkPointRay;
    public int checkPointAINum;
    public int preCheckPointAINum;
    public int nextCheckPointAINum;
    RankDetect rankDetect;
    RankDetect playerRank;
    AICar aICar;

    float Todtime = 0;

    public AIFunction(GameObject m_AIcar)
    {
        carAI = m_AIcar;
        aICar = carAI.GetComponent<AICar>();
        carPlay = GameObject.Find("PlayerCar");
        rankDetect = m_AIcar.GetComponent<RankDetect>();
        playerRank = carPlay.GetComponent<RankDetect>();
        checkPointAINum = rankDetect.checkPoint;
    }

    public void AIMovement(InputManager IM, RankDetect rankDetect, float velocity, GroundDection groundDection, WeaponOrigin weaponOrigin)
    {
        //carDetect = Physics.Raycast(carAI.transform.position + new Vector3(0, 0.5f, 0), carAI.transform.forward, out frontHit, 6.0f,  1 << 11 );
        //checkPointRay = Physics.Raycast(carAI.transform.position + new Vector3(0, 0.5f, 0), carAI.transform.forward, out checkPointHit, 4.0f, 1 << 12);
        CheckPointDetect(rankDetect);

        disBwPlayer = (carAI.transform.position - carPlay.transform.position).magnitude;
        //if(checkPointAINum == 0)
        //{
        //    IM.bFront = true; IM.bRight = false; IM.bRight = false;
        //}
        //else if (checkPointAINum==2 && velocity > 40)
        //{
        //    Debug.Log("do this");
        //    IM.bRight = true;
        //}

        //else if (checkPointAINum == 17 && velocity > 40)
        //{
        //    Debug.Log("do this drift");
        //    IM.bRight = true;
        //    IM.bSpace = true;
        //    //Todtime += Time.deltaTime;
        //    //if(Todtime > 0.2f)
        //    //{
        //    //    IM.bSpace = true;
        //    //}
        //}

        //else
        //{
        //Todtime = 0;
        //}

        if ((disBwPlayer > 250 ||(rankDetect.rank > playerRank.rank + 3 && disBwPlayer > 30)) &&  aICar.carBase.itemEffect == ItemEffect.NONE)
        {
            AICheating();
            IM.bFront = false;
            IM.bSpace = false;
            IM.bRight = false;
            IM.bLeft = false;
            IM.bBack = false;
        }
        else
        {
            InputDecide(IM, velocity, groundDection);
        }


        ItemDetect(IM, rankDetect, carAI, weaponOrigin);
    }

    void CheckPointDetect(RankDetect rankDetect)
    {
        //----------------------------------------------------------------------------
        checkPointAINum = rankDetect.checkPoint;
        targetObject = rankDetect.rankManager.checkPointList[checkPointAINum];
        targetPoint = rankDetect.rankManager.checkPointList[checkPointAINum].transform.position;

        preCheckPointAINum = checkPointAINum - 1;
        if (preCheckPointAINum == -1)
        {
            previousTargetObject = rankDetect.rankManager.checkPointList[rankDetect.rankManager.checkPointList.Length-1];
            previousTargetPoint = rankDetect.rankManager.checkPointList[rankDetect.rankManager.checkPointList.Length-1].transform.position;
        }
        else
        {
            previousTargetObject = rankDetect.rankManager.checkPointList[preCheckPointAINum];
            previousTargetPoint = rankDetect.rankManager.checkPointList[preCheckPointAINum].transform.position;
        }

        nextCheckPointAINum = checkPointAINum + 1;
        if (nextCheckPointAINum == rankDetect.rankManager.checkPointList.Length)
        {
            nextTargetObject = rankDetect.rankManager.checkPointList[0];
            nextTargetPoint = rankDetect.rankManager.checkPointList[0].transform.position;
        }
        else
        {
            nextTargetObject = rankDetect.rankManager.checkPointList[nextCheckPointAINum];
            nextTargetPoint = rankDetect.rankManager.checkPointList[nextCheckPointAINum].transform.position;
        }

        vectorBwTarget = targetPoint - previousTargetPoint;
        vectorBwTarget.Normalize();
        dotBwTargetArea = Vector3.Dot(carAI.transform.forward, vectorBwTarget);

        crossBwTargetArea = Vector3.Cross(carAI.transform.forward, vectorBwTarget);
        crossYBwTargetArea = crossBwTargetArea.y;

        nextVectorArea = nextTargetPoint - targetPoint;
        crossBwNextTarget = Vector3.Cross(carAI.transform.forward, nextVectorArea);
        crossBwNextTargetY = crossBwNextTarget.y;
        //----------------------------------------------------------------------------
        //if(checkPointRay)
        //{

        //    if (frontHit.collider.gameObject.name == rankDetect.rankManager.checkPointList[checkPointAINum].name)
        //    {

        //        checkPointAINum = checkPointAINum + 1;
        //        if(checkPointAINum == rankDetect.rankManager.checkPointList.Length)
        //        {
        //            checkPointAINum = 0;
        //        }
        //    }
        //}


        //if (rankDetect.checkPoint == 55)
        //{
        //    nextTargetObject = rankDetect.rankManager.checkPointList[0];
        //    nextTargetPoint = rankDetect.rankManager.checkPointList[0].transform.position;
        //}
        //else
        //{
        //    nextTargetObject = rankDetect.rankManager.checkPointList[rankDetect.checkPoint + 1];
        //    nextTargetPoint = rankDetect.rankManager.checkPointList[rankDetect.checkPoint + 1].transform.position;
        //}

        Vector3 toTarget = targetPoint - carAI.transform.position;
        disBwTarget = toTarget.magnitude;
        Vector3 toTargetNormalize = toTarget.normalized;

        Vector3 toNextTarget = nextTargetPoint - carAI.transform.position;
        disBwNextTarget = toNextTarget.magnitude;
        Vector3 toNextTargetNormalize = toNextTarget.normalized;
        dotWithTarget = Vector3.Dot(carAI.transform.forward, toTargetNormalize);

        dotTargetWithNextTarget = Vector3.Dot((carAI.transform.position - targetPoint).normalized, (targetPoint - nextTargetPoint).normalized);

        crossWithTarget = Vector3.Cross(carAI.transform.forward, toTargetNormalize);
        crossY = crossWithTarget.y;

        crossWithNextTarget = Vector3.Cross(carAI.transform.forward, toNextTargetNormalize);
        crossNextY = crossWithNextTarget.y;
    }

    void InputDecide(InputManager IM, float velocity, GroundDection groundDection)
    {
        //if(dotWithTarget > 0)
        //{
        //    IM.bFront = true;
        //    if(checkPointRay != true)
        //    {
        //        if (crossY < 1 && crossY > 0.1 && disBwTarget > 10) //cross 0.1 - 1之間
        //        {
        //            IM.bRight = true;
        //            IM.bLeft = false;
        //        }
        //        else if (crossY > -1 && crossY < -0.1 && disBwTarget > 10)
        //        {
        //            IM.bRight = false;
        //            IM.bLeft = true;
        //        }
        //    }

        //    else if(checkPointRay == true && velocity > 40)
        //    {
        //        if(crossBwNextTargetY < 1 && crossBwNextTargetY > 0)
        //        {
        //                IM.bRight = true;
        //                IM.bLeft = false;
        //            if(crossBwNextTargetY > 0.6f)
        //            {
        //                IM.bSpace = true;
        //            }
        //        }
        //        else if(crossBwNextTargetY > -1 && crossBwNextTargetY < 0)
        //        {
        //            IM.bRight = false;
        //            IM.bLeft = true;
        //            if (crossBwNextTargetY < -0.6f)
        //            {
        //                IM.bSpace = true;
        //            }
        //        }
        //    }

            
        //}

        if(disBwTarget < 15.5f && velocity > 40)
        {
            if (crossNextY < 1 && crossNextY > 0.1 && !IM.bSpace) //cross 0.1 - 1之間
            {
                IM.bRight = true;
                IM.bLeft = false;
                if (carAI.name == "AI1")
                {
                    Debug.Log("AI R");
                }
                //
            }
            else if (crossNextY > -1 && crossNextY < -0.1 && !IM.bSpace)
            {
                IM.bRight = false;
                IM.bLeft = true;
                if (carAI.name == "AI1")
                {
                    Debug.Log("AI L");
                }
            }
            else
            {
                IM.bRight = false;
                IM.bLeft = false;
            }


            if ((crossNextY > 0.6f && crossNextY < 0.8f) || (crossNextY < -0.6f && crossNextY > -0.8) && velocity > 43 && (IM.bLeft||IM.bRight))
            {
                IM.bSpace = true;
                if (crossY < 1 && crossY > 0.1) //cross 0.1 - 1之間
                {
                    IM.bRight = true;
                    IM.bLeft = false;
                    if (carAI.name == "AI1")
                    {
                        Debug.Log("AI R");
                    }
                    //
                }
                else if (crossY > -1 && crossY < -0.1)
                {
                    IM.bRight = false;
                    IM.bLeft = true;
                    if (carAI.name == "AI1")
                    {
                        Debug.Log("AI L");
                    }
                }
                //if (carAI.name == "AI1")
                //{
                //    Debug.LogError("do drift");
                //        }
                //Debug.Log("Drift");
            }

            else if ((crossNextY > 0.9f && crossNextY < 1) || (crossNextY < -0.9f && crossNextY > -1))
            {
                IM.bFront = false;
                IM.bSpace = false;
            }
            else
            {
                IM.bFront = true;
                IM.bSpace = false;
                //Debug.Log("Drift False");
            }
        }

        else if (dotWithTarget > 0 && (disBwTarget >= 15.5f|| velocity < 40))
        {
            if (crossY < 1 && crossY > 0.1 && disBwTarget > 10) //cross 0.1 - 1之間
            {
                IM.bRight = true;
                IM.bLeft = false;
                //Debug.Log("AI R");
            }
            else if (crossY > -1 && crossY < -0.1 && disBwTarget > 10)
            {
                IM.bRight = false;
                IM.bLeft = true;
                //Debug.Log("AI L");
            }
            else
            {
                IM.bRight = false;
                IM.bLeft = false;
            }


            if ((crossY > 0.6f && crossY < 0.9f) || (crossY < -0.6f && crossY > -0.9) && velocity > 40)
            {
                IM.bSpace = true;
                //Debug.Log("Drift");
            }

            else if ((crossY > 0.9f && crossY < 1) || (crossY < -0.9f && crossYBwTargetArea > -1))
            {
                IM.bFront = false;
               
            }
            else if(dotTargetWithNextTarget > 0.8f)
            {
                IM.bSpace = false;
            }
            else
            {
                IM.bFront = true;
                //IM.bSpace = false;
                //Debug.Log("Drift False");
            }
        }
        else
        {
            IM.bFront = false;
        }

        if (velocity < 20)
        {
            IM.bFront = true;
        }

        else if(dotWithTarget<0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetObject.transform.position - carAI.transform.position);
            carAI.transform.rotation = Quaternion.Slerp(carAI.transform.rotation, targetRotation, 5 * Time.deltaTime);
            //if (carAI.name == "AI1")
            //{
            //    Debug.LogError("dotWithTarget<0 ");
            //}
        }

    }

    void ItemDetect(InputManager IM, RankDetect rankDetect, GameObject car, WeaponOrigin weaponOrigin)
    {
        if (weaponOrigin.WeaponQueue[0] == 1)
        {
            if (dotWithTarget > 0.8f && dotWithTarget < 1 && disBwTarget > 45) IM.bShift = true;
        }
        else if (weaponOrigin.WeaponQueue[0] == 2)
        {
            shootTime += Time.deltaTime;
            if (shootTime > 3)
            {
                IM.bShift = true;
                shootTime = 0;
            }
            else
            {
                IM.bShift = false;
            }
        }
        else if (weaponOrigin.WeaponQueue[0] == 3)
        {
            IM.bShift = true;
        }
        else if (weaponOrigin.WeaponQueue[0] == 4)
        {
                IM.bShift = true;
        }
        else if (weaponOrigin.WeaponQueue[0] == 5)
        {
            IM.bShift = true;
        }
        else if (weaponOrigin.WeaponQueue[0] == 6)
        {
            shootTime += Time.deltaTime;
            if (shootTime > 3 && carDetect)
            {
                IM.bShift = true;
                shootTime = 0;
            }
            else
            {
                IM.bShift = false;
            }
          
        }
        else if (weaponOrigin.WeaponQueue[0] == 7)
        {
            IM.bShift = true;
        }
        else if (weaponOrigin.WeaponQueue[0] == 8)
        {
            IM.bShift = true;
        }
        else
        {
            IM.bShift = false;
            shootTime = 0;
        }
    }

   void AICheating()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetObject.transform.position - carAI.transform.position);
        carAI.transform.rotation = Quaternion.Slerp(carAI.transform.rotation, targetRotation, 5 * Time.deltaTime);
        carAI.transform.position += carAI.transform.forward * 45 * Time.deltaTime;
    }

}


