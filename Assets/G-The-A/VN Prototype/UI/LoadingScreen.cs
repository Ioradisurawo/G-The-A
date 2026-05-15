using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    private VisualElement loadingScreen;

    [SerializeField]
    private AudioClip loadingSfx;

    private void Awake()
    {
        SceneControllerSingleton.OnSceneLoadingStarted += Show;
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

        loadingScreen = root.Q<VisualElement>("loading-screen");

        //Hide();
    }
    void Show()
    {
        if(AudioPlayerSingleton.Instance != null)
        {
            AudioPlayerSingleton.SetMusicLoop(true);
            
            if(loadingSfx != null)
                AudioPlayerSingleton.PlayMusic(loadingSfx);
        }

        if(loadingScreen != null)
            loadingScreen.SetEnabled(true);
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
            loadingScreen.SetEnabled(false);
    }
}
