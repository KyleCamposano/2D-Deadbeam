
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight")]
    public Light2D flashlight;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainRate = 10f;

    [Header("Low Battery Flicker")]
    public float lowBatteryThreshold = 20f;
    public float flickerSpeed = 0.1f;
    public float minimumIntensity = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip flashlightClick;

    private Camera mainCamera;
    private float originalIntensity;
    private float flickerTimer;

    private void Start()
    {
        // get main camera for mouse flashlight aiming
        mainCamera = Camera.main;

        // store flashlight's orig brightness
        if (flashlight != null)
            originalIntensity = flashlight.intensity;

        // ensure batt starts within valid range
        currentBattery = Mathf.Clamp(
            currentBattery, 0f, maxBattery
        );
    }

    private void Update()
    {
        // continuously aim flashlight toward mouse cursor
        AimFlashlight();

        if (flashlight == null)
            return;

        // toggle flashlight with F or left click
        if (Input.GetKeyDown(KeyCode.F) ||
            Input.GetMouseButtonDown(0))
        {
            if (currentBattery > 0f)
            {
                flashlight.enabled = !flashlight.enabled;

                // play flashlight switch sound
                if (audioSource != null && flashlightClick != null)
                    audioSource.PlayOneShot(flashlightClick);
            }
        }

        // drain batt while flashlight is on
        if (flashlight.enabled)
        {
            currentBattery -= drainRate * Time.deltaTime;
            currentBattery = Mathf.Max(currentBattery, 0f);

            // Automatically turn off when the battery is depleted.
            if (currentBattery <= 0f)
            {
                flashlight.enabled = false;
            }
        }

        // randomly flicker flashlight when batt is low
        if (flashlight.enabled &&
            currentBattery <= lowBatteryThreshold)
        {
            flickerTimer -= Time.deltaTime;

            if (flickerTimer <= 0f)
            {
                flashlight.intensity = Random.Range(
                    minimumIntensity,
                    originalIntensity
                );

                flickerTimer = flickerSpeed;
            }
        }
        else
        {
            // restore normal brightness when flickering not needed
            flashlight.intensity = originalIntensity;
            flickerTimer = 0f;
        }
    }

    private void AimFlashlight()
    {
        if (mainCamera == null || flashlight == null)
            return;

        // convert mouse pos from screen to game world
        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // calculate direction from player to cursor
        Vector2 direction =
            mousePosition - transform.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        // rotate flashlight toward mouse cursor
        flashlight.transform.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void AddBattery(float amount)
    {
        // recharge flashlight w/o exceeding max capacity
        currentBattery = Mathf.Clamp(
            currentBattery + amount,
            0f,
            maxBattery
        );
    }
}
