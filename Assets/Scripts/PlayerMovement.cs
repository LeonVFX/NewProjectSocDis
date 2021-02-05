using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerMovement : MonoBehaviour
{
    public event System.Action OnStop;
    public event System.Action OnMove;
    private PhotonView playerView;

    // Mouse hit
    private RaycastHit hit;

    public RaycastHit Hit
    {
        get { return hit; }
        set { hit = value; }
    }

    // Decides when player is able to move
    private bool canMove;

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private bool isMoving = false;

    private Rigidbody rb = null;

    public float playerSpeed = 30.0f;
    LayerMask layer;

    private void Awake()
    {
        layer = LayerMask.GetMask("Ground");
        playerView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerView.IsMine)
            return;
    }

    public void Move()
    {
        // Movement
        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    this.hit = hit;
                    if (canMove)
                    {
                        // Moving
                        Vector3 dir = (hit.point - transform.position).normalized;
                        rb.AddForce(playerSpeed * dir, ForceMode.Force);
                    }
                    break;
                }
            }
            if (!isMoving)
            {
                OnMove?.Invoke();
                isMoving = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnStop?.Invoke();
            isMoving = false;
        }
    }
}