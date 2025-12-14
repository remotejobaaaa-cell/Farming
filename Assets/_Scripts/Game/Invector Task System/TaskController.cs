using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TaskController : MonoBehaviour
{
    public HudMenuManager hudMenu;

    public int needTaskAttempts;
    public List<GameObject> Task;
    public AudioClip soundEffect;

    public NeedGameOver needGameOver;
    public UnityEvent waypoint;

    private TaskObjects currentPlayer;
    private string taskName;
    [SerializeField]
    private string completeInfoText;
    private void Awake()
    {
        hudMenu = FindObjectOfType<HudMenuManager>();
    }
    private void Start()
    {
        taskName = Task[0].GetComponent<TaskToDo>().taskType.ToString();
    }
    private void FixedUpdate()
    {
        if (!currentPlayer)
        {
            if (!hudMenu)
            {
                hudMenu = FindObjectOfType<HudMenuManager>();
            }
            if (hudMenu.playerSpanwer.currentPlayer)
            {
                Invoke("AssignCurrentPlayer", 0.5f);
            }
        }
    }
    public void CheckRemainingCrops()
    {
        Debug.Log("None CheckRemainingCrops");
        if (Task.Count <= needTaskAttempts)
        {
            RemainingCropStruct(0);
        }
    }

    public void CheckRemainingCrops(int gameOverTime)
    {
        if (Task.Count <= needTaskAttempts)
        {
            RemainingCropStruct(gameOverTime);
        }
    }

    public void RemainingCropStruct(float gameOverTime)
    {
        if (needGameOver == NeedGameOver.Yes)
        {
            if (!hudMenu)
            {
                hudMenu = FindObjectOfType<HudMenuManager>();
            }
            hudMenu.gameplayMenu.infoPopup.SetActive(true);
            hudMenu.gameplayMenu.inforPopupText.text = completeInfoText;
            Invoke("DisableInfo", 2f);
            GameConstants.isPlayerWin = true;
            hudMenu.GameOver(gameOverTime);
        }
        if (needGameOver == NeedGameOver.No)
        {
            waypoint.Invoke();
        }
    }

    public void disableObjects()
    {
        if (currentPlayer)
        {
            currentPlayer.seedBucket.SetActive(false);
            currentPlayer.wateringCan.SetActive(false);
        }
    }
    public void AssignCurrentPlayer()
    {
        currentPlayer = hudMenu.playerSpanwer.taskObjects;
    }
    public void DisableInfo()
    {
        hudMenu.gameplayMenu.infoPopup.SetActive(false);
    }

    public void GameOverTaskController()
    {
        hudMenu.gameplayMenu.infoPopup.SetActive(true);
        hudMenu.gameplayMenu.inforPopupText.text = completeInfoText;
        Invoke("DisableInfo", 2f);
        GameConstants.isPlayerWin = true;
        hudMenu.GameOver(4);
    }

    public void DisableActionButton()
    {
        hudMenu.thirdPersonControl.actionButton.SetActive(false);
    }

    public void PlaySoundEffect()
    {
        hudMenu.GetComponent<AudioSource>().PlayOneShot(soundEffect);
    }
    public void StopSoundEffect()
    {
        hudMenu.GetComponent<AudioSource>().Stop();
    }
}
