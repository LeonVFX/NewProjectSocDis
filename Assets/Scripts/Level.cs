using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            GameManager.gm.NextStage();

        StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(1);
        if (PhotonNetwork.IsMasterClient)
            GameManager.gm.NextStage();
        yield return null;
    }
}