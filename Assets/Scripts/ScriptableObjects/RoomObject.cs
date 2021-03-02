using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class RoomObject : ScriptableObject
{
    public string roomName;
    public string roomDescription;
    public Item.ItemType requiredSabotageItem;
}
