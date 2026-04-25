using System;
using System.Collections.Generic;
using UnityEngine;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Serializable]
        public struct SoundEntry
        {
            [Tooltip("ID used by gameplay scripts to request this clip, e.g. \"hero_swing\".")]
            public string id;
            public AudioClip clip;
            [Tooltip("Per-clip multiplier on top of the global SFX/Music volume.")]
            [Range(0f, 1f)] public float volume;
        }

        [Header("Sources")]
        [Tooltip("Looping background music source. Auto-created on Awake if left empty.")]
        [SerializeField] AudioSource musicSource;

        [Tooltip("One-shot SFX source. Auto-created on Awake if left empty.")]
        [SerializeField] AudioSource sfxSource;

        [Header("Library")]
        [Tooltip("Short, overlapping sounds. Looked up by id from PlaySfx(id).")]
        [SerializeField] SoundEntry[] sfxClips;

        [Tooltip("Looping background tracks. Looked up by id from PlayMusic(id).")]
        [SerializeField] SoundEntry[] musicClips;

        [Header("Volumes")]
        [Range(0f, 1f)] public float musicVolume = 1f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        [Header("Lifecycle")]
        [Tooltip("Persist across scene loads. Disable for per-scene managers.")]
        [SerializeField] bool persistAcrossScenes = true;

        readonly Dictionary<string, SoundEntry> _sfxLookup = new Dictionary<string, SoundEntry>();
        readonly Dictionary<string, SoundEntry> _musicLookup = new Dictionary<string, SoundEntry>();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (persistAcrossScenes && transform.parent == null)
                DontDestroyOnLoad(gameObject);

            EnsureSources();
            BuildLookups();
            ApplyVolumes();
        }

        void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        void OnValidate()
        {
            if (musicSource != null) musicSource.volume = musicVolume;
            if (sfxSource != null) sfxSource.volume = sfxVolume;
        }

        void EnsureSources()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
            }
        }

        void BuildLookups()
        {
            _sfxLookup.Clear();
            if (sfxClips != null)
            {
                for (int i = 0; i < sfxClips.Length; i++)
                {
                    var entry = sfxClips[i];
                    if (string.IsNullOrEmpty(entry.id) || entry.clip == null) continue;
                    _sfxLookup[entry.id] = entry;
                }
            }

            _musicLookup.Clear();
            if (musicClips != null)
            {
                for (int i = 0; i < musicClips.Length; i++)
                {
                    var entry = musicClips[i];
                    if (string.IsNullOrEmpty(entry.id) || entry.clip == null) continue;
                    _musicLookup[entry.id] = entry;
                }
            }
        }

        void ApplyVolumes()
        {
            if (musicSource != null) musicSource.volume = musicVolume;
            if (sfxSource != null) sfxSource.volume = sfxVolume;
        }

        public void PlaySfx(string id, float volumeScale = 1f)
        {
            if (sfxSource == null) return;
            if (!_sfxLookup.TryGetValue(id, out var entry))
            {
                Debug.LogWarning($"AudioManager: SFX id '{id}' not found.");
                return;
            }
            float perClip = entry.volume <= 0f ? 1f : entry.volume;
            sfxSource.PlayOneShot(entry.clip, Mathf.Clamp01(sfxVolume * perClip * volumeScale));
        }

        public void PlayMusic(string id)
        {
            if (musicSource == null) return;
            if (!_musicLookup.TryGetValue(id, out var entry))
            {
                Debug.LogWarning($"AudioManager: Music id '{id}' not found.");
                return;
            }
            if (musicSource.isPlaying && musicSource.clip == entry.clip) return;
            float perClip = entry.volume <= 0f ? 1f : entry.volume;
            musicSource.clip = entry.clip;
            musicSource.volume = Mathf.Clamp01(musicVolume * perClip);
            musicSource.Play();
        }

        public void StopMusic()
        {
            if (musicSource != null) musicSource.Stop();
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = Mathf.Clamp01(value);
            if (musicSource != null) musicSource.volume = musicVolume;
        }

        public void SetSfxVolume(float value)
        {
            sfxVolume = Mathf.Clamp01(value);
            if (sfxSource != null) sfxSource.volume = sfxVolume;
        }
    }
}
