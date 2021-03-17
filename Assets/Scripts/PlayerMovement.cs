using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    private enum MoveState
    {
        Idle,
        Moving
    }

    private MoveState moveState = MoveState.Idle;

    public event System.Action OnStop;
    public event System.Action OnMove;
    private PhotonView playerView;
    private Player player;

    // Decides when player is able to move
    private bool canMove;
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private Rigidbody rb = null;

    private float playerSpeed = 0f;
    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

    LayerMask layer;
    float timeElapsed = 0f;

    private void Awake()
    {
        layer = LayerMask.GetMask("Ground");
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        // Mouse over UI
        if (player.IsPointerOverUIObject())
            return;

        // Movement
        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();

            foreach (var hit in hits)
            {
                // If not hitting UI
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (canMove)
                    {
                        // Moving
                        Vector3 dir = (hit.point - transform.position).normalized;
                        rb.AddForce(playerSpeed * dir, ForceMode.Force);
                    }
                    return;
                }
            }
        }
    }

    private void Update()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        // Mouse over UI
        if (player.IsPointerOverUIObject())
            return;

        // Movement Animation
        if (rb.velocity.magnitude > 1f)
        {
            ChangeState(MoveState.Moving);
        }
        else
        {
            ChangeState(MoveState.Idle);
        }
    }

    void ChangeState(MoveState state)
    {
        switch (state)
        {
            case MoveState.Idle:
                OnStop?.Invoke();
                break;
            case MoveState.Moving:
                OnMove?.Invoke();
                break;
            default:
                break;
        }
    }
}