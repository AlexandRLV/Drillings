using System.Collections;
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
            UIManager.AudioFinished(LayoutData.IsLastUnit, LayoutData.NextUnitName);
            return;
        }

        if (currentRoutine == null)
            currentRoutine = StartCoroutine(WaitAndPlayNextUnitVoice(LayoutData.delayBetweenVoicesInUnit, unit));
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
        
        miniatureAnimation.Play();
        objectBackingAnimation.Play();
        voiceSource.Play();

        IsPlaying = true;
        
        SetUpUnitWorldContent(LayoutData.CurrentUnit);
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
        
        DisableUnitWorldContent();
    }

    public void SetUpUnit()
    {
        ObjectInfoUnitData unit = LayoutData.CurrentUnit;
        
        miniatureAnimation.clip = unit.miniatureAnim;
        objectBackingAnimation.clip = unit.objectAnim;
        voiceSource.clip = unit.voices[0];
        currentUnitVoiceId = 0;
        
        //SetUpUnitWorldContent(unit);
        
        UIManager.UpdateUnit(unit);
    }

    
    
    
    private void SetUpUnitWorldContent(ObjectInfoUnitData unit)
    {
        if (unit.infoImage != null)
            layoutUI.SetUpImage(unit.infoImage);
        else
            layoutUI.SetUpText(unit.infoTextStrings[0]);

        if (unit.pointerReferenceId > 0)
            layoutUI.EnablePointer(referencePoints[unit.pointerReferenceId - 1]);
        else
            layoutUI.DisablePointer();
    }

    private void DisableUnitWorldContent()
    {
        layoutUI.DisableInfo();
        layoutUI.DisablePointer();
    }

    private IEnumerator WaitAndPlayNextUnitVoice(float time, ObjectInfoUnitData unit)
    {
        yield return new WaitForSeconds(time);
        
        currentUnitVoiceId++;
        voiceSource.clip = unit.voices[currentUnitVoiceId];
        layoutUI.SetUpText(unit.infoTextStrings[currentUnitVoiceId]);
        
        voiceSource.Play();
        IsPlaying = true;

        currentRoutine = null;
    }
}