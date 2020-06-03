using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using UnityEngine.Serialization;

namespace UI
{
    public class ObjectUIController : MonoBehaviour
    {
        public LayoutController layoutController;
        
        [Header("Settings")]
        [SerializeField] private float scrollViewContentPadding;
        [SerializeField] private float buttonsOffset;
        [SerializeField] private float delayBeforePlay;
    
        [Header("References")]
        [SerializeField] private Text objectNameText;
        [SerializeField] private Text unitNameText;
        [SerializeField] private Text loadingText;
        [SerializeField] private GameObject buttonHome;
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject scrollBackground;
        [SerializeField] private RectTransform scrollViewContent;
    
        [Header("Resources")]
        [SerializeField] private Button unitButton;
    
        private UIFadeManager loadingTextFade;
        private Coroutine currentRoutine;
        private List<Button> selectionButtons;


        private void OnEnable()
        {
            loadingTextFade = loadingText.GetComponent<UIFadeManager>();
            loadingTextFade.gameObject.SetActive(false);
            buttonHome.SetActive(false);
        }


        public void SetUpLayout()
        {
            layoutController.AudioFinished += GoHome;
            
            LayoutData layoutData = layoutController.LayoutData;
            objectNameText.text = layoutData.objectName;
            
            selectionButtons = new List<Button>();
            Vector2 anchors = new Vector2(0, 0.5f);
            
            // Prepare scroll view content width
            float width = scrollViewContentPadding * 2 + buttonsOffset * (layoutData.UnitsCount - 1);
            scrollViewContent.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);

            // Instantiate buttons
            for (int i = 0; i < layoutData.units.Length; i++)
            {
                Button button = Instantiate(unitButton, scrollViewContent);
                int id = i;
                button.onClick.AddListener(() => OpenUnit(id));
                
                RectTransform buttonTransform = button.GetComponent<RectTransform>();
                buttonTransform.anchorMax = anchors;
                buttonTransform.anchorMin = anchors;
                buttonTransform.anchoredPosition = new Vector2(scrollViewContentPadding + buttonsOffset * i, 0);

                button.GetComponentInChildren<Text>().text = layoutData.units[i].unitName;
                
                selectionButtons.Add(button);
            }
        }

        public void DisposeLayout()
        {
            GoHome();
            
            layoutController.AudioFinished -= GoHome;
            
            if (selectionButtons == null || selectionButtons.Count == 0)
                return;
        
            for (int i = 0; i < selectionButtons.Count; i++)
            {
                int id = i;
                selectionButtons[i].GetComponent<Button>().onClick.RemoveListener(() => OpenUnit(id));
                Destroy(selectionButtons[i].gameObject);
            }
        }

        public void GoHome()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            layoutController.Stop();
            unitNameText.gameObject.SetActive(false);
            scrollView.SetActive(true);
            scrollBackground.SetActive(true);
            buttonHome.SetActive(false);
        }

    
    
        // private button listener
        private void OpenUnit(int unitId)
        {
            if (unitId < 0 || unitId >= selectionButtons.Count) 
                return;
            
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
        
            unitNameText.gameObject.SetActive(true);
            
            scrollView.SetActive(false);
            scrollBackground.SetActive(false);
            buttonHome.SetActive(true);

            ObjectInfoUnitData unit = layoutController.OpenUnit(unitId);
            
            if (unit == null)
                return;
            
            unitNameText.text = unit.unitName;

            if (delayBeforePlay == 0)
            {
                unitNameText.gameObject.SetActive(true);
                layoutController.Play();
            }
            else
            {
                loadingTextFade.gameObject.SetActive(true);
                loadingTextFade.FadeIn();
                loadingText.text = unit.unitName;
                unitNameText.gameObject.SetActive(false);
                StartCoroutine(WaitAndPlay(delayBeforePlay));
            }
        }



        private IEnumerator WaitAndPlay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        
            if (loadingTextFade.gameObject.activeSelf)
                loadingTextFade.FadeOut();
            
            unitNameText.gameObject.SetActive(true);
            layoutController.Play();
            
            currentRoutine = null;
        }
    }
}