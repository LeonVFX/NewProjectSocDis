using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSprint : MonoBehaviour
{
    private PhotonView playerView;
    private Creature creature;
    private PlayerMovement pMovement;
    private Rigidbody rb;

    [SerializeField] int trackInterval = 2;
    [SerializeField] float speedDifference = 10f;
    [SerializeField] private GameObject footprintPrefab;

    private bool trackTimeout = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();

        if (!playerView.IsMine)
            return;

        creature = GetComponent<Creature>();
        pMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        CreatureObject creatureObject = creature.creatureObject;
        trackInterval = creatureObject.trackInterval;
        speedDifference = creatureObject.speedDifference;
        footprintPrefab = creatureObject.footprintPrefab;
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;

        if (Input.GetButtonDown("Run"))
            pMovement.PlayerSpeed += speedDifference;
        if (Input.GetButtonUp("Run"))
            pMovement.PlayerSpeed -= speedDifference;
    }

    private void LateUpdate()
    {
        if (!playerView.IsMine)
            return;

        if (rb.velocity.magnitude > 4f)
            LeaveTrack();
    }

    private void LeaveTrack()
    {
        if (!trackTimeout)
        {
            // playerView.RPC("RPC_Track", RpcTarget.All);
            PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "Track"), transform.position + new Vector3(0f, 0.01f, 0f), Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z));
            trackTimeout = true;
            IEnumerator trackLifespan = TrackLifespan();
            StartCoroutine(trackLifespan);
        }
    }

    private IEnumerator TrackLifespan()
    {
        yield return new WaitForSeconds(trackInterval);
        trackTimeout = false;
    }

    [PunRPC]
    private void RPC_Track()
    {
        PhotonNetwork.InstantiateRoomObject(System.IO.Path.Combine("GamePrefabs", "Track"), transform.position + new Vector3(0f, 0.01f, 0f), Quaternion.Euler(90f, transform.rotation.y, transform.rotation.z));
    }
}
