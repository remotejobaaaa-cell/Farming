using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector.Utils;

public class DialogueTrigger : MonoBehaviour
{
    private HudMenuManager hudMenu;
    private DialogueSystem dialogueSystem;

    public List<string> dialogues;
    public bool enableCutscene;

    [Header("Player in Cinematics")]
    public GameObject cenamatics;
    public bool isLookAtRequired;
    private GameObject player;
    public Transform lookAtTarget;
    public bool needLevelComplete;

    [Header("Events")]
    public UnityEvent customEvent;
    public UnityEvent OnCompleteEvent;

    private void Start()
    {
        CheckForDialogueSystem();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            dialogueSystem.NextDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            Invoke("AutoNextDiaogue", 2f);
            hudMenu.ShowThirdPersonControl(false);
            player = other.GetComponent<vThirdPersonController>().gameObject;
            CheckForDialogueSystem();
            ResetPlayerForCinematics();
            cenamatics.SetActive(true);
            this.GetComponent<BoxCollider>().enabled = false;
            dialogueSystem.SetEnablePlayerInput(true);
            ResetDialogueSystem();
            InvokeEvent();
        }
        if (other.GetComponent<RCC_CarControllerV3>())
        {
            Invoke("AutoNextDiaogue", 2f);
            hudMenu.ShowVehicleControl(false);
            CheckForDialogueSystem();
            cenamatics.SetActive(true);
            ResetDialogueSystem();
            InvokeEvent();
        }
        if (other.GetComponent<DroneMovement>())
        {
            Invoke("AutoNextDiaogue", 2f);
            hudMenu.ShowThirdPersonControl(false);
            CheckForDialogueSystem();
            hudMenu.playerSpanwer.currentCamera.SetActive(false);
            cenamatics.SetActive(true);
            ResetDialogueSystem();
            InvokeEvent();
        }
    }

    public void CheckForDialogueSystem()
    {
        if (!hudMenu)
        {
            hudMenu = HudMenuManager.instance;
        }
        if (!dialogueSystem)
        {
            dialogueSystem = hudMenu.gameplayMenu.dialogueSystem;
            dialogueSystem.currentDialogue = this;
        }
    }

    void ResetPlayerForCinematics()
    {
        if (isLookAtRequired)
        {
            player.GetComponent<vTargetLookAt>().target = lookAtTarget;
            player.GetComponent<vTargetLookAt>().enabled = true;
        }
    }

    void ResetDialogueSystem()
    {
        CheckForDialogueSystem();
        dialogueSystem.currentDialogueIndex = 0;
        dialogueSystem.dialogueList.Clear();
        foreach (string dialogue in dialogues)
        {
            dialogueSystem.dialogueList.Add(dialogue);
        }
        dialogueSystem.gameObject.SetActive(true);
        dialogueSystem.InitializeDialogues();
    }

    public void InvokeEvent()
    {
        customEvent.Invoke();
    }
    public void InvokeCompleteEvent()
    {
        OnCompleteEvent.Invoke();
        if (needLevelComplete)
        {
            needLevelComplete = false;
            HudMenuManager.instance.ShowThirdPersonControl(false);
            GameConstants.isPlayerWin = true;
            HudMenuManager.instance.GameOver(2);
            Debug.Log("Game Level f "+GameConstants.currentlySelectedLevel);
        }
    }

    public void AutoNextDiaogue()
    {
        if (dialogues.Count > 0)
        {
            dialogueSystem.NextDialogue();
            Invoke("AutoNextDiaogue", 2f);
        }
    }

    public void CancelAutoNextInvoke()
    {
        CancelInvoke("AutoNextDiaogue");
        Invoke("AutoNextDiaogue", 2f);
    }

    //public void AutoNextDiaogue()
    //{
    //    if (dialogues.Count > 0)
    //    {
    //        dialogueSystem.NextDialogue();
    //        Invoke("AutoSecondNextDiaogue", 2f);
    //    }
    //}
    //public void AutoSecondNextDiaogue()
    //{
    //    if (dialogues.Count > 0)
    //    {
    //        dialogueSystem.NextDialogue();
    //        Invoke("AutoNextDiaogue", 2f);
    //    }

    //}
}

