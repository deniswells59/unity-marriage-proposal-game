using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Constants
    public float textBoxAnimationDelay = 0.5f;

    // State
    private Queue<NPCScript> scripts;

    // Refs
    public TextBox gameTextBox;
    public Player player;
    public AudioPlayer musicPlayer;
    public SoundEffects soundEffectsPlayer;
    public SceneLoader sceneLoader;

    // Start is called before the first frame update
    void Start()
    {
      scripts = new Queue<NPCScript>();
      GameObject sceneObject = GameObject.Find("SceneLoader");
      sceneLoader = sceneObject.GetComponent<SceneLoader>();
    }

    public IEnumerator StartDialogue (Dialogue dialogue) {
      gameTextBox.ToggleOn();
      scripts.Clear();

      foreach (NPCScript script in dialogue.NPCScene) {
        scripts.Enqueue(script);
      }

      yield return new WaitForSeconds(textBoxAnimationDelay);
      DisplayNextSentence();
    }

    public void DisplayNextSentence() {
      bool currentAnimationIsComplete = gameTextBox.GetCurrentSentenceAnimationStatus();
      bool playerCanSkipDialogue = player.GetSkipDialogueStatus();
      if(!currentAnimationIsComplete && playerCanSkipDialogue) {
        gameTextBox.SetSkipAnimationStatus(true);
        return;
      }

      if (soundEffectsPlayer) {
        musicPlayer.PlayMusic();
        soundEffectsPlayer.StopSoundByte();
      }

      if (scripts.Count == 0) {
        EndDialogue();
        return;
      }

      NPCScript currentScript = scripts.Dequeue();
      gameTextBox.SetNameText(currentScript.name);
      gameTextBox.StartSentenceTextAnimation(currentScript.sentence, currentScript.font);

      if (currentScript.soundByte) {
        StartCoroutine(musicPlayer.FadeOutPause());
        soundEffectsPlayer.PlaySoundByte(currentScript.soundByte);
      }

      if (currentScript.sceneChange) {
        sceneLoader.turnOnOptionsUI();
      }
    }

    public void EndDialogue() {
      gameTextBox.ToggleOff();
      player.EndNPCDialogue();
    }
}
