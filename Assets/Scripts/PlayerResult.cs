using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResult : MonoBehaviour
{
    PhotonView playerView;

    private bool isInPod = false;

    // Win States
    public enum WinState
    {
        ReseracherEscaped,

    }

    private WinState winState;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();

        EndManager.em.OnEscape += ResearcherEscaped;
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;


    }

    // If Researcher Escaped
    private void ResearcherEscaped()
    {
        winState = WinState.ReseracherEscaped;
        Debug.Log($"Researcher Escaped");
    }
}