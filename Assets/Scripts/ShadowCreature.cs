
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowCreature : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform player;

    [Header("Flashlight")]
    public Light2D flashlight;

    [Header("Detection")]
    public float detectionRange = 8f;
    public LayerMask wallLayer;

    [Header("Damage")]
    public int damage = 1;
    public float damageCooldown = 1.5f;
    public float knockbackForce = 3f;

    private Rigidbody2D rb;
    private bool frozen;
    private bool canSeePlayer;
    private float nextDamageTime;

    private void Awake()
    {
        // get creature's Rigidbody2D for movement and physics
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // check whether creature is illuminated by flashlight
        frozen = IsInFlashlight();

        // check whether player is visible and within detection range
        canSeePlayer = CanSeePlayer();
    }

    private void FixedUpdate()
    {
        // stop movement if player is missing, creature is
        // frozen, or the player is outside its line of sight.
        if (player == null || frozen || !canSeePlayer)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // move creature toward player's curr pos
        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        // calculate dir and distance to player
        Vector2 direction =
            (Vector2)player.position - rb.position;

        float distance = direction.magnitude;

        // stop detecting player beyond detection range
        if (distance > detectionRange)
            return false;

        // check whether wall blocks creature's line of sight
        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            direction.normalized,
            distance,
            wallLayer
        );

        // player is visible if no wall blocks raycast
        return hit.collider == null;
    }

    private bool IsInFlashlight()
    {
        // creature cannot freeze if flashlight is off
        if (flashlight == null || !flashlight.enabled)
            return false;

        Vector2 origin = flashlight.transform.position;

        // calc dir and distance from flashlight
        Vector2 toEnemy =
            (Vector2)transform.position - origin;

        float distance = toEnemy.magnitude;

        // check whether creature is within flashlight range
        if (distance > flashlight.pointLightOuterRadius)
            return false;

        Vector2 flashlightDirection =
            flashlight.transform.up;

        // calc angle between beam and creature
        float angle = Vector2.Angle(
            flashlightDirection,
            toEnemy
        );

        // The creature must be inside the flashlight's cone.
        if (angle > flashlight.pointLightOuterAngle / 2f)
            return false;

        // Check whether a wall blocks the flashlight beam.
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            toEnemy.normalized,
            distance,
            wallLayer
        );

        // freeze creature only if beam is unobstructed
        return hit.collider == null;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // only apply dmg when colliding with player
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // prevent repeated damage during cooldown period
        if (Time.time < nextDamageTime)
            return;

        PlayerHealth health =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (health != null)
        {
            // reduce player's HP
            health.TakeDamage(damage);

            // set next time creature can deal dmg
            nextDamageTime =
                Time.time + damageCooldown;

            // calc dir away from player
            Vector2 pushDirection =
                (transform.position -
                 collision.transform.position).normalized;

            // push creature backward after attacking
            rb.AddForce(
                pushDirection * knockbackForce,
                ForceMode2D.Impulse
            );
        }
    }
}
