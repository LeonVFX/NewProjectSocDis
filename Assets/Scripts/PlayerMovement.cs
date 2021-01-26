using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMovement : MonoBehaviour
{
    public event System.Action OnStop;
    public event System.Action OnMove;

    // Mouse hit
    private RaycastHit2D hit;

    public RaycastHit2D Hit
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

    private PhotonView playerView = null;
    private Rigidbody2D rb = null;
    public float playerSpeed = 30.0f;
    private bool isMoving;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    public void Move()
    {
        if (canMove)
        {
            rb.MovePosition(Vector2.MoveTowards(transform.position, hit.point, playerSpeed * Time.fixedDeltaTime));
            if (!isMoving)
            {
                OnMove?.Invoke();
                isMoving = true;
            }
        }
    }

    public void Stop()
    {
        OnStop?.Invoke();
        isMoving = false;
    }

    #region Pathfinding Movement Class
    //// Event activated when moving
    //public event System.Action OnMove;

    //// Mouse hit
    //private RaycastHit2D hit;

    //public RaycastHit2D Hit
    //{
    //    get { return hit; }
    //    set { hit = value; }
    //}

    //// Decides when player is able to move
    //private bool canMove;

    //public bool CanMove
    //{
    //    get { return canMove; }
    //    set { canMove = value; }
    //}

    //// Returns the Pathfinding map
    //private Pathfinding pathFind;

    //public Pathfinding PathFind
    //{
    //    get { return pathFind; }
    //    set { pathFind = value; }
    //}

    //// Saves the path taken by the player
    //private List<Node> pathList;

    //public List<Node> PathList
    //{
    //    get { return pathList; }
    //    set { pathList = value; }
    //}

    //private Rigidbody2D rb;
    //private Coroutine moveCoroutine;

    //public float playerSpeed = 30.0f;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    hit = new RaycastHit2D();
    //    canMove = true;
    //    pathFind = GetComponent<Pathfinding>();
    //    pathList = new List<Node>();
    //}

    //public void Move()
    //{
    //    if (moveCoroutine != null)
    //        StopCoroutine(moveCoroutine);
    //    moveCoroutine = StartCoroutine(MovePlayerCoroutine());

    //    OnMove?.Invoke();
    //}

    //private IEnumerator MovePlayerCoroutine()
    //{
    //    if (pathList.Count <= 0)
    //    {
    //        yield return null;
    //    }
    //    else
    //    {
    //        pathList.RemoveAt(0);
    //        foreach (Node node in pathList)
    //        {
    //            while (Vector2.Distance(transform.position, node.pos) != 0.0f)
    //            {
    //                rb.MovePosition(Vector2.MoveTowards(transform.position, node.pos, playerSpeed * Time.fixedDeltaTime));
    //                yield return null;
    //            }
    //        }
    //    }
    //}
    #endregion
}