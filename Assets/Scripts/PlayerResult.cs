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
        ResearcherEscaped,
        CreatureKilledEverybody,
        CreatureVotedOut

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
        winState = WinState.ResearcherEscaped;
        FinishGame();
        Debug.Log($"Researcher Escaped");
    }
    private void CreatureKilledEverybody()
    {
        winState = WinState.CreatureKilledEverybody;
        FinishGame();
    }
    private void FinishGame()
    {
        EndManager.em.playerResults.Add(this);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.All);
        //CALL IN RPC-- > CheckForEndGame();
    }
    [PunRPC]
    void RPC_CheckForEndGame()
    {
        EndManager.em.CheckForMaxPlayers();
    }
}