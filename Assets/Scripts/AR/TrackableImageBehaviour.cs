#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class TrackableImageBehaviour : MonoBehaviour
{
#region Params

    [Header("Settings")]
    [SerializeField] private string sceneToLoad;

    [Header("References")]
    [SerializeField] private TrackableImage trackableImage;
    [SerializeField] private AppManager appManager;

    private Transform imageTransform;

#endregion


#region MonoMethods

    private void OnEnable()
    {
        trackableImage.OnTrackableFound += OnTrackableFound;
        trackableImage.OnTrackableLost += OnTrackableLost;
        trackableImage.OnTrackableUpdated += OnTrackableUpdated;
    }

    private void OnDisable()
    {
        trackableImage.OnTrackableFound -= OnTrackableFound;
        trackableImage.OnTrackableLost -= OnTrackableLost;
        trackableImage.OnTrackableUpdated -= OnTrackableUpdated;
    }

#endregion


#region PublicMethods

    [ContextMenu("Enable behaviour")]
    public void EnableBehaviour()
    {
        if (appManager.CurrentLayout != null)
            return;
        
        Debug.Log("Enabling behaviour");

#if UNITY_EDITOR
        if (imageTransform == null)
            imageTransform = transform;
#endif
        
        appManager.ActivateLayout(sceneToLoad, imageTransform);

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

#endregion
    
    
#region PrivateMethods

    private void OnTrackableFound(ARTrackedImage arTrackedImage)
    {
        imageTransform = arTrackedImage.transform;
        EnableBehaviour();
    }

    private void OnTrackableUpdated(ARTrackedImage arTrackedImage)
    {
        if (!sceneToLoad.Equals(appManager.CurrentScene))
            return;

        imageTransform = arTrackedImage.transform; 
        //appManager.UpdateRootTransform(imageTransform);
    }

    private void OnTrackableLost()
    {
        imageTransform = null;
    }

#endregion
}