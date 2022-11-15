using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public PlayerTest playertest;

    public Drill drill;

    //bool keyboardOnOff = false;

    //bool DrillOnOff = false;

    public AudioSource theAudio;

    [SerializeField]
    private AudioClip[] clip;

    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void KeyboardOnoff(bool Onoff)
    {
        if (Onoff)
        {
            theAudio.clip = clip[0];
            theAudio.Play();
        }
        else
        {
            theAudio.clip = clip[0];
            theAudio.Stop();
        }
    }

    private void DrillOnoff(bool Onoff)
    {
        if (Onoff)
        {
            theAudio.clip = clip[1];
            theAudio.Play();
        }
        else
        {
            theAudio.clip = clip[1];
            theAudio.Stop();
        }
    }

    private void DrillBroken(bool Onoff)
    {
        if (Onoff)
        {
            theAudio.clip = clip[2];
            theAudio.Play();
        }
        else
        {
            theAudio.clip = clip[2];
            theAudio.Stop();
        }
    }
}
