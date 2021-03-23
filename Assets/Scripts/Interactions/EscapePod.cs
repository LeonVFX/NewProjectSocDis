﻿using Photon.Pun;
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
    [SerializeField] private float countdown = 5;
    [SerializeField] private float movement = 0;

    private bool isButtonPressed = false;
    private bool isResearcher = false;

    private void Start()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            RegisterPlayer(player);
        }
    }

    private void Update()
    {
        if (inRange && isResearcher && !launched)
        {
            if (GameManager.gm.currentStage == GameManager.GameStage.Stage2)
            {
                if (isButtonPressed)
                {
                    playerInPod = true;
                    --spaceleft;
                }

                if (spaceleft <= 1)
                {
                    countdown -= Time.deltaTime;
                    if (countdown <= 0 || spaceleft <= 0)
                    {
                        EndManager.em.Escaped();
                        launched = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Researcher" || otherParent.tag == "Creature")
        {
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            inRange = true;
            if (playerView.GetComponent<Researcher>())
                isResearcher = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherParent = (other.transform.parent != null) ? other.transform.parent.gameObject : null;

        if (otherParent == null)
            return;

        if (otherParent.tag == "Researcher" || otherParent.tag == "Creature")
        {
            PhotonView playerView = otherParent.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            inRange = false;
        }
    }

    private void RegisterPlayer(Player player)
    {
        player.PHUD.OnInteraction += PressButton;
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