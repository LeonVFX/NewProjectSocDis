using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResult : MonoBehaviour
{
    PhotonView playerView;
    private Player[] players;

    private bool isInPod = false;

    // Win States
    public enum WinState
    {
        ReseracherEscaped,
        CreatureOverrun,
        CreatureVoted

    }

    private WinState winState;

    private void Start()
    {
        playerView = GetComponent<PhotonView>(); 
        //If researcher escapes
        EndManager.em.OnEscape += ResearcherEscaped;

        //If all researchers eliminated
        EndManager.em.AllEliminated += CreatureOverrun;

        //If Creature is voted out
        EndManager.em.VotedOut += CreatureElim;

        /*players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
           if (playerView != null)
            if (playerView.OwnerActorNr == playerID)
            {
                
            }
        }*/
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;  
              
    }

    private void EndScreen()
    {
        if (GameManager.gm.currentStage == GameManager.GameStage.End)
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {

            }

        }
    }
    // If Researcher Escaped
    private void ResearcherEscaped()
    {
        winState = WinState.ReseracherEscaped;
        GameManager.gm.NextStage();
        Debug.Log($"Researcher Escaped");
    }
    //If all researchers are eliminated
    private void CreatureOverrun()
    {
        winState = WinState.CreatureOverrun;
        GameManager.gm.NextStage();
        Debug.Log($"All Researcher Eliminated");
    }
    //If the Creature is voted out
    private void CreatureElim()
    {
        winState = WinState.CreatureVoted;
        GameManager.gm.NextStage();
        Debug.Log($"Creature voted out");
    }
}