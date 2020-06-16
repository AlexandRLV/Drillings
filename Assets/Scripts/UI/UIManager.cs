using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static float fadeInOutTime;
    
        [Header("Settings")]
        [SerializeField] private float loadingTime;
        [SerializeField] private float uiFadeInOutTime;
    
        [Header("References")]
        [SerializeField] private AppManager appManager;
        [SerializeField] private SearchingCircles searchingCircles;
        [SerializeField] private ARTrackedObjectManager arTrackedObjectManager;
        [SerializeField] private ARTrackedImageManager arTrackedImageManager;
        [SerializeField] private ObjectUIController objectLayout;
        [SerializeField] private GameObject introLayout;
        [SerializeField] private GameObject loadingIndicator;
        [SerializeField] private GameObject loadingText;
        [SerializeField] private GameObject startButton;

        private Coroutine currentRoutine;
    

        public void Start()
        {
            fadeInOutTime = uiFadeInOutTime;

            introLayout.SetActive(true);
            startButton.SetActive(false);
            loadingIndicator.SetActive(true);
            loadingText.SetActive(true);
            
            if (arTrackedObjectManager != null)
                arTrackedObjectManager.enabled = false;
            
            if (arTrackedImageManager != null)
                arTrackedImageManager.enabled = false;
            
            
            searchingCircles.gameObject.SetActive(false);
            objectLayout.gameObject.SetActive(false);
        
            StartCoroutine(Loading(loadingTime));
        }



        public void DisableControlElements()
        {
            Debug.Log("UIManager: Disabling control elements");
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            if (objectLayout.gameObject.activeSelf)
            {
                Debug.Log("UIManager: Disabling controls");
                objectLayout.DisposeLayout();
                objectLayout.gameObject.SetActive(false);
            }
            
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.Play();
        }

        public void ShowLoadingAnimation()
        {
            Debug.Log("UIManager: Enabling loading animation");
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.ShowLoading();
        }
    

    
        // Methods for buttons control
        public void DisableIntroLayout()
        {
            Debug.Log("UIManager: Disabling intro layout");
            introLayout.SetActive(false);
            
            if (arTrackedObjectManager != null)
                arTrackedObjectManager.enabled = true;
            
            if (arTrackedImageManager != null)
                arTrackedImageManager.enabled = true;
            
            objectLayout.gameObject.SetActive(false);
            searchingCircles.gameObject.SetActive(true);
        }

        public void Show()
        {
            Debug.Log("UIManager: Showing object");
            searchingCircles.gameObject.SetActive(false);
            appManager.ShowLoadedLayout();
            EnableControlElements();
            Debug.Log("Activation finished");
        }

    

        private void EnableControlElements()
        {
            Debug.Log("UIManager: Enabling controls");
            objectLayout.gameObject.SetActive(true);
            objectLayout.LayoutController = appManager.CurrentLayout;
            objectLayout.SetUpLayout();
        }
        
        private IEnumerator Loading(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            
            loadingIndicator.SetActive(false);
            loadingText.SetActive(false);
            startButton.SetActive(true);
        }
    }
}