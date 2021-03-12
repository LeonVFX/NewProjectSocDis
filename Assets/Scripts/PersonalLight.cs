﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalLight : MonoBehaviour
{
    [SerializeField] private float radius = 0f;
    [SerializeField] private Vector3 offset;
    private Vector3 pivot;

    private void Update()
    {
        pivot = transform.parent.position + offset;

        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(pivot);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Light angle
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - 45, 0f));

        // Orbit
        transform.position = pivot + (radius * transform.forward);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}