using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue introDialogue;
    public Dialogue secondaryDialogue;

    public void StartDialogue() {
      Dialogue dialogueToStart;
      var npcState = GetComponent<NPC>();

      if (npcState.HasBeenIntroduced()) {
        dialogueToStart = secondaryDialogue;
      } else {
        dialogueToStart = introDialogue;
      }

      StartCoroutine(FindObjectOfType<DialogueManager>().StartDialogue(dialogueToStart));
    }

    public void DisplayNextSentence() {
      FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }
}
