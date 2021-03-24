/// Kill Function is here

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
    private bool isKill;

    private List<Player> targetPlayers;

    protected override void Start()
    {
        base.Start();

        pMovement.PlayerSpeed *= speedMultiplier;

        targetPlayers = new List<Player>();
        canKill = true;
        isKill = false;

        GameManager.gm.OnStage2 += Stage2ToggleUI;
        PHUD.OnKill += PressKill;

        // Deactivate Task List
        PHUD.ToggleTaskListActive();
        // Deactivate Kill For Stage 1
        PHUD.ToggleKillButtonActive();
    }

    protected override void Update()
    {
        base.Update();

        if (!playerView.IsMine || !isAlive)
            return;

        // Call Killing
        if (isKill && canKill)
        {
            if (targetPlayers.Count > 0)
                KillObject(targetPlayers[0].GetComponent<PhotonView>().ViewID);
        }
    }

    public void KillObject(int objPhotonViewId)
    {
        // Basic Kill Effects
        playerView.RPC("RPC_KillPlayer", RpcTarget.All, new object[] { objPhotonViewId });
        OnKill?.Invoke();

        // Kill Timer
        PreventMovement();

        if (playerView.IsMine)
            StartCoroutine(KillTimer());
    }

    [PunRPC]
    private void RPC_KillPlayer(int targetObjPhotonViewId)
    {
        GameObject targetPlayer = PhotonView.Find(targetObjPhotonViewId).gameObject;
        targetPlayer.GetComponent<PhotonView>().RPC("RPC_Die", RpcTarget.All);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.transform.parent.tag == "Researcher")
        {

            GameObject otherParent = other.transform.parent.gameObject;
            Player target = otherParent.GetComponent<Player>();

            if (targetPlayers == null)
                return;

            if (target.isAlive)
            {
                targetPlayers.Add(target);

                // TODO: TOGGLE KILL
                if (targetPlayers.Count > 0)
                {
                    Debug.Log(targetPlayers.Count);
                    pHUD.ToggleKillButtonInteractableActive();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerView != null)
            if (!playerView.IsMine)
                return;

        if (other.transform.parent.tag == "Researcher")
        {

            GameObject otherParent = other.transform.parent.gameObject;
            Player target = otherParent.GetComponent<Player>();

            if (target.isAlive || targetPlayers.Contains(target))
            {
                targetPlayers.Remove(target);

                // TODO: TOGGLE KILL
                if (targetPlayers.Count == 0)
                {
                    Debug.Log(targetPlayers.Count);
                    pHUD.ToggleKillButtonInteractableInactive();
                }
            }
        }
    }

    private IEnumerator KillTimer()
    {
        canKill = false;
        yield return new WaitForSeconds(killStunTime);
        AllowMovement();
        canKill = true;
    }

    private void PressKill()
    {
        IEnumerator pressedKill = KillPressed();
        isKill = true;
        StartCoroutine(pressedKill);
    }

    private IEnumerator KillPressed()
    {
        yield return new WaitForEndOfFrame();
        isKill = false;
    }

    private void Stage2ToggleUI()
    {
        if (!playerView.IsMine)
            return;

        PHUD.ToggleKillButtonActive();
    }
}