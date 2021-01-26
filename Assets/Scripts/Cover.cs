using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public float coroutineWaitTime = 0.2f;

    private void Update()
    {
        if (!this.gameObject.GetComponent<PhotonView>().IsMine)
        {
            if (this.gameObject != null)
                Destroy(this.gameObject);
            return;
        }
    }
}