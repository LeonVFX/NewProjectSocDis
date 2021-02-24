using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    // Events
    public event System.Action<PhotonView> OnInteraction;
    // Item bool = holding an item or not
    public event System.Action<PhotonView, bool> OnItemInteraction;
    public event System.Action OnKill;

    public PhotonView playerView = null;

    // HUD Components
    [SerializeField] private GameObject background;
    [SerializeField] private Camera mapCam;
    [SerializeField] private GameObject map;
    [SerializeField] private Vector3 offset;
    private Level level = null;
    [Header("Buttons")]
    [SerializeField] private Button mapBtn = null;
    [SerializeField] private Button interactionBtn = null;
    [SerializeField] private Button itemBtn = null;
    [SerializeField] private Button killBtn = null;
    [Header("Item")]
    [SerializeField] private RawImage heldItem = null;
    [SerializeField] private Texture noItemTex = null;
    public bool hasHeldItem = false;
    [Header("Tasks")]
    [SerializeField] private Slider taskCompletionSlider = null;
    [SerializeField] private TaskList taskList = null;
    [SerializeField] private GameObject taskPrefab = null;
    [Header("Creature")]
    [SerializeField] private GameObject goopTimer = null;
    public float GoopValue
    {
        set { goopTimer.GetComponent<Image>().fillAmount = value; }
    }

    // Other
    private bool toggleMap = false;

    private void Start()
    {
        background.SetActive(false);
        level = FindObjectOfType<Level>();

        mapCam.transform.position = level.transform.position + offset;
        map.SetActive(false);

        taskCompletionSlider.maxValue = TaskManager.tm.maxNumberOfTasks;

        // Set Button Functionalities
        interactionBtn.onClick.AddListener(InteractionOnClick);
        mapBtn.onClick.AddListener(MapOnClick);
        itemBtn.onClick.AddListener(ItemInteractionOnClick);
        killBtn.onClick.AddListener(KillOnClick);

        // Set Starting Point For Buttons
        killBtn.interactable = false;
    }

    private void Update()
    {
        taskCompletionSlider.value = TaskManager.tm.numberOfCompletedTasks;
    }

    public void HoldItem(Texture itemTex)
    {
        if (!playerView.IsMine)
            return;

        heldItem.texture = itemTex;
    }

    public void DropItem()
    {
        if (!playerView.IsMine)
            return;

        heldItem.texture = noItemTex;
    }

    public void UpdateTaskList(List<Task> newTaskList)
    {
        if (!playerView.IsMine)
            return;

        ClearTaskList();
        foreach (Task task in newTaskList)
        {
            GameObject newTask = Instantiate(taskPrefab, taskList.transform);
            newTask.GetComponent<Text>().text = task.TaskDescription;
        }
    }

    public void ClearTaskList()
    {
        if (!playerView.IsMine)
            return;

        foreach (Transform child in taskList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    #region OnClickEvents
    private void InteractionOnClick()
    {
        OnInteraction?.Invoke(playerView);
    }

    private void MapOnClick()
    {
        switch (toggleMap)
        {
            case true:
                background.SetActive(false);
                map.SetActive(false);
                toggleMap = false;
                break;
            case false:
                background.SetActive(true);
                map.SetActive(true);
                toggleMap = true;
                break;
            default:
                break;
        }
    }

    private void ItemInteractionOnClick()
    {
        OnItemInteraction?.Invoke(playerView, hasHeldItem);
    }

    private void KillOnClick()
    {
        OnKill?.Invoke();
    }
    #endregion

    #region ButtonToggles
    public void ToggleInteractButtonInteractable()
    {
        switch (interactionBtn.interactable)
        {
            case true:
                interactionBtn.interactable = false;
                break;
            case false:
                interactionBtn.interactable = true;
                break;
            default:
                break;
        }
    }
    public void ToggleItemButtonInteractable()
    {
        switch (itemBtn.interactable)
        {
            case true:
                itemBtn.interactable = false;
                break;
            case false:
                itemBtn.interactable = true;
                break;
            default:
                break;
        }
    }
    public void ToggleKillButtonInteractableActive()
    {
        if (!killBtn.IsInteractable())
        {
            killBtn.interactable = true;
        }
    }
    public void ToggleKillButtonInteractableInactive()
    {
        if (killBtn.IsInteractable())
        {
            killBtn.interactable = false;
        }
    }
    public void ToggleKillButtonActive()
    {
        switch (killBtn.gameObject.activeSelf)
        {
            case true:
                killBtn.gameObject.SetActive(false);
                break;
            case false:
                killBtn.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ToggleTaskListActive()
    {
        switch (taskList.gameObject.activeSelf)
        {
            case true:
                taskList.gameObject.SetActive(false);
                break;
            case false:
                taskList.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ToggleGoopAmountActive()
    {
        switch (goopTimer.gameObject.activeSelf)
        {
            case true:
                goopTimer.gameObject.SetActive(false);
                break;
            case false:
                goopTimer.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion
}