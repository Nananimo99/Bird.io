using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType
    {
        BGM,
        SFX
    }

    [SerializeField] private Slider slider;
    [SerializeField] private VolumeType volumeType;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (AudioManager.instance == null)
        {
            return;
        }

        float volume;

        if (volumeType == VolumeType.BGM)
        {
            volume = AudioManager.instance.LoadBGMVolume();
        }
        else
        {
            volume = AudioManager.instance.LoadSFXVolume();
        }

        slider.SetValueWithoutNotify(volume);
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        if (AudioManager.instance == null)
        {
            return;
        }

        if (volumeType == VolumeType.BGM)
        {
            AudioManager.instance.AdjustBGMVolume(value);
        }
        else
        {
            AudioManager.instance.AdjustSFXVolume(value);
        }
    }

    private void OnDestroy()
    {
        if (slider != null)
        {
            slider.onValueChanged.RemoveListener(ChangeVolume);
        }
    }
}