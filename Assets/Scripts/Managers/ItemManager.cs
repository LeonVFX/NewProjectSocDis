using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Event
    public event System.Action<Item> OnGotItem;
    public event System.Action OnDropItem;

    public static ItemManager im;
    private PhotonView managerView;

    private Player[] players;
    private List<Item> itemList;
    private int numberOfItems = 8;
    public List<Item> itemsInRange;

    private void Awake()
    {
        // Singleton
        if (im == null)
        {
            im = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        managerView = GetComponent<PhotonView>();
    }

    public void SetupItemsToPlayers()
    {
        managerView.RPC("RPC_SetupItemsToPlayers", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_SetupItemsToPlayers()
    {
        itemList = new List<Item>(FindObjectsOfType<Item>());
        foreach (Player player in PlayerManager.pm.playerList)
        {
            if (!player.playerView.IsMine)
                continue;

            foreach (Item item in itemList)
            {
                player.PHUD.OnItemInteraction += item.GetItem;
            }
        }
        itemsInRange = new List<Item>();
    }

    public void GetItem(Item item)
    {
        OnGotItem?.Invoke(item);
    }

    public void DropItem()
    {
        OnDropItem?.Invoke();
    }
}
