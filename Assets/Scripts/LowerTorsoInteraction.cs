using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LowerTorsoInteraction : MonoBehaviour
{
    [Header("References")]
    public TMP_Text interactText;
    public LowerBodyBuffController buffController;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sprinkleSound;

    private MagicSarapPlayerItem playerItem;
    private bool playerInRange;
    private bool hasBeenUsed;

    private void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        hasBeenUsed = false;
    }

    private void Update()
    {
        if (!playerInRange || hasBeenUsed) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame &&
            playerItem != null &&
            playerItem.HasItem())
        {
            SprinkleBody();
        }
        else
        {
            UpdateInteractText();
        }
    }

    private void SprinkleBody()
    {
        hasBeenUsed = true;

        if (interactText != null)
            interactText.gameObject.SetActive(false);

        // 🔊 Play sprinkle sound
        if (audioSource != null && sprinkleSound != null)
            audioSource.PlayOneShot(sprinkleSound);

        // 🍬 Consume Magic Sarap
        if (playerItem != null)
            playerItem.ConsumeItem();

        // ☠ Start curse
        if (buffController != null)
            buffController.StartCurse();

        Debug.Log("🩸 Body sprinkled — curse started");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerItem = other.GetComponentInChildren<MagicSarapPlayerItem>();
        playerInRange = true;

        UpdateInteractText();
        Debug.Log("✅ Interaction available");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerItem = null;

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void UpdateInteractText()
    {
        if (interactText == null || hasBeenUsed) return;

        if (playerItem != null && playerItem.HasItem())
        {
            interactText.text = "[E] Sprinkle the body";
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void EnableLowerTorso()
    {
        Debug.Log("🧩 Lower torso enabled");
        gameObject.SetActive(true);

        Collider col = GetComponent<Collider>();
        if (col == null) return;

        Collider[] hits = Physics.OverlapBox(
            col.bounds.center,
            col.bounds.extents,
            Quaternion.identity
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                OnTriggerEnter(hit);
                break;
            }
        }
    }
}
