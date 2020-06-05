using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLoadingObject : MonoBehaviour
{
    public GameObject[] loadingObjects;

    
    
    private void Start()
    {
        foreach (GameObject obj in loadingObjects)
        {
            obj.SetActive(false);
        }
    }



    public void SelectObject(int id)
    {
        loadingObjects[id].SetActive(true);
    }
}