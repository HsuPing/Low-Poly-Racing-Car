using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerControl : MonoBehaviour {

    public GameObject mainCam;
    public GameObject AICam;
    public GameObject AICam2;
    public GameObject SceneView;
    public GameObject SceneView2;
    public GameObject SceneView3;
    public GameObject Canvas;

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCam.SetActive(true);
            AICam.SetActive(false);
            AICam2.SetActive(false);
            SceneView.SetActive(false);
            SceneView2.SetActive(false);
            SceneView3.SetActive(false);
            Canvas.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainCam.SetActive(false);
            AICam.SetActive(true);
            AICam2.SetActive(false);
            SceneView.SetActive(false);
            SceneView2.SetActive(false);
            SceneView3.SetActive(false);
            Canvas.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            mainCam.SetActive(false);
            AICam.SetActive(false);
            AICam2.SetActive(true);
            SceneView.SetActive(false);
            SceneView2.SetActive(false);
            SceneView3.SetActive(false);
            Canvas.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            mainCam.SetActive(false);
            AICam.SetActive(false);
            AICam2.SetActive(false);
            SceneView.SetActive(true);
            SceneView2.SetActive(false);
            SceneView3.SetActive(false);
            Canvas.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            mainCam.SetActive(false);
            AICam.SetActive(false);
            AICam2.SetActive(false);
            SceneView.SetActive(false);
            SceneView2.SetActive(true);
            SceneView3.SetActive(false);
            Canvas.SetActive(false);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            mainCam.SetActive(false);
            AICam.SetActive(false);
            AICam2.SetActive(false);
            SceneView.SetActive(false);
            SceneView2.SetActive(false);
            SceneView3.SetActive(true);
            Canvas.SetActive(false);
        }
    }
}
