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
        Gas,
        PenDrive,
        Crate,
        CreatureMaterial,
        Fertilizer,
        ResearcherFiles,
        Food,
        Belongings
    }

    // HUD
    [Header("Item Settings")]
    public ItemType itemType = ItemType.Default;

    // Player
    private bool isInRange = false;
    private bool itemBuffer = false;
    private bool isHeld = false;

    private void Start()
    {
        itemView = GetComponent<PhotonView>();
        transform.rotation = Quaternion.Euler(45f, 45f, 0f);
    }

    private void Update()
    {
        if (isInRange)
        {
            if (itemBuffer && !isHeld)
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

    public void GetItem(PhotonView playerView)
    {
        if (!playerView.IsMine)
            return;

        IEnumerator bufferTimer = ItemBuffer();
        Item heldItem = playerView.GetComponent<Player>().HeldItem;

        if (heldItem == this)
        {
            DropItem(playerView.transform);
            itemBuffer = true;
            StartCoroutine(bufferTimer);
            return;
        }

        if (isHeld)
            return;

        itemBuffer = true;
        StartCoroutine(bufferTimer);
    }

    private void DropItem(Transform playerPos)
    {
        ItemManager.im.DropItem();
        float[] randItemOffset = { -0.35f, 0.35f };
        Vector3 itemDropOffset = new Vector3(randItemOffset[Random.Range(0, randItemOffset.Length)], 0.3f, randItemOffset[Random.Range(0, randItemOffset.Length)]);
        //Debug.Log(itemDropOffset);
        itemView.RPC("RPC_EnableItem", RpcTarget.All, new object[] { playerPos.position, itemDropOffset });
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
    private void RPC_EnableItem(Vector3 playerPos, Vector3 itemDropOffset)
    {
        transform.position = playerPos + itemDropOffset;
        isHeld = false;
        gameObject.SetActive(true);
    }
}