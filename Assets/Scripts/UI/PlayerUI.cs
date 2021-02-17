using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviourPunCallbacks
{
    public Player localPlayer;
    private PhotonView playerView;

    public PhotonView PlayerView
    {
        get { return playerView; }
    }

    // Voting Related
    [SerializeField] GameObject votingPlayerTag = null;
    [SerializeField] private VotingUI votingUI = null;
    private GameObject votingGridChild = null;

    private void Awake()
    {
        playerView = GetComponentInParent<PhotonView>();
        localPlayer = playerView.GetComponentInParent<Player>();
    }

    private void Start()
    {
        if (!playerView.IsMine)
            return;

        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

        GameManager.gm.OnVoteStage += EnableVotingUI;
        GameManager.gm.OnStage2 += DisableVotingUI;

        votingUI.gameObject.SetActive(false);
        votingGridChild = votingUI.GetComponentInChildren<GridLayoutGroup>().gameObject;
    }

    private void Update()
    {
        if(!playerView.IsMine)
        {
            if (this != null)
                Destroy(this.gameObject);
            return;
        }
    }

    #region Voting System
    private void EnableVotingUI()
    {
        if (!playerView.IsMine)
            return;

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            GameObject newVotingPlayerTag = Instantiate(votingPlayerTag, votingGridChild.transform);
            newVotingPlayerTag.GetComponent<PlayerTagUI>().photonPlayer = player;
            newVotingPlayerTag.GetComponent<PlayerTagUI>().playerName.text = player.NickName;
            newVotingPlayerTag.GetComponent<PlayerTagUI>().playerTagNumber = player.ActorNumber;
        }
        votingUI.gameObject.SetActive(true);
    }

    private void DisableVotingUI()
    {
        if (!playerView.IsMine)
            return;

        votingUI.gameObject.SetActive(false);
    }
    #endregion

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (GameManager.gm.currentStage == GameManager.GameStage.Voting)
        {
            foreach (PlayerTagUI tag in votingGridChild.GetComponentsInChildren<PlayerTagUI>())
            {
                if (tag.photonPlayer == otherPlayer)
                {
                    tag.voteButton.interactable = false;
                }
            }
        }
    }
}