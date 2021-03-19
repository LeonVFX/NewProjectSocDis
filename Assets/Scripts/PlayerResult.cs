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
        if (!playerView.IsMine)
            return;

        Researcher researcher = player as Researcher;
        Debug.Log($"Researcher Escaped");
        if (researcher.isInfected == true)
        {
            winState = WinState.InfectedEscaped;
            Debug.Log("Infected Escaped");
        }

        endResult.ResultString = "Escaped Successfully!";

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
        if (!playerView.IsMine)
            return;

        playerView.RPC("RPC_AddToEndList", RpcTarget.All);
        //playerView.RPC("RPC_DeleteInstance", RpcTarget.All);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_AddToEndList()
    {
        EndManager.em.playerResults.Add(this);
    }

    [PunRPC]
    private void RPC_CheckForEndGame()
    {
        EndManager.em.CheckForMaxPlayers();
    }

    [PunRPC]
    private void DeleteInstance()
    {
        if (gameObject)
            Destroy(gameObject);
    }
}