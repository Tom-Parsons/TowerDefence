using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{

    public static MusicController instance;

    public AudioMixerSnapshot buildMode;
    public AudioMixerSnapshot battleMode;

    private float transitionIn;
    private float transitionOut;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionToBattle()
    {
        battleMode.TransitionTo(3);
    }

    public void TransitionToBuild()
    {
        buildMode.TransitionTo(3);
    }

}
