using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Drillings.Data;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public LayoutController CurrentLayout { get; private set; }
    public string CurrentScene { get; private set; }

	[SerializeField] private float noInputReloadTime;
	
    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Compass compass;

    [Header("Layouts")]
    [SerializeField] private List<LayoutDataContainer> scenesLayoutDataAssets;

	private float timer;
    private Coroutine currentRunningRoutine;

	
	
	private void Update()
	{
		#if UNITY_EDITOR
		return;
		#endif
		
		if (CurrentLayout == null || CurrentLayout.IsPlaying || currentRunningRoutine != null)
			return;
		
		if (Input.touchCount > 0)
			timer = noInputReloadTime;
		
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			compass.StopFollow();
		}
	}

    

    public void ActivateLayout(string sceneName, Transform targetTransform)
    {
        if (CurrentLayout != null || currentRunningRoutine != null || scenesLayoutDataAssets.All(x => x.sceneName != sceneName))
            return;
        
		timer = noInputReloadTime;
        uiManager.StartLoadingAnimation();
        CurrentScene = sceneName;
        currentRunningRoutine = StartCoroutine(LoadScene(targetTransform));
    }
    
    public bool DeactivateCurrentLayout()
    {
        if (CurrentLayout == null || currentRunningRoutine != null)
            return false;
        
        CurrentLayout.Stop();
        CurrentLayout.transform.parent = null;
        SceneManager.MoveGameObjectToScene(CurrentLayout.gameObject, SceneManager.GetSceneByName(CurrentScene));
        uiManager.DisableControlElements();
        currentRunningRoutine = StartCoroutine(UnloadScene());
        return true;
    }

    public void ShowLoadedLayout()
    {
        CurrentLayout.gameObject.SetActive(true);
        SetUpLayoutData();
    }
    


    private void UpdateRootTransform(Transform targetTransform)
    {
        if (CurrentLayout == null)
            return;

        Transform rootTransform = CurrentLayout.transform;
        rootTransform.SetParent(targetTransform);
        rootTransform.localPosition = Vector3.zero;
        rootTransform.localRotation = Quaternion.identity;
    }

    private void SetUpLayoutData()
    {
        CurrentLayout.LayoutData = scenesLayoutDataAssets.Single(x => x.sceneName == CurrentScene).data;
        CurrentLayout.LayoutData.ResetUnit();
        uiManager.EnableControlElements(CurrentLayout.LayoutData);
        CurrentLayout.SetUpUnit();
        compass.StartFollow(CurrentLayout.ObjectTransform);
    }

    private IEnumerator LoadScene(Transform targetTransform)
    {
		Debug.Log("Started loading scene");
        AsyncOperation loading = SceneManager.LoadSceneAsync(CurrentScene, LoadSceneMode.Additive);
        while (!loading.isDone)
			yield return null;

		Debug.Log("Loaded scene");
        CurrentLayout = FindObjectOfType<LayoutController>();
		Debug.Log("Found LayoutController");
        CurrentLayout.UIManager = uiManager;
        CurrentLayout.gameObject.SetActive(false);
        uiManager.StopLoadingAnimation();
        UpdateRootTransform(targetTransform);
        currentRunningRoutine = null;
    }

    private IEnumerator UnloadScene()
    {
        AsyncOperation unloading = SceneManager.UnloadSceneAsync(CurrentScene);
        while (!unloading.isDone)
            yield return new WaitForEndOfFrame();

        Resources.UnloadUnusedAssets();
        CurrentLayout = null;
        CurrentScene = null;
        currentRunningRoutine = null;
    }
}

[Serializable]
public class LayoutDataContainer
{
    public string sceneName;
    public LayoutData data;
}