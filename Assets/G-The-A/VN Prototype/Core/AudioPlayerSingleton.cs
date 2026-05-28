using UnityEngine;

public class AudioPlayerSingleton : MonoBehaviour
{
    public static AudioPlayerSingleton Instance;
    
    [SerializeField]
    public AudioSource audioSourceSFX;
    
    [SerializeField]
    public AudioSource audioSourceMusic;
    
    private void Awake()
    {
        if(Instance == null && Instance != this)
            Instance = this;
    }

    #region SFX
    public static void PlayOneshotSFX(AudioClip clip)
    {
        if(Instance.audioSourceSFX != null)
        {
            Instance.audioSourceSFX.PlayOneShot(clip);
        }
    }

    public static void PlaySFX(AudioClip clip)
    {
        if (Instance.audioSourceSFX != null)
        {
            Instance.audioSourceSFX.PlayOneShot(clip);
        }
    }
    #endregion

    #region Music
    public static void PlayMusic(AudioClip clip)
    {
        if (Instance.audioSourceMusic != null)
        {
            Instance.audioSourceMusic.clip = clip;
            Instance.audioSourceMusic.Play();
        }
    }
    public static void PauseMusic()
    {
        if (Instance.audioSourceMusic != null)
        {
            Instance.audioSourceMusic.Pause();
        }
    }
    public static void UnPauseMusic()
    {
        if (Instance.audioSourceMusic != null)
        {
            Instance.audioSourceMusic.UnPause();
        }
    }
    public static void StopMusic()
    {
        if (Instance.audioSourceMusic != null)
        {
            Instance.audioSourceMusic.Stop();
        }
    }
    public static void SetMusicLoop(bool isLooped)
    {
        Instance.audioSourceMusic.loop = isLooped;
    }
    #endregion


}
