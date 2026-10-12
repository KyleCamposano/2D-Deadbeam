
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    // amount of flashlight batt. restored when collected
    public float batteryAmount = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only allow player to collect batt.
        if (!other.CompareTag("Player"))
            return;

        // get player's flashlight controller
        FlashlightController flashlight =
            other.GetComponent<FlashlightController>();

        // recharge flashlight and remove collected batt.
        if (flashlight != null)
        {
            flashlight.AddBattery(batteryAmount);
            Destroy(gameObject);
        }
    }
}
