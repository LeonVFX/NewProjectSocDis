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

    // Decides when player is able to move
    private bool canMove;

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    private bool isMoving = false;

    private Rigidbody rb = null;

    private float playerSpeed = 0f;

    public float PlayerSpeed
    {
        get { return playerSpeed; }
        set { playerSpeed = value; }
    }

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

    public void Move()
    {
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