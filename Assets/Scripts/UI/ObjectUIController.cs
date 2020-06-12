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
        [SerializeField] private float buttonsTextOffset;
    
        [Header("References")]
        [SerializeField] private Text objectNameText;
        [SerializeField] private Text unitNameText;
        [SerializeField] private GameObject selectionsParent;
        [SerializeField] private RectTransform outerCircle;
        [SerializeField] private Compass compass;
    
        [Header("Resources")]
        [SerializeField] private Button unitButton;

        private bool isInMainPage;
        private Coroutine currentRoutine;
        private List<Button> selectionButtons;




        public void SetUpLayout()
        {
            layoutController.AudioFinished += GoHome;
            isInMainPage = true;
            
            LayoutData layoutData = layoutController.LayoutData;
            objectNameText.text = layoutData.objectName;
            unitNameText.gameObject.SetActive(false);
            
            selectionButtons = new List<Button>();

            float radius = outerCircle.rect.width / 2;
            int n = layoutData.units.Length;
            n = n % 2 == 0 ? n / 2 : (n + 1) / 2;

            float angle = 180f / (n + 1);
            for (int i = 0; i < n; i++)
            {
                float x = -radius * Mathf.Sin(angle * (i + 1) * Mathf.Deg2Rad);
                float y = radius * Mathf.Cos(angle * (i + 1) * Mathf.Deg2Rad);

                Button button = Instantiate(unitButton, selectionsParent.transform);
                int id = i;
                button.onClick.AddListener(() => OpenUnit(id));
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

                Text text = button.GetComponentInChildren<Text>();
                text.text = layoutData.units[i].unitName;

                RectTransform textRect = text.rectTransform;
                textRect.pivot = new Vector2(1, 0.5f);
                textRect.anchoredPosition = new Vector2(-buttonsTextOffset, 0);
                
                selectionButtons.Add(button);
            }
            
            angle = 180f / (layoutData.UnitsCount - n + 1);
            for (int i = n; i < layoutData.UnitsCount; i++)
            {
                float x = -radius * Mathf.Sin(angle * (i - n + 1) * Mathf.Deg2Rad);
                float y = radius * Mathf.Cos(angle * (i - n + 1) * Mathf.Deg2Rad);

                Button button = Instantiate(unitButton, selectionsParent.transform);
                int id = i;
                button.onClick.AddListener(() => OpenUnit(id));
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-x, y);

                Text text = button.GetComponentInChildren<Text>();
                text.text = layoutData.units[i].unitName;

                RectTransform textRect = text.rectTransform;
                textRect.pivot = new Vector2(0, 0.5f);
                textRect.anchoredPosition = new Vector2(buttonsTextOffset, 0);
                
                selectionButtons.Add(button);
            }



            // Vector2 anchors = new Vector2(0, 0.5f);
            //
            // // Prepare scroll view content width
            // float width = scrollViewContentPadding * 2 + buttonsOffset * (layoutData.UnitsCount - 1);
            // scrollViewContent.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
            //
            // // Instantiate buttons
            // for (int i = 0; i < layoutData.units.Length; i++)
            // {
            //     Button button = Instantiate(unitButton, scrollViewContent);
            //     int id = i;
            //     button.onClick.AddListener(() => OpenUnit(id));
            //     
            //     RectTransform buttonTransform = button.GetComponent<RectTransform>();
            //     buttonTransform.anchorMax = anchors;
            //     buttonTransform.anchorMin = anchors;
            //     buttonTransform.anchoredPosition = new Vector2(scrollViewContentPadding + buttonsOffset * i, 0);
            //
            //     button.GetComponentInChildren<Text>().text = layoutData.units[i].unitName;
            //     
            //     selectionButtons.Add(button);
            // }
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

        public void GoHome()
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
                GoToMainPage();
            }
        }

        public void PlayAllUnits()
        {
            
        }



        private void GoToMainPage()
        {
            layoutController.Stop();
            unitNameText.gameObject.SetActive(false);
            selectionsParent.SetActive(true);
            isInMainPage = true;
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
        
            unitNameText.gameObject.SetActive(true);
            selectionsParent.SetActive(false);
            
            unitNameText.text = unit.unitName;
            
            unitNameText.gameObject.SetActive(true);
            layoutController.Play();
        }
    }
}