using System.Collections;
using Data;
using Drillings.Data;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static float fadeInOutTime;
    
        [Header("Settings")]
        [SerializeField] private float loadingTime;
        [SerializeField] private float restartDelay;
        [SerializeField] private float continueDelay;
        [SerializeField] private float uiFadeInOutTime;
    
        [Header("References")]
        [SerializeField] private AppManager appManager;
        [SerializeField] private SearchingCircles searchingCircles;
        [SerializeField] private ARTrackedImageManager arTrackedImageManager;
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

            if (arTrackedImageManager != null)
                arTrackedImageManager.enabled = false;
            
            if (arTrackedObjectManager != null)
                arTrackedObjectManager.enabled = false;
            
            
            searchingCircles.gameObject.SetActive(false);
            objectLayout.gameObject.SetActive(false);
        
            StartCoroutine(Loading(loadingTime));
        }
    
    
        public void EnableControlElements(LayoutData layoutData)
        {
            objectLayout.gameObject.SetActive(true);
            objectLayout.SetUpLayout(layoutData);
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
                objectLayout.DisposeSelections();
                objectLayout.gameObject.SetActive(false);
            }
            
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.Play();
        }

        public void UpdateUnit(ObjectInfoUnitData unit)
        {
            objectLayout.SetUpUnit(unit);
        }

        public void ShowLoadingAnimation()
        {
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.ShowLoading();
        }

        public void AudioFinished(bool isLastUnit)
        {
            Debug.Log("Audio finished");
            objectLayout.AudioFinished();

            if (isLastUnit)
                currentRoutine = StartCoroutine(ShowRestartButtonWithDelay());
            else
                Continue();
        }

        public void Restart()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
        
            Debug.Log("Restarting");
            searchingCircles.gameObject.SetActive(false);
            objectLayout.gameObject.SetActive(true);
            objectLayout.ChangeActiveUnitSelection(0);
            OpenUnit(0);
        }

        public void Continue()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
        
            Debug.Log("Continue");
            NextUnit();
        }
    

    
        // Methods for buttons control
        public void DisableIntroLayout()
        {
            introLayout.SetActive(false);
            
            if (arTrackedImageManager != null)
                arTrackedImageManager.enabled = true;
            
            if (arTrackedObjectManager != null)
                arTrackedObjectManager.enabled = true;
            
            objectLayout.gameObject.SetActive(false);
            searchingCircles.gameObject.SetActive(true);
        }
    
        public void NextUnit()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
        
            Debug.Log("Activating next unit");
            appManager.CurrentLayout.NextUnit();
        
            objectLayout.ActivateNextUnitSelection();
        }

        public void PrevUnit()
        {
            if (currentRoutine != null)
                StopCoroutine(currentRoutine);
        
            Debug.Log("Activating prev unit");
            appManager.CurrentLayout.PrevUnit();
        
            objectLayout.ActivatePrevUnitSelection();
        }

        public void OpenUnit(int unitId)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
        
            appManager.CurrentLayout.OpenUnit(unitId);
        }

        public void PlayPause()
        {
            if (currentRoutine != null)
            {
                Continue();
            }
        
            if (appManager.CurrentLayout.IsPlaying)
                appManager.CurrentLayout.Stop();
            else
                appManager.CurrentLayout.Play();
        }

        public void Show()
        {
            searchingCircles.gameObject.SetActive(false);
            appManager.ShowLoadedLayout();
        }

    

        private IEnumerator Loading(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            loadingIndicator.SetActive(false);
            loadingText.SetActive(false);
            startButton.SetActive(true);
        }

        private IEnumerator ShowRestartButtonWithDelay()
        {
            Debug.Log("Started restart coroutine");
            yield return new WaitForSeconds(restartDelay);
        
            objectLayout.gameObject.SetActive(false);
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.ShowRestartButton();
        }

        private IEnumerator ContinueWithDelay()
        {
            yield return new WaitForSeconds(continueDelay);
        
            currentRoutine = null;
            NextUnit();
        }
    }
}