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
        AllResearchersEliminated,
        CreatureVotedOut
    }

    private WinState winState;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();

        GameManager.gm.OnStage1 += OnGameStart;
    }

    private void OnGameStart()
    {
        endResult = FindObjectOfType<EndResult>();

        EndManager.em.OnDie += Died;
        EndManager.em.OnEscape += ResearcherEscaped;
        EndManager.em.OnCreatureVoted += CreatureVotedOut;
        EndManager.em.OnAllResearchersEliminated += CreatureKilledEverybody;
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
        endResult.ResultBackground = Resources.Load<Texture2D>("Images/creatureWinScreen");
        player.PHUD.UpdateMessageLog($"You Died!", Color.red);

        winState = WinState.Died;

        IEnumerator waitForFinish = WaitForFinish();
        StartCoroutine(waitForFinish);

        // If only the creature is alive
        if (PlayerManager.pm.playersAlive == 1)
            EndManager.em.AllResearchersEliminated();
    }

    // If Researcher Escaped
    private void ResearcherEscaped()
    {
        if (!playerView.IsMine)
            return;

        Researcher researcher = GetComponent<Researcher>();
        if (!researcher)
            return;

        if (researcher.isInfected == true)
        {
            winState = WinState.InfectedEscaped;
            Debug.Log("Infected Escaped");
        }

        endResult.ResultString = "Escaped Successfully!";
        endResult.ResultBackground = Resources.Load<Texture2D>("Images/researchersWinScreen");
        player.PHUD.UpdateMessageLog($"Escaped Successfully!", Color.blue);

        winState = WinState.ResearcherEscaped;
        FinishGameAndDestroy();
    }

    // If creature killed all researchers
    private void CreatureKilledEverybody()
    {
        if (!playerView.IsMine)
            return;

        if (this != null)
        {
            if (GetComponent<Creature>())
            {
                endResult.ResultString = "You Eliminated All Researchers!";
                endResult.ResultBackground = Resources.Load<Texture2D>("Images/creatureWinScreen");
                player.PHUD.UpdateMessageLog($"You Eliminated All Researchers!", Color.red);
                winState = WinState.AllResearchersEliminated;

                IEnumerator waitForFinish = WaitForFinish();
                StartCoroutine(waitForFinish);
            }
        }
    }

    // If creature was voted out
    private void CreatureVotedOut()
    {
        if (!playerView.IsMine)
            return;

        if (this != null)
        {
            if (GetComponent<Creature>())
            {
                endResult.ResultString = "You Were Discovered!";
                player.PHUD.UpdateMessageLog($"You Were Discovered!", Color.red);
            }

            if (GetComponent<Researcher>())
            {
                endResult.ResultString = "You Kicked The Creature!";
                player.PHUD.UpdateMessageLog($"You Kicked The Creature!", Color.blue);
            }

            endResult.ResultBackground = Resources.Load<Texture2D>("Images/researchersWinScreen");

            winState = WinState.CreatureVotedOut;

            IEnumerator waitForFinish = WaitForFinish();
            StartCoroutine(waitForFinish);
        }
    }

    private IEnumerator WaitForFinish()
    {
        while (endResult.ResultBackground == null)
            yield return null;

        FinishGame();
        yield return null;
    }

    private void FinishGame()
    {
        if (!playerView.IsMine)
            return;

        playerView.RPC("RPC_AddToEndList", RpcTarget.MasterClient);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.MasterClient);
    }

    private void FinishGameAndDestroy()
    {
        if (!playerView.IsMine)
            return;

        playerView.RPC("RPC_AddToEndList", RpcTarget.MasterClient);
        playerView.RPC("RPC_CheckForEndGame", RpcTarget.MasterClient);

        // Destroys player on Network when finished
        PlayerManager.pm.DestroyPlayer(playerView.OwnerActorNr);
    }

    [PunRPC]
    private void RPC_AddToEndList()
    {
        ++EndManager.em.playerResults;
    }

    [PunRPC]
    private void RPC_CheckForEndGame()
    {
        EndManager.em.CheckForMaxPlayers();
    }
}