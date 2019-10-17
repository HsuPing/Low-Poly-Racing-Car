using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTNT : MonoBehaviour
{
    //GameObject TNTBomb;
    //public GameObject TNTOut;
    private float m_fTime;

    GameObject UsingCar;
    AudioSource PutDownSound;

    void Start()
    {
        m_fTime = 0.0f;
        PutDownSound = this.gameObject.GetComponent<AudioSource>();
        PutDownSound.Play();

    }

    void Update()
    {

        if (m_fTime > 200.0f)
        {
            this.gameObject.SetActive(false);
        }
        m_fTime += Time.deltaTime;



    }

    public void TNT(GameObject TheCarObject)
    {

        UsingCar = TheCarObject;

    }

    //撞到車輛判定消失
    public void OnTriggerEnter(Collider other)
    {

        this.gameObject.SetActive(false);
       
    }
    

}
