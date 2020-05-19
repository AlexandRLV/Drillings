using System;
using System.Collections;
using System.Collections.Generic;
using AssetVariables;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SelectionBlink : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private FloatVariable blinkTime;
    [SerializeField] private FloatVariable blinkDelay;

    [Header("References")]
    [SerializeField] private MeshRenderer selectionRenderer;
    
    private float timer;
    private bool isBlinking;

    

    private void OnEnable()
    {
        timer = 0;
        isBlinking = false;
        selectionRenderer.enabled = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isBlinking)
        {
            if (timer < blinkTime)
                return;
            
            timer = 0;
            isBlinking = false;
            selectionRenderer.enabled = true;
        }
        else
        {
            if (timer < blinkDelay)
                return;
            
            timer = 0;
            isBlinking = true;
            selectionRenderer.enabled = false;
        }
    }
}
