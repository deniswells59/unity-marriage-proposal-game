using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{

    // Refs
    [SerializeField] float runSpeed = 5f;
    Rigidbody2D myRigidBody;
    CapsuleCollider2D myCollider;
    Animator myAnimator;

    // State
    bool isInCinematicMode = false;
    bool skipDialogue = false; // Ability to skip dialogue is determined by debouncer
    bool startDialogue = true; // Ability to start dialogue is determined by debouncer to allow text box animation
    DialogueTrigger npcInDialogueTrigger;
    NPC npcInDialogueState;

    // Start is called before the first frame update
    void Start()
    {
      myRigidBody = GetComponent<Rigidbody2D>();
      myCollider = GetComponent<CapsuleCollider2D>();
      myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      CustomDebug();
      if (!isInCinematicMode) {
        Run();
        SpriteAnimation();
        TriggerNPCDialogue();
      }

      if (npcInDialogueTrigger && skipDialogue) {
        ContinueNPCDialogue();
      }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
      if(collision.gameObject.tag != "NPC") { return; }
      if (!IsTouchingNPCLayer()) { return; }

      npcInDialogueTrigger = collision.gameObject.GetComponent<DialogueTrigger>(); // Stores NPC
      npcInDialogueState = collision.gameObject.GetComponent<NPC>(); // Stores NPC
    }

    private void OnTriggerExit2D(Collider2D collision) {
      if (IsTouchingNPCLayer()) { return; }
      if(collision.gameObject.tag != "NPC") { return; }

      npcInDialogueTrigger = null;
    }

    private void Run() {
      float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // -1 to 1

      Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
      myRigidBody.velocity = playerVelocity;
    }

    private void SpriteAnimation() {
      bool playerHasHorizontalSpeed = PlayerHasHorizontalSpeed();

      myAnimator.SetBool("Walking", playerHasHorizontalSpeed);

      if (playerHasHorizontalSpeed) {
        FlipSprite();
      }
    }

    private void TriggerNPCDialogue() {
      // Checks conditionals before starting dialogue
      bool submitPress = CrossPlatformInputManager.GetButtonDown("Submit");
      if (!submitPress) { return; }

      if (skipDialogue) { return; }
      if (!startDialogue) { return; }
      if (!IsTouchingNPCLayer()) { return; }
      if (npcInDialogueTrigger == null) { return; }
      // After all the checks, continue with functionality
      StopHorizontalSpeed();

      npcInDialogueTrigger.StartDialogue();
      StartCoroutine(SetStartDialogue(false, 0f));
      SetCinematicMode(true);
      StartCoroutine(SetSkipDiagloue(true, 1.5f));
    }

    private void ContinueNPCDialogue() {
      // Checks conditionals before continuing dialogue
      bool submitPress = CrossPlatformInputManager.GetButtonDown("Submit");
      if (!submitPress) { return; }
      if (!skipDialogue) { return; }
      if (npcInDialogueTrigger == null) { return; }

      StartCoroutine(SetSkipDiagloue(false, 0.01f)); // Disable skip
      npcInDialogueTrigger.DisplayNextSentence();
      StartCoroutine(SetSkipDiagloue(true, 0.2f)); // Enables skip
    }

    public void EndNPCDialogue() {
      npcInDialogueState.SetIntroducedState(true);
      StartCoroutine(SetSkipDiagloue(false, 0.8f)); // Disable skip
      StartCoroutine(SetStartDialogue(true, 0.6f));   // Enable start
      SetCinematicMode(false);
    }

    private void FlipSprite() {
      transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
    }

    private bool PlayerHasHorizontalSpeed() {
      return Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    public void StopHorizontalSpeed() {
      myAnimator.SetBool("Walking", false);

      Vector2 playerVelocity = new Vector2(0f, myRigidBody.velocity.y);
      myRigidBody.velocity = playerVelocity;
    }

    private IEnumerator SetSkipDiagloue (bool set, float timeout) {
      yield return new WaitForSeconds(timeout);

      skipDialogue = set;
    }

    private IEnumerator SetStartDialogue (bool set, float timeout) {
      yield return new WaitForSeconds(timeout);

      startDialogue = set;
    }

    public void SetCinematicMode(bool set) {
      isInCinematicMode = set;
    }

    public bool GetSkipDialogueStatus () {
      return skipDialogue;
    }

    private bool IsTouchingNPCLayer() {
      LayerMask NPCLayerMask = LayerMask.GetMask("NPC");
      return myCollider.IsTouchingLayers(NPCLayerMask);
    }

    private void CustomDebug() {
      if (!CrossPlatformInputManager.GetButtonDown("Fire1")) { return; }
      bool testingNPC = npcInDialogueTrigger != null;

      Debug.Log("PLAYER - isInCinematicMode = " + isInCinematicMode);
      Debug.Log("PLAYER - skipDialogue = " + skipDialogue);
      Debug.Log("PLAYER - npcInDialogueTrigger (Bool) = " + testingNPC);
      Debug.Log("PLAYER - IsTouchingNPCLayer (Bool) = " + IsTouchingNPCLayer());
    }
}
