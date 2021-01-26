using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStgs : MonoBehaviour
{
    public enum PlayerRole
    {
        Researcher,
        Creature,
        Infected
    }

    PhotonView networkView = null;
    public List<PlayerRole> roleList;


    private void Start()
    {
        networkView = GetComponent<PhotonView>();
        roleList = new List<PlayerRole>();
    }

    public void RandomizePlayers(int numPlayers)
    {
        int whoCreature = Random.Range(0, numPlayers);
        networkView.RPC("RPC_RandomizeRoles", RpcTarget.All, new object[] { numPlayers, whoCreature });
    }

    [PunRPC]
    private void RPC_RandomizeRoles(int numPlayers, int whoCreature)
    {
        roleList.Clear();
        for (int i = 0; i < numPlayers; ++i)
        {
            if (whoCreature == i)
                roleList.Add(PlayerRole.Creature);
            else
                roleList.Add(PlayerRole.Researcher);
        }
    }
}
