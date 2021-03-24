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
        Died,
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

        EndManager.em.OnDie += Died;
        EndManager.em.OnEscape += ResearcherEscaped;
        EndManager.em.AllEliminated += CreatureKilledEverybody;
        EndManager.em.CreatureOut += CreatureVotedOut;
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return; 
    }

    // If Died
    private void Died()
    {
        if (!playerView.IsMine)
            return;

        endResult.ResultString = "You Died!";
        player.PHUD.UpdateMessageLog($"You Died!", Color.red);

        winState = WinState.Died;
        FinishGame();
    }

    // If Researcher Escaped
    private void ResearcherEscaped()
    {
        if (!playerView.IsMine)
            return;

        Researcher researcher = player as Researcher;
        if (researcher.isInfected == true)
        {
            winState = WinState.InfectedEscaped;
            Debug.Log("Infected Escaped");
        }

        endResult.ResultString = "Escaped Successfully!";
        player.PHUD.UpdateMessageLog($"Escaped Successfully!", Color.blue);

        winState = WinState.ResearcherEscaped;
        FinishGameAndDestroy();
    }

    private void CreatureKilledEverybody()
    {
        winState = WinState.CreatureKilledEverybody;
        FinishGameAndDestroy();
    }

    private void CreatureVotedOut()
    {
        winState = WinState.CreatureVotedOut;
        FinishGameAndDestroy();
    }

    private void FinishGame()
    {
        if (!playerView.IsMine)
            return;

        playerView.RPC("RPC_AddToEndList", RpcTarget.All);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.MasterClient);
    }

    private void FinishGameAndDestroy()
    {
        if (!playerView.IsMine)
            return;

        playerView.RPC("RPC_AddToEndList", RpcTarget.All);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.MasterClient);

        // Destroys player on Network when finished
        PlayerManager.pm.DestroyPlayer(playerView.OwnerActorNr);
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
}