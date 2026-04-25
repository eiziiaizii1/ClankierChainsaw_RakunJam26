using UnityEngine;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class EnemyMover : MonoBehaviour
    {
        enum State { Chase, Attack }

        [Tooltip("World units per second while chasing.")]
        [Min(0f)] public float speed = 2.5f;

        [Tooltip("Distance to target collider edge at which the enemy stops and starts attacking.")]
        [Min(0f)] public float attackRange = 0.5f;

        [Tooltip("Damage applied to the target per attack tick.")]
        [Min(0)] public int attackDamage = 1;

        [Tooltip("Seconds between attack ticks while in range.")]
        [Min(0.01f)] public float attackInterval = 1f;

        Transform _target;
        Transform _tf;
        Collider2D _targetCollider;
        IDamageable _targetDamageable;
        State _state;
        float _nextAttackTime;

        void Awake() => _tf = transform;

        void OnEnable()
        {
            _state = State.Chase;
            _nextAttackTime = 0f;
        }

        public void Init(Transform target)
        {
            _target = target;
            _targetCollider = target != null ? target.GetComponentInChildren<Collider2D>() : null;
            _targetDamageable = target != null ? target.GetComponentInParent<IDamageable>() : null;
        }

        void Update()
        {
            if (_target == null) return;

            Vector3 pos = _tf.position;
            Vector2 goal = _targetCollider != null
                ? _targetCollider.ClosestPoint(pos)
                : (Vector2)_target.position;

            float dist = Vector2.Distance(pos, goal);

            switch (_state)
            {
                case State.Chase:
                    if (dist <= attackRange)
                    {
                        _state = State.Attack;
                        _nextAttackTime = Time.time + attackInterval;
                    }
                    else
                    {
                        Vector3 next = Vector2.MoveTowards(pos, goal, speed * Time.deltaTime);
                        next.z = pos.z;
                        _tf.position = next;
                    }
                    break;

                case State.Attack:
                    if (dist > attackRange)
                    {
                        _state = State.Chase;
                    }
                    else if (Time.time >= _nextAttackTime)
                    {
                        if (_targetDamageable != null) _targetDamageable.TakeDamage(attackDamage);
                        _nextAttackTime = Time.time + attackInterval;
                    }
                    break;
            }
        }
    }
}
