using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private bool introduced = false;

    public void SetIntroducedState(bool set) {
      introduced = set;
    }

    public bool HasBeenIntroduced() {
      return introduced;
    }
}
