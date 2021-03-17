using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalLight : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    [SerializeField] private new GameObject light;
    [SerializeField] private float radius = 0f;
    [SerializeField] private Vector3 offset;
    private Vector3 pivot;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();

        light.SetActive(false);
        GameManager.gm.OnStage2 += ToggleFlashLights;
    }

    private void LateUpdate()
    {
        // Do not move light if it is mine and I am dead
        if (!playerView.IsMine || !player.isAlive)
            return;

        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(pivot);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        playerView.RPC("RPC_RotateLight", RpcTarget.All, new object[] { positionOnScreen, mouseOnScreen });
    }

    [PunRPC]
    private void RPC_RotateLight(Vector2 positionOnScreen, Vector2 mouseOnScreen)
    {
        pivot = transform.position + offset;

        // Light angle
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        light.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - 45, 0f));

        // Orbit
        light.transform.position = pivot + (radius * light.transform.forward);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void ToggleFlashLights()
    {
        light.SetActive(true);
    }
}