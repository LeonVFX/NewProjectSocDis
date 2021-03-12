using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePod : MonoBehaviour
{
    [SerializeField] EscapePod escapePod;
    [SerializeField] private bool inRange;
    [SerializeField] private bool playerInPod;
    [SerializeField] private bool launched;
    [SerializeField] private float spaceleft = 2;
    [SerializeField] private float countdown = 30;
    [SerializeField] private float movement = 0;

    private void Update()
    {
        if (GameManager.gm.currentStage == GameManager.GameStage.Stage2)
        {
            if (inRange == true)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    // GameManager.gm.InEscapePod();
                    Debug.Log("Player has entered escape pod");
                    playerInPod = true;
                    spaceleft--;
                }

                if (spaceleft <= 1)
                {
                    Debug.Log("Countdown has begun");
                    countdown -= Time.deltaTime;
                    if (countdown <= 0 || spaceleft <= 0)
                    {
                        EndManager.em.Escaped();
                        //GameManager.gm.NextStage();
                        launched = true;
                        //escapePod.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Researcher" || player.tag == "Creature")
        {
            PhotonView playerView = player.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            Debug.Log($"Player {player} in reach of escape pod");
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Researcher" || player.tag == "Creature")
        {
            PhotonView playerView = player.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            Debug.Log($"Player {player} out of range");
            inRange = false;
        }
    }
}