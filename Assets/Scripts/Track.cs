using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    [SerializeField] private float trackLifespan = 5f;

    private void Start()
    {
        IEnumerator fadeTrack = ShrinkTracks();
        StartCoroutine(fadeTrack);
    }

    private IEnumerator ShrinkTracks()
    {
        Vector3 originalSize = transform.localScale;
        Vector3 targetSize = new Vector3(0f, 0f, 0f);
        float timer = 0f;

        while (timer <= trackLifespan)
        {
            timer += Time.deltaTime;
            float t = timer / trackLifespan;

            transform.localScale = Vector3.Lerp(originalSize, targetSize, t);
            yield return null;
        }

        yield return null;

        if (!PhotonNetwork.IsMasterClient)
            yield break;

        PhotonNetwork.Destroy(this.gameObject);
    }
}
