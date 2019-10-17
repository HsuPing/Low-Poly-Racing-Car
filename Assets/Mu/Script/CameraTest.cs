using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

    public GameObject PlayerCar;
    float CameraDistance;
    float x;
    //float z = 1;
    float xspeed = 3;
    //float zspeed = 1;
    Vector3 CameraMove;
    Vector3 CameraRotate;
    Quaternion RotationEular;
    
	void Start () {
        CameraMove = gameObject.transform.position - PlayerCar.transform.position;
        //gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        CameraDistance = Vector3.Distance(PlayerCar.transform.position, gameObject.transform.position);

    }
	
	void Update () {

        x += Vector3.Distance(PlayerCar.transform.rotation.eulerAngles, gameObject.transform.rotation.eulerAngles) * xspeed * Time.deltaTime;

        if (x > 360)
        {
            x -= 360;
        }
        else if (x < 0)
        {
            x += 360;
        }


        RotationEular = Quaternion.Euler(0, x, 0);
        CameraMove = RotationEular * new Vector3(0, 0, -CameraDistance) + PlayerCar.transform.position;


        //CameraRotate.x = CameraDistance * Mathf.Cos(-PlayerCar.transform.rotation.x);
        //CameraRotate.z = CameraDistance * Mathf.Sin(-PlayerCar.transform.rotation.z);
        gameObject.transform.rotation = RotationEular;
        gameObject.transform.position = CameraMove;
        //gameObject.transform.position = PlayerCar.transform.position + CameraMove;



        //transform.LookAt(PlayerCar.transform.forward);

    }
}
