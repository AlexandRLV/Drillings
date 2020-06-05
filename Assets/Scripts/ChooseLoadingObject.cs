using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLoadingObject : MonoBehaviour
{
    public GameObject[] loadingObjects;

    
    
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
}