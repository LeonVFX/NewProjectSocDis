using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHinting : MonoBehaviour
{
    private PhotonView playerView;
    private Creature creature;
    private Player player;

    [SerializeField] private int goopInterval = 1;
    [SerializeField] private GameObject goopParticlePrefab;
    private ParticleSystem goopParticle;

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

        creature = GetComponent<Creature>();
        player = GetComponent<Player>();

        GameManager.gm.OnStage1 += OnGameStart;

        CreatureObject creatureObject = creature.creatureObject;
        goopInterval = creatureObject.goopInterval;
        goopParticlePrefab = creatureObject.goopParticlePrefab;
        goopStartTime = creatureObject.goopStartTime;
        goopBufferTime = creatureObject.goopBufferTime;

        goopParticle = goopParticlePrefab.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = goopParticle.main;
        main.playOnAwake = false;
        main.loop = false;
    }

    private void OnGameStart()
    {
        if (!playerView.IsMine)
            return;

        GameObject goop = Instantiate(goopParticlePrefab, new Vector3(0.4f, 0.4f, 0.4f), Quaternion.Euler(45, 45, 0), player.transform);
        goopParticle = goop.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!playerView.IsMine || creature.hasMorphed)
            return;

        if (player.HeldItem != null)
        {
            goopElapsedTime += Time.deltaTime;
            goopElapsedBufferTime = goopBufferTime;

            if (goopElapsedTime >= goopStartTime)
                Goop();

            player.PHUD.GoopValue = goopElapsedTime / goopStartTime;
        }
        else
        {
            goopElapsedBufferTime -= Time.deltaTime;

            if (goopElapsedBufferTime <= 0f)
                goopElapsedTime = 0f;

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
