using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Event
    public event System.Action<Item> OnGotItem;

    public static ItemManager im;

    private Player[] players;
    private List<Item> itemList;

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
        itemList = new List<Item>(FindObjectsOfType<Item>());
        players = FindObjectsOfType<Player>();
        foreach (Player player in players)
        {
            foreach (Item item in itemList)
            {
                player.PHUD.OnItemInteraction += item.GetItem;
            }
        }
    }

    public void GetItem(Item item)
    {
        OnGotItem?.Invoke(item);
    }
}
