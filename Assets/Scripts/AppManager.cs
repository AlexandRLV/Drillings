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
		if (CurrentLayout == null || CurrentLayout.IsPlaying)
			return;

		if (objectTransform == null)
			return;

		UpdateRootTransform();
	}

    

    public void ActivateLayout(string objectName, Transform targetTransform)
    {
	    if (CurrentLayout != null || !string.IsNullOrWhiteSpace(CurrentObjectName))
		    return;
	    
	    LayoutDataContainer layoutContainer = scenesLayoutDataAssets.SingleOrDefault(x => x.objectName.Equals(objectName));

	    if (layoutContainer == null)
		    return;

	    CurrentObjectName = layoutContainer.objectName;
		objectTransform = targetTransform;
		
		CurrentLayout = Instantiate(layoutContainer.sceneRoot, targetTransform.position, Quaternion.identity).GetComponent<LayoutController>();
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
        
        uiManager.DisableControlElements();
        Destroy(CurrentLayout.gameObject);
        CurrentLayout = null;
        CurrentObjectName = null;
        objectTransform = null;
        
        return true;
    }

    public void ShowLoadedLayout()
    {
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