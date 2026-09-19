using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // กด Easy Mode
    public void EasyMode()
    {
        
        SceneManager.LoadScene("EasyMode");
    }

    // กด Hard Mode
    public void HardMode()
    {
        SceneManager.LoadScene("HardMode");
    }

    // ออกจากเกม
    public void ExitGame()
    {
        Application.Quit();
    }
}