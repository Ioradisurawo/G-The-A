using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VectorGraphics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerSingleton : MonoBehaviour
{
    public static SceneControllerSingleton Instance;

    public static Action OnSceneLoadingStarted;
    public static Action OnSceneLoadingFinished;

    public static string currentSceneName;
    public static bool isBusy;

    [SerializeField]
    private SceneAsset firstScene;
    private void Awake()
    {
        isBusy = false;
     
        if (Instance == null && Instance != this)
            Instance = this;
    }
    private void Start()
    {
        LoadSceneAdditive(firstScene);
    }
    public static void LoadSceneAdditive(SceneAsset sceneName)
    {
        if (isBusy)
        {
            Debug.LogWarning("SceneController is busy!");
            return;
        }

        Instance.StartCoroutine(LoadSceneProcess(sceneName));
    }
    private static IEnumerator LoadSceneProcess(SceneAsset sceneName)
    {
        isBusy = true;

        OnSceneLoadingStarted?.Invoke();

        yield return new WaitForSeconds(0.5f);

        if (currentSceneName != null)
            SceneManager.UnloadSceneAsync(currentSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects); // UnloadEmbedded because we won't need em

        currentSceneName = sceneName.name;

        var process = SceneManager.LoadSceneAsync(sceneName.name, LoadSceneMode.Additive);
        while (process.isDone != true)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        OnSceneLoadingFinished?.Invoke();
        isBusy = false;
    }
}
