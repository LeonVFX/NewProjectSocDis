using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : Player
{
    public bool isInfected = false;
    public bool inPod = false;

    protected override void Start()
    {
        base.Start();

        pMovement.PlayerSpeed *= speedMultiplier;

        // Deactivate Creature UI
        PHUD.ToggleKillButtonActive();
        pHUD.ToggleGoopAmountActive();
    }

    protected override void Update()
    {
        base.Update();

        if (!playerView.IsMine || !isAlive)
            return;
    }

    public void SetInfected(bool isInfected)
    {
        this.isInfected = isInfected;
    }
    public void EnterPod(bool inPod)
    {
        this.inPod = inPod;
    }
}