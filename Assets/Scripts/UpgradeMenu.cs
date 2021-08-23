using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text speedText;
    private PlayerStats stats;

    void OnEnable () {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues () {
        healthText.text = stats.maxHealth.ToString();
        speedText.text = stats.movementSpeed.ToString();
    }
}
