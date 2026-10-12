using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image[] hearts;

    private void Update()
    {
        // update each heart based on player's curr health
        for (int i = 0; i < hearts.Length; i++)
        {
            // display hearts for remaining HP and hide lost hearts
            hearts[i].enabled = i < playerHealth.currentHealth;
        }
    }
}
