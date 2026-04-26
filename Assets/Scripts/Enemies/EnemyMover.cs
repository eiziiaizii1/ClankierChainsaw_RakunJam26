using UnityEngine;
using URandom = UnityEngine.Random;

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

        [Tooltip("Radius used to detect nearby enemies and push apart. 0 disables separation.")]
        [Min(0f)] public float separationRadius = 0.4f;

        [Tooltip("Push speed away from overlapping enemies, in units/sec at full overlap.")]
        [Min(0f)] public float separationStrength = 3f;

        [Tooltip("Child transform holding the visual / animator. Rotates to face the target each frame. " +
                 "Sprite must be authored facing +Y (up). Leave null to disable facing.")]
        [SerializeField] Transform visualRoot;

        [Tooltip("Animator trigger fired on each attack tick. Leave empty to skip.")]
        [SerializeField] string attackTriggerName = "Attack";

        Transform _target;
        Transform _tf;
        Collider2D _targetCollider;
        IDamageable _targetDamageable;
        Animator _animator;
        int _attackTriggerHash;
        int _scaledDamage;
        State _state;
        float _nextAttackTime;

        static readonly Collider2D[] _sepBuffer = new Collider2D[16];

        void Awake()
        {
            _tf = transform;
            if (visualRoot != null) _animator = visualRoot.GetComponentInChildren<Animator>();
            if (!string.IsNullOrEmpty(attackTriggerName))
                _attackTriggerHash = Animator.StringToHash(attackTriggerName);
        }

        void OnEnable()
        {
            _state = State.Chase;
            _nextAttackTime = 0f;
            _scaledDamage = attackDamage;
        }

        public void Init(Transform target)
        {
            _target = target;
            _targetCollider = target != null ? target.GetComponentInChildren<Collider2D>() : null;
            _targetDamageable = target != null ? target.GetComponentInParent<IDamageable>() : null;
        }

        // Called by the spawner per-spawn so each new enemy hits harder as the wave list loops.
        public void ApplyDifficulty(float damageMul)
        {
            _scaledDamage = Mathf.Max(0, Mathf.RoundToInt(attackDamage * damageMul));
        }

        void Update()
        {
            if (_target == null) return;

            Vector3 pos = _tf.position;
            Vector2 goal = _targetCollider != null
                ? _targetCollider.ClosestPoint(pos)
                : (Vector2)_target.position;

            float dist = Vector2.Distance(pos, goal);
            Vector2 separation = ComputeSeparation(pos) * separationStrength;

            switch (_state)
            {
                case State.Chase:
                    if (dist <= attackRange)
                    {
                        _state = State.Attack;
                        // First tick is jittered so a cluster doesn't all swing on the same frame.
                        _nextAttackTime = Time.time + URandom.Range(0f, attackInterval);
                    }
                    else
                    {
                        Vector3 next = Vector2.MoveTowards(pos, goal, speed * Time.deltaTime);
                        next += (Vector3)(separation * Time.deltaTime);
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
                        if (_targetDamageable != null) _targetDamageable.TakeDamage(_scaledDamage);
                        if (_animator != null && _attackTriggerHash != 0) _animator.SetTrigger(_attackTriggerHash);
                        _nextAttackTime = Time.time + attackInterval;
                    }
                    if (separation.sqrMagnitude > 0.0001f)
                    {
                        // Strip the radial component so attackers slide tangentially around the target instead of out of range.
                        Vector2 toTarget = dist > 0.0001f ? (goal - (Vector2)pos) / dist : Vector2.zero;
                        Vector2 tangent = separation - Vector2.Dot(separation, toTarget) * toTarget;
                        Vector3 nudged = pos + (Vector3)(tangent * Time.deltaTime);
                        nudged.z = pos.z;
                        _tf.position = nudged;
                    }
                    break;
            }

            if (visualRoot != null)
            {
                Vector2 dir = goal - (Vector2)pos;
                if (dir.sqrMagnitude > 0.0001f) visualRoot.up = dir;
            }
        }

        Vector2 ComputeSeparation(Vector2 pos)
        {
            if (separationRadius <= 0f || separationStrength <= 0f) return Vector2.zero;
            int count = Physics2D.OverlapCircleNonAlloc(pos, separationRadius, _sepBuffer);
            Vector2 push = Vector2.zero;
            for (int i = 0; i < count; i++)
            {
                var c = _sepBuffer[i];
                if (c == null || !c.CompareTag("Enemy")) continue;
                if (c.transform == _tf || c.transform.IsChildOf(_tf)) continue;
                Vector2 delta = pos - (Vector2)c.transform.position;
                float d = delta.magnitude;
                if (d < 0.0001f)
                {
                    float a = (_tf.GetInstanceID() & 0xFFFF) * 0.0001f;
                    push += new Vector2(Mathf.Cos(a), Mathf.Sin(a));
                    continue;
                }
                if (d < separationRadius)
                {
                    float falloff = 1f - d / separationRadius;
                    push += (delta / d) * falloff;
                }
            }
            return push;
        }
    }
}
