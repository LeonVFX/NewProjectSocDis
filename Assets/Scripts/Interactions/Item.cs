using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Item Type
    enum ItemType
    {
        Gas
    }

    // HUD
    [SerializeField] private Sprite itemSprite = null;
}
