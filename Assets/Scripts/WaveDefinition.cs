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

            [Tooltip("Spawn events per minute. Each event emits one group (size given by min/max below). " +
                     "With groupSizeMax=1 this is equivalent to enemies/min.")]
            [Min(0f)] public float spawnsPerMinute;

            [Tooltip("Minimum enemies per group. Set both min and max to 1 for classic per-enemy spawn.")]
            [Min(1)] public int groupSizeMin;

            [Tooltip("Maximum enemies per group. Actual group size is a random int in [min, max].")]
            [Min(1)] public int groupSizeMax;

            [Tooltip("Arc width (degrees) around each group's random direction. " +
                     "360 = spawns uniformly on the ring; ~30 = tight cluster from one direction.")]
            [Range(0f, 360f)] public float angleSpreadDegrees;
        }

        [Tooltip("Enemy entries spawned in parallel during this wave; each runs on its own schedule.")]
        public EnemyEntry[] entries;
    }
}
