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
	    if (CurrentTrackable != null)
	    {
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
        if (CurrentTrackable == null)
        {
	        return false;
        }
        
        uiManager.DisableControlElements();
        CurrentTrackable.Disable();
        CurrentTrackable = null;

        return true;
    }
}