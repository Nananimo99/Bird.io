using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    [Header("Forward")]
    public float forwardSpeed = 3f;

    [Header("Flap")]
    public float flapForce = 5f;

    [Header("Rotation")]
    public float rotateSpeed = 8f;
    public float maxUpAngle = 25f;
    public float maxDownAngle = -70f;

    [Header("Ready State")]
    public float idleSpeed = 3f;
    public float idleHeight = 0.25f;

    [Header("Death")]
    public string deadlyTag = "Obstacle";
    public bool dieOnAnything = false;
    public bool showCollisionLog = true;

    private Rigidbody rb;
    private bool isAlive = true;
    private float startY;



  
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Start()
    {
        startY = transform.position.y;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    void Update()
    {
        if (!isAlive) return;

        if (!GameManager.instance.isPlaying)
        {
            Idle();
            if (FlapPressed()) StartGame();
            return;
        }

        if (FlapPressed()) Flap();
        ApplyRotation();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
        if (!GameManager.instance.isPlaying) return;

        Vector3 v = rb.linearVelocity;
        v.x = forwardSpeed;
        rb.linearVelocity = v;
    }

    bool FlapPressed()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Keyboard.current == null) return false;

        return Keyboard.current.spaceKey.wasPressedThisFrame
            || Keyboard.current.upArrowKey.wasPressedThisFrame
            || Keyboard.current.wKey.wasPressedThisFrame;
    }

    void Idle()
    {
        float y = startY + Mathf.Sin(Time.time * idleSpeed) * idleHeight;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void StartGame()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        GameManager.instance.StartGame();
        Flap();
    }

    void Flap()
    {
        Vector3 v = rb.linearVelocity;
        v.y = flapForce;
        rb.linearVelocity = v;
    }

    void ApplyRotation()
    {
        float t = Mathf.InverseLerp(-8f, 8f, rb.linearVelocity.y);
        float targetAngle = Mathf.Lerp(maxDownAngle, maxUpAngle, t);
        Quaternion target = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * rotateSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isAlive) return;

        if (showCollisionLog) Debug.Log("BIRD HIT: " + collision.gameObject.name);

        if (!dieOnAnything && !collision.gameObject.CompareTag(deadlyTag)) return;

        Die();
    }

    void Die()
    {
        isAlive = false;
        GameManager.instance.GameOver();
    }
}