/*
 * AudioManager - Persistent singleton for song playback and DSP timing.
 *
 * Provides a DSP-accurate SongTime for rhythm game synchronization.
 * All timing derives from AudioSettings.dspTime, never Time.time.
 *
 * Usage:
 *   AudioManager.Instance.PlaySong(clip);
 *   double currentBeat = AudioManager.Instance.SongTime;
 *
 * Setup:
 *   1. Create a prefab with this component and an AudioSource.
 *   2. Assign to Bootstrapper._persistentPrefabs (becomes DontDestroyOnLoad).
 */

using UnityEngine;

namespace Toolkit.Audio
{
    public class AudioManager : Debugger
    {
        public static AudioManager Instance { get; private set; }

        protected override string LogCategory => "Audio";

        [Header("Audio")]
        [SerializeField] private AudioSource _musicSource;

        [Header("Calibration")]
        [Tooltip("Offset in seconds to compensate for audio latency. Positive = audio is late.")]
        [SerializeField] private double _calibrationOffsetSeconds;

        private double _songStartDspTime;
        private bool _isPlaying;

        /// <summary>
        /// Current position in the song, in seconds. DSP-accurate.
        /// Use this for ALL rhythm timing -- never use Time.time.
        /// </summary>
        public double SongTime
        {
            get
            {
                if (!_isPlaying) return 0.0;
                return AudioSettings.dspTime - _songStartDspTime
                       + _calibrationOffsetSeconds;
            }
        }

        public bool IsPlaying => _isPlaying;
        public AudioSource MusicSource => _musicSource;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (_musicSource == null)
                _musicSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Start playing a song with an optional countdown delay.
        /// Uses PlayScheduled for sample-accurate start timing.
        /// </summary>
        public void PlaySong(AudioClip clip, double delaySeconds = 3.0)
        {
            _musicSource.clip = clip;
            _songStartDspTime = AudioSettings.dspTime + delaySeconds;
            _musicSource.PlayScheduled(_songStartDspTime);
            _isPlaying = true;
            Log($"PlaySong: {clip.name}, delay={delaySeconds}s");
        }

        /// <summary>
        /// Resume from a specific position (e.g. after pause).
        /// </summary>
        public void PlayFrom(double positionSeconds)
        {
            _musicSource.time = (float)positionSeconds;
            _songStartDspTime = AudioSettings.dspTime - positionSeconds;
            _musicSource.Play();
            _isPlaying = true;
            Log($"PlayFrom: {positionSeconds}s");
        }

        public void Pause()
        {
            _musicSource.Pause();
            _isPlaying = false;
            Log("Paused");
        }

        public void StopSong()
        {
            _musicSource.Stop();
            _isPlaying = false;
            Log("Stopped");
        }

        /// <summary>
        /// Set calibration offset at runtime (e.g. from a settings menu).
        /// </summary>
        public void SetCalibration(double offsetSeconds)
        {
            _calibrationOffsetSeconds = offsetSeconds;
            Log($"Calibration set to {offsetSeconds}s");
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
