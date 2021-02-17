using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    private RectTransform rec = null;
    private float width = 0f;
    private float height = 0f;
    private int childNum = 0;

    private void Start()
    {
        rec = GetComponent<RectTransform>();
        width = rec.rect.width;
        height = rec.rect.height;
    }

    private void Update()
    {
        // Adjust Size of TaskList
        childNum = 0;

        foreach (Transform item in GetComponentsInChildren<Transform>())
        {
            childNum++;
        }

        rec.sizeDelta = new Vector2(width, height * childNum);
    }
}
