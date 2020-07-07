using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class NPCScript
{
  public string name;
  public TMPro.TMP_FontAsset font;
  [TextArea(4, 10)]
  public string sentence;
  public AudioClip soundByte;
  public bool sceneChange;
}
