using UnityEngine;

public class AICar : MonoBehaviour {

    InputManager inputManager;
    public CarBase carBase;
    public AIFunction aiFunction;
    public float velocity;
    public float speedValue = 50;
    public float accValue = 0.25f;
    AllSceneManager allSceneManager;
    public int CarTypeNum = 0;

    private void Awake()
    {
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();
    }

    void Start()
    {     
        inputManager = new InputManager();
        carBase = new CarBase(this.gameObject, speedValue, accValue);
        aiFunction = new AIFunction(this.gameObject);    
    }

    void FixedUpdate()
    {
        if (allSceneManager.StartGame)
        {
            inputManager.InputKey(false);
        }
        aiFunction.AIMovement(inputManager, carBase.rankDetect, velocity, carBase.groundDection, carBase.weaponOrigin);
        carBase.Movement(inputManager);
        velocity = carBase.velocityMagnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(this.transform.position + new Vector3(0, 0.5f, 0), this.transform.forward * 6);

    }
}
