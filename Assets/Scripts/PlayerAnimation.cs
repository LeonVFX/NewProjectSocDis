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
    private Animator anim;

    [SerializeField] private Type[] playerTypes;

    private void Start()
    {
        playerView = GetComponentInParent<PhotonView>();
        player = GetComponentInParent<Player>();

        pMovement = GetComponent<PlayerMovement>();
        pMovement.OnStop += Idle;
        pMovement.OnMove += Move;

        anim = GetComponentInChildren<Animator>();
        anim.speed *= GetComponent<Player>().speedMultiplier;
    }

    private void Update()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        if (Input.mousePosition.x > (Screen.width * 0.5f))
        {
            playerView.RPC("RPC_Flip", RpcTarget.All, new object[] { true });
        }
        else
        {
            playerView.RPC("RPC_Flip", RpcTarget.All, new object[] { false });
        }
    }

    private void Idle()
    {
        playerView.RPC("RPC_Idle", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Idle()
    {
        anim.SetTrigger("Idle");
    }

    private void Move()
    {
        playerView.RPC("RPC_Move", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Move()
    {
        anim.SetTrigger("Run");
    }

    [PunRPC]
    private void RPC_Flip(bool flip)
    {
        if (anim != null)
            anim.GetComponent<SpriteRenderer>().flipX = flip;
    }
}
