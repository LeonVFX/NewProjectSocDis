using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameManager.gm.NextStage();
            StartCoroutine(WaitForStart());
        }
    }

    private IEnumerator WaitForStart()
    {
        int playerCount = PlayerManager.pm.playerList.Count;

        while (PlayerManager.pm.playersReady < playerCount)
            yield return null;



        GameManager.gm.NextStage();
        yield return null;
    }
}