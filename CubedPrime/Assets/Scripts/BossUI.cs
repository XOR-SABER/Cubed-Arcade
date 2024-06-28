using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public GameObject BossHealthBarObj;
    public TextMeshProUGUI text; 
    public Image BossHealthBar;

    // Method to enable the UI elements
    public void EnableUI()
    {
        BossHealthBarObj.SetActive(true);
    }

    // Method to disable the UI elements
    public void DisableUI()
    {
        BossHealthBarObj.SetActive(false);
    }
}
