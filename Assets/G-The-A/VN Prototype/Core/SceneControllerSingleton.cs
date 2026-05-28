using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerSingleton : MonoBehaviour
{
    #region Fields
    public static SceneControllerSingleton Instance;

    public static Action OnSceneLoadingStarted;
    public static Action OnSceneLoadingFinished;

    public static string currentSceneName;
    public static bool isBusy;

    [SerializeField]
    private string firstScene;
    #endregion
    
    #region Init
    private void Awake()
    {
        isBusy = false;
     
        if (Instance == null && Instance != this)
            Instance = this;
    }
    private void Start()
    {
        if(firstScene != "")
            LoadSceneAdditive(firstScene);
    }
    #endregion
    public static void LoadSceneAdditive(string sceneName)
    {
        if (isBusy)
        {
            Debug.LogWarning("SceneController is busy!");
            return;
        }
        Instance.StartCoroutine(LoadSceneProcess(sceneName, LoadSceneMode.Additive));
    }
    public static string GetCurrentSceneName()
    {
        return currentSceneName;
    }
    private static IEnumerator LoadSceneProcess(string sceneName, LoadSceneMode loadSceneMode)
    {
        isBusy = true;

        OnSceneLoadingStarted?.Invoke();

        yield return new WaitForSeconds(0.5f);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects); // UnloadEmbedded because we won't need em

        currentSceneName = sceneName;

        var process = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        while (process.isDone != true)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        OnSceneLoadingFinished?.Invoke();
        isBusy = false;
    }
    public static void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
            Debug.Break();
        #endif
    }
}
