using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    InputManager inputManager;
    public CarBase carBase;
    Camera mainCamera;
    AIFunction aIFunction;
    AllSceneManager allSceneManager;
    public float speedValue = 50;
    public float accValue = 0.25f;

    public float velocity;

    private void Awake()
    {
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();  
    }

    void Start()
    {
        inputManager = new InputManager();
        carBase = new CarBase(this.gameObject, speedValue, accValue);
        aIFunction = new AIFunction(this.gameObject);
    }

    void FixedUpdate()
    {
        if (allSceneManager.StartGame)
        {
            if (carBase.rankDetect.lapCount < 4)
            {
                inputManager.InputKey(true);
            }
            else
            {
                inputManager.InputKey(false);  
                aIFunction.AIMovement(inputManager, carBase.rankDetect, velocity, carBase.groundDection, carBase.weaponOrigin);
            }
        }

        carBase.Movement(inputManager);
        velocity = carBase.velocityMagnitude;
    }

    void cameraMove()
    {
        float wantedRotationAngleY = this.transform.eulerAngles.y;
        float wantedRotationAngleZ = this.transform.eulerAngles.z;
        float wantedHeight = this.transform.position.y + 2.6f;

        float currentRotationAngleY = mainCamera.transform.eulerAngles.y;
        float currentRotationAngleZ = mainCamera.transform.eulerAngles.z;
        float currentHeight = mainCamera.transform.position.y;

        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 25 * Time.deltaTime);
        currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, 20 * Time.deltaTime);
        currentRotationAngleZ = Mathf.LerpAngle(currentRotationAngleZ, wantedRotationAngleZ, 5 * Time.deltaTime);
        
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, this.transform.position - this.transform.forward * 4f, 0.6f);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, currentHeight, mainCamera.transform.position.z);

         mainCamera.transform.eulerAngles = new Vector3(20, currentRotationAngleY, currentRotationAngleZ);
         mainCamera.transform.eulerAngles = new Vector3(20, currentRotationAngleY, currentRotationAngleZ);
       
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(carBase.groundDection.backRightHit.point, carBase.groundDection.backLeftHit.point);
        //Gizmos.DrawLine(carBase.groundDection.frontRightHit.point, carBase.groundDection.backRightHit.point);
        //Gizmos.DrawLine(carBase.groundDection.frontLeftHit.point, carBase.groundDection.frontRightHit.point);
        //Gizmos.DrawLine(carBase.groundDection.backLeftHit.point, carBase.groundDection.frontLeftHit.point);

        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(carBase.groundDection.frontLeftPoint.transform.position, carBase.groundDection.frontLeftHit.point);
        //Gizmos.DrawLine(carBase.groundDection.frontRightPoint.transform.position, carBase.groundDection.frontRightHit.point);
        //Gizmos.DrawLine(carBase.groundDection.backLeftPoint.transform.position, carBase.groundDection.backLeftHit.point);
        //Gizmos.DrawLine(carBase.groundDection.backRightPoint.transform.position, carBase.groundDection.backRightHit.point);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(carBase.groundDection.backRightHit.point, Vector3.Cross(carBase.groundDection.frontRightHit.point - carBase.groundDection.backRightHit.point, carBase.groundDection.backRightHit.point - carBase.groundDection.backLeftHit.point));
        //Gizmos.DrawRay(carBase.groundDection.frontRightHit.point, Vector3.Cross(carBase.groundDection.frontLeftHit.point - carBase.groundDection.frontRightHit.point, carBase.groundDection.frontRightHit.point - carBase.groundDection.backRightHit.point));
        //Gizmos.DrawRay(carBase.groundDection.frontLeftHit.point, Vector3.Cross(carBase.groundDection.backLeftHit.point - carBase.groundDection.frontLeftHit.point, carBase.groundDection.frontLeftHit.point - carBase.groundDection.frontRightHit.point));
        //Gizmos.DrawRay(carBase.groundDection.backLeftHit.point, Vector3.Cross(carBase.groundDection.backRightHit.point - carBase.groundDection.backLeftHit.point, carBase.groundDection.backLeftHit.point - carBase.groundDection.frontLeftHit.point));

        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(this.transform.position, carBase.groundDection._upNew * 3);
    }
}
