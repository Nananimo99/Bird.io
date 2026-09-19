using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] bgm;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfx;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    private const string MASTER_VOLUME = "MasterVolume";

    private const string MASTER_KEY = "MasterVolumeValue";
    private const string BGM_KEY = "BGMVolumeValue";
    private const string SFX_KEY = "SFXVolumeValue";

    // =========================
    // AWAKE
    // =========================
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // โหลดเสียงที่บันทึกไว้
        LoadCurrentVolumes();
        LoadBGMVolume();
        LoadSFXVolume();

        // ตรวจปุ่มทุกครั้งที่เปลี่ยน Scene
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ใส่เสียงให้ปุ่มใน Scene แรก
        AddButtonSounds();
    }

    // =========================
    // BGM
    // =========================
    public void PlayBGM(int index)
    {
        if (bgmSource == null)
        {
            Debug.LogError("ยังไม่ได้ใส่ BGM AudioSource");
            return;
        }

        if (bgm == null || bgm.Length == 0)
        {
            Debug.LogError("ยังไม่ได้ใส่เพลง BGM");
            return;
        }

        if (index < 0 || index >= bgm.Length)
        {
            Debug.LogError("BGM index ไม่ถูกต้อง : " + index);
            return;
        }

        if (bgm[index] == null)
        {
            Debug.LogError("BGM ช่อง " + index + " ยังไม่มีเพลง");
            return;
        }

        // ถ้าเป็นเพลงเดิมและกำลังเล่นอยู่ ไม่ต้องเริ่มใหม่
        if (bgmSource.clip == bgm[index] && bgmSource.isPlaying)
        {
            return;
        }

        bgmSource.clip = bgm[index];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    // =========================
    // SFX
    // =========================
    public void PlaySFX(int index)
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("ยังไม่ได้ใส่ SFX AudioSource");
            return;
        }

        if (sfx == null || sfx.Length == 0)
        {
            Debug.LogWarning("ยังไม่ได้ใส่เสียง SFX");
            return;
        }

        if (index < 0 || index >= sfx.Length)
        {
            Debug.LogWarning("SFX index ไม่ถูกต้อง : " + index);
            return;
        }

        if (sfx[index] == null)
        {
            Debug.LogWarning("SFX ช่อง " + index + " ยังไม่มีเสียง");
            return;
        }

        sfxSource.PlayOneShot(sfx[index]);
    }

    // =========================
    // MASTER VOLUME
    // =========================
    public void AdjustMasterVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (mixer != null)
        {
            float db = volume <= 0.0001f
                ? -80f
                : Mathf.Log10(volume) * 20f;

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

        PlayerPrefs.SetFloat(MASTER_KEY, volume);
        PlayerPrefs.Save();
    }

    public float LoadCurrentVolumes()
    {
        float volume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);

        if (mixer != null)
        {
            float db = volume <= 0.0001f
                ? -80f
                : Mathf.Log10(volume) * 20f;

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

    // =========================
    // BGM VOLUME
    // =========================
    public void AdjustBGMVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }

        PlayerPrefs.SetFloat(BGM_KEY, volume);
        PlayerPrefs.Save();
    }

    public float LoadBGMVolume()
    {
        float volume = PlayerPrefs.GetFloat(BGM_KEY, 1f);

        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }

        return volume;
    }

    // =========================
    // SFX VOLUME
    // =========================
    public void AdjustSFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }

        PlayerPrefs.SetFloat(SFX_KEY, volume);
        PlayerPrefs.Save();
    }

    public float LoadSFXVolume()
    {
        float volume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }

        return volume;
    }

    // =========================
    // BUTTON SOUND
    // =========================
    private void AddButtonSounds()
    {
        Button[] buttons = FindObjectsByType<Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (Button button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonSound);
            button.onClick.AddListener(PlayButtonSound);
        }
    }

    // =========================
    // SCENE LOADED
    // =========================
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddButtonSounds();
    }

    // =========================
    // BUTTON CLICK SOUND
    // =========================
    private void PlayButtonSound()
    {
        PlaySFX(0);
    }

    // =========================
    // ON DESTROY
    // =========================
    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}