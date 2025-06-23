using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music & Ambience")]
    public AudioSource music1;
    public AudioSource music2;
    public AudioSource ambience;

    [Header("SFX Sources")]
    public AudioSource sfxSource;           // For one-shot SFX
    public AudioSource loopingSFXSource;    // For looping footstep

    [Header("SFX Clips")]
    public AudioClip runLoopClip;
    public AudioClip fallClip;
    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip checkpointClip;
    public AudioClip runeCollectClip;
    public AudioClip runeInsertClip;
    public AudioClip healthRestoreClip;
    public AudioClip respawnClip;
    public AudioClip clickPromptClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void StartRunLoop()
    {
        if (loopingSFXSource != null && !loopingSFXSource.isPlaying)
        {
            loopingSFXSource.clip = runLoopClip;
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

    public void PlayMusic2AndAmbience()
    {
        music1.Stop();
        music2.Play();
        ambience.Play();
    }
}
