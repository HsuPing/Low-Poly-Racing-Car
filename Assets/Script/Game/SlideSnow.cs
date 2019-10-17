using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideSnow : MonoBehaviour {

    Animator SnowAnimation;
    float m_fTime;


	// Use this for initialization
	void Start () {
        SnowAnimation = this.gameObject.GetComponent<Animator>();
        m_fTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        m_fTime += Time.deltaTime;
        if(m_fTime >= 100.0f)
        {
            SnowAnimation.SetBool("StartSlide", true);
            Debug.Log("SnoeStartSlide");
        }
	}
}
