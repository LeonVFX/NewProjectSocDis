using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoverTransparency : MonoBehaviour
{
    private PhotonView playerView = null;
    private PhotonView coverView = null;
    [SerializeField] private float minOpacity = 0.5f;
    [SerializeField] private float maxOpacity = 1.0f;
    private float coroutineWaitTime = 0.2f;

    private Tilemap tm;

    private Coroutine OpacityUpCoroutine;
    private Coroutine OpacityDownCoroutine;
    private bool isOpacityUp = false;
    private bool isOpacityDown = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;

        if (coverView == null)
        {
            Cover[] allCovers = Resources.FindObjectsOfTypeAll<Cover>();

            // Set Coverview
            foreach (Cover cover in allCovers)
            {
                if (cover.GetComponent<PhotonView>().AmOwner)
                {
                    coverView = cover.GetComponent<PhotonView>();
                    coroutineWaitTime = coverView.GetComponent<Cover>().coroutineWaitTime;
                    break;
                }
            }
        }

        if (isOpacityUp)
        {
            Color tempColor = tm.color;

            if (tempColor.a < maxOpacity)
            {
                tempColor.a = Mathf.MoveTowards(tempColor.a, maxOpacity, Time.fixedDeltaTime);
                tm.color = tempColor;
            }
            else
                isOpacityUp = false;
        }

        if (isOpacityDown)
        {
            Color tempColor = tm.color;

            if (tempColor.a > minOpacity)
            {
                tempColor.a = Mathf.MoveTowards(tempColor.a, minOpacity, Time.fixedDeltaTime);
                tm.color = tempColor;
            }
            else
                isOpacityDown = false;
        }
    }

    private IEnumerator OpacityUp()
    {
        if (!playerView.IsMine)
            yield break;

        if (OpacityDownCoroutine != null)
            StopCoroutine(OpacityDownCoroutine);

        isOpacityDown = false;
        yield return new WaitForSeconds(coroutineWaitTime);
        isOpacityUp = true;
    }

    private IEnumerator OpacityDown()
    {
        if (!playerView.IsMine)
            yield break;

        if (OpacityUpCoroutine != null)
            StopCoroutine(OpacityUpCoroutine);

        isOpacityUp = false;
        yield return new WaitForSeconds(coroutineWaitTime);
        isOpacityDown = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.tag == "Cover")
        {
            if (other.GetComponentInParent<PhotonView>().IsMine)
            {
                tm = other.GetComponent<Tilemap>();
                OpacityDownCoroutine = StartCoroutine(OpacityDown());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.tag == "Cover")
        {
            if(other.GetComponentInParent<PhotonView>().IsMine)
            {
                tm = other.GetComponent<Tilemap>();
                OpacityUpCoroutine = StartCoroutine(OpacityUp());
            }
        }
    }
}