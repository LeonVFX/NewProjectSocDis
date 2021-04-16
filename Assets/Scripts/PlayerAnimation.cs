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
    private Animator[] animList;
    private bool flip = false;
    private float halfScreenWidth;

    public bool Flip
    {
        get { return flip; }
        set
        {
            playerView.RPC("RPC_Flip", RpcTarget.All, new object[] { value });
            flip = value;
        }
    }

    [SerializeField] private Type[] playerTypes;

    private void Start()
    {
        playerView = GetComponentInParent<PhotonView>();

        player = GetComponentInParent<Player>();
        player.OnDeath += Death;

        pMovement = GetComponent<PlayerMovement>();
        pMovement.OnStop += Idle;
        pMovement.OnMove += Move;

        halfScreenWidth = Screen.width * 0.5f;

        animList = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animList)
            anim.speed *= GetComponent<Player>().speedMultiplier;

        GameManager.gm.OnStage2 += CreatureMorph;
    }

    private void LateUpdate()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        if (Input.mousePosition.x > halfScreenWidth)
        {
            if (flip == false)
                Flip = true;
        }
        else
        {
            if (flip == true)
                Flip = false;
        }
    }

    private void Idle()
    {
        playerView.RPC("RPC_Idle", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Idle()
    {
        if (animList != null)
        {
            foreach (Animator anim in animList)
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                    anim.SetTrigger("Idle");
        }
    }

    private void Move()
    {
        playerView.RPC("RPC_Move", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Move()
    {
        if (animList != null)
        {
            foreach (Animator anim in animList)
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                    anim.SetTrigger("Run");
        }
    }

    [PunRPC]
    private void RPC_Flip(bool flip)
    {
        // Flip like Paper Mario
        if (flip == true)
        {
            if (animList != null)
                foreach (Animator anim in animList)
                {
                    IEnumerator flipRoutine = FlipRoutine(anim, anim.transform.localScale, new Vector3(-1, 1, 1));
                    StartCoroutine(flipRoutine);
                }
        }
        else
        {
            if (animList != null)
            {
                foreach (Animator anim in animList)
                {
                    IEnumerator flipRoutine = FlipRoutine(anim, anim.transform.localScale, new Vector3(1, 1, 1));
                    StartCoroutine(flipRoutine);
                }
            }
        }
    }

    private IEnumerator FlipRoutine(Animator anim, Vector3 originalTransform, Vector3 targetTransform)
    {
        float elapsedTime = 0f;
        float totalTime = 0.2f;
        while (elapsedTime < totalTime)
        {
            anim.transform.localScale = Vector3.Lerp(originalTransform, targetTransform, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        anim.transform.localScale = targetTransform;
        yield return null;
    }

    private void Death()
    {
        playerView.RPC("RPC_Death", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Death()
    {
        if (animList != null)
        {
            foreach (Animator anim in animList)
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                    anim.SetTrigger("Death");
        }
    }

    private void CreatureMorph()
    {
        if (!player.isCreature)
            return;

        if (animList != null)
        {
            foreach (Animator anim in animList)
                anim.runtimeAnimatorController = Resources.Load("Animations/Creature") as RuntimeAnimatorController;
        }
    }
}