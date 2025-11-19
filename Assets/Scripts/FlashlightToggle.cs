using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlightLight;      // Reference to the flashlight light
    public AudioSource toggleSound;    // Sound effect when toggling
    private bool isOn = true;          // Start with light on or off

    void Start()
    {
        // Auto-find Light and AudioSource if not assigned
        if (flashlightLight == null)
            flashlightLight = GetComponentInChildren<Light>();

        if (toggleSound == null)
            toggleSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;                          // Toggle state
            flashlightLight.enabled = isOn;        // Turn the light on/off

            if (toggleSound != null)
                toggleSound.Play();                // Play toggle sound
        }
    }
}
