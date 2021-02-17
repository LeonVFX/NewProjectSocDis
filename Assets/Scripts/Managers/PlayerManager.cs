using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pm;
    public List<PhotonView> playerViews;

    private void Awake()
    {
        // Singleton
        if (pm == null)
        {
            pm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void KillPlayer(int playerID)
    {
        foreach (PhotonView playerView in playerViews)
        {
            if (playerView != null)
                if (playerView.OwnerActorNr == playerID)
                {
                    //playerView.GetComponent<Player>().Die();
                    playerView.RPC("RPC_Die", RpcTarget.All);
                    break;
                }
        }
    }
}