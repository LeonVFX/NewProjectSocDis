using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Researcher : Player
{
    public bool isInfected = false;
    public bool inEscapePod = false;
    public bool inPod = false;

    protected override void Start()
    {
        base.Start();

        pMovement.playerSpeed *= speedMultiplier;

        // Deactivate Kill Button
        PHUD.ToggleKillButtonActive();
    }

    protected override void Update()
    {
        if (!playerView.IsMine || !isAlive)
            return;

        if(inPod == true)
        {

        }

        base.Update();
    }

    public void SetInfected(bool isInfected)
    {
        this.isInfected = isInfected;
    }

    public void InPod(bool inEscapePod)
    {
        this.inEscapePod = inEscapePod;
    }
}