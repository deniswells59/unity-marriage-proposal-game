using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
  // Refs
  AudioSource myAudioSource;
  float startVolume;

  private void Start() {
    myAudioSource = GetComponent<AudioSource>();
    startVolume = myAudioSource.volume;

    StartCoroutine(FadeIn(1f));
  }

  public void PlayMusic() {
    if (!myAudioSource.isPlaying) {
      myAudioSource.Play();
      myAudioSource.volume = 1f;
    }
  }

  public IEnumerator FadeOutPause () {
    while (myAudioSource.volume > 0) {
        myAudioSource.volume -= startVolume * Time.deltaTime / 0.8f;
        yield return null;
    }

    myAudioSource.Pause();
  }

  public IEnumerator FadeOut(float FadeTime) {
    while (myAudioSource.volume > 0) {
        myAudioSource.volume -= startVolume * Time.deltaTime / FadeTime;
        yield return null;
    }
    myAudioSource.Stop();
  }

  public IEnumerator FadeIn(float FadeTime) {
    myAudioSource.Play();
    myAudioSource.volume = 0f;
    while (myAudioSource.volume < startVolume) {
        myAudioSource.volume += Time.deltaTime / FadeTime;
        yield return null;
    }
  }
}
