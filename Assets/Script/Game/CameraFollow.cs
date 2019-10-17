using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    GameObject Target;
    AllSceneManager allSceneManager;

    public float height = 2.75f;
    public float rotatonX = 22;
    public float disbwCar = 4.25f;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Kart");
        allSceneManager = GameObject.Find("Main").GetComponent<AllSceneManager>();
        height = 2.6f;
        rotatonX = 17.25f;
        disbwCar = 5;
    }

    private void FixedUpdate()
    {
        if (allSceneManager.EndGame != true)
        {
            float wantedRotationAngleY = Target.transform.eulerAngles.y;
            float wantedRotationAngleZ = Target.transform.eulerAngles.z;
            float wantedRotationAngleX = Target.transform.eulerAngles.x;
            float wantedHeight = Target.transform.position.y + height;

            float currentRotationAngleY = this.transform.eulerAngles.y;
            float currentRotationAngleZ = this.transform.eulerAngles.z;
            float currentRotationAngleX = this.transform.eulerAngles.x;
            float currentHeight = this.transform.position.y;

            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 25 * Time.deltaTime);
            currentRotationAngleY = Mathf.LerpAngle(currentRotationAngleY, wantedRotationAngleY, 20 * Time.deltaTime);
            currentRotationAngleZ = Mathf.LerpAngle(currentRotationAngleZ, wantedRotationAngleZ, 4 * Time.deltaTime);
            currentRotationAngleX = Mathf.LerpAngle(currentRotationAngleX, wantedRotationAngleX + rotatonX, 4 * Time.deltaTime);

            this.transform.position = Vector3.Lerp(this.transform.position, Target.transform.position - Target.transform.forward * disbwCar, 0.8f);
            this.transform.position = new Vector3(this.transform.position.x, currentHeight, this.transform.position.z);

            this.transform.eulerAngles = new Vector3(currentRotationAngleX, currentRotationAngleY, currentRotationAngleZ);
        }
        else if(allSceneManager.EndGame == true)
        {
            var wantedRotationAngle = Target.transform.eulerAngles.y;
            var wantedHeight = Target.transform.position.y + 2.4f;

            var currentRotationAngle = transform.eulerAngles.y;
            var currentHeight = transform.position.y;
         

            // Damp the rotation around the y-axis
            //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 30 * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 20 * Time.deltaTime);

            // Convert the angle into a rotation
            var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            transform.position = Target.transform.position;
            transform.position -= currentRotation * Vector3.forward * 7;

            // Set the height of the camera
            transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

            // Always look at the target
            transform.LookAt(Target.transform);
        }
    }

}

