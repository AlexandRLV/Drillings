using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public LayoutController CurrentLayout { get; private set; }
    public string CurrentScene { get; private set; }

    [Header("References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Compass compass;

    [Header("Layouts")]
    [SerializeField] private List<LayoutDataContainer> scenesLayoutDataAssets;

    private Coroutine currentRunningRoutine;


    // private void Start()
    // {
    //     foreach (LayoutDataContainer layoutData in scenesLayoutDataAssets)
    //     {
    //         layoutData.data.ResetUnit();
    //     }
    // }

    

    public void ActivateLayout(string sceneName, Transform targetTransform)
    {
        if (CurrentLayout != null || currentRunningRoutine != null || scenesLayoutDataAssets.All(x => x.sceneName != sceneName))
            return;
        
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
        AsyncOperation loading = SceneManager.LoadSceneAsync(CurrentScene, LoadSceneMode.Additive);
        while (!loading.isDone)
            yield return new WaitForEndOfFrame();

        CurrentLayout = FindObjectOfType<LayoutController>();
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