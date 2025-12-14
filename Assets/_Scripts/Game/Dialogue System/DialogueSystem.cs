using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.Utils;
using DG.Tweening;
using Invector.vCharacterController;
public class DialogueSystem : MonoBehaviour
{
    public Text infoText;
    public List<string> dialogueList;
    [HideInInspector]
    public int currentDialogueIndex;

    public DialogueTrigger currentDialogue;

    public AudioClip typingSound;
    private Button tapButton;

    private void Awake()
    {
        tapButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        InitializeDialogues();
    }
    public void InitializeDialogues()
    {
        infoText.text = dialogueList[0];
    }
    public void NextDialogue()
    {
        tapButton.interactable = false;
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogueList.Count)
        {
            infoText.text = "";
            //infoText.text = dialogueList[currentDialogueIndex];
            //infoText.DOText(dialogueList[currentDialogueIndex],0.5f);
            infoText.DOText(dialogueList[currentDialogueIndex], 0.5f).OnComplete(() => AllowNextDilogue());
            this.GetComponent<AudioSource>().PlayOneShot(typingSound);
            //infoText.text = dialogueList[currentDialogueIndex];
        }
        else
        {
            if (!currentDialogue.enableCutscene)
            {
                currentDialogue.cenamatics.SetActive(false);
                currentDialogue.InvokeCompleteEvent();
                SetEnablePlayerInput(true);
                if (HudMenuManager.instance.playerSpanwer.thirdPersonController)
                {
                    HudMenuManager.instance.playerSpanwer.thirdPersonController.enabled = false;
                }
                this.gameObject.SetActive(false);
            }
            else
            {
                infoText.text = "Great Discussion";
            }
        }
    }
        
    public void SetEnablePlayerInput(bool setInput)
    {
        HudMenuManager.instance.ShowThirdPersonControl(setInput);
    }

    public void AllowNextDilogue()
    {
        tapButton.interactable = true;
    }

    public void CancelAutoNextInvoke()
    {
        currentDialogue.CancelAutoNextInvoke();
    }
}
