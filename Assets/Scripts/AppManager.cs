﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Drillings.Data;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class AppManager : MonoBehaviour
{
    public LayoutController CurrentLayout { get; private set; }
    public string CurrentObjectName { get; private set; }

	[SerializeField] private float noInputReloadTime;
	
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Compass compass;

    [Header("Layouts")]
    [SerializeField] private List<LayoutDataContainer> scenesLayoutDataAssets;

	private float reloadWhenInactiveTimer;



    private void Update()
	{
		#if UNITY_EDITOR
		//return;
		#endif
		
		if (CurrentLayout == null || CurrentLayout.IsPlaying)
			return;
		
		if (Input.touchCount > 0)
			reloadWhenInactiveTimer = 0;
		
		reloadWhenInactiveTimer += Time.deltaTime;
		if (reloadWhenInactiveTimer >= noInputReloadTime)
		{
			compass.StopFollow();
			reloadWhenInactiveTimer = 0;
		}
	}

    

    public void ActivateLayout(string objectName, Transform targetTransform)
    {
        if (CurrentLayout != null)
        {
	        compass.StopFollow();
        }

        LayoutDataContainer layoutContainer = scenesLayoutDataAssets.SingleOrDefault(x => x.objectName.Equals(objectName));

        if (layoutContainer == null)
	        return;
        
		reloadWhenInactiveTimer = 0;
		
		CurrentObjectName = layoutContainer.objectName;
		
		CurrentLayout = Instantiate(layoutContainer.sceneRoot, targetTransform).GetComponent<LayoutController>();
		CurrentLayout.UIManager = uiManager;
		CurrentLayout.LayoutData = layoutContainer.data;
		CurrentLayout.gameObject.SetActive(false);
		CurrentLayout.LayoutData.ResetUnit();
		
		compass.StartFollow(CurrentLayout.ObjectTransform);
		
        uiManager.ShowLoadingAnimation();
    }
    
    public bool DeactivateCurrentLayout()
    {
        if (CurrentLayout == null)
            return false;
        
        CurrentLayout.Stop();
        Destroy(CurrentLayout.gameObject);
        CurrentLayout = null;
        CurrentObjectName = null;
        uiManager.DisableControlElements();
        
        return true;
    }

    public void ShowLoadedLayout()
    {
        CurrentLayout.gameObject.SetActive(true);
        uiManager.EnableControlElements(CurrentLayout.LayoutData);
        CurrentLayout.SetUpUnit();
    }
}

[Serializable]
public class LayoutDataContainer
{
    public string objectName;
    public LayoutData data;
    public GameObject sceneRoot;
}