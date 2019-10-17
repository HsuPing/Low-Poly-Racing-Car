using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDefense : MonoBehaviour
{
    public float m_fTime;
    AudioSource DefenseSound;


    void Start () {
        m_fTime = 0.0f;
        DefenseSound = this.gameObject.GetComponent<AudioSource>();
        DefenseSound.Play();
        
    }
	
	void Update () {
        this.gameObject.transform.Rotate(0, 5, 0);
        m_fTime += 1.0f;
        //Debug.Log(m_fTime);

	}

    ////防禦
    //public void Defense(GameObject TheCarObject)
    //{
    //    Invoke("UnDefense", 5.0f);
        
    //}

    //public void OnTriggerEnter(Collider other)
    //{
    //}

    ////效果結束
    //public void UnDefense()
    //{
    //    Debug.Log("UnDefense");

    //    Sheild.gameObject.SetActive(false);


    //}

}
