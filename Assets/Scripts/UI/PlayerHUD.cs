using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public event System.Action OnInteraction;

    [SerializeField] private GameObject background;
    [SerializeField] private Camera mapCam;
    [SerializeField] private GameObject map;
    [SerializeField] private Vector3 offset;
    private Level level = null;
    [SerializeField] private Button MapBtn = null;
    [SerializeField] private Button InteractionBtn = null;

    private bool toggleMap = false;

    private void Start()
    {
        background.SetActive(false);

        level = FindObjectOfType<Level>();

        mapCam.transform.position = level.transform.position + offset;
        map.SetActive(false);

        // Set Button Functionalities
        InteractionBtn.onClick.AddListener(InteractionOnClick);
        MapBtn.onClick.AddListener(MapOnClick);
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
}