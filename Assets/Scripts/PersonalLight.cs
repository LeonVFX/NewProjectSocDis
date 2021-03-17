using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalLight : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    [SerializeField] private float radius = 0f;
    [SerializeField] private Vector3 offset;
    private Vector3 pivot;

    private void Start()
    {
        playerView = GetComponentInParent<PhotonView>();
        player = playerView.GetComponent<Player>();

        gameObject.SetActive(false);
        GameManager.gm.OnStage2 += ToggleFlashLights;
    }

    private void Update()
    {
        // Do not move light if it is mine and I am dead
        if (playerView.IsMine && !player.isAlive)
            return;

        pivot = transform.parent.position + offset;

        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(pivot);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Light angle
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - 45, 0f));

        // Orbit
        transform.position = pivot + (radius * transform.forward);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void ToggleFlashLights()
    {
        gameObject.SetActive(true);
    }
}