using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetUpMaterial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material material;
    [SerializeField] private Shader shader;

    
    
    [ContextMenu("Set Shader")]
    public void SetShader()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.sharedMaterial.shader = shader;
        }
    }
    
    [ContextMenu("Set Material")]
    public void SetMaterial()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.sharedMaterial = material;
        }
    }
}