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

    [SerializeField] private float forceAmount;

    public float playerSpeed = 30.0f;
    LayerMask layer;

    private void Awake()
    {
        layer = LayerMask.GetMask("Ground");
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
        // Movement
        if (Input.GetMouseButton(0))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), Mathf.Infinity, layer).OrderBy(h => h.distance).ToArray();
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    this.hit = hit;
                    Move();
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

    public void Move()
    {
        if (canMove)
        {
            // Moving
            rb.AddForce(forceAmount * (hit.point - transform.position), ForceMode.Force);

            if (rb.velocity.magnitude > playerSpeed)
                rb.velocity = rb.velocity.normalized * playerSpeed;

            // Rotating
            //Quaternion targetRotation = Quaternion.LookRotation(hit.point - transform.position);
            //targetRotation = new Quaternion(transform.rotation.x, targetRotation.y, transform.rotation.z, targetRotation.w);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerSpeed * Time.deltaTime);
        }
    }
}