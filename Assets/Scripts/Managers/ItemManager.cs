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

    public void SetupItemsToPlayers()
    {
        itemList = new List<Item>(FindObjectsOfType<Item>());
        Debug.Log($"{itemList.Count} / {PlayerManager.pm.playerList.Count}");
        foreach (Player player in PlayerManager.pm.playerList)
        {
            foreach (Item item in itemList)
            {
                player.PHUD.OnItemInteraction += item.GetItem;
                Debug.Log($"{item} setup to {player}");
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
