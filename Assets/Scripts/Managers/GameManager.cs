using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event System.Action OnVoteStage;
    public event System.Action OnStage2;

    public static GameManager gm;

    public enum GameStage
    {
        Stage1,
        Voting,
        Stage2
    }
    private enum endStates
    {
        begin,
        successEscape,
        unsuccessEscape,
        researcherElim,
        creatureElim,
        successInfil,
        unsuccessInfil
    }
    private PhotonView gameView;
    public GameStage currentStage;

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
                break;
        }
    }
}