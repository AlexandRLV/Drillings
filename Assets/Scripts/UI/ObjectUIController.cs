using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float controlsDefaultWidth;
    [SerializeField] private float controlsWidthPerUnit;
    [SerializeField] private float controlsButtonOffset;
    [SerializeField] private float delayBeforePlay;
    [SerializeField] private Color defaultUnitColor;
    [SerializeField] private Color selectedUnitColor;
    
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Text objectNameText;
    [SerializeField] private Text unitNameText;
    [SerializeField] private Text loadingText;
    [SerializeField] private Image playButtonImage;
    [SerializeField] private RectTransform controlsBackground;
    [SerializeField] private RectTransform nextUnitButton;
    [SerializeField] private RectTransform prevUnitButton;
    
    [Header("Resources")]
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Image unitSelectionImage;
    
    private bool isPlaying;
    private int currentUnitId;
    private UIFadeManager loadingTextFade;
    private Coroutine currentRunningRoutine;
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
        
        // Set controls panel position and size
        int unitCount = layoutData.UnitsCount;
        float width = controlsDefaultWidth + controlsWidthPerUnit * unitCount;
        float yPosition = controlsBackground.anchoredPosition.y;
        
        Rect rect = controlsBackground.rect;
        rect.Set(rect.x, rect.y, width, rect.height);
        controlsBackground.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, width);
        
        Vector2 anchors = new Vector2(0.5f, 0);
        controlsBackground.anchorMax = anchors;
        controlsBackground.anchorMin = anchors;
        controlsBackground.anchoredPosition = new Vector2(0, yPosition);
        
        nextUnitButton.anchoredPosition = new Vector2(width / 2 - controlsButtonOffset, yPosition);
        prevUnitButton.anchoredPosition = new Vector2(-(width / 2 - controlsButtonOffset), yPosition);
        
        // Set unit selections
        float a = unitCount % 2 == 0 ? unitCount / 2 - 0.5f : (unitCount - 1) / 2;
        selections = new List<Image>();
        for (float i = -a; i <= a; i += 1)
        {
            Image unitSelection = Instantiate(unitSelectionImage, controlsBackground.parent);
            
            RectTransform selectionTransform = unitSelection.rectTransform;
            selectionTransform.anchorMax = anchors;
            selectionTransform.anchorMin = anchors;
            selectionTransform.anchoredPosition = new Vector2(controlsWidthPerUnit * i, yPosition);

            int id = selections.Count;
            unitSelection.GetComponent<Button>().onClick.AddListener(() => OpenUnit(id));
            
            selections.Add(unitSelection);
        }

        selections[currentUnitId].color = selectedUnitColor;
    }

    public void SetUpUnit(ObjectInfoUnitData unitData)
    {
        if (loadingTextFade.gameObject.activeSelf)
            loadingTextFade.FadeOut();
        
        unitNameText.gameObject.SetActive(true);
        unitNameText.text = unitData.unitName;
        isPlaying = false;
        playButtonImage.sprite = playSprite;
        
        if (currentRunningRoutine != null)
            StopCoroutine(currentRunningRoutine);
        currentRunningRoutine = StartCoroutine(WaitAndPlay(delayBeforePlay));
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
        if (currentRunningRoutine != null)
            StopCoroutine(currentRunningRoutine);
        
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

    public void AudioFinished(string nextUnitName)
    {
        playButtonImage.sprite = playSprite;
        isPlaying = false;

        if (!string.IsNullOrWhiteSpace(nextUnitName))
        {
            loadingTextFade.gameObject.SetActive(true);
            loadingTextFade.FadeIn();
            loadingText.text = nextUnitName;
            unitNameText.gameObject.SetActive(false);
        }
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
    }
}