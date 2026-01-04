using UnityEngine;
using UnityEngine.InputSystem;

public class LowerTorsoInteraction : MonoBehaviour
{
    [Header("References")]
    public MagicSarapPlayerItem playerItem;
    public Canvas interactionCanvas;
    public LowerBodyBuffController buffController;

    private bool playerInRange;
    private bool hasBeenUsed;

    private void Start()
    {
        interactionCanvas.gameObject.SetActive(false);
        gameObject.SetActive(false); // spawn later
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (hasBeenUsed) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SprinkleBody();
        }
    }

    private void SprinkleBody()
    {
        hasBeenUsed = true;
        interactionCanvas.gameObject.SetActive(false);

        buffController.StartCurse(); // start countdown + aggression

        Debug.Log("🩸 Body sprinkled — curse started");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!playerItem.HasItem()) return;
        if (hasBeenUsed) return;

        playerInRange = true;
        interactionCanvas.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        interactionCanvas.gameObject.SetActive(false);
    }

    // Called externally when item is picked up
    public void EnableLowerTorso()
    {
        gameObject.SetActive(true);
    }
}
