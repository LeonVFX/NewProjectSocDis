using System.Collections;
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

        Vector3 dir = Vector3.Normalize(new Vector3(mouseOnScreen.x, 0.0f, mouseOnScreen.y) - new Vector3(positionOnScreen.x, 0.0f, positionOnScreen.y));
        transform.position = pivot + (dir * radius);

        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - Camera.main.transform.rotation.y, 0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}