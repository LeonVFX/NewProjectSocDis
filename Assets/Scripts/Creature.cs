/// Kill Function is here

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    private PlayerMovement pMovement;
    public CreatureObject creatureObject;

    public bool hasMorphed = false;

    // Event activated when killing
    public event System.Action OnKill;

    [SerializeField]
    private int killStunTime = 3;

    private bool canKill;
    private bool isKill;

    private List<Player> targetPlayers;

    private void Awake()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        pMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (!playerView.IsMine)
            return;

        pMovement.PlayerSpeed *= player.speedMultiplier;
        killStunTime = creatureObject.killStunTime;

        targetPlayers = new List<Player>();
        canKill = true;
        isKill = false;

        GameManager.gm.OnStage2 += CreatureMorph;
        player.PHUD.OnKill += PressKill;

        // Deactivate Task List
        player.PHUD.ToggleTaskListActive();
        // Deactivate Kill For Stage 1
        player.PHUD.ToggleKillButtonActive();
    }

    private void Update()
    {
        if (!playerView.IsMine || !player.isAlive)
            return;

        // Call Killing
        if (isKill && canKill)
        {
            if (targetPlayers.Count > 0)
                KillObject(targetPlayers[0].GetComponent<PhotonView>().OwnerActorNr);
        }
    }

    public void KillObject(int playerNumber)
    {
        // Basic Kill Effects
        PlayerManager.pm.KillPlayer(playerNumber);
        OnKill?.Invoke();

        // Kill Timer
        player.PreventMovement();

        if (playerView.IsMine)
            StartCoroutine(KillTimer());
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
                    //Debug.Log(targetPlayers.Count);
                    player.PHUD.ToggleKillButtonInteractableActive();
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
                    player.PHUD.ToggleKillButtonInteractableInactive();
                }
            }
        }
    }

    private IEnumerator KillTimer()
    {
        canKill = false;
        yield return new WaitForSeconds(killStunTime);
        player.AllowMovement();
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

    private void CreatureMorph()
    {
        if (!playerView.IsMine)
            return;

        player.PHUD.ToggleKillButtonActive();
        hasMorphed = true;
    }
}