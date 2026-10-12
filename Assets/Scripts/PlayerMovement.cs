
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Camera mainCamera;

    private void Awake()
    {
        // get player's Rigidbody2D and main camera
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // read horz and vert movement input from WASD or arrow keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // normalize movement to prevent faster diagonal movement
        movement = movement.normalized;
    }

    private void FixedUpdate()      
    {
        // calc player's next pos based on movement speed
        Vector2 newPosition =
            rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // get visible boundaries of main camera
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, 0)
        );

        Vector3 topRight = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 1, 0)
        );

        // restrict horz movement to camera boundaries
        newPosition.x = Mathf.Clamp(
            newPosition.x,
            bottomLeft.x,
            topRight.x
        );

        // restrict vert movement to camera boundaries
        newPosition.y = Mathf.Clamp(
            newPosition.y,
            bottomLeft.y,
            topRight.y
        );

        // move player using Rigidbody2D physics
        rb.MovePosition(newPosition);
    }
}
