using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public event System.Action OnDie;
    public event System.Action OnEscape;
    public event System.Action OnCreatureVoted;
    public event System.Action OnAllResearchersEliminated;
    //public event System.Action GameEnded;

    // Singleton
    public static EndManager em;

    private PhotonView managerView;
    public EndResult endResult;
    public int playerResults;

    private void Awake()
    {
        // Singleton
        if (em == null)
        {
            em = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        managerView = GetComponent<PhotonView>();
    }

    public void CheckForMaxPlayers()
    {
        // TODO: Right now it does not work if somebody drops the game
        if (playerResults == PhotonNetwork.PlayerList.Length)
        {
            Debug.Log($"{playerResults} out of {PhotonNetwork.PlayerList.Length}");

            // Make sure everyone is set
            IEnumerator check = WaitForCheck();
            StartCoroutine(check);
        }
    }

    private IEnumerator WaitForCheck()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
        yield return new WaitForSeconds(1);
        GameManager.gm.NextStage();
        yield return null;
    }

    public void Die()
    {
        OnDie?.Invoke();
    }

    public void Escaped()
    {
        OnEscape?.Invoke();
    }

    public void CreatureVoted()
    {
        managerView.RPC("RPC_CreatureVoted", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_CreatureVoted()
    {
        OnCreatureVoted?.Invoke();
    }

    public void AllResearchersEliminated()
    {
        managerView.RPC("RPC_AllResearchersEliminated", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_AllResearchersEliminated()
    {
        OnAllResearchersEliminated?.Invoke();
    }
}