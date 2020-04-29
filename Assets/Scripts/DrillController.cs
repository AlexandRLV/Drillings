using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillController : MonoBehaviour
{
    [SerializeField] private float drillRotationSpeed;
    [SerializeField] private Transform arrowsParent;

    private Vector3 arrowsFlatPos;
    private Vector3 camFlatPos;
    private Transform camTransform;


    private void OnEnable()
    {
        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        transform.Rotate(0, drillRotationSpeed * Time.deltaTime, 0);
        
        if (arrowsParent == null)
            return;
        
        arrowsFlatPos = new Vector3(arrowsParent.position.x, 0, arrowsParent.position.z);
        camFlatPos = new Vector3(camTransform.position.x, 0, camTransform.position.z);
        
        arrowsParent.rotation = Quaternion.LookRotation(arrowsFlatPos - camFlatPos, Vector3.up);
    }
}
