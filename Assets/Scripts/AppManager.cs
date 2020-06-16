using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UI;

public class AppManager : MonoBehaviour
{
    public LayoutController CurrentLayout { get; private set; }
    public string CurrentObjectName { get; private set; }
	
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Compass compass;

    [Header("Layouts")]
    [SerializeField] private List<LayoutDataContainer> scenesLayoutDataAssets;

	private Transform objectTransform;



    private void Update()
	{
		if (objectTransform == null || CurrentLayout == null)
			return;
		
		UpdateRootTransform();
	}

    

    public void ActivateLayout(string objectName, Transform targetTransform)
    {
	    // Debug.Log("");
	    // Debug.Log("Activation of new object:");
	    // Debug.Log($"AppManager: Activating {objectName}");
	    if (CurrentLayout != null || !string.IsNullOrWhiteSpace(CurrentObjectName))
	    {
		    // Debug.Log($"AppManager: Already activated object, returning");
		    return;
	    }
	    
	    LayoutDataContainer layoutContainer = scenesLayoutDataAssets.SingleOrDefault(x => x.objectName.Equals(objectName));

	    if (layoutContainer == null)
		    return;
	    
	    // Debug.Log($"AppManager: Found data for {objectName}");
		
		CurrentObjectName = layoutContainer.objectName;
		objectTransform = targetTransform;
		
		CurrentLayout = Instantiate(layoutContainer.sceneRoot, targetTransform.position, Quaternion.identity).GetComponent<LayoutController>();
		CurrentLayout.LayoutData = layoutContainer.data;
		CurrentLayout.gameObject.SetActive(false);
		CurrentLayout.LayoutData.ResetUnit();
		
		// Debug.Log($"AppManager: Activated {objectName}");
		
		compass.StartFollow(CurrentLayout.ObjectTransform);
		
        uiManager.ShowLoadingAnimation();
    }
    
    public bool DeactivateCurrentLayout()
    {
	    // Debug.Log("AppManager: Deactivating");
        if (CurrentLayout == null)
        {
	        // Debug.Log("AppManager: No active layout");
	        return false;
        }
        
        uiManager.DisableControlElements();
        Destroy(CurrentLayout.gameObject);
        CurrentLayout = null;
        CurrentObjectName = null;
        objectTransform = null;
        // Debug.Log("AppManager: Deactivated");

        return true;
    }

    public void ShowLoadedLayout()
    {
	    // Debug.Log("AppManager: Show");
        CurrentLayout.gameObject.SetActive(true);
        CurrentLayout.SetUpUnit();
    }


    
    private void UpdateRootTransform()
    {
	    CurrentLayout.transform.position = objectTransform.position;
	    CurrentLayout.transform.rotation = Quaternion.Euler(0, objectTransform.rotation.eulerAngles.y, 0);
    }
}

[Serializable]
public class LayoutDataContainer
{
    public string objectName;
    public LayoutData data;
    public GameObject sceneRoot;
}