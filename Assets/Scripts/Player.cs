using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerMovement))]

public class Player : MonoBehaviour
{
    // Events
    public event System.Action OnDeath;

    // Basic Player Components
    public PhotonView playerView = null;
    protected PlayerMovement pMovement;
    protected Camera cam;
    protected PlayerHUD pHUD;
    public PlayerHUD PHUD
    {
        get { return pHUD; }
    }

    // Player Stats
    [SerializeField] protected int playerNumber;
    public int PlayerNumber
    {
        get { return playerNumber; }
        set { playerNumber = value; }
    }
    [SerializeField] protected float baseSpeed = 30.0f;
    public float speedMultiplier = 1.0f;
    public bool isAlive;

    // Other
    private Item heldItem = null;
    public Item HeldItem
    {
        get { return heldItem; }
    }


    protected virtual void Awake()
    {
        pMovement = GetComponent<PlayerMovement>();

        // Setting HUD
        pHUD = GetComponentInChildren<PlayerHUD>();
        playerView = GetComponent<PhotonView>();
        pHUD.playerView = playerView;

    }

    protected virtual void Start()
    {
        DontDestroyOnLoad(this);
        isAlive = true;

        GameManager.gm.OnVoteStage += PreventMovement;
        GameManager.gm.OnStage2 += AllowMovement;

        ItemManager.im.OnGotItem += HoldItem;
        ItemManager.im.OnDropItem += DropItem;

        PlayerManager.pm.playerViews.Add(playerView);
        
        pMovement.PlayerSpeed = baseSpeed;
        
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
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void HoldItem(Item item)
    {
        if (!playerView.IsMine || !isAlive)
            return;

        heldItem = item;
        pHUD.HoldItem(heldItem.GetComponentInChildren<SpriteRenderer>().sprite.texture);
    }

    private void DropItem()
    {
        if (!playerView.IsMine || !isAlive)
            return;

        heldItem = null;
        pHUD.DropItem();
    }
}