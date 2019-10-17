using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdEffect : MonoBehaviour {

    ParticleSystem Bird;
    bool BirdOn;

	// Use this for initialization
	void Start () {
        Bird = this.gameObject.GetComponent<ParticleSystem>();
        BirdOn = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        Bird.Play();
        BirdOn = false;
        Invoke("BirdOnTime", 10.0f);
    }


    void BirdOnTime()
    {
        BirdOn = true;
    }
}
