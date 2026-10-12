
using UnityEngine;

public class BatteryUI : MonoBehaviour
{
    public FlashlightController flashlight;
    public RectTransform batteryFill;

    private float fullWidth;

    private void Start()
    {
        // store batt bar's original width
        fullWidth = batteryFill.sizeDelta.x;
    }

    private void Update()
    {
        // ensure flashlight and batt UI references exist
        if (flashlight == null || batteryFill == null)
            return;

        // calculate remaining batt percentage
        float batteryPercent =
            flashlight.currentBattery / flashlight.maxBattery;

        // resize batt bar to reflect remaining charge
        batteryFill.sizeDelta = new Vector2(
            fullWidth * batteryPercent,
            batteryFill.sizeDelta.y
        );
    }
}
