using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteButton : MonoBehaviour
{
    private bool isInRange = false;
    private bool isButtonPressed = false;
    private bool votePassed = false;

    private void Start()
    {
        PlayerManager.pm.OnSpawn += RegisterPlayer;
    }

    private void Update()
    {
        if (isInRange && !votePassed)
        {
            if (isButtonPressed)
            {
                votePassed = true;
                GameManager.gm.NextStage();
            }
        }
    }

    private void RegisterPlayer(Player player)
    {
        player.PHUD.OnInteraction += PressButton;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Creature" || otherParent.tag == "Researcher")
        {
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Creature" || otherParent.tag == "Researcher")
        {
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = false;
        }
    }

    private void PressButton(PhotonView playerView)
    {
        IEnumerator pressedButton = ButtonPressed();
        isButtonPressed = true;
        StartCoroutine(pressedButton);
    }

    private IEnumerator ButtonPressed()
    {
        yield return new WaitForEndOfFrame();
        isButtonPressed = false;
    }
}