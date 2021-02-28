using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event System.Action OnVoteStage;
    public event System.Action OnStage2;
    public event System.Action OnEndStage;

    public static GameManager gm;
    private Player[] players;
    private Player playerViews;

    public enum GameStage
    {
        Stage1,
        Voting,
        Stage2,
        EndStage
    }

    public enum PlayerState
    {
        Begin,
        SuccessEscape,
        UnsuccessEscape,
        RsearcherElim,
        CreatureElim,
        CreatureFound,
        SuccessInfil,
        UnsuccessInfil
    }

    private PhotonView gameView;
    public GameStage currentStage;
    public PlayerState currentState;

    private void Awake()
    {
        // Singleton
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameView = GetComponent<PhotonView>();
        currentStage = GameStage.Stage1;
        currentState = PlayerState.Begin;
    }

    private void Update()
    {
        // TODO: Check for certain things to switch stages.
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    gameView.RPC("RPC_NextStage", RpcTarget.All);
        //}
    }

    public void NextStage()
    {
        gameView.RPC("RPC_NextStage", RpcTarget.All);
    }
    /* public void InEscapePod()
     {
         players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            if (!playerView.IsMine || !isAlive)
        }

     }*/
    /*public void EndState()
    {
        gameView.RPC("RPC_NextState", RpcTarget.All);
    }*/

    [PunRPC]
    private void RPC_NextStage()
    {
        switch (currentStage)
        {
            case GameStage.Stage1:
                currentStage = GameStage.Voting;
                OnVoteStage?.Invoke();
                break;
            case GameStage.Voting:
                currentStage = GameStage.Stage2;
                // Kill Most voted Player
                OnStage2?.Invoke();
                break;
            case GameStage.Stage2:
                // End game
                currentStage = GameStage.EndStage;
                OnEndStage?.Invoke();
                break;
           /* case GameStage.EndStage:
                currentStage = GameStage.EndStage;
                break;*/
        }
    }

   /* private void RPC_NextState()
    {
        switch (currentState)
        {
            case PlayerState.Begin:
                //Researcher EndGame States
                currentState = PlayerState.SuccessEscape;
                currentState = PlayerState.UnsuccessEscape;
                currentState = PlayerState.CreatureElim;

                //Creature EndGame States
                currentState = PlayerState.RsearcherElim;
                currentState = PlayerState.CreatureFound;

                //Infected EndGame States
                currentState = PlayerState.SuccessInfil;
                currentState = PlayerState.UnsuccessInfil;

                OnStage2?.Invoke();
                break;

            
        }
    }*/
}

/*case PlayerState.Begin:
                currentState = PlayerState.UnsuccessEscape;
                OnStage2?.Invoke();
                break;
            case GameStage.Stage2:
                // End game
                break;*/