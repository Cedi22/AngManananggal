using UnityEngine;

public class MagicSarapPlayerItem : MonoBehaviour
{
    [Header("Visual Reference")]
    public GameObject magicSarapVisual;

    private bool hasItem;

    private void Start()
    {
        magicSarapVisual.SetActive(false);
    }

    public void EnableItem()
    {
        hasItem = true;
        magicSarapVisual.SetActive(true);
        Debug.Log("🍬 Magic Sarap collected!");
    }

    public bool HasItem()
    {
        return hasItem;
    }
}
