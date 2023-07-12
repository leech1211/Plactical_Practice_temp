using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancingGroup : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<MeshRenderer> subMesh;
    [SerializeField] private string shaderProperty;
    [SerializeField] private Color color;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = Instantiate(meshRenderer.material);
            meshRenderer.material.SetColor(shaderProperty, color);
            foreach (var mesh in subMesh)
            {
                mesh.material = meshRenderer.material;
            }
        }
    }
}
