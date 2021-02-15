﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMovement))]

public class Player : MonoBehaviour
{
    public event System.Action OnDeath;

    protected PlayerMovement pMovement;
    protected PlayerHUD pHUD;

    public PlayerHUD PHUD
    {
        get { return pHUD; }
    }

    [SerializeField]
    protected int playerNumber;

    [SerializeField]
    protected float baseSpeed = 30.0f;

    public float speedMultiplier = 1.0f;

    public bool isAlive;

    public PhotonView playerView = null;

    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }

    protected Camera cam;

    protected virtual void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();
        pHUD = GetComponentInChildren<PlayerHUD>();
    }

    protected virtual void Start()
    {
        isAlive = true;

        playerView = GetComponent<PhotonView>();

        GameManager.gm.OnVoteStage += PreventMovement;
        GameManager.gm.OnStage2 += AllowMovement;

        PlayerManager.pm.playerViews.Add(playerView);
        
        pMovement.playerSpeed = baseSpeed;
        
        cam = Camera.main.GetComponent<Camera>();

        if (playerView.IsMine)
            cam.GetComponent<CameraFollow>().setTarget(gameObject.transform);
    }

    protected virtual void Update()
    {
        if (!playerView.IsMine || !isAlive)
            return;

        // Mouse over UI
        if (IsPointerOverUIObject())
            return;

        pMovement.Move();
    }

    protected void PreventMovement()
    {
        pMovement.CanMove = false;
    }

    protected void AllowMovement()
    {
        pMovement.CanMove = true;
    }

    //public void Die()
    //{
    //    Debug.Log($"Player { this.name } Died");
    //    OnDeath?.Invoke();
    //    isAlive = false;
    //}

    public void Die()
    {
        playerView.RPC("RPC_Die", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_Die()
    {
        Debug.Log($"Player { this.name } Died");
        OnDeath?.Invoke();
        isAlive = false;
    }

    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}