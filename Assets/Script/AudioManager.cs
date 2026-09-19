using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] bgm;

    [Header("SFX (เอาไว้ทำภายหลัง)")]
    [SerializeField] private AudioSource[] sfx;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    private const string MASTER_VOLUME = "MasterVolume";
    private const string VOLUME_KEY = "MasterVolumeValue";

    private void Awake()
    {
        // กัน AudioManager ซ้ำ
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCurrentVolumes();
    }

    // =========================
    // BGM
    // =========================
    public void PlayBGM(int index)
    {
        if (bgmSource == null)
        {
            Debug.LogError("ยังไม่ได้ใส่ BGM AudioSource ใน AudioManager");
            return;
        }

        if (bgm == null || bgm.Length == 0)
        {
            Debug.LogError("ยังไม่ได้ใส่เพลงใน BGM");
            return;
        }

        if (index < 0 || index >= bgm.Length)
        {
            Debug.LogError("BGM index ไม่ถูกต้อง : " + index);
            return;
        }

        // ถ้าเป็นเพลงเดิม ไม่ต้องเล่นซ้ำ
        if (bgmSource.clip == bgm[index] && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = bgm[index];
        bgmSource.loop = true;
        bgmSource.Play();

        Debug.Log("เล่น BGM เพลงที่ : " + index);
    }

    // =========================
    // SFX
    // =========================
    public void PlaySFX(int index)
    {
        if (sfx == null || sfx.Length == 0)
        {
            Debug.LogWarning("ยังไม่ได้ตั้งค่า SFX");
            return;
        }

        if (index < 0 || index >= sfx.Length)
        {
            Debug.LogError("SFX index ไม่ถูกต้อง : " + index);
            return;
        }

        if (sfx[index] != null)
        {
            sfx[index].Play();
        }
    }

    // =========================
    // VOLUME
    // =========================
    public void AdjustMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        // ถ้ามี Audio Mixer
        if (mixer != null)
        {
            float db;

            if (volume <= 0.0001f)
            {
                db = -80f;
            }
            else
            {
                db = Mathf.Log10(volume) * 20f;
            }

            bool success = mixer.SetFloat(MASTER_VOLUME, db);

            if (!success)
            {
                Debug.LogWarning("หา Exposed Parameter ชื่อ MasterVolume ไม่เจอ");
                AudioListener.volume = volume;
            }
        }
        else
        {
            // ถ้าไม่ได้ใช้ Mixer
            AudioListener.volume = volume;
        }

        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    // =========================
    // LOAD VOLUME
    // =========================
    public float LoadCurrentVolumes()
    {
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);

        if (mixer != null)
        {
            float db;

            if (volume <= 0.0001f)
            {
                db = -80f;
            }
            else
            {
                db = Mathf.Log10(volume) * 20f;
            }

            bool success = mixer.SetFloat(MASTER_VOLUME, db);

            if (!success)
            {
                AudioListener.volume = volume;
            }
        }
        else
        {
            AudioListener.volume = volume;
        }

        return volume;
    }
}