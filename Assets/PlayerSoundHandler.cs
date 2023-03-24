using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundHandler : MonoBehaviour
{
    public AudioSource sfxSource;
    public bool soundIsPlaying;
    [Header("SFX")]
    public AudioClip footstep;
    public AudioClip magicShoot;
    public AudioClip jump;
    public AudioClip groundHit;
    public AudioClip dash;

    public IEnumerator PlaySFXwDelay(AudioClip sound, float repeatDelay, bool randomPitch)
    {
        soundIsPlaying = true;
        if (randomPitch == true)
        {
            sfxSource.pitch = Random.Range(0.80f, 1.20f);
            sfxSource.PlayOneShot(sound);
        }
        else
        {
            sfxSource.pitch = 1;
            sfxSource.PlayOneShot(sound);
        }
        yield return new WaitForSeconds(repeatDelay);
        soundIsPlaying = false;
        yield break;
    }
}
