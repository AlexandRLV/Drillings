using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeManager : MonoBehaviour
{
    [SerializeField] private Graphic[] targetGraphics;
    
    private Coroutine currentRunningRoutine;
    
    
    
    public void FadeIn()
    {
        if (currentRunningRoutine != null)
        {
            StopCoroutine(currentRunningRoutine);
        }
        
        currentRunningRoutine = StartCoroutine(FadeIn(UIManager.fadeInOutTime));
    }

    public void FadeOut()
    {
        if (currentRunningRoutine != null)
        {
            StopCoroutine(currentRunningRoutine);
        }

        currentRunningRoutine = StartCoroutine(FadeOut(UIManager.fadeInOutTime));
    }



    private IEnumerator FadeIn(float fadeInTime)
    {
        float timer = 0;
        while (timer < fadeInTime)
        {
            float t = timer / fadeInTime;
            foreach (Graphic graphic in targetGraphics)
            {
                graphic.color = new Color(1, 1, 1, t);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Graphic graphic in targetGraphics)
        {
            graphic.color = Color.white;
        }
    }

    private IEnumerator FadeOut(float fadeOutTime)
    {
        float timer = 0;
        while (timer < fadeOutTime)
        {
            float t = timer / fadeOutTime;
            foreach (Graphic graphic in targetGraphics)
            {
                graphic.color = new Color(1, 1, 1, 1 - t);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Graphic graphic in targetGraphics)
        {
            graphic.color = Color.white;
        }
        
        gameObject.SetActive(false);
    }
}