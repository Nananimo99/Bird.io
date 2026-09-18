using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [Header("Skin Models")]
    [Tooltip("ลากโมเดลสกินทั้งหมดมาใส่ตามลำดับ (0 = Burger, 1 = Donut, 2 = Fries)")]
    public GameObject[] skinModels;

    void Start()
    {
        UpdateSkin();
    }

    public void UpdateSkin()
    {
        // โหลดค่าสกินที่บันทึกไว้ (ถ้ายังไม่เคยเลือก จะใช้ค่าเริ่มต้นคือ 0)
        int selectedIndex = PlayerPrefs.GetInt("SelectedSkin", 0);

        for (int i = 0; i < skinModels.Length; i++)
        {
            if (skinModels[i] != null)
            {
                // เปิดเฉพาะสกินที่ตรงกับ index ที่เลือก และปิดสกินอื่น
                skinModels[i].SetActive(i == selectedIndex);
            }
        }
    }
}