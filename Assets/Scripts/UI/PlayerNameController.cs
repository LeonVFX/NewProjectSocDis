using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameController : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMesh playerName = null;

    private void Update()
    {
        playerName.text = GetComponent<PhotonView>().Owner.NickName;
    }
}