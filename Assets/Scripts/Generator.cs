
using UnityEngine;

public class Generator : MonoBehaviour
{
    public bool activated = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip activationSound;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // get generator's sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // allow activation with E only if generator is not already active
        if (!activated && Input.GetKeyDown(KeyCode.E))
        {
            // check whether player is within interaction range
            Collider2D player = Physics2D.OverlapCircle(
                transform.position, 1.5f
            );

            if (player != null && player.CompareTag("Player"))
            {
                ActivateGenerator();
            }
        }
    }

    private void ActivateGenerator()
    {
        // mark generator as activated and change color
        activated = true;
        spriteRenderer.color = Color.green;

        // notify manager to update generator progress
        GeneratorManager manager =
            FindFirstObjectByType<GeneratorManager>();

        if (manager != null)
        {
            manager.GeneratorActivated();
        }

        // play generator activation sound
        if (audioSource != null && activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
    }
}
