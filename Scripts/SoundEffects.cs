using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{

    private AudioSource audioPlayer;

    void Start()
    {
      audioPlayer = GetComponent<AudioSource>();
    }

    public void PlaySoundByte(AudioClip source) {
      audioPlayer.clip = source;
      audioPlayer.Play();
    }

    public void StopSoundByte() {
      audioPlayer.Stop();
    }
}
