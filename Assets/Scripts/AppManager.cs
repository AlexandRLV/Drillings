using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UI;
using AR;

public class AppManager : MonoBehaviour
{
	public Trackable CurrentTrackable { get; private set; }

    [Header("References")]
    [SerializeField] private UIManager uiManager;



	public void ActivateLayout(Trackable trackable)
    {
	    Debug.Log($"Activating layout {trackable.ReferenceName}");
	    if (CurrentTrackable != null)
	    {
		    Debug.Log($"Already activated: {CurrentTrackable.ReferenceName}");
		    return;
	    }

		CurrentTrackable = trackable;
		CurrentTrackable.Enable();
		CurrentTrackable.UpdatePositionAndRotation();
		
		CurrentTrackable.LayoutController.SetUpUnit();

		uiManager.Show();
    }
    
    public bool DeactivateCurrentLayout()
    {
	    Debug.Log("Deactivating layout");
        if (CurrentTrackable == null)
        {
	        Debug.Log("Deactivation failed: no active layout");
	        return false;
        }
        Debug.Log($"Deactivating {CurrentTrackable.ReferenceName}");
        
        uiManager.DisableControlElements();
        CurrentTrackable.Disable();
        CurrentTrackable = null;

        return true;
    }
}