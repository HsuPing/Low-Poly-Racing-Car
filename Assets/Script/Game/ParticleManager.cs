using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public ParticleSystem[] SpeedUpEngineFires;
    public ParticleSystem[] TiresSmokes;
    public ParticleSystem[] DriftSparks;
    public ParticleSystemRenderer[] DriftSparksRender;
 
    public Material DriftSparkMaterialY;
    public Material DriftSparkMaterialR;
    public Material DriftSparkMaterialP;

    private void Start()
    {
        SpeedUpEngineFires = this.transform.GetChild(1).transform.GetChild(0).GetComponentsInChildren<ParticleSystem>();
        TiresSmokes = this.transform.GetChild(1).transform.GetChild(5).GetComponentsInChildren<ParticleSystem>();
        DriftSparks = this.transform.GetChild(1).transform.GetChild(6).GetComponentsInChildren<ParticleSystem>();
        DriftSparksRender = this.transform.GetChild(1).transform.GetChild(6).GetComponentsInChildren<ParticleSystemRenderer>();
       
        DriftSparkMaterialY = Resources.Load<Material>("Material/SparksYellow");
        DriftSparkMaterialR = Resources.Load<Material>("Material/SparksRed");
        DriftSparkMaterialP = Resources.Load<Material>("Material/SparksPurple");
    }
}
