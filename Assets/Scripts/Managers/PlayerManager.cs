using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public event System.Action<Player> OnSpawn;

    private PhotonView managerView;

    public static PlayerManager pm;
    public List<PhotonView> playerViews;
    public List<Player> playerList;
    public int playersAlive
    {
        get
        {
            int result = 0;
            foreach (Player player in playerList)
            {
                if (player.isAlive)
                    ++result;
            }
            return result;
        }
    }

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

    private void Start()
    {
        managerView = GetComponent<PhotonView>();
    }

    public void SpawnPlayer(Player player)
    {
        OnSpawn?.Invoke(player);
    }

    public void KillPlayer(int playerID)
    {
        foreach (PhotonView playerView in playerViews)
        {
            if (playerView != null)
                if (playerView.OwnerActorNr == playerID)
                {
                    playerView.GetComponent<Player>().Die();
                    //playerView.RPC("RPC_Die", RpcTarget.All);
                    break;
                }
        }
    }

    public void DestroyPlayer(int playerID)
    {
        managerView.RPC("RPC_DestroyPlayer", RpcTarget.All, new object[] { playerID });
    }

    [PunRPC]
    private void RPC_DestroyPlayer(int playerID)
    {
        foreach (PhotonView playerView in playerViews)
        {
            if (playerView != null)
                if (playerView.OwnerActorNr == playerID)
                {
                    Destroy(playerView.gameObject);
                }
        }
    }
}