using System;
using System.Collections;
using UnityEngine;
using Data;
using UI;

public class LayoutController : MonoBehaviour
{
    public event Action AudioFinished;
    public LayoutData LayoutData { get; set; }
    public bool IsPlaying { get; private set; }
    public Transform ObjectTransform => objectAnimation.transform;
	
    [Header("References")]
    [SerializeField] private Animation miniatureAnimation;
    [SerializeField] private Animation objectAnimation;
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
            AudioFinished?.Invoke();
            Stop();
            return;
        }

        if (currentRoutine == null)
            currentRoutine = StartCoroutine(WaitAndPlayNextUnitVoice(unit.delayBetweenVoices, unit));
    }

    public ObjectInfoUnitData OpenUnit(int unitId)
    {
        Stop();
        
        if (!LayoutData.OpenUnit(unitId)) 
            return null;
        
        SetUpUnit();
        return LayoutData.CurrentUnit;
    }

    public void Play()
    {
        voiceSource.clip = LayoutData.CurrentUnit.voices[0];
        currentUnitVoiceId = 0;
        
        SetUpUnitContent(LayoutData.CurrentUnit);
        
		if (miniatureAnimation.clip != null)
			miniatureAnimation.Play();
		if (objectAnimation.clip != null)
			objectAnimation.Play();
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
        objectAnimation.Stop();
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
    }

    
    
    
    private void SetUpUnitContent(ObjectInfoUnitData unit)
    {
		// UI
        if (unit.infoImage != null)
            layoutUI.SetUpImage(unit.infoImage);
        else
            layoutUI.SetUpText(unit.infoTextStrings[0]);

        
        if (unit.pointerReferenceIds.Length > 0)
        {
            if (unit.pointerReferenceIds[0] > 0)
                layoutUI.EnablePointer(referencePoints[unit.pointerReferenceIds[0] - 1]);
        }
        else
            layoutUI.DisablePointer();
        
        
        if (unit.photos.Length > 0)
        {
            PhotoContainer photoContainer = unit.photos[0];
            if (photoContainer.photo != null)
                layoutUI.SetUpPhoto(photoContainer.photo, photoContainer.aspectRatio);
        }
        
        
        if (unit.video.clip != null)
            layoutUI.SetUpVideo(unit.video.clipId, unit.video.aspectRatio, unit.video.renderTexture);
        
        
        if (unit.sizes != null && unit.sizes.Length > 0)
            layoutUI.EnableArrows(unit.sizes[0], unit.sizes[1], unit.sizes[2]);
        else
            layoutUI.DisableArrows();
		
		
		// animations
		miniatureAnimation.clip = unit.miniatureAnim != null ? unit.miniatureAnim : emptyMiniature;
		
		objectAnimation.clip = unit.objectAnim != null ? unit.objectAnim : emptyObject;
    }

    private void DisableUnitContent()
    {
        layoutUI.DisableInfo();
        layoutUI.DisablePointer();
		layoutUI.DisableArrows();
		
		miniatureAnimation.clip = emptyMiniature;
		if (emptyMiniature != null)
			miniatureAnimation.Play();
		
		objectAnimation.clip = emptyObject;
		if (emptyObject != null)
			objectAnimation.Play();
    }

    private IEnumerator WaitAndPlayNextUnitVoice(float time, ObjectInfoUnitData unit)
    {
        yield return new WaitForSeconds(time);
        
        currentUnitVoiceId++;
        voiceSource.clip = unit.voices[currentUnitVoiceId];
        layoutUI.SetUpText(unit.infoTextStrings[currentUnitVoiceId]);
        layoutUI.DisableArrows();
        
        
        if (unit.pointerReferenceIds.Length > 1)
        {
            if (unit.pointerReferenceIds[currentUnitVoiceId] > 0)
                layoutUI.EnablePointer(referencePoints[unit.pointerReferenceIds[currentUnitVoiceId] - 1]);
            else
                layoutUI.DisablePointer();
        }
        else
        {
            layoutUI.DisablePointer();
        }
        
        
        if (unit.photos.Length > 1)
        {
            PhotoContainer photoContainer = unit.photos[currentUnitVoiceId];
            if (photoContainer.photo != null)
                layoutUI.SetUpPhoto(photoContainer.photo, photoContainer.aspectRatio);
        }
        
        
        voiceSource.Play();
        IsPlaying = true;

        currentRoutine = null;
    }
}