using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    private PlayerMovement pMovement;
    private Animator[] animList;
    private bool flip = false;
    private float halfScreenWidth;

    public bool Flip
    {
        get { return flip; }
        set
        {
            playerView.RPC("RPC_Flip", RpcTarget.All, new object[] { value });
            flip = value;
        }
    }

    [SerializeField] private Type[] playerTypes;

    private void Start()
    {
        playerView = GetComponentInParent<PhotonView>();

        player = GetComponentInParent<Player>();
        player.OnDeath += Death;

        pMovement = GetComponent<PlayerMovement>();
        pMovement.OnStop += Idle;
        pMovement.OnMove += Move;

        halfScreenWidth = Screen.width * 0.5f;

        animList = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animList)
            anim.speed *= GetComponent<Player>().speedMultiplier;
    }

    private void LateUpdate()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        if (Input.mousePosition.x > halfScreenWidth)
        {
            if (flip == false)
                Flip = true;
        }
        else
        {
            if (flip == true)
                Flip = false;
        }
    }

    private void Idle()
    {
        playerView.RPC("RPC_Idle", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Idle()
    {
        foreach (Animator anim in animList)
            anim.SetTrigger("Idle");
    }

    private void Move()
    {
        playerView.RPC("RPC_Move", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Move()
    {
        foreach (Animator anim in animList)
            anim.SetTrigger("Run");
    }

    [PunRPC]
    private void RPC_Flip(bool flip)
    {
        if (animList.Length != null)
            foreach (Animator anim in animList)
                if (anim != null)
                    anim.GetComponent<SpriteRenderer>().flipX = flip;
    }

    private void Death()
    {
        playerView.RPC("RPC_Death", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Death()
    {
        foreach (Animator anim in animList)
            anim.SetTrigger("Death");
    }
}