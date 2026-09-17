using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offset")]
    public float offsetX = 0f;
    public float offsetY = 2f;  // เพิ่ม Offset แกน Y เข้ามา
    public float offsetZ = -10f;

    [Header("Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Axis Control")]
    public bool followY = false;

    private float fixedY;

    void Start()
    {
        // บันทึกค่า Y เริ่มต้นของกล้องไว้ใช้เมื่อ followY = false
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        float x = target.position.x + offsetX;
        float y = followY ? (target.position.y + offsetY) : fixedY;
        float z = target.position.z + offsetZ;

        Vector3 desiredPosition = new Vector3(x, y, z);

        // เคลื่อนที่กล้องแบบนุ่มนวล
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}