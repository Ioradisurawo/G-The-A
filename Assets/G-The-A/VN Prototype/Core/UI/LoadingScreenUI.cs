using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement loadingScreen;
    public string loadingScreenName = "loading-screen";
    private Label labelCurrentScene;
    public string labelCurrentSceneName = "scene-name";

    [SerializeField]
    private AudioClip loadingSfx;

    private void Awake()
    {
        SceneControllerSingleton.OnSceneLoadingFinished += Show;
        SceneControllerSingleton.OnSceneLoadingFinished += Hide;
    }
    private void OnDestroy()
    {
        SceneControllerSingleton.OnSceneLoadingStarted  -= Show;
        SceneControllerSingleton.OnSceneLoadingFinished -= Hide;
    }
    private void Start()
    {
        var root = uiDocument.rootVisualElement;

        loadingScreen = root.Q<VisualElement>(loadingScreenName);

        labelCurrentScene = root.Q<Label>(labelCurrentSceneName);
        if (labelCurrentScene == null)
            Debug.Log("Couldn't find mane I dunno");
    }
    void Show()
    {
        if (SceneControllerSingleton.Instance != null)
            labelCurrentScene.text = SceneControllerSingleton.GetCurrentSceneName();

        if(AudioPlayerSingleton.Instance != null)
        {
            AudioPlayerSingleton.SetMusicLoop(true);
            
            if(loadingSfx != null)
                AudioPlayerSingleton.PlayMusic(loadingSfx);
        }

        if(loadingScreen != null)
        {
            loadingScreen.visible = true;
            loadingScreen.SetEnabled(true);
        }

    }
    void Hide()
    {
        if (AudioPlayerSingleton.Instance != null)
        {
            AudioPlayerSingleton.SetMusicLoop(false);

            if (loadingSfx != null)
                AudioPlayerSingleton.StopMusic();
        }
        
        if (loadingScreen != null)
        {
            loadingScreen.SetEnabled(false);
            StartCoroutine(WaitThenDisplay(0.8f));
        }

    }

    public IEnumerator WaitThenDisplay(float time)
    {
        yield return new WaitForSeconds(time);
        loadingScreen.visible = false;
    }
}
