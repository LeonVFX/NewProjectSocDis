using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSprint : MonoBehaviour
{
    private PhotonView playerView;
    private PlayerMovement pMovement;

    [SerializeField] int trackInterval = 2;
    [SerializeField] float speedDifference = 10f;
    [SerializeField] private GameObject footprintPrefab;

    private bool trackTimeout = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        pMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Run"))
        {
            pMovement.PlayerSpeed += speedDifference;
        }
        if (Input.GetButton("Run"))
        {
            LeaveTrack();
        }
        if (Input.GetButtonUp("Run"))
        {
            pMovement.PlayerSpeed -= speedDifference;
        }
    }

    private void LeaveTrack()
    {
        if (!trackTimeout)
        {
            playerView.RPC("RPC_Track", RpcTarget.All);
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
