using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject theCutscene;
    public float cutsceneLength;
    public float cutsceneLengthFinal;
    public AudioClip riddlSong;
    public GameObject musicFlick;
    public AudioSource musicHold;

    public void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        theCutscene.SetActive(true);
        StartCoroutine(FinishCut());
        StartCoroutine(FinishCut2());

    }

    IEnumerator FinishCut()
    {
        yield return new WaitForSeconds(cutsceneLength);
        thePlayer.SetActive(true);
    }

    IEnumerator FinishCut2()
    {
        yield return new WaitForSeconds(cutsceneLengthFinal);
        theCutscene.SetActive(false);
        musicFlick.SetActive(true);
        musicHold.clip = riddlSong;
        musicHold.Play();
    }
}
