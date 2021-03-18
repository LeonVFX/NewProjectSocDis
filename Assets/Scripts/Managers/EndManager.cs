using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public event System.Action OnEscape;
    public event System.Action AllEliminated;
    public event System.Action CreatureOut;
    //public event System.Action GameEnded;

    // Singleton
    public static EndManager em;
    public List<PlayerResult> playerResults;

    

    private void Awake()
    {
        // Singleton
        if (em == null)
        {
            em = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckForMaxPlayers()
    {
        if (playerResults.Count == PhotonNetwork.PlayerList.Length)
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.endScene);
            return;
    }
    public void Escaped()
    {
        OnEscape?.Invoke();
    }
    public void ResearchElim()
    {
        AllEliminated.Invoke();
    }
    public void CreatureVoted()
    {
        CreatureOut.Invoke();
    }
   /* public void GameOver()
    {
        GameEnded.Invoke();
    }*/
}