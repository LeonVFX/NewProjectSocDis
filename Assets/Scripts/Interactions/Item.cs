using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private PhotonView itemView;

    // Item Type
    public enum ItemType
    {
        Default,
        Gas
    }

    // HUD
    [Header("Item Settings")]
    [SerializeField] private ItemType itemType = ItemType.Default;
    [SerializeField] private Sprite itemSprite = null;

    // Player
    private bool isInRange = false;
    private bool getItem = false;

    private void Start()
    {
        itemView = GetComponent<PhotonView>();
        itemSprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (isInRange)
        {
            if (Input.GetButtonDown("Interact") || getItem)
            {
                ItemManager.im.GetItem(this);
                itemView.RPC("RPC_DisableItem", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Creature" || other.tag == "Researcher")
        {
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            isInRange = false;
        }
    }

    public void GetItem()
    {
        IEnumerator gotItem = GotItem();
        getItem = true;
        StartCoroutine(gotItem);
    }

    private IEnumerator GotItem()
    {
        yield return new WaitForEndOfFrame();
        getItem = false;
    }

    [PunRPC]
    private void RPC_DisableItem()
    {
        gameObject.SetActive(false);
    }
}