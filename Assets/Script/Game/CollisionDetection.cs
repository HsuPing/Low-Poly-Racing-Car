using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection
{
    Collider[] colliders;  //overlap收集的碰撞collider
    int collisionDetectlayerMask;
    Vector3 boxScale;
    public ItemEffect itemEffect;

    int sceneObjectCollisionLayerMask;

    float timer;
    public bool speedTimeBool = false;
    bool carReturn = false;

    public bool carCollision;
    float carCollisonTime;
   
    public float thisCarF;
    public float otherCarF;
    public Vector3 hitV;

    public bool sceneObjectCollision;
    public bool sceneObjColAudio;

    RaycastHit rForwardHit;
    RaycastHit rForwardLeftHit;
    RaycastHit rForwardRightHit;
    RaycastHit rBackHit;
    RaycastHit rBackRightHit;
    RaycastHit rBackLeftHit;
    RaycastHit rLeftHit;
    RaycastHit rRightHit;

    public bool[] hit = new bool[8];
    float[] hitTime = new float[8];

    float carVecity;

    RankManager rankManager;

    public CollisionDetection()
    {
        collisionDetectlayerMask = 1 << LayerMask.NameToLayer("SceneObject") | 1 << LayerMask.NameToLayer("Arrow") | 1 << LayerMask.NameToLayer("Kart");
        sceneObjectCollisionLayerMask = 1 << LayerMask.NameToLayer("SceneObject");
        rankManager = GameObject.Find("Main").GetComponent<RankManager>();
        boxScale = new Vector3(0.65f, 0.45f, 1.22f);
        itemEffect = ItemEffect.NONE;

    }

    public void CollisionObject(Transform carTransform, GameObject selfCar, WeaponOrigin weaponOrigin, RankDetect rankDetect, GroundDection groundDection, CarBase carBase)
    {
        carVecity = carBase.velocityMagnitude;
        colliders = Physics.OverlapBox(carTransform.position + carTransform.up * 0.5f, boxScale, carTransform.rotation, collisionDetectlayerMask);

        foreach (Collider collider in colliders)
        {
            if (itemEffect != ItemEffect.RETURN)
            {
                OnCollider(collider, selfCar, weaponOrigin, rankDetect, groundDection);
            }
        }

        if (speedTimeBool)
        {
            timer += Time.deltaTime;

            if (timer <= 1.5f && (weaponOrigin.oilEffect == false || weaponOrigin.bombEffect == false))
            {
                itemEffect = ItemEffect.SPEEDUP;
       
            }

            else 
            {
                itemEffect = ItemEffect.NONE;
                timer = 0;
                speedTimeBool = false;
            }

            if ( weaponOrigin.oilEffect == true)
            {
                itemEffect = ItemEffect.OIL;
               
          
            }

            else if(weaponOrigin.bombEffect == true)
            {
                itemEffect = ItemEffect.BOMB;
               
          
            }
          
        }

        if(weaponOrigin.oilEffect == true && itemEffect == ItemEffect.NONE)
        {
            itemEffect = ItemEffect.OIL;
        }
        else if(weaponOrigin.oilEffect == false && itemEffect == ItemEffect.OIL)
        {
            itemEffect = ItemEffect.NONE;
        }

        if(weaponOrigin.flyEffect == true && (itemEffect != ItemEffect.BOMB || itemEffect != ItemEffect.OIL || itemEffect != ItemEffect.RETURN))
        {
            itemEffect = ItemEffect.FLY;
            Quaternion targetRotation = Quaternion.LookRotation(rankManager.checkPointList[rankDetect.checkPoint].transform.position - (selfCar.transform.position - new Vector3(0,2.5f,0)));
            selfCar.transform.rotation = Quaternion.Slerp(selfCar.transform.rotation, targetRotation, 5 * Time.deltaTime);
            //selfCar.transform.LookAt(rankManager.checkPointList[rankDetect.checkPoint].transform.position + new Vector3(0, selfCar.transform.position.y, 0));
            //selfCar.transform.forward = Vector3.Lerp(selfCar.transform.forward, rankManager.checkPointList[rankDetect.checkPoint].transform.forward, 0.1f);
        }
        else if(weaponOrigin.flyEffect == false && itemEffect == ItemEffect.FLY) itemEffect = ItemEffect.NONE;

        if (weaponOrigin.bombEffect == true && itemEffect == ItemEffect.NONE) itemEffect = ItemEffect.BOMB;

        else if (weaponOrigin.bombEffect == false && itemEffect == ItemEffect.BOMB) itemEffect = ItemEffect.NONE;
       
        if (itemEffect == ItemEffect.RETURN)
        {

            if (selfCar.transform.position.y < rankDetect.rankManager.checkPointList[rankDetect.checkPoint - 1].transform.position.y + 6 && carReturn == true)
            {
                selfCar.transform.Translate(Vector3.up * 25 * Time.deltaTime, Space.World);
            }

            else
            {
                carReturn = false;
                selfCar.transform.position = Vector3.MoveTowards(selfCar.transform.position, rankDetect.rankManager.checkPointList[rankDetect.checkPoint - 1].transform.position, 1.0f);
                selfCar.transform.rotation = Quaternion.Lerp(selfCar.transform.rotation, rankDetect.rankManager.checkPointList[rankDetect.checkPoint - 1].transform.rotation, 2.0f);
            }

            float carDisBwWayPoint = (selfCar.transform.position - rankDetect.rankManager.checkPointList[rankDetect.checkPoint - 1].transform.position).magnitude;

            if (carDisBwWayPoint < 2)
            {
                itemEffect = ItemEffect.NONE;
            }
        }

        CarCollisionMovement(selfCar, thisCarF, otherCarF);
        SceneObjectCollisionMovemewn(selfCar);

    }

    void OnCollider(Collider col, GameObject selfCar, WeaponOrigin weaponOrigin, RankDetect rankDetect, GroundDection groundDection)
    {
        Vector3 rayOriginal = selfCar.transform.position + selfCar.transform.up * 0.5f;


        if (col.gameObject.tag == "Return")
        {
            itemEffect = ItemEffect.RETURN;
            carReturn = true;
            Debug.Log(selfCar.name + " Fall at " + selfCar.transform.position);
        }

        if (col.gameObject.tag == "Arrow" )
        {
            speedTimeBool = true;
            timer = 0;
        }

        if (col.gameObject.tag == "SceneObject")
        {
            if (Physics.Raycast(rayOriginal, selfCar.transform.forward, out rForwardHit, 1.0f, sceneObjectCollisionLayerMask))
            {
                //撞到正前
                hitTime[0] = 0;
                hit[0] = true;
            }

            if (Physics.Raycast(rayOriginal, -selfCar.transform.forward, out rBackHit, 1.0f, sceneObjectCollisionLayerMask))
            {
                //撞到正後
                hitTime[1] = 0;
                hit[1] = true;
            }

            if (Physics.Raycast(rayOriginal, -selfCar.transform.right, out rLeftHit, 1.2f, sceneObjectCollisionLayerMask))
            {
                //撞到正左邊
                hitTime[2] = 0;
                hit[2] = true;

            }

            if (Physics.Raycast(rayOriginal, selfCar.transform.right, out rRightHit, 1.2f, sceneObjectCollisionLayerMask))
            {
                //撞到正右邊
                hitTime[3] = 0;
                hit[3] = true;

            }

            if (Physics.Raycast(rayOriginal, selfCar.transform.forward - selfCar.transform.right + selfCar.transform.forward, out rForwardLeftHit, 1.5f, sceneObjectCollisionLayerMask))
            {
                //左前方
                hitTime[4] = 0;
                hit[4] = true;
            }

            if (Physics.Raycast(rayOriginal, selfCar.transform.forward + selfCar.transform.right + selfCar.transform.forward, out rForwardRightHit, 1.5f, sceneObjectCollisionLayerMask))
            {
                //右前方
                hitTime[5] = 0;
                hit[5] = true;
            }

            
            if (Physics.Raycast(rayOriginal, -selfCar.transform.forward - selfCar.transform.right - selfCar.transform.forward, out rBackLeftHit, 1.5f, sceneObjectCollisionLayerMask))
            {
                //左後方
                hitTime[6] = 0;
                hit[6] = true;
            }

            if (Physics.Raycast(rayOriginal, -selfCar.transform.forward + selfCar.transform.right - selfCar.transform.forward, out rBackRightHit, 1.5f, sceneObjectCollisionLayerMask))
            {
                //右後方
                hitTime[7] = 0;
                hit[7] = true;
            }
        } 

        if (col.gameObject.layer == 10 && col.gameObject.name != selfCar.name) //車輛碰撞
        {
            carCollision = true;
            carCollisonTime = 0;

            hitV = col.gameObject.transform.position - selfCar.transform.position;
            hitV.Normalize();

            if (selfCar.tag == "Kart")
            {
                thisCarF = selfCar.GetComponent<PlayerCar>().velocity;
                otherCarF = col.gameObject.GetComponent<AICar>().velocity;
            }
            else if (col.tag == "Kart")
            {
                thisCarF = selfCar.GetComponent<AICar>().velocity;
                otherCarF = col.gameObject.GetComponent<PlayerCar>().velocity;
            }
            else
            {
                thisCarF = selfCar.GetComponent<AICar>().velocity;
                otherCarF = col.gameObject.GetComponent<AICar>().velocity;
            }
        }
    }

    void CarCollisionMovement(GameObject thisCar, float thisCarSpeed, float colCarSpeed)
    {

        if(carCollision)
        {
            Vector3 rayOriginal = thisCar.transform.position + thisCar.transform.up * 0.5f;

            bool flBool = Physics.Raycast(rayOriginal, thisCar.transform.forward - thisCar.transform.right + thisCar.transform.forward, 1.8f, 1 << 10);
            bool frBool = Physics.Raycast(rayOriginal, thisCar.transform.forward + thisCar.transform.right + thisCar.transform.forward, 1.8f, 1 << 10);
            bool blBool = Physics.Raycast(rayOriginal, -thisCar.transform.forward - thisCar.transform.right - thisCar.transform.forward, 1.8f, 1 << 10);
            bool brBool = Physics.Raycast(rayOriginal, -thisCar.transform.forward + thisCar.transform.right - thisCar.transform.forward, 1.8f, 1 << 10);

            carCollisonTime += Time.deltaTime;

            if (Mathf.Abs(otherCarF - thisCarF) > 15)
            {
                thisCar.transform.position -= hitV * ((otherCarF - thisCarF)* 1.1F) * Time.deltaTime;
            }
            else
            {
                thisCar.transform.position -= hitV * 10 * Time.deltaTime;
            }

            if (flBool)
            {
                thisCar.transform.Rotate(0, 90 * Time.deltaTime, 0);
            }

            if (frBool)
            {
                thisCar.transform.Rotate(0, -90 * Time.deltaTime, 0);
            }

            if (blBool)
            {
                thisCar.transform.Rotate(0, -120 * Time.deltaTime, 0);
            }

            if (brBool)
            {
                thisCar.transform.Rotate(0, 120 * Time.deltaTime, 0);
            }
            if (carCollisonTime > 0.1f)
            {
                carCollision = false;
            }
        }
    }

    void SceneObjectCollisionMovemewn(GameObject selfCar)
    {
        //正前後的碰撞會使forceSpeed = 0
        sceneObjectCollision = hit[0] || hit[1] ? true : false;
        sceneObjColAudio = hit[0] || hit[1] || hit[2] || hit[3] || hit[4] || hit[5] || hit[6] || hit[7] ? true : false;
        //-------撞到正前方 
        if (hit[0])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[0] += Time.deltaTime;
            selfCar.transform.position -= selfCar.transform.forward * (mSpeed) * Time.deltaTime;

            if (hitTime[0] > 0.1f)
            {
                hit[0] = false;
            }
        }
        //-------撞到正後方
        if (hit[1])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[1] += Time.deltaTime;
            selfCar.transform.position += selfCar.transform.forward * (mSpeed) * Time.deltaTime;
           
            if (hitTime[1] > 0.1f)
            {
                hit[1] = false;
            }
        }
        //-------撞到正左方
        if (hit[2])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 20) mSpeed = 20;
            else mSpeed = carVecity;

            hitTime[2] += Time.deltaTime;
            selfCar.transform.position += selfCar.transform.right * (mSpeed) * Time.deltaTime;
          
            if (hitTime[2] > 0.05f)
            {
                hit[2] = false;
             
            }
        }
        //-------撞到正右方
        if (hit[3])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 20) mSpeed = 20;
            else mSpeed = carVecity;

            hitTime[3] += Time.deltaTime;
            selfCar.transform.position -= selfCar.transform.right * mSpeed * Time.deltaTime;

            if (hitTime[3] > 0.05f)
            {
                hit[3] = false;
            }
        }
        //-------撞到左前方
        if (hit[4])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[4] += Time.deltaTime;
            selfCar.transform.Rotate(0, mSpeed * 3 * Time.deltaTime, 0);

            if (hitTime[4] > 0.12f)
            {
                hit[4] = false;
            }
        }
        //-------撞到右前方
        if (hit[5])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[5] += Time.deltaTime;
            selfCar.transform.Rotate(0, -mSpeed * 3 * Time.deltaTime, 0);

            if (hitTime[5] > 0.12f)
            {
                hit[5] = false;
            }
        }
        //-------撞到左後方
        if (hit[6])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[6] += Time.deltaTime;
            selfCar.transform.Rotate(0, mSpeed * 2.5f * Time.deltaTime, 0);

            if (hitTime[6] > 0.12f)
            {
                hit[6] = false;
            }
        }
        //-------撞到右後方
        if (hit[7])
        {
            float mSpeed = carVecity;
            if (mSpeed < 10) mSpeed = 10;
            else if (mSpeed > 30) mSpeed = 30;
            else mSpeed = carVecity;

            hitTime[7] += Time.deltaTime;
            selfCar.transform.Rotate(0, -mSpeed * 2.5f * Time.deltaTime, 0);

            if (hitTime[7] > 0.12f)
            {
                hit[7] = false;
            }
        }
    }


}



//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f - carTransform.forward - carTransform.right - carTransform.forward, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f - carTransform.forward + carTransform.right - carTransform.forward, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f + carTransform.forward * 1.5f, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f - carTransform.forward * 1.5f, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f + carTransform.right * 1.2f, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f - carTransform.right * 1.2f, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f + carTransform.forward - carTransform.right + carTransform.forward, Color.red);
//Debug.DrawLine(rayOriginal, carTransform.position + carTransform.up * 0.5f + carTransform.forward + carTransform.right + carTransform.forward, Color.red);