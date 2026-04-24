using UnityEngine;

namespace AzizStuff
{
    [CreateAssetMenu(menuName = "Aziz/Waves/Wave Sequence", fileName = "WaveSequence_")]
    public class WaveSequenceSO : ScriptableObject
    {
        [Tooltip("Ordered waves. Spawner progresses through these in sequence.")]
        public WaveDefinition[] waves;

        [Tooltip("Pause (seconds) between waves; the upgrade event fires at the start of this gap.")]
        [Min(0f)] public float pauseBetweenWaves = 3f;

        [Tooltip("If true, restart at wave 0 after the final wave.")]
        public bool loop;
    }
}
