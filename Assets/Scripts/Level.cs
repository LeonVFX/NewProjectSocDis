using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitForStart());
    }

    private IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(2);
        if (PhotonNetwork.IsMasterClient)
            GameManager.gm.NextStage();
        yield return null;
    }
}