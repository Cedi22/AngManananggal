using UnityEngine;

public class MagicSarapSpawner : MonoBehaviour
{
    [Header("Existing Collectible")]
    public Transform collectible; // DRAG the MagicSarap object here

    [Header("Map Bounds")]
    public float minX = -147.15f;
    public float maxX = 138.3f;
    public float minZ = -184f;
    public float maxZ = 104.2f;

    [Header("Tree Settings")]
    public LayerMask treeLayer;
    public float spawnOffsetFromTree = 2f;
    public float treeSearchRadius = 200f;

    private void Start()
    {
        MoveNearTree();
    }

    void MoveNearTree()
    {
        Collider[] trees = Physics.OverlapSphere(Vector3.zero, treeSearchRadius, treeLayer);

        if (trees.Length == 0)
        {
            Debug.LogWarning("No trees found!");
            return;
        }

        Collider chosenTree = trees[Random.Range(0, trees.Length)];

        Vector3 direction = Random.insideUnitSphere;
        direction.y = 0f;
        direction.Normalize();

        Vector3 pos = chosenTree.transform.position + direction * spawnOffsetFromTree;

        // Clamp to map bounds
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = 0f;

        collectible.position = pos;
    }
}
