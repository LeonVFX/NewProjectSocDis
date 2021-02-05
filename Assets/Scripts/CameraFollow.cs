using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(-2, 4, -2);
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