using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    private Transform target;
    private float angle;
    private float distance;
    private bool isFollowing;
    
    [Header("Settings")]
    [SerializeField] private float maxAngle;
    [SerializeField] private float maxDistance;

    [Header("References")] 
    [SerializeField] private Transform arCamera;
    [SerializeField] private AppManager appManager;

    
    private void Update()
    {
        if (!isFollowing)
            return;

        CalculateAngleAndDistance();

        if (angle > maxAngle || distance > maxDistance)
            StopFollow();
    }
    

    public void StartFollow(Transform targetObject)
    { 
        if (isFollowing) 
            return;
    
        target = targetObject;
        isFollowing = true;
    }
    
    
    public void StopFollow()
    {
        if (!appManager.DeactivateCurrentLayout())
            return;
        
        isFollowing = false;
        angle = -1;
        distance = -1;
        target = null;
    }
    
    private void CalculateAngleAndDistance()
    {
        Vector3 direction = target.position - arCamera.position;
        angle = Vector3.Angle(arCamera.forward, direction);
        distance = direction.magnitude;
    }
}