using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteButton : MonoBehaviour
{
    private Player[] players;
    private bool isInRange = false;
    private bool isButtonPressed = false;
    private bool votePassed = false;

    private void Start()
    {
        players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            player.PHUD.OnInteraction += PressButton;
        }
    }

    private void Update()
    {
        if (isInRange && !votePassed)
        {
            if (Input.GetButtonDown("Interact") || isButtonPressed)
            {
                votePassed = true;
                GameManager.gm.NextStage();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = false;
        }
    }

    private void PressButton()
    {
        IEnumerator pressedButton = ButtonPressed();
        isButtonPressed = true;
        StartCoroutine(pressedButton);
    }

    private IEnumerator ButtonPressed()
    {
        yield return new WaitForSeconds(1);
        isButtonPressed = false;
    }
}