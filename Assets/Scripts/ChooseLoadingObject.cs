using System;
using System.Collections;
using System.Collections.Generic;
using AR;
using UnityEngine;

public class ChooseLoadingObject : MonoBehaviour
{
    public GameObject[] loadingObjects;

    public TrackableObjectBehavior[] behaviors;
    
    
    private void Start()
    {
        
    }



    public void SelectObject(int id)
    {
        for (int i = 0; i < loadingObjects.Length; i++)
        {
            if (i != id)
                loadingObjects[i].SetActive(false);
        }
        
        gameObject.SetActive(false);
    }

    [ContextMenu("Activate")]
    public void ActivateAllBehaviors()
    {
        foreach (var behavior in behaviors)
        {
            behavior.EnableBehaviour();
        }
    }
}