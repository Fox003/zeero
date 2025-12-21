using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Singleton

    public static SceneController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion

    [SerializeField] private LoadingOverlay _loadingOverlay;

    private Dictionary<string, string> _loadedSceneBySlot = new();

    private bool _isBusy = false;

    public SceneTransitionPlan NewTransition()
    {
        return new SceneTransitionPlan();
    }

    private Coroutine ExecutePlan(SceneTransitionPlan plan)
    {
		Debug.Log("Executing plan");
        if (_isBusy)
        {
            Debug.LogWarning("Scene controller is busy");
            return null;
        }

        _isBusy = true;
        return StartCoroutine(ChangeSceneRoutine(plan));
    }

    private IEnumerator ChangeSceneRoutine(SceneTransitionPlan plan)
    {
		Debug.Log("Changing scene");
        if (plan.Overlay)
        {
            yield return _loadingOverlay.FadeInBlack();
            yield return new WaitForSeconds(0.5f);
        }

        foreach (var slotkey in plan.ScenesToUnload)
        {
            yield return UnloadSceneRoutine(slotkey); 
        }

        if (plan.ClearUnusedAssets) yield return CleanupUnusedAssetsRoutine();

        foreach (var kvp in plan.ScenesToLoad)
        {
            if (_loadedSceneBySlot.ContainsKey(kvp.Key))
            {
                yield return UnloadSceneRoutine(kvp.Key);
            }
            yield return LoadAdditiveRoutine(kvp.Key, kvp.Value, plan.ActiveSceneName == kvp.Value);
        }

        if (plan.Overlay)
        {
            yield return _loadingOverlay.FadeOutBlack();
        }

        _isBusy = false;
    }

    private IEnumerator LoadAdditiveRoutine(string slotKey, string sceneName, bool setActive)
    {
		Debug.Log("Loadig additive");
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOp.progress < 0.9f)
        {
            yield return null;
        }
        
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone)
        {
            yield return null;
        }

        if (setActive)
        {
            Scene newScene = SceneManager.GetSceneByName(sceneName);
            if (newScene.IsValid() && newScene.isLoaded)
            {
                SceneManager.SetActiveScene(newScene);
            }
        }
        
        _loadedSceneBySlot[slotKey] = sceneName;
    }
    
    private IEnumerator UnloadSceneRoutine(string slotKey) 
    {
		Debug.Log("Unloading scene");
        if (!_loadedSceneBySlot.TryGetValue(slotKey, out string sceneName))
        {
            yield break;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            yield break;
        }
        
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        if (unloadOp != null)
        {
            while (!unloadOp.isDone)
            {
                yield return null;
            }
        }
        _loadedSceneBySlot.Remove(slotKey);
    }

    private IEnumerator CleanupUnusedAssetsRoutine()
    {
        AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
        
        while (!cleanupOp.isDone)
        {
            yield return null;
        }
    }

public class SceneTransitionPlan
    {
        public Dictionary<string, string> ScenesToLoad = new();
        public List<string> ScenesToUnload = new();
        public string ActiveSceneName { get; private set; } = "";
        public bool ClearUnusedAssets { get; private set; } = false;
        public bool Overlay { get; private set; } = false;
        
        public SceneTransitionPlan Load(string slotKey, string sceneName, bool setActive = false)
        {
            ScenesToLoad[slotKey] = sceneName;
            if (setActive) ActiveSceneName = sceneName;
            return this;
        }
        
        public SceneTransitionPlan Unload(string slotKey)
        {
            ScenesToUnload.Add(slotKey);
            return this;
        }
        
        public SceneTransitionPlan WithOverlay()
        {
            Overlay = true;
            return this;
        }
        
        public SceneTransitionPlan WithClearUnusedAssets() 
        {
            ClearUnusedAssets = true;
            return this;
        }
        
        public Coroutine Perform()
        {
            return SceneController.Instance.ExecutePlan(this);
        }
    }
}
