using UnityEngine;
using System.Collections.Generic;

public class CameraCulling : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Culling Settings")]
    public float visionRadius = 3f;                // Radius of the vision cylinder
    public LayerMask obstructionMask;                // Layers that can obstruct view
    public Material fadeMaterial;                    // Transparent override material

    private Dictionary<Renderer, Material[]> originalMaterials = new();
    private List<Renderer> hiddenRenderers = new();

    void LateUpdate()
    {
        if (!player) return;

        HandleObstructions();
    }

    void HandleObstructions()
    {
        // Restore previously hidden objects
        foreach (var rend in hiddenRenderers)
        {
            if (originalMaterials.ContainsKey(rend))
                rend.materials = originalMaterials[rend];
        }
        hiddenRenderers.Clear();

        Vector3 start = transform.position;
        Vector3 end = player.position;


        Collider[] hits = Physics.OverlapCapsule(start, end, visionRadius, obstructionMask);

        foreach (var col in hits)
        {
            Renderer rend = col.GetComponent<Renderer>();
            if (rend == null || hiddenRenderers.Contains(rend)) continue;

           // Debug.Log($"Fading: {rend.gameObject.name}");

            // Save original materials for unfading
            if (!originalMaterials.ContainsKey(rend))
                originalMaterials[rend] = rend.materials;

            // Replace all materials with fade material
            Material[] fadeMats = new Material[rend.materials.Length];
            for (int i = 0; i < fadeMats.Length; i++)
                fadeMats[i] = fadeMaterial;

            rend.materials = fadeMats;
            hiddenRenderers.Add(rend);
        }
    }

    // Visualize the capsule in the Scene view
    void OnDrawGizmosSelected()
    {
        if (!player) return;

        Gizmos.color = new Color(0f, 1f, 1f, 0.3f);

        Vector3 start = transform.position;
        Vector3 end = player.position;
        DrawCapsuleGizmo(start, end, visionRadius);
    }

    void DrawCapsuleGizmo(Vector3 start, Vector3 end, float radius)
    {
        Vector3 up = (end - start).normalized * radius;
        Quaternion rot = Quaternion.LookRotation(end - start);
        Matrix4x4 angleMatrix = Matrix4x4.TRS(start, rot, Vector3.one);
        Gizmos.matrix = angleMatrix;

        float height = Vector3.Distance(start, end);
        float pointOffset = (height - radius * 2) * 0.5f;

        Gizmos.DrawWireSphere(Vector3.forward * pointOffset, radius);
        Gizmos.DrawWireSphere(Vector3.forward * -pointOffset, radius);
    }
}