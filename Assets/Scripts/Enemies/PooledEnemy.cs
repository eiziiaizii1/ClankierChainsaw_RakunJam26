using UnityEngine;
using UnityEngine.Pool;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class PooledEnemy : MonoBehaviour, IDamageable
    {
        [Tooltip("Hit points the enemy spawns with. Decremented by TakeDamage; released to pool when <= 0.")]
        [SerializeField][Min(1)] int maxHp = 1;

        [Tooltip("Money awarded to MoneyManager when this enemy dies. 0 = no reward.")]
        [SerializeField][Min(0)] int killReward = 1;

        IObjectPool<GameObject> _pool;
        int _hp;
        int _scaledReward;
        bool _released;

        public void Bind(IObjectPool<GameObject> pool) => _pool = pool;

        void OnEnable()
        {
            _released = false;
            _hp = maxHp;
            _scaledReward = killReward;
        }

        // Called by the spawner right after pool.Get(). Applies per-spawn multipliers from
        // wave-loop difficulty scaling. Defaults of 1 preserve authoring-time values.
        public void ApplyDifficulty(float hpMul, float rewardMul)
        {
            _hp = Mathf.Max(1, Mathf.RoundToInt(maxHp * hpMul));
            _scaledReward = Mathf.Max(0, Mathf.RoundToInt(killReward * rewardMul));
        }

        public void TakeDamage(int amount)
        {
            if (_released) return;
            _hp -= amount;
            if (_hp <= 0)
            {
                if (_scaledReward > 0 && MoneyManager.Instance != null)
                    MoneyManager.Instance.AddMoney(_scaledReward);
                ReleaseToPool();
            }
        }

        public void ReleaseToPool()
        {
            if (_released) return;
            _released = true;
            if (_pool != null) _pool.Release(gameObject);
            else Destroy(gameObject);
        }
    }
}
