using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(-2.0f, 4.0f, -2.0f);
    }

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
        }
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}