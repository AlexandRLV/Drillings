using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR
{
    public class TrackableObjectBehavior : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string sceneToLoad;

        [Header("References")]
        [SerializeField] private TrackableObject trackableObject;
        [SerializeField] private AppManager appManager;
        [SerializeField] private Compass compass;

        private Transform objectTransform;
        private bool isLoaded;
        


        private void OnEnable()
        {
            trackableObject.OnTrackableFound += OnTrackableFound;
            trackableObject.OnTrackableLost += OnTrackableLost;
            trackableObject.OnTrackableUpdated += OnTrackableUpdated;
        }

        private void OnDisable()
        {
            trackableObject.OnTrackableFound -= OnTrackableFound;
            trackableObject.OnTrackableLost -= OnTrackableLost;
            trackableObject.OnTrackableUpdated -= OnTrackableUpdated;
        }




        [ContextMenu("Enable behaviour")]
        public void EnableBehaviour()
        {
            if (!trackableObject.IsManagerEnabled)
                return;
            
            if (appManager.CurrentObjectName == sceneToLoad)
                return;

            Debug.Log("Enabling behaviour");

#if UNITY_EDITOR
            if (objectTransform == null)
                objectTransform = transform;
#endif
        
            appManager.ActivateLayout(sceneToLoad, objectTransform);
        }

        private void DisableBehaviour()
        {
            compass.StopFollow();
        }

    
    

        private void OnTrackableFound(ARTrackedObject trackedObject)
        {
            objectTransform = trackedObject.transform;
            EnableBehaviour();
        }

        private void OnTrackableUpdated(ARTrackedObject trackedObject)
        {
            objectTransform = trackedObject.transform;
            EnableBehaviour();
        }

        private void OnTrackableLost()
        {
            objectTransform = null;
            DisableBehaviour();
        }
    }
}
