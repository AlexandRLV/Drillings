﻿using System.Collections;
using AR;
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
        [SerializeField] private ObjectUIController objectLayout;
        [SerializeField] private TrackingManager trackingManager;
        
        [Header("UI elements")]
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

            trackingManager.IsInSearchMode = false;
            
            searchingCircles.gameObject.SetActive(false);
            objectLayout.gameObject.SetActive(false);
        
            StartCoroutine(Loading(loadingTime));
        }



        public void DisableControlElements()
        {
            //Debug.Log("UIManager: Disabling control elements");
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            if (objectLayout.gameObject.activeSelf)
            {
                //Debug.Log("UIManager: Disabling controls");
                objectLayout.DisposeLayout();
                objectLayout.gameObject.SetActive(false);
            }
            
            searchingCircles.gameObject.SetActive(true);
            searchingCircles.Play();
        }
        
        
        
        // Methods for buttons control
        public void DisableIntroLayout()
        {
            introLayout.SetActive(false);

            trackingManager.IsInSearchMode = true;
            
            objectLayout.gameObject.SetActive(false);
            searchingCircles.gameObject.SetActive(true);
        }

        public void Show()
        {
            searchingCircles.gameObject.SetActive(false);
            EnableControlElements();
        }

    

        private void EnableControlElements()
        {
            objectLayout.gameObject.SetActive(true);
            objectLayout.LayoutController = appManager.CurrentTrackable.LayoutController;
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