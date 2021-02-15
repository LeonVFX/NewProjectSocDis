using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public event System.Action OnInteraction;

    [SerializeField] private Button InteractionBtn = null;
    [SerializeField] private Button MapBtn = null;

    private void Start()
    {
        InteractionBtn.onClick.AddListener(InteractionOnClick);
    }
    private void Update()
    {

    }
    private void InteractionOnClick()
    {
        OnInteraction.Invoke();
    }
}
