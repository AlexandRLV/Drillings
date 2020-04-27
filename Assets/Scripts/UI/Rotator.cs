using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationAngle;
    [SerializeField, Range(0.01f, 0.2f)] private float rotationDelay;

    private Coroutine currentRoutine;
    
    private void OnEnable()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(WaitAndRotate());
    }

    private IEnumerator WaitAndRotate()
    {
        while (true)
        {
            yield return new WaitForSeconds(rotationDelay);
            transform.Rotate(0,0,rotationAngle);            
        }
    }
}