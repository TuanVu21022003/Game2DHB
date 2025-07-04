using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private AudioSource musicSource;
    private const int MAX_SFX_SOURCES = 10;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
    }

    [SerializeField] private Sound[] musicClips;
    [SerializeField] private Sound[] sfxClips;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.loop = true;

        for (int i = 0; i < MAX_SFX_SOURCES; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixerGroup;
            sfxSources.Add(source);
        }

        // Cache audio clips
        foreach (Sound sound in musicClips)
        {
            audioClips[sound.name] = sound.clip;
        }
        foreach (Sound sound in sfxClips)
        {
            audioClips[sound.name] = sound.clip;
        }
    }

    public void PlayMusic(string name, float fadeDuration = 0.5f)
    {
        Sound sound = GetSound(musicClips, name);
        if (sound == null) return;

        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeMusic(sound, fadeDuration));
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.volume = sound.volume;
            musicSource.pitch = sound.pitch;
            musicSource.loop = sound.loop;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = GetSound(sfxClips, name);
        if (sound == null) return;

        AudioSource availableSource = GetAvailableSFXSource();
        if (availableSource != null)
        {
            availableSource.PlayOneShot(sound.clip, sound.volume);
            availableSource.pitch = sound.pitch;
        }
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    private Sound GetSound(Sound[] sounds, string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
                return sound;
        }
        Debug.LogWarning($"Sound {name} not found!");
        return null;
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }

    private System.Collections.IEnumerator FadeMusic(Sound newSound, float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newSound.clip;
        musicSource.volume = newSound.volume;
        musicSource.pitch = newSound.pitch;
        musicSource.loop = newSound.loop;
        musicSource.Play();

        time = 0;
        startVolume = musicSource.volume;
        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, startVolume, time / duration);
            yield return null;
        }
    }

    private System.Collections.IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }
        musicSource.Stop();
        musicSource.volume = startVolume;
    }
}