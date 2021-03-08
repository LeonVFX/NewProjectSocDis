using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHinting : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;

    [SerializeField] int goopInterval = 1;
    [SerializeField] private GameObject goopPrefab;
    [SerializeField] private ParticleSystem goopParticle;

    private bool goopTimeout = false;

    // Goop Management Time
    [Header("Goop Time Management")]
    [SerializeField] private float goopStartTime;
    private float goopElapsedTime;
    [SerializeField] private float goopBufferTime;
    private float goopElapsedBufferTime;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;

        if (player.HeldItem != null)
        {
            goopElapsedTime += Time.deltaTime;
            goopElapsedBufferTime = goopBufferTime;

            if (goopElapsedTime >= goopStartTime)
            {
                Goop();
            }

            player.PHUD.GoopValue = goopElapsedTime / goopStartTime;
        }
        else
        {
            goopElapsedBufferTime -= Time.deltaTime;

            if (goopElapsedBufferTime <= 0f)
            {
                goopElapsedTime = 0f;
            }

            player.PHUD.GoopValue = goopElapsedBufferTime / goopBufferTime;
        }
    }

    private void Goop()
    {
        if (!goopTimeout)
        {
            playerView.RPC("RPC_Goop", RpcTarget.All);
            goopTimeout = true;
            IEnumerator goopLifespan = GoopLifespan();
            StartCoroutine(goopLifespan);
        }
    }

    private IEnumerator GoopLifespan()
    {
        yield return new WaitForSeconds(goopInterval);
        goopTimeout = false;
    }

    [PunRPC]
    private void RPC_Goop()
    {
        //PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "Goop"), transform.position + new Vector3(0f, 0.01f, 0f), Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z));
        goopParticle.Play();
    }
}
