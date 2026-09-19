using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private int bgmIndex = 0;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBGM(bgmIndex);
        }
    }
}