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
            
            Debug.Log("ObjectUI: SetUp");
            DebugWriter.Write("ObjectUI: SetUp");
            layoutController.AudioFinished += GoHome;
            isInMainPage = true;
            isInPlayAllMode = false;
            
            LayoutData layoutData = layoutController.LayoutData;
            objectNameText.text = layoutData.objectName;
            unitNameText.gameObject.SetActive(false);
            selectionsParent.SetActive(true);
            
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
                text.text = layoutData.units[i].shortName;

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
                text.text = layoutData.units[i].shortName;

                RectTransform textRect = text.rectTransform;
                textRect.pivot = new Vector2(0, 0.5f);
                textRect.anchoredPosition = new Vector2(buttonsTextOffset, 0);
                
                selectionButtons.Add(button);
            }
        }

        public void DisposeLayout()
        {
            Debug.Log("ObjectUI: Dispose");
            DebugWriter.Write("ObjectUI: Dispose");
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
                        isInPlayAllMode = false;
                    }
                    else
                        currentRoutine = StartCoroutine(WaitAndPlayNextUnit(1));
                }
                else
                {
                    GoToMainPage();
                }
            }
        }

        public void PlayAllUnits()
        {
            isInPlayAllMode = true;
            OpenUnit(0);
        }



        private void GoToMainPage()
        {
            Debug.Log("ObjectUI: ToMainPage");
            DebugWriter.Write("ObjectUI: ToMainPage");
            layoutController.Stop();
            unitNameText.gameObject.SetActive(false);
            selectionsParent.SetActive(true);
            isInMainPage = true;
            isInPlayAllMode = false;
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
        
            selectionsParent.SetActive(false);
            unitNameText.gameObject.SetActive(true);
            unitNameText.text = unit.unitName;

            lastPlayedUnit = unitId;
            
            layoutController.Play();
        }

        private IEnumerator WaitAndPlayNextUnit(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            OpenUnit(lastPlayedUnit + 1);
            currentRoutine = null;
        }
    }
}