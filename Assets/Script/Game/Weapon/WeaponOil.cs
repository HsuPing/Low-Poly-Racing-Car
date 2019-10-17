using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOil : MonoBehaviour
{

    GameObject OilObject;
    public GameObject OilOut;
    private float m_fTime;
    AudioSource GlassSound;
    //AudioSource BreakSound;
    MeshRenderer OilMesh;

    void Start()
    {
        m_fTime = 0.0f;
        GlassSound = gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        //BreakSound = gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        GlassSound.Play();
        Invoke("GlassSoundStop", 1.0f);
    }

    void Update()
    {
        if (m_fTime > 200.0f)
        {
            this.gameObject.SetActive(false);
        }
        m_fTime += Time.deltaTime;
    }

    public void Oil(GameObject TheCarObject)
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        //BreakSound.Play();
        OilMesh = this.gameObject.GetComponent<MeshRenderer>();
        OilMesh.enabled = false;
        Invoke("OilOff", 1.0f);
    }

    void OilOff()
    {
        this.gameObject.SetActive(false);
    }

    void GlassSoundStop()
    {
        GlassSound.Stop();
    }


}
