using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager{

    public AudioSource EngineAudio;
    public AudioSource DriftAudio;
    public AudioSource CarCollisionAudio;
    public AudioSource CarCollisionSceneObjectAudio;

    AudioClip CarCollisionAudioClip;
    AudioClip CarEngineAudioClip;
    AudioClip CarCollisionSceneObjectAudioClip;

    public AudioManager(GameObject go)
    {
        EngineAudio = go.transform.GetChild(2).transform.GetChild(0).GetComponent<AudioSource>();
        DriftAudio = go.transform.GetChild(2).transform.GetChild(1).GetComponent<AudioSource>();
        CarCollisionAudio = go.transform.GetChild(2).transform.GetChild(2).GetComponent<AudioSource>();
        CarCollisionSceneObjectAudio = go.transform.GetChild(2).transform.GetChild(7).GetComponent<AudioSource>();

        CarCollisionAudioClip = Resources.Load<AudioClip>("Audios/carhit0");
        CarCollisionAudio.clip = CarCollisionAudioClip;
        CarEngineAudioClip = Resources.Load<AudioClip>("Audios/carEngine");
        EngineAudio.clip = CarEngineAudioClip;
        CarCollisionSceneObjectAudioClip = Resources.Load<AudioClip>("Audios/carhit3");
        CarCollisionSceneObjectAudio.clip = CarCollisionSceneObjectAudioClip;

        EngineAudio.Stop();
        DriftAudio.Stop();
        CarCollisionAudio.Stop();

        CarCollisionAudio.spatialBlend = 1;
        CarCollisionAudio.volume = 0.7f;

        CarCollisionSceneObjectAudio.spatialBlend = 1;
        CarCollisionSceneObjectAudio.volume = 0.6f;
        CarCollisionSceneObjectAudio.maxDistance = 20;

        EngineAudio.rolloffMode = AudioRolloffMode.Linear;
        EngineAudio.maxDistance = 20;
        EngineAudio.reverbZoneMix = 1;
        EngineAudio.spatialBlend = 1f;
        EngineAudio.volume = 1.5f;

        DriftAudio.rolloffMode = AudioRolloffMode.Linear;
        DriftAudio.maxDistance = 20;
        DriftAudio.volume = 0.75f;
        DriftAudio.spatialBlend = 1.1f;
    }
}
