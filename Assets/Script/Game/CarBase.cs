using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarBase
{
    GameObject thisCar;

    public CarState carState;  //車輛狀態
    public ItemEffect itemEffect;  //道具影響狀態

    public MainForceSpeed mainForceSpeed;  //計算油門力道，取得forceSpeed
    public MainCurrentSteeringAngle mainCurrentSteeringAngle;  //計算轉彎，currentSteeringAngle & currentRadius
    public GroundDection groundDection;  //使車輛與地面狀況判斷
    CollisionDetection collisionDetection;  //碰撞情況判斷
    public WeaponOrigin weaponOrigin;  //道具系統
    public RankDetect rankDetect;  //排名系統
    CarWheel[] carWheels;  //車輪狀態

    Vector3 velocity = Vector3.zero;  //車輛速度向量
    Vector3 lastPosition = Vector3.zero;  //車輛上個frame位置
    public float velocityMagnitude = 0;  //車輛速度

    float minDriftSpeed = 30;  //最低甩尾限速
    bool isDriftingRight = false;  //甩尾方向，右為true
    float driftRatio = 0;  //甩尾選轉車體的比例, for Vecter3.Lerp
    bool setDriftRatio;  //使driftRatio作用的旗標
    public Vector3 carF = Vector3.zero;  //車體前進的向量
    public Vector3 carR = Vector3.zero;  //車體右進的向量

    ParticleManager particleManager;
    AudioManager audioManager;

    public CarBase(GameObject go, float m_maxSpeed, float m_acceleration)
    {
        thisCar = go;
        carState = CarState.PARK;
        carWheels = go.GetComponentsInChildren<CarWheel>();

        mainForceSpeed = new MainForceSpeed(m_maxSpeed, m_acceleration);
        mainCurrentSteeringAngle = new MainCurrentSteeringAngle(carWheels);
        groundDection = new GroundDection(go.transform);
        collisionDetection = new CollisionDetection();
       
        weaponOrigin = go.GetComponent<WeaponOrigin>();
        rankDetect = go.GetComponent<RankDetect>();

        carF = go.transform.forward;
        carR = go.transform.right;

        particleManager = go.GetComponent<ParticleManager>();
        audioManager = new AudioManager(go);
        audioManager.EngineAudio.Play();
     
    }

    public void Movement(InputManager IM) 
    {
        lastPosition = thisCar.transform.position;
        ParticleDetect();
        collisionDetection.CollisionObject(thisCar.transform, thisCar, weaponOrigin, rankDetect, groundDection, this); //碰撞偵測
        itemEffect = collisionDetection.itemEffect;  //道具影響狀態根據collisionDetection的itemEffect決定

        groundDection.GroundAlign(thisCar.transform, itemEffect);  //地面偵測
        weaponOrigin.WeaponCalled(IM);  //使用道具

        //車體狀態判斷-------------------------------
        if (IM.inputS == 1 && IM.inputV > IM.InputThreshold && velocityMagnitude > minDriftSpeed  &&(IM.inputH != 0 && driftRatio == 1 || carState == CarState.DRIFT) && itemEffect != ItemEffect.FLY )
        {
            MaintainDriftSpeed(IM); //進入甩尾狀態的transition過度
        }

        else if ((mainForceSpeed.forceSpeed > 0 && IM.bBack) || (mainForceSpeed.forceSpeed < 0 && IM.bFront))
        {
            carState = CarState.BRAKE;
            
        }

        else if ((mainForceSpeed.forceSpeed < 0 && IM.bBack))
        {
            carState = CarState.REVERSE;
        }

        else if (velocityMagnitude == 0 && IM.inputV == 0)
        {
            carState = CarState.PARK;
        }

        else
        {
            carState = CarState.DRIVE;
        }
        //--------------------------------------------
        mainForceSpeed.CalForceSpeed(IM, carState, groundDection, itemEffect, weaponOrigin, collisionDetection);  //計算forceSpeed
        float forceSpeed = mainForceSpeed.forceSpeed;  //此forceSpeed如果正前或正後有碰撞，其為0, 其餘則取決於mainForceSpeed.forceSpeed

        mainCurrentSteeringAngle.CalculateSteeringAngle(IM, mainForceSpeed, carState, isDriftingRight, itemEffect);  //計算CurrentSteeringAngle
        UpdateCarPosition(thisCar.transform, forceSpeed, mainCurrentSteeringAngle.currentSteeringAngle);  //更改車體位置
        updateWheelData();  //使輪胎取得選轉資訊
        //AudioEffect();
    }

    void MaintainDriftSpeed(InputManager IM)  //進入甩尾狀態的transition過度
    {
        if (carState != CarState.DRIFT)
        {
            audioManager.DriftAudio.Play();
            isDriftingRight = IM.inputH > 0;  //isDriftingRight由進入這個過渡時的IM.inputH判定(右為true, 左為false)
            carState = CarState.DRIFT;
            driftRatio = 0;  //初始化 driftRatio
            setDriftRatio = false;  
        }
    }

    void UpdateCarPosition(Transform car, float forceSpeed, float currentSteeringAngle)  //位置更新，直行需給forceSpeed，有轉彎需給forceSpeed和currentSteeringAngle。
    {
        Vector3 driftVector = isDriftingRight ? car.forward - car.right : car.forward + car.right;  //甩尾時的直行目標向量
        Vector3 driftRigheVector = isDriftingRight ? car.forward + car.right : car.forward - car.right;  //甩尾時的右行目標向量
        driftVector.Normalize();
        driftRigheVector.Normalize();

        //-----使直行、右行目標向量轉換時的平滑效果--------
        if (carState == CarState.DRIFT)
        {
            driftRatio += Time.deltaTime/7;
            if (driftRatio > 1) driftRatio = 1;
            carF = Vector3.Lerp(carF, driftVector, driftRatio);
            carR = Vector3.Lerp(carR, driftRigheVector, driftRatio);
        }

        else if (carState != CarState.DRIFT)
        {
            audioManager.DriftAudio.Stop();
            if (setDriftRatio == false)
            {
                driftRatio = 0;
                setDriftRatio = true;
            }

            if (setDriftRatio)
            {
                driftRatio += Time.deltaTime*2;
                if (driftRatio > 1) driftRatio = 1;
                carF = Vector3.Lerp(carF, car.forward, driftRatio);
                carR = Vector3.Lerp(carR, car.right, driftRatio);
            }
        }
        //--------------------------------------------------------------

        //---------------------直行或轉彎的位置計算--------------------------------------
        if (currentSteeringAngle == 0)  //直行
        {
            car.position += carF * forceSpeed * Time.deltaTime;
        }
        else //轉彎或甩尾的位置處理
        {
            mainCurrentSteeringAngle.currentRadius = mainCurrentSteeringAngle.CalculateSteeringRadius(carWheels, car.transform); //圓點到物體中心的距離，圓半徑
            float neg = currentSteeringAngle > 0 ? 1 : -1;  //車體左右選轉判斷
            float circumference = 2 * Mathf.PI * mainCurrentSteeringAngle.currentRadius;  //此選轉半徑的圓周長
            float distanceToTravel = forceSpeed * Time.deltaTime;  //前進距離
            float percentRevolutions = distanceToTravel / circumference; //前進距離佔此周長比例
            float degreesToTravel = percentRevolutions * 360f;  //行使距離所佔此圓旋轉度數

            float rightToTravel = mainCurrentSteeringAngle.currentRadius * (1 - Mathf.Cos(degreesToTravel * Mathf.PI / 180));  //向右移動距離
            float forwardToTravel = mainCurrentSteeringAngle.currentRadius * Mathf.Sin(degreesToTravel * Mathf.PI / 180);  //向前移動距離

            if (carState == CarState.DRIFT)  //如果在甩尾狀態
            {
                //driftVector = isDriftingRight ? car.forward - car.right : car.forward + car.right;
                //driftRigheVector = isDriftingRight ? car.forward + car.right : car.forward - car.right;

                //driftVector.Normalize();
                //driftRigheVector.Normalize();

               // Vector3 moveF = Vector3.Lerp(car.transform.forward, driftVector, driftRatio);
                //Vector3 moveR = Vector3.Lerp(car.transform.right, driftRigheVector, driftRatio);

                if (isDriftingRight) car.Rotate(0, 10 * Time.deltaTime, 0);
                else car.Rotate(0, -10 * Time.deltaTime, 0);

                car.Translate(carR * rightToTravel * neg, Space.World);
                car.Translate(carF * forwardToTravel, Space.World);
            }
            else
            {
                car.Translate(carR * rightToTravel * neg, Space.World);
                car.Translate(carF * forwardToTravel, Space.World);
            }

            car.Rotate(new Vector3(0, degreesToTravel * neg, 0));  //車體需選轉度數
            //Debug.Log(degreesToTravel * neg);
        }
        //-------------------------------------------------------------------------
        //真實速度計算-----
        Vector3 lastVelocity = velocity;
        velocity = (car.position - lastPosition) / Time.deltaTime;
        velocityMagnitude = velocity.magnitude;
        //--------------------
    }

    void updateWheelData()  //根據車體選轉角度，決定輪胎選轉幅度
    {
        foreach (CarWheel w in carWheels)
        {
            w.state = carState;
            if (w.steers)
                w.steeringAngle = mainCurrentSteeringAngle.currentSteeringAngle;
        }
    }

    void ParticleDetect()
    {
        if (carState == CarState.BRAKE)
        {
            particleManager.TiresSmokes[0].Play();
            particleManager.TiresSmokes[1].Play();
        }
        else
        {
            particleManager.TiresSmokes[0].Stop();
            particleManager.TiresSmokes[1].Stop();
        }

        if (mainForceSpeed.flameParticle)
        {
            for (int i = 0; i < particleManager.SpeedUpEngineFires.Length; i++)
            {
                particleManager.SpeedUpEngineFires[i].Play();
            }
        }
        else
        {
            for (int i = 0; i < particleManager.SpeedUpEngineFires.Length; i++)
            {
                particleManager.SpeedUpEngineFires[i].Stop();
            }
        }

        if (carState == CarState.DRIFT)
        {
            if (mainForceSpeed.driftPower <= 2.0f && !particleManager.DriftSparks[0].isPlaying)
            {
                for (int i = 0; i <= 1; i++)
                {
                    particleManager.DriftSparks[i].Play();
                    particleManager.DriftSparksRender[i].material = particleManager.DriftSparkMaterialY;
                    particleManager.DriftSparks[i].emissionRate = 60;
                    particleManager.DriftSparks[i].startSize = 0.02f;
                    particleManager.DriftSparks[i].startLifetime = 0.1f;
                    
                }
            }
            else if (mainForceSpeed.driftPower > 2.0f && mainForceSpeed.driftPower <= 4.0f)
            {
                for (int i = 0; i <= 1; i++)
                {
                    particleManager.DriftSparks[i].emissionRate = 70;
                    particleManager.DriftSparks[i].startSize = 0.04f;
                    particleManager.DriftSparks[i].startLifetime = 0.15f;
                   
                }
            }

            else if (mainForceSpeed.driftPower > 3.2f && mainForceSpeed.driftPower <= 5.5f)
            {
                for (int i = 0; i <= 1; i++)
                {
                    particleManager.DriftSparksRender[i].material = particleManager.DriftSparkMaterialR;
                    particleManager.DriftSparks[i].emissionRate = 80;
                    particleManager.DriftSparks[i].startSize = 0.05f;
                    particleManager.DriftSparks[i].startLifetime = 0.2f;
                  
                }
            }

            else if (mainForceSpeed.driftPower > 5.5f)
            {
                for (int i = 0; i <= 1; i++)
                {
                    particleManager.DriftSparksRender[i].material = particleManager.DriftSparkMaterialP;
                    particleManager.DriftSparks[i].emissionRate = 80;
                    particleManager.DriftSparks[i].startSize = 0.05f;
                    particleManager.DriftSparks[i].startLifetime = 0.2f;
                   
                }
            }
        }
        else if(carState != CarState.DRIFT)
        {
            particleManager.DriftSparks[0].Stop();
            particleManager.DriftSparks[1].Stop();
        }

    }
        
     void AudioEffect()
    {
        audioManager.EngineAudio.pitch = (Mathf.Abs(mainForceSpeed.forceSpeed)/((mainForceSpeed.maxSpeed)+20)) + 0.5f;
        if(collisionDetection.carCollision)
        {
            if(!audioManager.CarCollisionAudio.isPlaying) audioManager.CarCollisionAudio.PlayOneShot(audioManager.CarCollisionAudio.clip);
        }

        if(collisionDetection.sceneObjColAudio)
        {
            if(!audioManager.CarCollisionSceneObjectAudio.isPlaying) audioManager.CarCollisionSceneObjectAudio.PlayOneShot(audioManager.CarCollisionSceneObjectAudio.clip);
        }
    }
}

[System.Serializable]
public class MainForceSpeed
{
    float acceleration;  //加速度
    public float maxSpeed;  //最大限速
    float minSpeed = -30;  //倒車最大限速
    public float forceSpeed = 0; //給予油門速度
    float brakeStrength = 2.0f;     //煞車減速
    float dragStrength = 0.6f;      //自然減速
    public float driftPower = 0;  //甩尾時累積的能量值
    public float speedUpTime = 1.5f;  //甩尾能量達標時，加速的時間
    public bool flameParticle;  //噴射特效旗標

    public MainForceSpeed(float m_maxSpeed, float m_acceleration)
    {
        maxSpeed = m_maxSpeed;
        acceleration = m_acceleration;
    }

    public void CalForceSpeed(InputManager IM, CarState carState, GroundDection groundDection, ItemEffect itemEffect, WeaponOrigin weaponOrigin, CollisionDetection collisionDetection)  //計算forceSpeed
    {
        //判斷甩尾能量的累積和初始化---------------------------------------
        if (carState == CarState.DRIFT && speedUpTime == 1.5f && IM.bSpace && itemEffect == ItemEffect.NONE)
        {
            driftPower += Time.deltaTime;
        }
        else if (driftPower < 2.0f || IM.bBack || (carState == CarState.DRIFT && speedUpTime != 0) || itemEffect != ItemEffect.NONE)
        {
            //if (forceSpeed > maxSpeed - 20 && driftPower > 0 && driftPower < 2.0f) forceSpeed = forceSpeed - 5;   //甩尾能量未達標減速懲罰
            driftPower = 0;
            speedUpTime = 1.5f;
        }
        //--------------------------------------------------------------
        if (collisionDetection.sceneObjectCollision) forceSpeed = 0;  
        //-------forceSpeed在各個狀態下的優先集及計算
        if (itemEffect != ItemEffect.NONE)  //道具影響狀態
        {
            ItemMovement(itemEffect);
        }

        else if (weaponOrigin.itemOneSpeedUp)  //加速道具
        {
            flameParticle = true;
            forceSpeed += 2;
            if (forceSpeed >= maxSpeed + 25) forceSpeed = maxSpeed + 25;
        }

        else if (driftPower > 2.0f && IM.bBack == false && speedUpTime > 0.0f && carState != CarState.DRIFT)  //甩尾能量達標推進狀態
        {
            speedUpTime -= Time.deltaTime;
            DriftPowerSpeed(driftPower);

            if (speedUpTime <= 0)
            {
                speedUpTime = 1.5f;
                driftPower = 0;
                flameParticle = false;
            }
        }

        else if (IM.bFront == false && IM.bBack == false || groundDection.onGround == false)  //自然減速
        {
            SlowDown(false);
            flameParticle = false;
        }

        else if ((forceSpeed > 0 && IM.bBack) || (forceSpeed < 0 && IM.bFront))  //煞車減速
        {
            SlowDown(true);
            flameParticle = false;
        }

        else if ((IM.inputV > IM.InputThreshold || IM.inputV < -IM.InputThreshold) && groundDection.onGround)  //一般加速
        {
            Accelerate(IM, carState, groundDection);
            flameParticle = false;
        }
    }

    void ItemMovement(ItemEffect itemEffect)  //道具影響優先級和forceSpeed計算
    {
        if (itemEffect == ItemEffect.RETURN)  //拉回跑到
        {
            forceSpeed = 0;
            flameParticle = false;
        }

        else if (itemEffect == ItemEffect.BOMB)  //被炸彈炸
        {
            forceSpeed = Mathf.Lerp(forceSpeed, 0, 0.1f);
            flameParticle = false;
        }

        else if (itemEffect == ItemEffect.OIL)  //油瓶影響
        {
            flameParticle = false;
            if (forceSpeed > 0)
            {
                forceSpeed -= 100 * Time.deltaTime;
                if (forceSpeed < 0) forceSpeed = 0;
            }
            else if (forceSpeed < 0)
            {
                forceSpeed += 50 * Time.deltaTime;
                if (forceSpeed > 0) forceSpeed = 0;
            }
            else
            {
                forceSpeed = 0;
            }
        }

        else if (itemEffect == ItemEffect.SPEEDUP)//加速
        {
            flameParticle = true;
            forceSpeed += 40 * Time.deltaTime;
            if (forceSpeed >= maxSpeed + 25) forceSpeed = maxSpeed + 25;
            
        }

        else if(itemEffect == ItemEffect.FLY)
        {
            flameParticle = true;
            forceSpeed += 40 * Time.deltaTime;
            if(forceSpeed >= maxSpeed + 35) forceSpeed = maxSpeed + 35;
        }
    }

    void SlowDown(bool isBraking = false)   //一般減速及煞車forceSpeed計算
    {
        float decel = (isBraking ? brakeStrength : dragStrength);

        if (forceSpeed > 0)
        {
            forceSpeed -= decel;
            if (forceSpeed < 0)
            {
                forceSpeed = 0;
            }
        }

        else if (forceSpeed < 0)
        {
            forceSpeed += decel;
            if (forceSpeed > 0)
            {
                forceSpeed = 0;
            }
        }
    }

    void Accelerate(InputManager IM, CarState carState, GroundDection groundDection)  //一般、甩尾狀態forceSpeed計算
    {
        float speedGuide = maxSpeed;
        float speedUp = acceleration * IM.inputV;

        if (groundDection.onTrack == false && forceSpeed > maxSpeed / 2)
        {
            SlowDown(true);
        }

        else if (IM.inputH == 0 && carState != CarState.DRIFT)  //一般直行
        {
            forceSpeed += speedUp;

            if (forceSpeed > maxSpeed) SlowDown();
            if (forceSpeed <= minSpeed) forceSpeed = minSpeed;
        }

        else if (IM.inputH != 0 && carState != CarState.DRIFT)  //一般轉彎
        {
            speedGuide = maxSpeed - 2.0f;
            if (forceSpeed > speedGuide)
            {
                forceSpeed -= speedUp * 0.05f;
                if (forceSpeed < speedGuide) forceSpeed = speedGuide;
            }

            else
            {
                forceSpeed += speedUp * 0.9f;
                if (forceSpeed > speedGuide) forceSpeed = speedGuide;//SlowDown();
            }
        }

        else if (carState == CarState.DRIFT && groundDection.onTrack == true)  //甩尾狀態forceSpeed計算
        {
            speedGuide = maxSpeed - 10.0f;  //甩尾最大限速

            if(forceSpeed > maxSpeed)
            {
                SlowDown(true);
            }
            else if (forceSpeed > maxSpeed - 5)
            {
                forceSpeed -= speedUp * 0.1f;
                if (forceSpeed < maxSpeed - 5)
                {
                    forceSpeed = maxSpeed - 5;
                }
            }
            else if (IM.inputH != 0 && forceSpeed > speedGuide)
            {
                forceSpeed -= speedUp * 0.2f;
                if (forceSpeed < speedGuide) forceSpeed = speedGuide;
            }
            else if(IM.inputH == 0 && forceSpeed >= speedGuide || forceSpeed < speedGuide)
            {
                forceSpeed += speedUp * 0.3f;
            }
        }
    }

    void DriftPowerSpeed(float driftPower)  //甩尾能量達標forceSpeed計算
    {
        if (driftPower > 5.5f)
        {
            flameParticle = true;
            forceSpeed += 15 * Time.deltaTime;
            if (forceSpeed >= maxSpeed + 25) forceSpeed = maxSpeed + 25;
      
        }

        else if (driftPower <= 5.5f && driftPower > 3.2f)
        {
            flameParticle = true;
            forceSpeed += 15 * Time.deltaTime;
            if (forceSpeed >= maxSpeed + 22) forceSpeed = maxSpeed + 22;

        }

        else
        {
            flameParticle = true;
            forceSpeed += 15 * Time.deltaTime;
            if (forceSpeed >= maxSpeed + 18) forceSpeed = maxSpeed + 18;
      
        }
    }
}

[System.Serializable]
public class MainCurrentSteeringAngle
{
    public float currentSteeringAngle;  //旋轉每偵角度
    public float innerRadius;
    float maxSteeringAngleSlow;  //低速轉彎幅度
    float maxSteeringAngleFast;  //高速轉彎幅度
    float driftSteeringRange = 0.25f;  //甩尾時轉彎調整幅度
    float currentDriftDelta; //driftSteeringRange * inputH, -0.25 ~ 0.25
    float axleDistance = 1.05f;  //車體長
    public float currentRadius;
    public Vector3 currentCircleCenter = Vector3.zero;

    private float calculateAxleDistance(CarWheel[] carWheels)
    {
        float low = 777, high = 777;

        foreach (CarWheel w in carWheels)
        {
            float z = w.transform.position.x;
            if (low == 777 || z < low)
                low = z;
            if (high == 777 || z > high)
                high = z;
        }
        return high - low;
    }

    public MainCurrentSteeringAngle(CarWheel[] carWheels)
    {
        maxSteeringAngleSlow = 2.2f;
        maxSteeringAngleFast = 0.4f;
        axleDistance = calculateAxleDistance(carWheels);
    }

    public void CalculateSteeringAngle(InputManager IM, MainForceSpeed mainForceSpeed, CarState carState, bool isDriftingRight, ItemEffect itemEffect)
    {
        float rotateScale = Mathf.Clamp(Mathf.Abs(mainForceSpeed.forceSpeed) / mainForceSpeed.maxSpeed, 0, 1);  //速度與最大速之比例

        if (carState == CarState.DRIFT)  //甩尾狀態轉彎
        {
            currentSteeringAngle = (isDriftingRight ? 1 : -1) * (rotateScale * (maxSteeringAngleFast - maxSteeringAngleSlow) + maxSteeringAngleSlow);
            currentDriftDelta = IM.inputH * driftSteeringRange;
            currentSteeringAngle = currentSteeringAngle + currentDriftDelta;
            if (IM.inputS != 1) IM.inputH = 0;//限制離開甩尾狀態後再按空白鍵不會在回到甩尾狀態
        }
        else
        {
            currentSteeringAngle = IM.inputH *  (rotateScale * (maxSteeringAngleFast - maxSteeringAngleSlow) + maxSteeringAngleSlow);
        }

        if (currentSteeringAngle == 0)
        {
            currentRadius = 0;
            innerRadius = 0;
        }
    }

    public float CalculateSteeringRadius(CarWheel[] carWheels, Transform car)  //算出旋轉軸心到車體距離
    {
        innerRadius = (axleDistance / 2) / Mathf.Cos((90-currentSteeringAngle) * Mathf.PI / 180); //前輪到轉心半徑
        int index = currentSteeringAngle > 0 ? 1 : 0;  //右轉以前右輪為起始點計算，反之
        CarWheel w = carWheels[index];  
        currentCircleCenter = w.transform.position + w.transform.forward * innerRadius;  //旋轉點到輪胎向量
        return (currentCircleCenter - car.transform.position).magnitude;  //旋轉點到車體中心距離 
    }
}




