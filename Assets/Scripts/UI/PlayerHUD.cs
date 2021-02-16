using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    // Events
    public event System.Action OnInteraction;
    public event System.Action OnItemInteraction;
    public event System.Action OnKill;

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

    // Other
    private bool toggleMap = false;

    private void Start()
    {
        background.SetActive(false);

        level = FindObjectOfType<Level>();

        mapCam.transform.position = level.transform.position + offset;
        map.SetActive(false);

        // Set Button Functionalities
        interactionBtn.onClick.AddListener(InteractionOnClick);
        mapBtn.onClick.AddListener(MapOnClick);
        itemBtn.onClick.AddListener(ItemInteractionOnClick);
        killBtn.onClick.AddListener(KillOnClick);

        // Set Starting Point For Buttons
        killBtn.interactable = false;
    }

    private void InteractionOnClick()
    {
        OnInteraction?.Invoke();
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
        OnItemInteraction?.Invoke();
    }

    private void KillOnClick()
    {
        OnKill?.Invoke();
    }


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
    public void ToggleKillButtonInteractle()
    {
        switch (killBtn.interactable)
        {
            case true:
                killBtn.interactable = false;
                break;
            case false:
                killBtn.interactable = true;
                break;
            default:
                break;
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
    #endregion
}