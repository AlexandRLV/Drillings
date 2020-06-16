using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    public class TrackableObject : BaseTrackable<ARTrackedObject>
    {
        public bool IsManagerEnabled => arTrackedObjectManager.enabled;
        
        [Header("Settings")]
        [SerializeField] private string referenceObjectName;

        [Header("References")]
        [SerializeField] private ARTrackedObjectManager arTrackedObjectManager;

        private TrackingState currentTrackingState = TrackingState.None;

        private void OnEnable()
        {
            arTrackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
        }

        private void OnDisable()
        {
            arTrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
        }


        private void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs args)
        {
            ProcessAddedObjects(args.added);
            ProcessUpdatedObjects(args.updated);
            ProcessRemovedObjects(args.removed);
        }

        private void ProcessAddedObjects(List<ARTrackedObject> added)
        {
            ARTrackedObject trackedObject =
                added.SingleOrDefault(x => x.referenceObject.name.Equals(referenceObjectName));
            
            if (trackedObject == null)
                return;

            currentTrackingState = trackedObject.trackingState;
            InvokeOnTrackableFound(trackedObject);
        }

        private void ProcessUpdatedObjects(List<ARTrackedObject> updated)
        {
            ARTrackedObject trackedObject =
                updated.SingleOrDefault(x => x.referenceObject.name.Equals(referenceObjectName));
            
            if (trackedObject == null)
                return;

            TrackingState prevTrackingState = currentTrackingState;
            currentTrackingState = trackedObject.trackingState;

            if (currentTrackingState != prevTrackingState)
            {
                if (currentTrackingState == TrackingState.Tracking)
                {
                    InvokeOnTrackableFound(trackedObject);
                }
                else
                {
                    InvokeOnTrackableLost();
                }
            }
            
            if (currentTrackingState != TrackingState.None)
            {
                InvokeOnTrackableUpdated(trackedObject);
            }
        }

        private void ProcessRemovedObjects(List<ARTrackedObject> removed)
        {
            ARTrackedObject trackedObject =
                removed.SingleOrDefault(x => x.referenceObject.name.Equals(referenceObjectName));
            
            if (trackedObject == null)
                return;

            currentTrackingState = trackedObject.trackingState;
            InvokeOnTrackableLost();
        }
    }
}
