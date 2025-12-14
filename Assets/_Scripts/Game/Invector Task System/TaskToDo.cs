using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Invector.vCharacterController;
using DG.Tweening;

public class TaskToDo : MonoBehaviour
{
    public TaskController taskController;
    public static TaskToDo instance;
    public enum NeedProgressBar { No, Yes }
    [Header("Need Task Progress Bar")]
    public NeedProgressBar needProgressBar;

    public enum NeedAttampt { No, Yes }
    [Header("Action attapmts If need")]
    public NeedAttampt needAttampt;

    private float totalTry;
    public int actionTry;
    private float ratioTry;

    public float actionParticletime;
    public UnityEvent afterNoAttempt;
    public UnityEvent particleEvent;
    public UnityEvent droneEvents;
    DroneObjects droneObjects;
    public TaskType taskType;

    private GameObject currentPlayer;
    public GameObject playerObject;
    //new bool for drone
    public bool invokeEvent;
    public enum NeedActionButton { No, Yes }
    [Header("Action Button")]
    public NeedActionButton needActionButton;

    private void Start()
    {
        instance = this;
        droneObjects = FindObjectOfType<DroneObjects>();
        if (needProgressBar == NeedProgressBar.Yes)
        {
            taskController.hudMenu.gameplayMenu.progressBar.gameObject.SetActive(true);
            taskController.hudMenu.gameplayMenu.progressBar.value = 0;
            totalTry = actionTry + 1;
        }
    }

    public void Update()
    {
        if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.E) && invokeEvent)
        {
            droneEvents.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {
            //currentPlayer = other.GetComponent<vThirdPersonController>().gameObject;
            if (needActionButton == NeedActionButton.Yes)
            {
                taskController.hudMenu.thirdPersonControl.actionButton.SetActive(true);
            }
            

            switch (taskType)
            {
                case TaskType.Seeding:
                    taskController.hudMenu.playerSpanwer.taskObjects.seedBucket.SetActive(true);
                    playerObject = taskController.hudMenu.playerSpanwer.taskObjects.seedBucket;
                    break;
                case TaskType.Vaccine:
                    taskController.hudMenu.playerSpanwer.taskObjects.wateringCan.SetActive(true);
                    playerObject = taskController.hudMenu.playerSpanwer.taskObjects.wateringCan;
                    break;
                case TaskType.Tree:
                    taskController.hudMenu.playerSpanwer.taskObjects.sharpAxe.SetActive(true);
                    playerObject = taskController.hudMenu.playerSpanwer.taskObjects.sharpAxe;
                    break;
                case TaskType.Watermelon:
                    taskController.hudMenu.playerSpanwer.taskObjects.watermelon.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        if (other.GetComponent<DroneMovement>())
        {
            if (needActionButton == NeedActionButton.Yes)
            {
                invokeEvent = true;
                taskController.hudMenu.thirdPersonControl.actionButton.SetActive(true);
                //droneEvents.Invoke();
            }
            else if (needActionButton == NeedActionButton.No)
            {
                droneEvents.Invoke();
            }
            switch (taskType)
            {
                case TaskType.Seeding:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(1);
                    break;
                case TaskType.Vaccine:
                    //taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(3);
                    //ControlsOff(3);
                    break;
                case TaskType.Tree:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(1);
                    break;
                case TaskType.Watermelon:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(1);
                    break;
                default:
                    break;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DroneMovement>())
        {
            //currentPlayer = other.GetComponent<DroneMovement>().gameObject;
            invokeEvent = false;
            if (needActionButton == NeedActionButton.Yes)
            {
                taskController.hudMenu.thirdPersonControl.actionButton.SetActive(false);
            }
            
            switch (taskType)
            {
                case TaskType.Seeding:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOff();
                    break;
                case TaskType.Vaccine:
                    //taskController.hudMenu.playerSpanwer.currentPlayer.GetComponent<DroneObjects>().BucketOff();
                    break;
                case TaskType.Tree:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOff();
                    break;
                case TaskType.Watermelon:
                    taskController.hudMenu.playerSpanwer.droneObjects.BucketOff();
                    break;
                default:
                    break;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<vThirdPersonController>())
        {

            if (needAttampt == NeedAttampt.No)
            {
                if (!this.transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    this.GetComponent<BoxCollider>().enabled = false;
                    taskController.Task.Remove(this.gameObject);
                    taskController.CheckRemainingCrops(4);
                }
            }
        }
        else
        {

            if (needAttampt == NeedAttampt.No)
            {
                if (!this.transform.GetChild(1).gameObject.activeInHierarchy)
                {
                    this.GetComponent<BoxCollider>().enabled = false;
                    taskController.Task.Remove(this.gameObject);
                    taskController.CheckRemainingCrops(4);
                }
            }
        }
    }

    public void ObjectsOff()
    {
        taskController.hudMenu.playerSpanwer.droneObjects.BucketOff();
    }
    public void TPS()
    {
        taskController.hudMenu.thirdPersonControl.droneJoystickToggle.SetActive(true);
        taskController.hudMenu.thirdPersonControl.joystickToggle.SetActive(true);
        taskController.hudMenu.playerSpanwer.droneObjects.BucketOff();
    }
    public void ControlsOff(float time)
    {
        taskController.hudMenu.thirdPersonControl.actionButton.SetActive(false);
        taskController.hudMenu.thirdPersonControl.droneJoystickToggle.SetActive(false);
        taskController.hudMenu.thirdPersonControl.joystickToggle.SetActive(false);
        taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(3);
        Invoke("TPS", time);
    }
    public void DeductActionTry()
    {
        actionTry -= 1;
        if (taskType == TaskType.Tree)
        {
            Invoke("InvokeTreeParticle", actionParticletime);
        }
        if (taskType == TaskType.Vaccine)
        {
            Invoke("InvokeTreeParticle", actionParticletime);
        }
        if (taskType == TaskType.Seeding)
        {
            Invoke("InvokeTreeParticle", actionParticletime);
        }
    }
    public void CheckRemainingAttampts()
    {
        if (actionTry < 0)
        {
            afterNoAttempt.Invoke();
            if (playerObject)
            {
                playerObject.SetActive(false);
            }
            this.GetComponent<BoxCollider>().enabled = false;
            taskController.Task.Remove(this.gameObject);
            taskController.CheckRemainingCrops(4);
        }
    }
    public void InvokeTreeParticle()
    {
        GetPercentage();
        particleEvent.Invoke();
        playerObject.GetComponent<ToolParticleSystem>().particle.Play();

    }
    public void InvokeVaccineParticle()
    {
        GetPercentage();
        particleEvent.Invoke();
        playerObject.GetComponent<ToolParticleSystem>().particle.Play();
    }
    public void DisableParticle()
    {
        playerObject.gameObject.SetActive(false);
    }
    public void GetPercentage()
    {
        if (needProgressBar == NeedProgressBar.Yes)
        {
            ratioTry = (actionTry + 1) / totalTry;
            Debug.Log("ratioTry: " + ratioTry);
            HudMenuManager.instance.gameplayMenu.progressBar.value = 1 - ratioTry;
        }
    }

    public void DroneTask(int itemIndex)
    {
        taskController.hudMenu.playerSpanwer.droneObjects.BucketOn(itemIndex);
    }
}
