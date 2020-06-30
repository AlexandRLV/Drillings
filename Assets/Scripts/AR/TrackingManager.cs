using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR
{
    [RequireComponent(typeof(ARTrackedObjectManager))]
    public class TrackingManager : MonoBehaviour
    {
        public Trackable[] Trackables => trackables;
        
        [Header("Settings")]
        [SerializeField] private float angleToHide;
        [SerializeField] private float angleToShow;
        
        [Header("References")]
        [SerializeField] private AppManager appManager;

        [Header("Trackables")]
        [SerializeField] private Trackable[] trackables;

        private ARTrackedObjectManager aRTrackedObjectManager;
        private Transform cameraTransform;


        private void Awake()
        {
            foreach (Trackable t in trackables)
            {
                t.Disable();
            }
        }

        private void OnEnable()
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;

            aRTrackedObjectManager = GetComponent<ARTrackedObjectManager>();
            aRTrackedObjectManager.trackedObjectsChanged += TrackedObjectsChanged;
        }

        private void OnDisable()
        {
            aRTrackedObjectManager.trackedObjectsChanged -= TrackedObjectsChanged;
        }

        private void Update()
        {
            CheckTrackablesToDeactivate();
            CheckTrackablesToActivate();
        }


        
        public void EnableTrackableWithDefaultTransform(Trackable t)
        {
            t.UpdateObjectTransform(transform);
            appManager.ActivateLayout(t);
        }



        // AR Tracked Manager event's handlers
        private void TrackedObjectsChanged(ARTrackedObjectsChangedEventArgs obj)
        {
            foreach (ARTrackedObject o in obj.added)
            {
                HandleAdded(o);
            }
            foreach (ARTrackedObject o in obj.updated)
            {
                HandleUpdated(o);
            }
            foreach (ARTrackedObject o in obj.removed)
            {
                HandleRemoved(o);
            }
        }

        private void HandleAdded(ARTrackedObject trackedObject)
        {
            string refName = trackedObject.referenceObject.name;
            Debug.Log("ADDED at " + Time.time + ": " + refName);

            Trackable trackable = trackables.SingleOrDefault(x => x.ReferenceName == refName);

            if (trackable == null)
                return;
            
            trackable.UpdateObjectTransform(trackedObject.transform);
            appManager.ActivateLayout(trackable);
        }

        private void HandleUpdated(ARTrackedObject trackedObject)
        {
            string refName = trackedObject.referenceObject.name;
            Debug.Log("UPDATED at " + Time.time + ": " + refName);
        
            Trackable trackable = trackables.SingleOrDefault(x => x.ReferenceName == refName);
        
            if (trackable == null) 
                return;
            
            trackable.UpdateObjectTransform(trackedObject.transform);
        
            if (!trackable.IsActive)
                appManager.ActivateLayout(trackable);
        }

        private void HandleRemoved(ARTrackedObject trackedObject)
        {
            string refName = trackedObject.referenceObject.name;
            Debug.Log("REMOVED at " + Time.time + ": " + trackedObject.referenceObject.name);
            
            Trackable trackable = trackables
                .SingleOrDefault(x => 
                    x.ReferenceName == refName
                    && x.IsActive);
            
            if (trackable != null)
                appManager.DeactivateCurrentLayout();
        }



        private void CheckTrackablesToDeactivate()
        {
            foreach (Trackable trackable in trackables.Where(x => x.IsActive))
            {
                Vector3 directionToTr = trackable.Position - cameraTransform.transform.position;
                if (Vector3.Angle(directionToTr, cameraTransform.transform.forward) > angleToHide)
                {
                    appManager.DeactivateCurrentLayout();
                }
            }
        }

        private void CheckTrackablesToActivate()
        {
            foreach (Trackable trackable in trackables.Where(x => x.IsFound && !x.IsActive))
            {
                Vector3 directionToTr = trackable.Position - cameraTransform.transform.position;
                if (Vector3.Angle(directionToTr, cameraTransform.transform.forward) < angleToShow)
                {
                    appManager.ActivateLayout(trackable);
                }
            }
        }
    }
}
