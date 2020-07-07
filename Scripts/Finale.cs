using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale : MonoBehaviour
{

  public AudioPlayer audioPlayer;
  BoxCollider2D myCollider;
  bool off = false;

  private void Start() {
    myCollider = GetComponent<BoxCollider2D>();
  }

  private void Update() {
    LayerMask CheyLayerMask = LayerMask.GetMask("Player");
    if (myCollider.IsTouchingLayers(CheyLayerMask) && !off) {
      off = true;
      StartCoroutine(audioPlayer.FadeOut(3f));
    }
  }

}
