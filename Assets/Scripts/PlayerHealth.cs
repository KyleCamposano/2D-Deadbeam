
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Damage Audio")]
    public AudioSource damageAudio;
    public AudioClip damageSound;

    private bool isDead = false;

    private void Start()
    {
        // init player's health to max val
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // prevent further damage after player dies
        if (isDead)
            return;

        // reduce health w/o allowing it to drop below zero
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // play player dmg sound effect
        if (damageAudio != null && damageSound != null)
        {
            damageAudio.PlayOneShot(damageSound);
        }

        // log player's remaining health (for debugging)
        Debug.Log("Player HP: " + currentHealth);

        // trigger death when player runs out of health
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // mark player as dead to prevent repeated death events
        isDead = true;

        // find GameManager to trigger game over screen
        GameManager manager =
            FindFirstObjectByType<GameManager>();

        if (manager != null)
        {
            manager.GameOver();
        }
    }
}
