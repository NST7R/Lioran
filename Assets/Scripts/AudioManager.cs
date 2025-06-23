using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("AudioSources")]
    public AudioSource musicSource1;
    public AudioSource musicSource2;       // second music for scene 3 (end game)
    public AudioSource ambienceSource1;
    public AudioSource ambienceSource2;    // second ambience for scene 3 (end game)

    [Header("SFX Sources")]
    public AudioSource sfxSource;
    public AudioSource loopingSFXSource;   // for run loop sounds

    [Header("Music Clips")]
    public AudioClip mainMenuMusicClip;

    public AudioClip caveMusicClip;         // Scene 2
    public AudioClip forestMusicClip1;      // Scene 3 first music
    public AudioClip forestMusicClip2;      // Scene 3 second music (end game)

    [Header("Ambience Clips")]
    public AudioClip mainMenuAmbienceClip;

    public AudioClip caveAmbienceClip;      // Scene 2
    public AudioClip forestAmbienceClip1;   // Scene 3 first ambience (start)
    public AudioClip forestAmbienceClip2;   // Scene 3 second ambience (end game)
    
    [Header("Run Loop Clips (different per scene)")]
    public AudioClip runLoopClipMainMenu;  // probably null or no run sound in menu
    public AudioClip runLoopClipCave;      // Scene 2 run sound
    public AudioClip runLoopClipForest;    // Scene 3 run sound

    [Header("UI SFX Clips")]
    public AudioClip uiClickClip;
    public AudioClip uiHoverClip;

    [Header("Other SFX Clips")]
    public AudioClip fallClip;
    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip checkpointClip;
    public AudioClip runeCollectClip;
    public AudioClip runeInsertClip;
    public AudioClip healthRestoreClip;
    public AudioClip respawnClip;

    private AudioClip currentRunLoopClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllSounds();

        switch (scene.name)
        {
            case "MainMenu":
                SetupMusicAndAmbience(mainMenuMusicClip, mainMenuAmbienceClip);
                currentRunLoopClip = runLoopClipMainMenu;
                break;

            case "TheForgottenDephth_GA": // Cave Scene (Scene 2)
                SetupMusicAndAmbience(caveMusicClip, caveAmbienceClip);
                currentRunLoopClip = runLoopClipCave;
                break;

            case "TheSilentGlade_GA": // Forest Scene (Scene 3)
                SetupMusicAndAmbience(forestMusicClip1, forestAmbienceClip1);
                currentRunLoopClip = runLoopClipForest;
                // musicSource2 and ambienceSource2 start stopped, only play after end game
                break;

            default:
                Debug.LogWarning("AudioManager: Unknown scene, stopping all sounds.");
                currentRunLoopClip = null;
                break;
        }
    }

    private void SetupMusicAndAmbience(AudioClip musicClip, AudioClip ambienceClip)
    {
        musicSource1.clip = musicClip;
        musicSource1.loop = true;
        musicSource1.Play();

        ambienceSource1.clip = ambienceClip;
        ambienceSource1.loop = true;
        ambienceSource1.Play();

        musicSource2.Stop();
        ambienceSource2.Stop();
    }

    // Call this when player reaches the end of game in forest scene
    public void PlayEndGameMusicAndAmbience()
    {
        if (SceneManager.GetActiveScene().name == "TheSilentGlade_GA")
        {
            musicSource1.Stop();

            musicSource2.clip = forestMusicClip2;
            musicSource2.loop = true;
            musicSource2.Play();

            ambienceSource1.Stop();
            ambienceSource1.clip = forestAmbienceClip2;
            ambienceSource1.loop = true;
            ambienceSource1.Play();

            ambienceSource2.clip = forestAmbienceClip2;
            ambienceSource2.loop = true;
            ambienceSource2.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void StartRunLoop()
    {
        if (loopingSFXSource != null && !loopingSFXSource.isPlaying && currentRunLoopClip != null)
        {
            loopingSFXSource.clip = currentRunLoopClip;
            loopingSFXSource.loop = true;
            loopingSFXSource.Play();
        }
    }

    public void StopRunLoop()
    {
        if (loopingSFXSource != null && loopingSFXSource.isPlaying)
        {
            loopingSFXSource.Stop();
        }
    }

    // Helper functions to play UI sounds
    public void PlayUIClick()
    {
        PlaySFX(uiClickClip);
    }

    public void PlayUIHover()
    {
        PlaySFX(uiHoverClip);
    }

    public void StopAllSounds()
    {
        musicSource1.Stop();
        musicSource2.Stop();
        ambienceSource1.Stop();
        ambienceSource2.Stop();
        loopingSFXSource.Stop();
    }
    
}
