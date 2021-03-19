using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResult : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    private EndResult endResult;

    private bool isInPod = false;

    // Win States
    public enum WinState
    {
        ResearcherEscaped,
        InfectedEscaped,
        CreatureKilledEverybody,
        CreatureVotedOut

    }

    private WinState winState;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        endResult = FindObjectOfType<EndResult>();

        EndManager.em.OnEscape += ResearcherEscaped;
        EndManager.em.AllEliminated += CreatureKilledEverybody;
        EndManager.em.CreatureOut += CreatureVotedOut;
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return; 
    }

    // If Researcher Escaped
    private void ResearcherEscaped()
    {
        Researcher researcher = player as Researcher;
        Debug.Log($"Researcher Escaped");
        if (researcher.isInfected == true)
        {
            winState = WinState.InfectedEscaped;
            Debug.Log("Infected Escaped");
        }

        endResult.ResultString = "Escaped Successfully!";
        Debug.Log("ResultString Modified!");

        winState = WinState.ResearcherEscaped;
        FinishGame();
    }

    private void CreatureKilledEverybody()
    {
        winState = WinState.CreatureKilledEverybody;
        FinishGame();
    }

    private void CreatureVotedOut()
    {
        winState = WinState.CreatureVotedOut;
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