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

        [Header("UI")]
        [Tooltip("Optional parent object for the HP bar. Kept hidden until the enemy takes non-lethal damage.")]
        [SerializeField] GameObject hpBarRoot;

        [Tooltip("Optional transform for the HP bar fill. It will be scaled horizontally (localScale.x) based on current HP.")]
        [SerializeField] Transform hpBarFill;

        IObjectPool<GameObject> _pool;
        int _currentMaxHp;
        int _hp;
        int _scaledReward;
        bool _released;

        public void Bind(IObjectPool<GameObject> pool) => _pool = pool;

        void OnEnable()
        {
            _released = false;
            _currentMaxHp = maxHp;
            _hp = _currentMaxHp;
            _scaledReward = killReward;

            if (hpBarRoot != null) hpBarRoot.SetActive(false);
            if (hpBarFill != null)
            {
                Vector3 scale = hpBarFill.localScale;
                scale.x = 1f;
                hpBarFill.localScale = scale;
            }
        }

        // Called by the spawner right after pool.Get(). Applies per-spawn multipliers from
        // wave-loop difficulty scaling. Defaults of 1 preserve authoring-time values.
        public void ApplyDifficulty(float hpMul, float rewardMul)
        {
            _currentMaxHp = Mathf.Max(1, Mathf.RoundToInt(maxHp * hpMul));
            _hp = _currentMaxHp;
            _scaledReward = Mathf.Max(0, Mathf.RoundToInt(killReward * rewardMul));
        }

        public void TakeDamage(int amount)
        {
            if (_released) return;
            _hp -= amount;

            if (_hp > 0 && _hp < _currentMaxHp)
            {
                if (hpBarRoot != null && !hpBarRoot.activeSelf)
                {
                    hpBarRoot.SetActive(true);
                }
                
                if (hpBarFill != null)
                {
                    Vector3 scale = hpBarFill.localScale;
                    scale.x = (float)_hp / _currentMaxHp;
                    hpBarFill.localScale = scale;
                }
            }

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
