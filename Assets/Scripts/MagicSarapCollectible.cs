using UnityEngine;

public class MagicSarapCollectible : MonoBehaviour
{
    [Header("Glow Settings")]
    public Renderer itemRenderer;
    public Color glowColor = new Color(0.2f, 0.15f, 0.05f);
    public float glowDistance = 5f;

    [Header("Audio")]
    public AudioSource humAudio;

    private Material itemMaterial;
    private Transform player;
    private bool isGlowing;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        itemMaterial = itemRenderer.material;

        DisableGlow();
        humAudio.Stop();
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= glowDistance)
        {
            EnableGlow();
        }
        else
        {
            DisableGlow();
        }
    }

    private void EnableGlow()
    {
        if (isGlowing) return;

        itemMaterial.EnableKeyword("_EMISSION");
        itemMaterial.SetColor("_EmissionColor", glowColor);
        humAudio.Play();

        isGlowing = true;
    }

    private void DisableGlow()
    {
        if (!isGlowing) return;

        itemMaterial.SetColor("_EmissionColor", Color.black);
        humAudio.Stop();

        isGlowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        MagicSarapPlayerItem item =
            other.GetComponent<MagicSarapPlayerItem>();

        if (item != null)
        {
            item.EnableItem();
        }

        humAudio.Stop();
        Destroy(gameObject);
    }
}
