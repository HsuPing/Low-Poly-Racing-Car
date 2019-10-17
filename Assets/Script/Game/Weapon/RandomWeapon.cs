using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeapon : MonoBehaviour {

    public GameObject WeaponBox;
    WeaponOrigin WP;
    public GameObject WeaponItems;
    bool WPD;
    public bool RW;
    ParticleSystem Explos;
    Collider m_Collider;
    AudioSource TriggerSound;

    private void Awake()
    {
        Explos = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        Explos.Pause();
        m_Collider = GetComponent<Collider>();
        TriggerSound = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        this.gameObject.transform.Rotate(1,1,1);
    }


    //碰撞偵測
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kart" || other.tag == "AI")
        {
            Explos.Play();
            TriggerSound.Play();
            m_Collider.enabled = false;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            WPD = false;
            RW = true;
            if (WPD == false)
            {
                Invoke("WeaponBoxInstantiate", 2.0f);
            }

        }
    }

    //道具箱重生
    void WeaponBoxInstantiate()
    {
        m_Collider.enabled = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        Explos.Stop();
        WPD = true;
    }



}
