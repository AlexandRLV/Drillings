using System;
using System.Collections;
using System.Collections.Generic;
using AR;
using UnityEngine;

public class ObjectToCameraAngleController : MonoBehaviour
{
    public float activateAngle;
    public float activateSqrDist;
    public TrackableObjectBehavior[] behaviors;

    private Transform arCamera;

    private void Awake()
    {
        arCamera = Camera.main.transform;
    }

    private void Update()
    {
        float minDistance = float.MaxValue;
        int minId = -1;

        for (int i = 0; i < behaviors.Length; i++)
        {
            var behavior = behaviors[i];
            if (behavior.objectTransform != null)
            {
                Vector3 direction = behavior.objectTransform.position - arCamera.position;
                float angle = Vector3.Angle(arCamera.forward, direction);
                float dist = direction.sqrMagnitude;

                if (angle < activateAngle && dist < activateSqrDist)
                {
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        minId = i;
                    }
                }
            }
        }

        if (minId != -1)
        {
            behaviors[minId].EnableBehaviour();
        }
    }
}
