using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;

    private bool isOpen = false;

    public void ToggleButtons()
    {
        isOpen = !isOpen;

        button1.SetActive(isOpen);
        button2.SetActive(isOpen);
    }
}

