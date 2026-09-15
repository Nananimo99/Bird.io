using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public float offsetX = 5f;
    public float offsetZ = -12f;

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Axis")]
    public bool followY = false;

    private float fixedY;

    void Start()
    {
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float x = target.position.x + offsetX;
        float y = followY ? target.position.y : fixedY;
        float z = target.position.z + offsetZ;

        Vector3 desired = new Vector3(x, y, z);
        transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * smoothSpeed);
    }
}
