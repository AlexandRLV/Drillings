#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private Transform objectTransform;
        


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
            Debug.Log("Enabling behaviour");

#if UNITY_EDITOR
            if (objectTransform == null)
                objectTransform = transform;
#endif
        
            appManager.ActivateLayout(sceneToLoad, objectTransform);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
        }

        public void DisableBehaviour()
        {
            appManager.DeactivateCurrentLayout();
        
#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
#endif
        }

    
    

        private void OnTrackableFound(ARTrackedObject trackedObject)
        {
            objectTransform = trackedObject.transform;
            EnableBehaviour();
        }

        private void OnTrackableUpdated(ARTrackedObject trackedObject)
        {
            if (!sceneToLoad.Equals(appManager.CurrentObjectName))
                return;

            objectTransform = trackedObject.transform; 
        }

        private void OnTrackableLost()
        {
            objectTransform = null;
        }
    }
}
