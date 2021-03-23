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

    private GameObject targetPlayer = null;

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
                    //playerView.GetComponent<Player>().Die();
                    playerView.RPC("RPC_Die", RpcTarget.All);
                    break;
                }
        }
    }

    public void DestroyPlayer(Player player)
    {
        targetPlayer = player.gameObject;
        managerView.RPC("RPC_DestroyPlayer", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RPC_DestroyPlayer()
    {
        if (targetPlayer)
            PhotonNetwork.Destroy(targetPlayer);
    }
}