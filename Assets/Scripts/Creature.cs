using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature : Player
{
    // Event activated when killing
    public event System.Action OnKill;

    [SerializeField]
    private int killStunTime = 3;

    private bool canKill;

    private List<int> targetNum;
    private Player[] targetPlayers;

    protected override void Start()
    {
        base.Start();
        pMovement.playerSpeed *= speedMultiplier;

        targetNum = new List<int>();
        targetPlayers = new Player[8];
        canKill = true;
    }

    protected override void Update()
    {
        if (!playerView.IsMine)
            return;

        base.Update();
        
        // Call Killing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (targetNum.Count > 0)
            {
                int index = targetNum[Random.Range(0, targetNum.Count)];
                if (targetPlayers[index] != null && canKill)
                    KillObject(targetPlayers[index].GetComponent<PhotonView>().ViewID);
            }
        }
    }

    public void KillObject(int objPhotonViewId)
    {
        playerView.RPC("RPC_KillPlayer", RpcTarget.All, new object[] { objPhotonViewId });
        OnKill?.Invoke();
        PreventMovement();

        if (playerView.IsMine)
            StartCoroutine(KillTimer());
    }

    [PunRPC]
    private void RPC_KillPlayer(int targetObjPhotonViewId)
    {
        GameObject targetPlayer = PhotonView.Find(targetObjPhotonViewId).gameObject;
        targetPlayer.GetComponent<Player>().Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.tag == "Researcher")
        {
            Player target = other.GetComponent<Player>();
            targetPlayers[target.PlayerNumber] = target;
            targetNum.Add(target.PlayerNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.tag == "Researcher")
        {
            Player target = other.GetComponent<Player>();
            targetPlayers[target.PlayerNumber] = null;
            targetNum.Remove(target.PlayerNumber);
        }
    }

    private IEnumerator KillTimer()
    {
        canKill = false;
        yield return new WaitForSeconds(killStunTime);
        AllowMovement();
        canKill = true;
    }
}