using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class NightSwitch : MonoBehaviour
{
    public Animator nightSwitcher;
    public float cycleTime;
    [Header("Music")]
    public AudioMixer musicHere;
    public float musicSpeedPercent;
    private float musicSpeed = 1;

    //private void Awake()
    //{
    //    Invoke("NightCycle", cycleTime);
    //}
    private void Update()
    {
        //InputNightSwitch();
        musicHere.SetFloat("MusicPitch", musicSpeedPercent);
    }

    //private void InputNightSwitch()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        Debug.Log("fucked");
    //        if (nightSwitcher.GetBool("Nighter"))
    //        {
    //            Debug.Log("true");
    //            nightSwitcher.SetBool("Nighter", false);
    //        }
    //        else
    //        {
    //            Debug.Log("false");
    //            nightSwitcher.SetBool("Nighter", true);
    //        }
    //    }
    //}

    private void NightCycle() //not of the bike variant
    {
        if (nightSwitcher.GetBool("Nighter"))
        {
            nightSwitcher.SetBool("Nighter", false);
        }
        else
        {
            nightSwitcher.SetBool("Nighter", true);
        }
        Invoke("NightCycle", cycleTime +5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        nightSwitcher.SetBool("Nighter", true);
    }
}
