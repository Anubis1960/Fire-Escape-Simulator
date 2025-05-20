using UnityEngine;
using UnityEditor;

public class CleanEmptyMaterials : EditorWindow
{
    [MenuItem("Tools/Clean Empty Materials")]
    static void Apply()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Renderer renderer = go.GetComponent<Renderer>();

            if (renderer == null)
            {
                Debug.LogWarning($"'{go.name}' has no Renderer component.");
                continue;
            }

            Material[] materials = renderer.sharedMaterials;
            int originalCount = materials.Length;

            // Filter out null or "None" materials
            Material[] cleaned = System.Array.FindAll(materials, mat => mat != null);

            if (cleaned.Length < originalCount)
            {
                Undo.RecordObject(renderer, "Removed Empty Materials");
                renderer.materials = cleaned;
                Debug.Log($"Removed {originalCount - cleaned.Length} empty materials from '{go.name}'.");
            }
        }

        Debug.Log("✅ Done cleaning empty materials!");
    }
}