using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource; // Background music
    public AudioSource sfxSource; // Sound effects

    [Header("Sound Effects")]
    public AudioClip cardFlipClip;
    public AudioClip levelCompleteClip;
    public AudioClip cardRevealClip;
    public AudioClip gameRestartClip;
    public AudioClip buttonClickClip;
    public AudioClip comboClip; 
    public AudioClip matchSuccessClip;
    public AudioClip missmatchClip; 
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent); // Keep the AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    // Play background music
    public void PlayBackgroundMusic()
    {
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Play();
            backgroundMusicSource.loop = true;
        }
    }

    // Play sound effects
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
