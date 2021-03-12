using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public event System.Action OnEscape;
    public event System.Action AllEliminated;
    public event System.Action VotedOut;

    // Singleton
    public static EndManager em;

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
    private void Update()
    {
       /*if(GameManager.gm.currentStage == GameManager.GameStage.End)
        {
            GameManager.gm.NextStage();
        }*/
    }

    public void Escaped()
    {
        OnEscape?.Invoke();
    }

    public void Eliminated()
    {
        AllEliminated?.Invoke();
    }
    public void CreatureVoted()
    {
        VotedOut?.Invoke();
    }
}