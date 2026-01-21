using UnityEngine;

public class MagicSarapSpawner : MonoBehaviour
{
    [Header("Collectible")]
    public Transform collectible;

    [Header("Map Bounds")]
    public float minX = -147.15f;
    public float maxX = 138.3f;
    public float minZ = -184f;
    public float maxZ = 104.2f;

    [Header("Raycast Settings")]
    public LayerMask groundLayer;
    public LayerMask obstacleLayer;
    public float rayHeight = 50f;
    public float spawnHeightOffset = 0.1f;
    public float obstacleCheckRadius = 0.5f;

    [Header("Attempts")]
    public int maxAttempts = 50;

    private void Start()
    {
        SpawnSafely();
    }

    void SpawnSafely()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(minX, maxX),
                rayHeight,
                Random.Range(minZ, maxZ)
            );

            // Raycast DOWN to find ground
            if (Physics.Raycast(randomPos, Vector3.down, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 spawnPos = hit.point + Vector3.up * spawnHeightOffset;

                // Check if inside obstacle
                bool blocked = Physics.CheckSphere(
                    spawnPos,
                    obstacleCheckRadius,
                    obstacleLayer
                );

                if (!blocked)
                {
                    collectible.position = spawnPos;
                    Debug.Log("✅ MagicSarap spawned safely");
                    return;
                }
            }
        }

        Debug.LogWarning("❌ Failed to find safe spawn location");
    }
    
}
    