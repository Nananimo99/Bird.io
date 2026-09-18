using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    // บันทึกค่า Index ของสกินลงเครื่อง
    public void SelectSkin(int skinIndex)
    {
        PlayerPrefs.SetInt("SelectedSkin", skinIndex);
        PlayerPrefs.Save();
        Debug.Log("เลือกสกินสำเร็จ: " + skinIndex);
    }
}