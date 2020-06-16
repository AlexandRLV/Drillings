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

    private float sqrMaxDistance;

    private void Awake()
    {
        sqrMaxDistance = maxDistance * maxDistance;
    }

    private void Update()
    {
        if (!isFollowing)
            return;

        CalculateAngleAndDistance();

        if (angle > maxAngle || distance > sqrMaxDistance)
            StopFollow();
    }
    

    public void StartFollow(Transform targetObject)
    {
        //Debug.Log("Compass: Start");
        if (isFollowing)
        {
            //Debug.Log("Compass: Already started");
            return;
        }
    
        target = targetObject;
        isFollowing = true;
    }
    
    [ContextMenu("Stop Follow")]
    public void StopFollow()
    {
        //Debug.Log("");
        //Debug.Log("Disabling an object:");
        //Debug.Log("Compass: Stop");
        if (!appManager.DeactivateCurrentLayout())
        {
            Debug.Log("Compass: Deactivation not completed");
            return;
        }
        
        isFollowing = false;
        angle = -1;
        distance = -1;
        target = null;
        //Debug.Log("Disabling finished");
    }
    
    private void CalculateAngleAndDistance()
    {
        Vector3 direction = target.position - arCamera.position;
        angle = Vector3.Angle(arCamera.forward, direction);
        distance = direction.sqrMagnitude;
    }
}