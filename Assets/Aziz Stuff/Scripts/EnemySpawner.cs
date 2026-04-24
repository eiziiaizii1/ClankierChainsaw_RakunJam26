using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using URandom = UnityEngine.Random;

namespace AzizStuff
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform target;
        [SerializeField] WaveSequenceSO sequence;
        [SerializeField] WaveEventChannelSO waveCompletedChannel;

        [Header("Spawn Ring")]
        [Tooltip("Distance from target at which enemies spawn. Set just outside camera view.")]
        [Min(0f)][SerializeField] float spawnRadius = 12f;
        [Tooltip("Random +/- jitter added to spawn radius each spawn.")]
        [Min(0f)][SerializeField] float radiusJitter = 0.5f;

        [Header("Pooling")]
        [SerializeField] int defaultPoolCapacity = 32;
        [SerializeField] int maxPoolSize = 512;
        [SerializeField] Transform poolParent;

        readonly Dictionary<GameObject, IObjectPool<GameObject>> _pools =
            new Dictionary<GameObject, IObjectPool<GameObject>>();

        void Awake()
        {
            if (target == null) { Debug.LogError("[EnemySpawner] target not assigned.", this); enabled = false; return; }
            if (sequence == null) { Debug.LogError("[EnemySpawner] sequence not assigned.", this); enabled = false; return; }
            if (poolParent == null) poolParent = transform;
            WarmPools();
        }

        void Start() => StartCoroutine(RunWaves());

        void WarmPools()
        {
            if (sequence.waves == null) return;
            for (int w = 0; w < sequence.waves.Length; w++)
            {
                var wave = sequence.waves[w];
                if (wave == null || wave.entries == null) continue;
                for (int e = 0; e < wave.entries.Length; e++)
                {
                    var prefab = wave.entries[e].enemyPrefab;
                    if (prefab == null || _pools.ContainsKey(prefab)) continue;
                    _pools[prefab] = CreatePool(prefab);
                }
            }
        }

        IObjectPool<GameObject> CreatePool(GameObject prefab)
        {
            IObjectPool<GameObject> pool = null;
            pool = new ObjectPool<GameObject>(
                createFunc: () =>
                {
                    var go = Instantiate(prefab, poolParent);
                    go.SetActive(false);
                    if (go.TryGetComponent<PooledEnemy>(out var pooled)) pooled.Bind(pool);
                    return go;
                },
                actionOnGet: go => go.SetActive(true),
                actionOnRelease: go => go.SetActive(false),
                actionOnDestroy: go => { if (go != null) Destroy(go); },
                collectionCheck: false,
                defaultCapacity: defaultPoolCapacity,
                maxSize: maxPoolSize
            );
            return pool;
        }

        IEnumerator RunWaves()
        {
            int waveIndex = 0;
            var waves = sequence.waves;
            while (true)
            {
                if (waves == null || waves.Length == 0) yield break;

                if (waveIndex >= waves.Length)
                {
                    if (sequence.loop) waveIndex = 0;
                    else yield break;
                }

                var wave = waves[waveIndex];
                if (wave != null) yield return RunWave(wave);

                if (waveCompletedChannel != null) waveCompletedChannel.Raise(waveIndex);
                Debug.Log($"Wave {waveIndex + 1} complete. Upgrade event triggered.");

                if (sequence.pauseBetweenWaves > 0f)
                    yield return new WaitForSeconds(sequence.pauseBetweenWaves);

                waveIndex++;
            }
        }

        IEnumerator RunWave(WaveDefinition wave)
        {
            if (wave.entries == null || wave.entries.Length == 0) yield break;

            int running = 0;
            for (int i = 0; i < wave.entries.Length; i++)
            {
                var entry = wave.entries[i];
                if (entry.enemyPrefab == null || entry.count <= 0 || entry.spawnsPerMinute <= 0f) continue;
                running++;
                StartCoroutine(SpawnEntry(entry, () => running--));
            }

            while (running > 0) yield return null;
        }

        IEnumerator SpawnEntry(WaveDefinition.EnemyEntry entry, Action onComplete)
        {
            float interval = 60f / entry.spawnsPerMinute;
            var wait = new WaitForSeconds(interval);

            int groupMin = Mathf.Max(1, entry.groupSizeMin);
            int groupMax = Mathf.Max(groupMin, entry.groupSizeMax);
            float halfSpreadRad = Mathf.Max(0f, entry.angleSpreadDegrees) * Mathf.Deg2Rad * 0.5f;

            int remaining = entry.count;
            while (remaining > 0)
            {
                int groupSize = URandom.Range(groupMin, groupMax + 1);
                if (groupSize > remaining) groupSize = remaining;

                float centerAngle = URandom.value * Mathf.PI * 2f;
                for (int i = 0; i < groupSize; i++)
                {
                    float offset = halfSpreadRad > 0f ? URandom.Range(-halfSpreadRad, halfSpreadRad) : 0f;
                    Spawn(entry.enemyPrefab, centerAngle + offset);
                }

                remaining -= groupSize;
                if (remaining > 0) yield return wait;
            }

            onComplete?.Invoke();
        }

        void Spawn(GameObject prefab, float angleRadians)
        {
            if (!_pools.TryGetValue(prefab, out var pool)) return;
            var enemy = pool.Get();

            float r = radiusJitter > 0f
                ? spawnRadius + URandom.Range(-radiusJitter, radiusJitter)
                : spawnRadius;

            Vector3 center = target.position;
            enemy.transform.SetPositionAndRotation(
                new Vector3(center.x + Mathf.Cos(angleRadians) * r, center.y + Mathf.Sin(angleRadians) * r, center.z),
                Quaternion.identity
            );

            if (enemy.TryGetComponent<EnemyMover>(out var mover)) mover.Init(target);
        }
    }
}
