using UnityEngine;
using UnityEngine.Pool;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class PooledEnemy : MonoBehaviour, IDamageable
    {
        [Tooltip("Hit points the enemy spawns with. Decremented by TakeDamage; released to pool when <= 0.")]
        [SerializeField][Min(1)] int maxHp = 1;

        IObjectPool<GameObject> _pool;
        int _hp;
        bool _released;

        public void Bind(IObjectPool<GameObject> pool) => _pool = pool;

        void OnEnable() { _released = false; _hp = maxHp; }

        public void TakeDamage(int amount)
        {
            if (_released) return;
            _hp -= amount;
            if (_hp <= 0) ReleaseToPool();
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
