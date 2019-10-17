using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDection{

    int groundDetectlayerMask;  //設定射線只對layer為Track和Grass影響
    //------取車體四個角周圍四個點----------
    public GameObject frontLeftPoint;
    public GameObject frontRightPoint;
    public GameObject backLeftPoint;
    public GameObject backRightPoint;
    //--------------------------------------
    //-------射線集中資訊-------------------
    RaycastHit centerHit;
    public RaycastHit frontLeftHit;
    public RaycastHit frontRightHit;
    public RaycastHit backLeftHit;
    public RaycastHit backRightHit;
    //--------------------------------------
    public Vector3 _upNew;  //總和四點法向量
    Quaternion _newRot;  

    float groundAlignRayLength;  //使車體平行地面的射線長度
    float centerRayLength;       //車體中心向下射線長度
    float gravity;  //地吸引力

    public bool onTrack;
    public bool onGround;

    public GroundDection(Transform go)
    {
        groundDetectlayerMask = 1 << LayerMask.NameToLayer("Track") | 1 << LayerMask.NameToLayer("Grass");
        groundAlignRayLength = 4.5f;
        centerRayLength = 1.15f;
        gravity = 15.0f;
        //為車體產生周圍四個點，並設定其位置
        frontLeftPoint = new GameObject("frontLeftPoint");
        frontRightPoint = new GameObject("frontRightPoint");
        backLeftPoint = new GameObject("backLeftPoint");
        backRightPoint = new GameObject("backRightPoint");

        frontLeftPoint.transform.SetParent(go);
        frontRightPoint.transform.SetParent(go);
        backLeftPoint.transform.SetParent(go);
        backRightPoint.transform.SetParent(go);

        frontLeftPoint.transform.localPosition = new Vector3(-0.5f, 1, 1);
        frontLeftPoint.transform.localRotation = Quaternion.identity;
        frontRightPoint.transform.localPosition = new Vector3(0.5f, 1, 1);
        frontRightPoint.transform.localRotation = Quaternion.identity;

        backLeftPoint.transform.localPosition = new Vector3(-0.5f, 1, -1);
        backLeftPoint.transform.localRotation = Quaternion.identity;
        backRightPoint.transform.localPosition = new Vector3(0.5f, 1, -1);
        backRightPoint.transform.localRotation = Quaternion.identity;
    }

    public void GroundAlign(Transform car, ItemEffect itemEffect)
    {
        Physics.Raycast(frontLeftPoint.transform.position, Vector3.down, out frontLeftHit, groundAlignRayLength, groundDetectlayerMask);
        Physics.Raycast(frontRightPoint.transform.position, Vector3.down, out frontRightHit, groundAlignRayLength, groundDetectlayerMask);
        Physics.Raycast(backLeftPoint.transform.position, Vector3.down, out backLeftHit, groundAlignRayLength, groundDetectlayerMask);
        Physics.Raycast(backRightPoint.transform.position, Vector3.down, out backRightHit, groundAlignRayLength, groundDetectlayerMask);

        //_upNew = (Vector3.Cross(backRightHit.point - Vector3.up, backLeftHit.point - Vector3.up) + Vector3.Cross(backLeftHit.point - Vector3.up, frontLeftHit.point - Vector3.up) +
        //     Vector3.Cross(frontLeftHit.point - Vector3.up, frontRightHit.point - Vector3.up) + Vector3.Cross(frontRightHit.point - Vector3.up, backRightHit.point - Vector3.up)).normalized;

        Vector3 a = backRightHit.point - backLeftHit.point;
        Vector3 b = frontRightHit.point - backRightHit.point;
        Vector3 c = frontLeftHit.point - frontRightHit.point;
        Vector3 d = backLeftHit.point - frontLeftHit.point;

        Vector3 crossBA = Vector3.Cross(b, a);
        Vector3 crossCB = Vector3.Cross(c, b);
        Vector3 crossDC = Vector3.Cross(d, c);
        Vector3 crossAD = Vector3.Cross(a, d);

        _upNew = (crossBA + crossCB + crossDC + crossAD).normalized;

        if (itemEffect == ItemEffect.FLY) centerRayLength = 5.0f;
        else centerRayLength = 1.15f;

        onGround = Physics.Raycast(car.position + car.up, -car.up, out centerHit, centerRayLength, groundDetectlayerMask);
        if (itemEffect == ItemEffect.RETURN) return;
        else if (onGround)
        {
            float centerHitY;
            if (itemEffect == ItemEffect.FLY) centerHitY = centerHit.point.y + 3f;
            else centerHitY = centerHit.point.y - 0.01f;

            car.position = Vector3.Lerp(car.position, new Vector3(car.position.x, centerHitY, car.position.z), 0.65f);  //車體離地面高度
            _newRot = Quaternion.LookRotation(Vector3.Cross(car.right, _upNew).normalized, _upNew);
            car.rotation = Quaternion.Slerp(car.rotation, _newRot, 0.5f);
            onTrack = centerHit.collider.tag == "track" ? true : false;
        }
       
        else if(onGround == false)
        {
            car.transform.rotation = Quaternion.Lerp(car.transform.rotation,new Quaternion(0, car.transform.rotation.y,0, car.transform.rotation.w) , 0.1f);
            car.position += Vector3.down * gravity * Time.deltaTime;
        }
    }
}
