using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTagUI : MonoBehaviour
{
    private PhotonView voteView;
    private Image tagBackground;
    public Photon.Realtime.Player photonPlayer;
    public Text playerName;
    public Button voteButton;
    // Number of local Player
    public int playerNumber;
    // Number From The Player in Tag
    public int playerTagNumber;

    public Player debugPlayer;

    private void Start()
    {
        voteView = FindObjectOfType<VoteManager>().GetComponent<PhotonView>();
        playerNumber = GetComponentInParent<PlayerUI>().PlayerView.OwnerActorNr;
        tagBackground = GetComponent<Image>();
    }

    public void Vote()
    {
        voteView.RPC("RPC_Vote", RpcTarget.All, new object[] { playerName.text, playerTagNumber, playerNumber });
        tagBackground.color = Color.red;

        GridLayoutGroup parent = GetComponentInParent<GridLayoutGroup>();
        foreach (PlayerTagUI playerTags in parent.GetComponentsInChildren<PlayerTagUI>())
        {
            playerTags.voteButton.interactable = false;
        }
    }
}