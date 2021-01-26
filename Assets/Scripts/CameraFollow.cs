using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    private int depth = -10;

    [SerializeField]
    private float camSize = 3.0f;

    private void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + new Vector3(0, 0, depth);
        }
    }

    public void setTarget(Transform target)
    {
        GetComponent<Camera>().orthographicSize = camSize;
        playerTransform = target;
    }
}