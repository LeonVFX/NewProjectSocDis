using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event System.Action OnSetup;
    public event System.Action OnStage1;
    public event System.Action OnVoteStage;
    public event System.Action OnStage2;
    public event System.Action OnEnd;

    public static GameManager gm;

    public enum GameStage
    {
        Lobby,
        Setup,
        Stage1,
        Voting,
        Stage2,
        End
    }

    private PhotonView managerView;
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
        managerView = GetComponent<PhotonView>();
        currentStage = GameStage.Lobby;
    }

    private void Update()
    {
        // TODO: Check for certain things to switch stages.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            managerView.RPC("RPC_NextStage", RpcTarget.All);
        }
    }

    public void NextStage()
    {
        managerView.RPC("RPC_NextStage", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_NextStage()
    {
        switch (currentStage)
        {
            case GameStage.Lobby:
                currentStage = GameStage.Setup;
                OnSetup?.Invoke();
                break;
            case GameStage.Setup:
                currentStage = GameStage.Stage1;
                OnStage1?.Invoke();
                break;
            case GameStage.Stage1:
                currentStage = GameStage.Voting;
                OnVoteStage?.Invoke();
                break;
            case GameStage.Voting:
                currentStage = GameStage.Stage2;
                OnStage2?.Invoke();
                break;
            case GameStage.Stage2:
                OnEnd?.Invoke();
                break;
            case GameStage.End:
                break;
        }
    }
}