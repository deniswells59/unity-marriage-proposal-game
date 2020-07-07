using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
  // Refs
  public Canvas optionsCanvas;
  public Player player;
  public Animator sceneTransistionAnim;
  public AudioSource audioSource;

  AudioPlayer audioPlayer;

  private void Start() {
    audioPlayer = audioSource.GetComponent<AudioPlayer>();
  }

  public void TriggerLoadSceneCoroutine() {
    StartCoroutine(LoadNextScene());
  }

  IEnumerator LoadNextScene() {
    sceneTransistionAnim.SetBool("end", true);
    StartCoroutine(audioPlayer.FadeOut(1f));

    yield return new WaitForSeconds(1f);

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    Debug.Log(currentSceneIndex);
    SceneManager.LoadScene(currentSceneIndex + 1);
  }

  private void OnTriggerEnter2D(Collider2D collision) {
    if(collision.gameObject.name != "Player") { return; }

    turnOnOptionsUI();
  }

  public void turnOnOptionsUI() {
    optionsCanvas.enabled = true;

    if (player) {
      player.SetCinematicMode(true);
      player.StopHorizontalSpeed();
    }
  }

  public void turnOffOptionsUI() {
    optionsCanvas.enabled = false;

    if (player) {
      player.SetCinematicMode(false);
    }
  }
}
