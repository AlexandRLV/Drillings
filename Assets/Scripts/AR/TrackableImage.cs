using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    public class TrackableImage : BaseTrackable<ARTrackedImage>
    {

        [Header("Settings")]
        [SerializeField] private string referenceImageName;
    
        [Header("References")]
        [SerializeField] private ARTrackedImageManager arTrackedImageManager;

        private TrackingState currentTrackingState = TrackingState.None;




        private void OnEnable()
        {
            arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        private void OnDisable()
        {
            arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }




        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
        {
            ProcessAddedImages(args.added);
            ProcessUpdatedImages(args.updated);
            ProcessRemovedImages(args.removed);
        }

        private void ProcessAddedImages(List<ARTrackedImage> addedImages)
        {
            foreach (ARTrackedImage image in addedImages)
            {
                if (!image.referenceImage.name.Equals(referenceImageName))
                    continue;
            
                currentTrackingState = image.trackingState;
                InvokeOnTrackableFound(image);
            }
        }

        private void ProcessUpdatedImages(List<ARTrackedImage> updatedImages)
        {
            foreach (ARTrackedImage image in updatedImages)
            {
                if (!image.referenceImage.name.Equals(referenceImageName))
                    continue;

                TrackingState prevTrackingState = currentTrackingState;
                currentTrackingState = image.trackingState;

                if (currentTrackingState != prevTrackingState)
                {
                    if (currentTrackingState == TrackingState.Tracking)
                        InvokeOnTrackableFound(image);
                    else
                        InvokeOnTrackableLost();
                }
            
                if (currentTrackingState != TrackingState.None)
                    InvokeOnTrackableUpdated(image);
            }
        }

        private void ProcessRemovedImages(List<ARTrackedImage> removedImages)
        {
            foreach (ARTrackedImage image in removedImages.Where(image => image.referenceImage.name.Equals(referenceImageName)))
            {
                currentTrackingState = image.trackingState;
                InvokeOnTrackableLost();
            }
        }
    }
}
