using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

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
        [SerializeField] private GameObject buttonPlayAll;
        [SerializeField] private GameObject scrollView;
        [SerializeField] private GameObject scrollBackground;
        [SerializeField] private RectTransform scrollViewContent;
        [SerializeField] private Compass compass;
    
        [Header("Resources")]
        [SerializeField] private Button unitButton;

        private bool isInMainPage;
        private bool isInPlayAllMode;
        private int lastPlayedUnit;
        private Coroutine currentRoutine;
        private List<Button> selectionButtons;


        public void SetUpLayout()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            layoutController.AudioFinished += GoHome;
            isInMainPage = true;
            isInPlayAllMode = false;
            
            LayoutData layoutData = layoutController.LayoutData;
            objectNameText.text = layoutData.objectName;
            unitNameText.gameObject.SetActive(false);
            scrollView.SetActive(true);
            scrollBackground.SetActive(true);
            buttonPlayAll.SetActive(true);
            
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
            GoToMainPage();
            
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

        public void GoHome(bool isLastUnit)
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }
            
            if (isInMainPage)
                compass.StopFollow();
            else
            {
                if (isInPlayAllMode)
                {
                    if (isLastUnit)
                    {
                        GoToMainPage();
                    }
                    else
                        currentRoutine = StartCoroutine(WaitAndPlayNextUnit(delayBeforePlay));
                }
                else
                    GoToMainPage();
            }
        }

        public void PlayAllUnits()
        {
            isInPlayAllMode = true;
            OpenUnit(0);
        }



        private void GoToMainPage()
        {
            layoutController.Stop();
            unitNameText.gameObject.SetActive(false);
            scrollView.SetActive(true);
            scrollBackground.SetActive(true);
            isInMainPage = true;
            isInPlayAllMode = false;
            buttonPlayAll.SetActive(true);
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

            ObjectInfoUnitData unit = layoutController.OpenUnit(unitId);
            
            if (unit == null)
                return;

            isInMainPage = false;
        
            buttonPlayAll.SetActive(false);
            
            scrollView.SetActive(false);
            scrollBackground.SetActive(false);
            
            unitNameText.gameObject.SetActive(true);
            unitNameText.text = unit.unitName;

            lastPlayedUnit = unitId;
            
            layoutController.Play();
        }



        private IEnumerator WaitAndPlayNextUnit(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            OpenUnit(lastPlayedUnit + 1);
            currentRoutine = null;
        }
    }
}