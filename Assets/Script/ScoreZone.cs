using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    [Header("Backup")]
    public bool alsoCountByPosition = true;

    [Header("Debug")]
    public bool showLog = true;

    private bool scored;
    private Transform bird;

    void Awake()
    {
        Collider col = GetComponent<Collider>();

        if (col == null)
        {
            Debug.LogWarning("ScoreZone: no Collider on " + gameObject.name);
            return;
        }

        col.isTrigger = true;
    }

    void Start()
    {
        Bird b = FindFirstObjectByType<Bird>();
        if (b != null) bird = b.transform;
        else Debug.LogWarning("ScoreZone: Bird not found in scene.");
    }

    void Update()
    {
        if (scored) return;
        if (!alsoCountByPosition) return;
        if (bird == null) return;
        if (GameManager.instance == null) return;
        if (!GameManager.instance.isPlaying) return;

        if (bird.position.x > transform.position.x) Score("position");
    }

    void OnTriggerEnter(Collider other)
    {
        if (scored) return;
        if (other.GetComponentInParent<Bird>() == null) return;

        Score("trigger");
    }

    void Score(string source)
    {
        scored = true;

        if (GameManager.instance != null) GameManager.instance.AddScore();
        if (showLog) Debug.Log("SCORE by " + source + ": " + gameObject.name);
    }
}
