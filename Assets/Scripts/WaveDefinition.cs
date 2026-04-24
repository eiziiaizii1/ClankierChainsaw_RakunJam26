using System;
using UnityEngine;

namespace AzizStuff
{
    [CreateAssetMenu(menuName = "Aziz/Waves/Wave Definition", fileName = "Wave_")]
    public class WaveDefinition : ScriptableObject
    {
        [Serializable]
        public struct EnemyEntry
        {
            [Tooltip("Enemy prefab. Should have an EnemyMover (and optionally a PooledEnemy).")]
            public GameObject enemyPrefab;

            [Tooltip("Total of this enemy to spawn over the wave.")]
            [Min(0)] public int count;

            [Tooltip("Spawn rate in enemies per minute.")]
            [Min(0f)] public float spawnsPerMinute;
        }

        [Tooltip("Enemy entries spawned in parallel during this wave; each runs on its own schedule.")]
        public EnemyEntry[] entries;
    }
}
