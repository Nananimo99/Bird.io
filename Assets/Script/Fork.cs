using UnityEngine;

public class Fork : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;

    [Header("Despawn")]
    public float despawnX = -12f;

    void Update()
    {
        if (!GameManager.instance.isPlaying) return;

        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < despawnX) Destroy(gameObject);
    }
}
