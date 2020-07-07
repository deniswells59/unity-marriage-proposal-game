using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Cinematic : MonoBehaviour
{
    public SceneLoader sceneLoader;

    // Update is called once per frame
    void Update()
    {
      bool readyToLeave = CrossPlatformInputManager.GetButtonDown("Submit");

      if (readyToLeave) {
        sceneLoader.turnOnOptionsUI();
      }
    }
}
