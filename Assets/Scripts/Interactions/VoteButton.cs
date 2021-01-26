using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteButton : MonoBehaviour
{
    private bool isInRange = false;

    private void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.gm.NextStage();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = false;
        }
    }
}