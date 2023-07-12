using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancing : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private string shaderProperty;
    [SerializeField] private Color color;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = Instantiate(meshRenderer.material);
            meshRenderer.material.SetColor(shaderProperty, color);
        }
        else
        {
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.material = Instantiate(skinnedMeshRenderer.material);
            skinnedMeshRenderer.material.SetColor(shaderProperty, color);
        }
    }
}
