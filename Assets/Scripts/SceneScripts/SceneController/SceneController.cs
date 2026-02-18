using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] private LoadingScreenOverlay loadingScreen;

    //slotok a scenek rendezettségéhez
    private Dictionary<string, string> loadedSceneBySlot = new();

    private bool alreadyInScene = false;

    // API (egyetlen public method ami kell)

    public SceneTransitionPlan NewTransition()
    {
        return new SceneTransitionPlan();
    }

    //Implementáció

    private Coroutine ExecutePlan(SceneTransitionPlan plan)
    {
        if (alreadyInScene)
        {
            Debug.LogWarning("Scene change already in progress.");
            return null;
        }
        alreadyInScene = true;
        return StartCoroutine(ChangeSceneRoutine(plan));
    }

    private IEnumerator ChangeSceneRoutine(SceneTransitionPlan plan)
    {
        if (plan.LoadingScreenOverlay)
        {
            yield return loadingScreen.FadeINBlack();
            yield return new WaitForSeconds(0.5f);
        }

        foreach (var slotKey in plan.ScenesToUnload)
        {
            yield return UnloadSceneRoutine(slotKey);
        }

        if (plan.ClearUnusedAssets)
        {
            yield return ClearUnusuedAssetsRoutine();
        }

        foreach (var kvp in plan.ScenesToLoad)
        {
            if (loadedSceneBySlot.ContainsKey(kvp.Key))
            {
                yield return UnloadSceneRoutine(kvp.Key);
            }
            yield return LoadAdditiveRoutine(kvp.Key,kvp.Value,plan.ActiveSceneName == kvp.Value);
        }
        if (plan.LoadingScreenOverlay)
        {
            yield return loadingScreen.FadeOUTBlack();
        }
        alreadyInScene = false;
    }

    private IEnumerator LoadAdditiveRoutine(string slotKey, string sceneName, bool setActive)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOp == null)
        {
            yield break;
        }
        loadOp.allowSceneActivation = false;
        //várakozás hogy majdnem befejezze már
        while (loadOp.progress < 0.9f)
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
        loadedSceneBySlot[slotKey] = sceneName;
    }
    private IEnumerator UnloadSceneRoutine(string slotKey)
    {
        if (!loadedSceneBySlot.TryGetValue(slotKey,out string sceneName)) 
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
        loadedSceneBySlot.Remove(slotKey);
    }

    private IEnumerator ClearUnusuedAssetsRoutine()
    {
        AsyncOperation cleanupOp = Resources.UnloadUnusedAssets();
        while (cleanupOp.isDone)
        {
            yield return null;
        }
    }


    /*
    Scene transition plan class:
    összes adat ami a scene transitionhöz kell
    -Melyiket kell load és melyiket unload
    -Aktív scene neve
    -Kitakarítani a nem felhasznált elemeket
    -Használni a LoadingScreent
    */
    public class SceneTransitionPlan
    {
        public Dictionary<string, string> ScenesToLoad { get; } = new();
        public List<string> ScenesToUnload { get; } = new();
        public string ActiveSceneName { get; private set; } = "";
        public bool ClearUnusedAssets { get; private set; } = false;
        public bool LoadingScreenOverlay { get; private set; } = false;

        public SceneTransitionPlan Load(string slotKey,string sceneName,bool setActive = false)
        {
            ScenesToLoad[slotKey] = sceneName;
            if (setActive)
            {
                ActiveSceneName = sceneName;
            }
            return this;
        }

        public SceneTransitionPlan UnLoad(string slotKey)
        {
            ScenesToUnload.Add(slotKey);
            return this;
        }

        public SceneTransitionPlan WithLoading()
        {
            LoadingScreenOverlay = true;
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
