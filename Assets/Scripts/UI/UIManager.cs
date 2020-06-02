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
            
            
            searchingCircles.gameObject.SetActive(false);
            objectLayout.gameObject.SetActive(false);
        
            StartCoroutine(Loading(loadingTime));
        }
    
    
        public void EnableControlElements()
        {
            objectLayout.gameObject.SetActive(true);
            objectLayout.layoutController = appManager.CurrentLayout;
            objectLayout.SetUpLayout();
        }

        public void DisableControlElements()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            if (objectLayout.gameObject.activeSelf)
            {
                objectLayout.DisposeLayout();
                objectLayout.gameObject.SetActive(false);
            }
            
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.Play();
        }

        public void ShowLoadingAnimation()
        {
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.ShowLoading();
        }
    

    
        // Methods for buttons control
        public void DisableIntroLayout()
        {
            introLayout.SetActive(false);
            
            if (arTrackedObjectManager != null)
                arTrackedObjectManager.enabled = true;
            
            objectLayout.gameObject.SetActive(false);
            searchingCircles.gameObject.SetActive(true);
        }

        public void Show()
        {
            searchingCircles.gameObject.SetActive(false);
            appManager.ShowLoadedLayout();
            EnableControlElements();
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