using System;
using System.Collections;
using System.Collections.Generic;
using AssetVariables;
using UnityEngine;

public class WireframeMaterialController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ColorVariable wireColor;
    [SerializeField] private ColorVariable baseColor;

    [Header("References")]
    [SerializeField] private Material miniatureMaterial;


    private void OnEnable()
    {
        Debug.Log("Setting miniature colors");
        miniatureMaterial.SetColor("_WireColor", wireColor);
        miniatureMaterial.SetColor("_BaseColor", baseColor);
    }
}