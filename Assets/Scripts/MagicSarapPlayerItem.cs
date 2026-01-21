using UnityEngine;

public class MagicSarapPlayerItem : MonoBehaviour
{
    [Header("Visual")]
    public GameObject magicSarapVisual;

    private bool hasItem;

    private void Start()
    {
        if (magicSarapVisual != null)
            magicSarapVisual.SetActive(false);
    }

    public void EnableItem()
    {
        hasItem = true;

        if (magicSarapVisual != null)
            magicSarapVisual.SetActive(true);

        Debug.Log("🍬 Magic Sarap collected");
    }

    public void ConsumeItem()
    {
        hasItem = false;

        if (magicSarapVisual != null)
            magicSarapVisual.SetActive(false);

        Debug.Log("🍬 Magic Sarap consumed");
    }

    public bool HasItem()
    {
        return hasItem;
    }
}
