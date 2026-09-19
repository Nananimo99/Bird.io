using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        if (AudioManager.instance != null)
        {
            float volume = AudioManager.instance.LoadCurrentVolumes();

            slider.SetValueWithoutNotify(volume);

            slider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    private void ChangeVolume(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.AdjustMasterVolume(value);
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
