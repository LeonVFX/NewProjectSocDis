using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task")]
public class TaskObject : ScriptableObject
{
    public string taskName;
    public string taskDescription;
    public Item.ItemType taskRequiredItemType;
}