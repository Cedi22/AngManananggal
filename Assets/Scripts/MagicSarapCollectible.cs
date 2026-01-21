using UnityEngine;

public class MagicSarapCollectible : MonoBehaviour
{
    public LowerTorsoInteraction lowerTorso;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MagicSarapPlayerItem item =
            other.GetComponentInChildren<MagicSarapPlayerItem>();

        if (item == null)
        {
            Debug.LogError("❌ MagicSarapPlayerItem NOT found in player");
            return;
        }

        item.EnableItem();

        lowerTorso.EnableLowerTorso(); // 🔥 THIS IS THE LINE YOU ASKED ABOUT

        Destroy(gameObject);
    }
}
