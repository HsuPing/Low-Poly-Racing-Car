using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour
{

    public GameObject wheelMesh;
    public bool steers = false;

    public float wheelRadius;

    private CarState _state;
    public CarState state
    {
        set { _state = value; }
        get { return _state; }
    }

    private Vector3 lastPosition;
    private float _steeringAngle;
    private float wheelRotation = 0;

    public float steeringAngle { set { _steeringAngle = value; } }

    Skidmarks skidmarksController;
    RaycastHit wheelHitInfo;
    int lastSkid = -1;
    float intensity;
    Vector3 dist;

    void Start()
    {
        skidmarksController = GameObject.Find("Main").GetComponent<Skidmarks>();
        wheelMesh = gameObject.transform.GetChild(0).gameObject;
        if (wheelMesh.tag == "FrontWheel")
        {
            steers = true;
            wheelRadius = 0.25f;
        }
        else
        {
            wheelRadius = 0.32f;
        }
    }

    void FixedUpdate()
    {

        dist = transform.position - lastPosition;  //輪胎每個frame移動距離
        float fullRotationDistance = 2 * Mathf.PI * wheelRadius;  //輪胎圓周長
        float distFraction = dist.magnitude / fullRotationDistance;  //移動距離佔其圓周比例

        SkidMark();

        if (state == CarState.REVERSE)  //倒車時輪胎反向旋轉
        {
            distFraction *= -1;
        }

        wheelRotation += distFraction * 360 % 360;  //每個frame旋轉的度數

        float steerNormalize;
        if (_steeringAngle > 0)
        {
            steerNormalize = 1;
        }
        else if (_steeringAngle < 0)
        {
            steerNormalize = -1;
        }
        else
        {
            steerNormalize = 0;
        }

        if (steers)
        {
            //前輪
            wheelMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, steerNormalize * Mathf.Sqrt(Mathf.Abs(_steeringAngle)) * 30, wheelRotation));
            transform.localRotation = Quaternion.Euler(new Vector3(0, _steeringAngle, 0));
        }
        else
        {
            //後輪
            wheelMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, wheelRotation));
        }
        lastPosition = transform.position;
    }

    void SkidMark()
    {
        if (wheelMesh.tag != "FrontWheel" && (state == CarState.DRIFT || state == CarState.BRAKE))
        {
            if (Physics.Raycast(this.transform.position, Vector3.down, out wheelHitInfo, 1.0f, 1 << 16 | 1 << 8))
            {
                intensity = Mathf.MoveTowards(intensity, 0.7f, 0.04f);
                Vector3 lastVelocity = lastPosition;
                Vector3 skidPoint = wheelHitInfo.point + dist;
                lastSkid = skidmarksController.AddSkidMark(skidPoint, wheelHitInfo.normal, intensity, lastSkid);
            }        
        }
        else
        {
            lastSkid = -1;
            intensity = 0;
        }
    }
}

