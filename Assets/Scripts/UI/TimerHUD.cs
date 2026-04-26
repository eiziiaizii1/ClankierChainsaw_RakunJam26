using UnityEngine;
using TMPro;

namespace AzizStuff
{
    /// <summary>
    /// Counts elapsed seconds from 0 upward and writes them to a TMP text in MM:SS
    /// (or HH:MM:SS once it crosses an hour). One string alloc per second of game time.
    /// </summary>
    public class TimerHUD : MonoBehaviour
    {
        [Tooltip("Where to display the elapsed time.")]
        [SerializeField] TMP_Text timerText;

        [Tooltip("If on, timer pauses whenever Time.timeScale is 0 (e.g. hero death, pause menu). " +
                 "Turn off to count in real time regardless of timescale.")]
        [SerializeField] bool pauseWithTimeScale = true;

        float _elapsed;
        int _lastDisplayedSecond = -1;

        void Awake()
        {
            _elapsed = 0f;
            _lastDisplayedSecond = -1;
            UpdateText();
        }

        void Update()
        {
            _elapsed += pauseWithTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;
            UpdateText();
        }

        void UpdateText()
        {
            if (timerText == null) return;

            int totalSeconds = Mathf.FloorToInt(_elapsed);
            if (totalSeconds == _lastDisplayedSecond) return;
            _lastDisplayedSecond = totalSeconds;

            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds / 60) % 60;
            int seconds = totalSeconds % 60;

            timerText.text = hours > 0
                ? $"{hours:D2}:{minutes:D2}:{seconds:D2}"
                : $"{minutes:D2}:{seconds:D2}";
        }

        /// <summary>Resets the elapsed time back to 0. Useful on retry / new run.</summary>
        public void ResetTimer()
        {
            _elapsed = 0f;
            _lastDisplayedSecond = -1;
            UpdateText();
        }

        public float ElapsedSeconds => _elapsed;
    }
}
