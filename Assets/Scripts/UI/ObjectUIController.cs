﻿using System.Collections;
using System.Collections.Generic;
using Data;
using Drillings.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ObjectUIController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float controlsButtonsOffset;
        [SerializeField] private float controlsUnitWidthMultiplier;
        [SerializeField] private float delayBeforePlay;
        [SerializeField] private Color defaultUnitColor;
        [SerializeField] private Color selectedUnitColor;
    
        [Header("References")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Compass compass;
        [SerializeField] private Text objectNameText;
        [SerializeField] private Text unitNameText;
        [SerializeField] private Text loadingText;
        [SerializeField] private Image playButtonImage;
        [SerializeField] private RectTransform nextUnitButton;
        [SerializeField] private RectTransform prevUnitButton;
    
        [Header("Resources")]
        [SerializeField] private Sprite playSprite;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Image unitSelectionImage;
    
        private bool isPlaying;
        private int currentUnitId;
        private UIFadeManager loadingTextFade;
        private Coroutine currentRoutine;
        private List<Image> selections;


        private void OnEnable()
        {
            loadingTextFade = loadingText.GetComponent<UIFadeManager>();
            loadingTextFade.gameObject.SetActive(false);
        }


        public void SetUpLayout(LayoutData layoutData)
        {
            // Set layout main info
            objectNameText.text = layoutData.objectName;
            unitNameText.text = layoutData.CurrentUnit.unitName;
            currentUnitId = layoutData.CurrentUnitId;
            
            // Calculate unit selections values
            int unitCount = layoutData.UnitsCount;
            float yPosition = nextUnitButton.anchoredPosition.y;

            float diff = nextUnitButton.anchoredPosition.x - prevUnitButton.anchoredPosition.x;
            float unitsOffset = diff / 2 + prevUnitButton.anchoredPosition.x;
            float widthPerUnit = (diff - controlsButtonsOffset * 2) / unitCount;
            float unitLength = widthPerUnit * controlsUnitWidthMultiplier;
            
            // Set unit selections
        
            Vector2 anchors = new Vector2(0.5f, 0);
            
            float a = unitCount % 2 == 0 ? unitCount / 2 - 0.5f : (unitCount - 1) / 2;
            selections = new List<Image>();
            for (float i = -a; i <= a; i += 1)
            {
                Image unitSelection = Instantiate(unitSelectionImage, nextUnitButton.parent);
            
                RectTransform selectionTransform = unitSelection.rectTransform;
                selectionTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, unitLength);
                selectionTransform.anchorMax = anchors;
                selectionTransform.anchorMin = anchors;
                selectionTransform.anchoredPosition = new Vector2(widthPerUnit * i + unitsOffset, yPosition);

                int id = selections.Count;
                unitSelection.GetComponent<Button>().onClick.AddListener(() => OpenUnit(id));
            
                selections.Add(unitSelection);
            }

            selections[currentUnitId].color = selectedUnitColor;
        }

        public void SetUpUnit(ObjectInfoUnitData unitData)
        {
            unitNameText.text = unitData.unitName;
        
            loadingTextFade.gameObject.SetActive(true);
            loadingTextFade.FadeIn();
            loadingText.text = unitData.unitName;
            unitNameText.gameObject.SetActive(false);
        
            isPlaying = false;
            playButtonImage.sprite = playSprite;
        
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
            }
            currentRoutine = StartCoroutine(WaitAndPlay(delayBeforePlay));
        }

        public void DisposeSelections()
        {
            if (selections == null || selections.Count == 0)
                return;
        
            for (int i = 0; i < selections.Count; i++)
            {
                int id = i;
                selections[i].GetComponent<Button>().onClick.RemoveListener(() => OpenUnit(id));
                Destroy(selections[i].gameObject);
            }
        }

        public void PlayPause()
        {
            if (currentRoutine != null)
            {
                StopCoroutine(currentRoutine);
                currentRoutine = null;
            }

            if (loadingTextFade.gameObject.activeSelf)
                loadingTextFade.FadeOut();
        
            unitNameText.gameObject.SetActive(true);
        
            isPlaying = !isPlaying;
            playButtonImage.sprite = isPlaying ? pauseSprite : playSprite;
        
            uiManager.PlayPause();
        }

        public void NextUnit()
        {
            uiManager.NextUnit();
        }

        public void PrevUnit()
        {
            uiManager.PrevUnit();
        }

        public void GoHome()
        {
            compass.StopFollow();
        }

        public void ActivateNextUnitSelection()
        {
            if (currentUnitId == selections.Count - 1)
                return;

            selections[currentUnitId].color = defaultUnitColor;
            currentUnitId++;
            selections[currentUnitId].color = selectedUnitColor;
        }

        public void ActivatePrevUnitSelection()
        {
            if (currentUnitId == 0)
                return;

            selections[currentUnitId].color = defaultUnitColor;
            currentUnitId--;
            selections[currentUnitId].color = selectedUnitColor;
        }

        public void ChangeActiveUnitSelection(int unitId)
        {
            selections[currentUnitId].color = defaultUnitColor;
            currentUnitId = unitId;
            selections[currentUnitId].color = selectedUnitColor;
        }

        public void AudioFinished()
        {
            playButtonImage.sprite = playSprite;
            isPlaying = false;
        }

    
    
        // private button listener
        private void OpenUnit(int unitId)
        {
            if (currentUnitId == unitId || unitId < 0 || unitId > selections.Count - 1) 
                return;
        
            if (isPlaying)
                PlayPause();
        
            uiManager.OpenUnit(unitId);
            ChangeActiveUnitSelection(unitId);
        }



        private IEnumerator WaitAndPlay(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        
            PlayPause();
            currentRoutine = null;
        }
    }
}