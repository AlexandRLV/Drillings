using System.Collections;
using AssetVariables;
using Data;
using Drillings.Data;
using UI;
using UnityEngine;

public class LayoutController : MonoBehaviour
{
    public UIManager UIManager { get; set; }
    public LayoutData LayoutData { get; set; }
    public bool IsPlaying { get; private set; }
    public Transform ObjectTransform => objectBackingAnimation.transform;
	
    [Header("References")]
    [SerializeField] private Animation miniatureAnimation;
    [SerializeField] private Animation objectBackingAnimation;
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private LayoutWorldUI layoutUI;
    [SerializeField] private Transform[] referencePoints;
	
	[Header("Assets")]
	[SerializeField] private AnimationClip emptyMiniature;
	[SerializeField] private AnimationClip emptyObject;
    
    private int currentUnitVoiceId;
    private Coroutine currentRoutine;

    
    
    private void Update()
    {
        if (!IsPlaying)
            return;

        //elapsedTime += Time.deltaTime;
        
        if (voiceSource.isPlaying)
            return;

        ObjectInfoUnitData unit = LayoutData.CurrentUnit;
        
        if (currentUnitVoiceId >= unit.voices.Length - 1)
        {
            Stop();
            UIManager.AudioFinished(LayoutData.IsLastUnit);
            return;
        }

        if (currentRoutine == null)
            currentRoutine = StartCoroutine(WaitAndPlayNextUnitVoice(unit.delayBetweenVoices, unit));
    }


    public void NextUnit()
    {
        Stop();
        if (LayoutData.NextUnit())
            SetUpUnit();
    }

    public void PrevUnit()
    {
        Stop();
        if (LayoutData.PrevUnit())
            SetUpUnit();
    }

    public void OpenUnit(int unitId)
    {
        Stop();
        if (LayoutData.OpenUnit(unitId))
            SetUpUnit();
    }

    public void Play()
    {
        voiceSource.clip = LayoutData.CurrentUnit.voices[0];
        currentUnitVoiceId = 0;
        
        SetUpUnitContent(LayoutData.CurrentUnit);
        
		if (miniatureAnimation.clip != null)
			miniatureAnimation.Play();
		if (objectBackingAnimation.clip != null)
			objectBackingAnimation.Play();
        voiceSource.Play();

        IsPlaying = true;
    }

    public void Stop()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
        
        miniatureAnimation.Stop();
        objectBackingAnimation.Stop();
        voiceSource.Stop();

        IsPlaying = false;
        
        DisableUnitContent();
    }

    public void SetUpUnit()
    {
        ObjectInfoUnitData unit = LayoutData.CurrentUnit;
		
        voiceSource.clip = unit.voices[0];
        currentUnitVoiceId = 0;
        
        DisableUnitContent();
        
        UIManager.UpdateUnit(unit);
    }

    
    
    
    private void SetUpUnitContent(ObjectInfoUnitData unit)
    {
		// UI
        if (unit.infoImage != null)
            layoutUI.SetUpImage(unit.infoImage);
        else
            layoutUI.SetUpText(unit.infoTextStrings[0]);

        if (unit.pointerReferenceId > 0)
            layoutUI.EnablePointer(referencePoints[unit.pointerReferenceId - 1]);
        else
            layoutUI.DisablePointer();
        
        if (unit.sizes != null && unit.sizes.Length > 0)
            layoutUI.EnableArrows(unit.sizes[0], unit.sizes[1], unit.sizes[2]);
        else
            layoutUI.DisableArrows();
		
		
		// animations
		miniatureAnimation.clip = unit.miniatureAnim != null ? unit.miniatureAnim : emptyMiniature;
		
		objectBackingAnimation.clip = unit.objectAnim != null ? unit.objectAnim : emptyObject;
    }

    private void DisableUnitContent()
    {
        layoutUI.DisableInfo();
        layoutUI.DisablePointer();
		layoutUI.DisableArrows();
		
		miniatureAnimation.clip = emptyMiniature;
		if (emptyMiniature != null)
			miniatureAnimation.Play();
		
		objectBackingAnimation.clip = emptyObject;
		if (emptyObject != null)
			objectBackingAnimation.Play();
    }

    private IEnumerator WaitAndPlayNextUnitVoice(float time, ObjectInfoUnitData unit)
    {
        yield return new WaitForSeconds(time);
        
        currentUnitVoiceId++;
        voiceSource.clip = unit.voices[currentUnitVoiceId];
        layoutUI.SetUpText(unit.infoTextStrings[currentUnitVoiceId]);
        layoutUI.DisableArrows();
        
        voiceSource.Play();
        IsPlaying = true;

        currentRoutine = null;
    }
}