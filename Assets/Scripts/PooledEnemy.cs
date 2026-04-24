using UnityEngine;
using UnityEngine.Pool;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class PooledEnemy : MonoBehaviour, IDamageable
    {
        IObjectPool<GameObject> _pool;
        bool _released;

        public void Bind(IObjectPool<GameObject> pool) => _pool = pool;

        void OnEnable() => _released = false;

        public void TakeDamage(int amount) => ReleaseToPool();

        public void ReleaseToPool()
        {
            if (_released) return;
            _released = true;
            if (_pool != null) _pool.Release(gameObject);
            else Destroy(gameObject);
        }
    }
}
