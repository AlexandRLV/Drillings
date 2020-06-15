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
        Debug.Log("Compass: Start");
        if (isFollowing) 
            return;
    
        target = targetObject;
        isFollowing = true;
    }
    
    [ContextMenu("Stop Follow")]
    public void StopFollow()
    {
        Debug.Log("Compass: Stop");
        if (!appManager.DeactivateCurrentLayout())
            return;
        
        isFollowing = false;
        angle = -1;
        distance = -1;
        target = null;
    }

    public bool SuitableAngleAndDistance(Transform target)
    {
        Vector3 direction = target.position - arCamera.position;
        float angle = Vector3.Angle(arCamera.forward, direction);
        float distance = direction.magnitude;

        return angle < maxAngle && distance < maxDistance;
    }
    
    private void CalculateAngleAndDistance()
    {
        Vector3 direction = target.position - arCamera.position;
        angle = Vector3.Angle(arCamera.forward, direction);
        distance = direction.magnitude;
    }
}