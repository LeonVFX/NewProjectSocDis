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
    public ItemType itemType = ItemType.Default;
    [SerializeField] private Sprite itemSprite = null;
    [SerializeField] private Vector3 itemDropOffset = Vector3.zero;

    // Player
    private bool isInRange = false;
    private bool itemBuffer = false;
    private bool isHeld = false;

    private void Start()
    {
        itemView = GetComponent<PhotonView>();
        itemSprite = GetComponentInChildren<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (isInRange)
        {
            if (Input.GetButtonDown("Interact") && !isHeld || itemBuffer && !isHeld)
            {
                ItemManager.im.GetItem(this);

                // Disable item for everyone
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

    public void GetItem(PhotonView playerView, bool hasHeldItem)
    {
        if (!playerView.IsMine)
            return;

        IEnumerator bufferTimer = ItemBuffer();

        if (hasHeldItem)
        {
            DropItem(playerView.transform);
            itemBuffer = true;
            playerView.GetComponent<Player>().PHUD.hasHeldItem = false;
            StartCoroutine(bufferTimer);
            return;
        }

        if (isHeld)
            return;

        itemBuffer = true;
        playerView.GetComponent<Player>().PHUD.hasHeldItem = true;
        StartCoroutine(bufferTimer);
    }

    private void DropItem(Transform playerPos)
    {
        ItemManager.im.DropItem();
        itemView.RPC("RPC_EnableItem", RpcTarget.All, new object[] { playerPos.position });
    }

    private IEnumerator ItemBuffer()
    {
        yield return new WaitForEndOfFrame();
        itemBuffer = false;
    }

    [PunRPC]
    private void RPC_DisableItem()
    {
        isHeld = true;
        gameObject.SetActive(false);
    }

    [PunRPC]
    private void RPC_EnableItem(Vector3 playerPos)
    {
        transform.position = playerPos + itemDropOffset;
        isHeld = false;
        gameObject.SetActive(true);
    }
}