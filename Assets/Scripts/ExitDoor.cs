
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool unlocked = false;

    private void Start()
    {
        // initialize exit door in locked state
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
    }

    private void Update()
    {
        // check whether all 3 generators have been activated
        GeneratorManager manager =
            FindFirstObjectByType<GeneratorManager>();

        if (manager != null && manager.generatorsActivated >= 3)
        {
            UnlockExit();
        }
    }

    private void UnlockExit()
    {
        // prevent exit from being unlocked multiple times
        if (unlocked)
            return;

        // unlock exit and change color to green
        unlocked = true;
        spriteRenderer.color = Color.green;

        Debug.Log("Exit unlocked!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // prevent player from escaping before unlocking exit
        if (!unlocked)
            return;

        // trigger victory when player enters unlocked exit
        if (other.CompareTag("Player"))
        {
            GameManager manager =
                FindFirstObjectByType<GameManager>();

            if (manager != null)
            {
                manager.Victory();
            }
        }
    }
}
