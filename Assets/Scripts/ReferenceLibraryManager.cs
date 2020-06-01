using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ReferenceLibraryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ARTrackedObjectManager objectManager;
    [SerializeField] private Text loadedLibText;
    
    [Header("Libraries")]
    [SerializeField] private XRReferenceObjectLibrary[] libraries;


    private void Start()
    {
        if (libraries == null || libraries.Length == 0)
            return;
        
        objectManager.referenceLibrary = libraries[0];
        loadedLibText.text = "Loaded library: 1";
    }

    

    public void SetLibrary(int id)
    {
        if (libraries == null || libraries.Length == 0)
            return;
        
        if (id < 0 || id >= libraries.Length)
            return;

        objectManager.referenceLibrary = libraries[id];
        loadedLibText.text = $"Loaded library: {id + 1}";
    }
}
