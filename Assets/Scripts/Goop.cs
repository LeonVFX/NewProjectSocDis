using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goop : MonoBehaviour
{
    [SerializeField] private float goopLifespan = 5f;

    private void Start()
    {
        IEnumerator fadeGoop = ShrinkGoop();
        StartCoroutine(fadeGoop);
    }

    private IEnumerator ShrinkGoop()
    {
        Vector3 originalSize = transform.localScale;
        Vector3 targetSize = new Vector3(0f, 0f, 0f);
        float timer = 0f;

        while (timer <= goopLifespan)
        {
            timer += Time.deltaTime;
            float t = timer / goopLifespan;

            transform.localScale = Vector3.Lerp(originalSize, targetSize, t);
            yield return null;
        }

        if (!PhotonNetwork.IsMasterClient)
            yield break;

        PhotonNetwork.Destroy(this.gameObject);
    }
}
