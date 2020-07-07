using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBox : MonoBehaviour
{
    // Constants
    private float textAnimationDelta = 0.02f;

    // GameObject Refs
    public Canvas canvas;
    public TextMeshProUGUI nameTextMesh;
    public TextMeshProUGUI sentenceTextMesh;
    public Image textBoxImage;
    public SpriteRenderer textBoxSpriteRenderer;
    public TMPro.TMP_FontAsset defaultFont;

    // GameObject Component Refs
    Animator textBoxAnimator;

    // Kinda Constants
    [SerializeField]
    public float textBoxAnimationDelay = 0.5f;

    // Animation State
    bool skipTextAnimation = false;
    bool currentSentenceAnimationComplete = false;

    // Start is called before the first frame update
    void Start()
    {
      if (textBoxImage) {
        textBoxAnimator = textBoxImage.GetComponent<Animator>();
      } else {
        textBoxAnimator = textBoxSpriteRenderer.GetComponent<Animator>();
      }
    }

    public void ToggleOn() {
      canvas.enabled = true;
      if (textBoxSpriteRenderer) {
        textBoxSpriteRenderer.enabled = true;
      }

      textBoxAnimator.SetBool("TextBoxOpen", true);
    }

    public void ToggleOff() {
      SetNameText("");
      SetSentenceText("");
      textBoxAnimator.SetBool("TextBoxOpen", false);

      if (textBoxSpriteRenderer) {
        StartCoroutine(TurnOffTextBoxSprite());
      }

      StartCoroutine(DisableCanvas());
    }

    private void BeforeAnimation() {
      SetCurrentSentenceAnimationStatus(false);
      SetSkipAnimationStatus(false);
    }

    IEnumerator TurnOffTextBoxSprite() {
      yield return new WaitForSeconds(textBoxAnimationDelay);

      textBoxSpriteRenderer.enabled = false;
    }

    IEnumerator AnimateSentenceText(string text, TMPro.TMP_FontAsset font, TextMeshProUGUI destination) {
      BeforeAnimation();
      int currentPosition = 0;

      if (font) {
        destination.font = font;
      } else {
        destination.font = defaultFont;
      }

      destination.text = "";

      while (currentPosition < text.Length) {
        string newText = destination.text + text[currentPosition++];
        destination.text = newText;

        if (!skipTextAnimation) {
          yield return new WaitForSeconds(textAnimationDelta);
        }
      }

      AfterAnimation();
    }

    private void AfterAnimation() {
      SetCurrentSentenceAnimationStatus(true);
    }

    IEnumerator DisableCanvas() {
      yield return new WaitForSeconds(textBoxAnimationDelay);
      canvas.enabled = false;
    }

    public void SetNameText(string name) {
      nameTextMesh.text = name;
    }

    public void SetSentenceText(string sentence) {
      sentenceTextMesh.text = sentence;
    }

    public void StartSentenceTextAnimation(string sentence, TMPro.TMP_FontAsset font) {
      StartCoroutine(AnimateSentenceText(sentence, font, sentenceTextMesh));
    }

    public void SetSkipAnimationStatus(bool set) {
      skipTextAnimation = set;
    }

    public void SetCurrentSentenceAnimationStatus(bool set) {
      currentSentenceAnimationComplete = set;
    }

    public bool GetSkipAnimationStatus() {
      return skipTextAnimation;
    }

    public bool GetCurrentSentenceAnimationStatus() {
      return currentSentenceAnimationComplete;
    }
}
