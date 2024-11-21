using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private const string VOLUME_PREF_KEY = "MasterVolume";

    [SerializeField] private AudioSource soundFXPrefab;
    private AudioSource backgroundMusicSource;

    protected override void Awake()
    {
        base.Awake();
        InitializeAudioSources();
        SetVolume(GetSavedVolume());
    }

    private void InitializeAudioSources()
    {
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.loop = true;
    }

    public float GetSavedVolume()
    {
        return PlayerPrefs.GetFloat(VOLUME_PREF_KEY, 1f);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(VOLUME_PREF_KEY, volume);
        PlayerPrefs.Save();
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        if (audioClip == null) return;

        AudioSource audioSource = Instantiate(soundFXPrefab, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioClip.length);
    }

    public void PlayMusic(AudioClip musicClip, float volume = 0.15f)
    {
        if (musicClip == null) return;

        StopMusic();
        backgroundMusicSource.clip = musicClip;
        backgroundMusicSource.volume = volume;
        backgroundMusicSource.Play();
    }

    public void StopMusic()
    {
        if (backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.Stop();
        }
    }
}