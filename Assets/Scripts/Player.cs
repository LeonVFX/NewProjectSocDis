using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]

public class Player : MonoBehaviour
{
    protected PlayerMovement pMovement;

    [SerializeField]
    protected int playerNumber;

    [SerializeField]
    protected float baseSpeed = 30.0f;

    public float speedMultiplier = 1.0f;

    public bool isAlive = true;

    public PhotonView playerView = null;

    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }

    protected Camera cam;

    protected virtual void Start()
    {
        playerView = GetComponent<PhotonView>();

        GameManager.gm.OnVoteStage += PreventMovement;
        GameManager.gm.OnStage2 += AllowMovement;

        PlayerManager.pm.playerViews.Add(playerView);

        pMovement = GetComponent<PlayerMovement>();
        pMovement.playerSpeed = baseSpeed;
        isAlive = true;
        cam = Camera.main.GetComponent<Camera>();

        if (playerView.IsMine)
            cam.GetComponent<CameraFollow>().setTarget(gameObject.transform);
    }

    protected virtual void Update()
    {
        if (!playerView.IsMine || !isAlive)
            return;

        if (Input.GetMouseButton(0))
        {
            Vector2 ray = cam.ScreenToWorldPoint(Input.mousePosition);
            pMovement.Hit = Physics2D.Raycast(ray, Vector2.zero);

            if (pMovement.Hit)
                pMovement.Move();
        }
        if (Input.GetMouseButtonUp(0))
        {
            pMovement.Stop();
        }
    }

    protected void PreventMovement()
    {
        pMovement.CanMove = false;
    }

    protected void AllowMovement()
    {
        pMovement.CanMove = true;
    }

    public void Die()
    {
        playerView.RPC("RPC_Die", RpcTarget.All);
    }

    [PunRPC]
    protected void RPC_Die()
    {
        Destroy(playerView.gameObject);
    }
}