using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : MonoBehaviour
{
    PhotonView playerView;
    Player player;
    PlayerMovement pMovement;
    public ResearcherObject researcherObject;

    public bool isInfected = false;

    private void Awake()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        pMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        pMovement.PlayerSpeed *= player.speedMultiplier;

        // Deactivate Creature UI
        player.PHUD.ToggleKillButtonActive();
        player.PHUD.ToggleGoopAmountActive();
    }

    private void Update()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;
    }

    public void SetInfected(bool isInfected)
    {
        this.isInfected = isInfected;
    }
}