using UnityEngine;

namespace AzizStuff
{
    [DisallowMultipleComponent]
    public class EnemyMover : MonoBehaviour
    {
        [Tooltip("World units per second.")]
        [Min(0f)] public float speed = 2.5f;

        Transform _target;
        Transform _tf;
        Collider2D _targetCollider;

        void Awake() => _tf = transform;

        public void Init(Transform target)
        {
            _target = target;
            _targetCollider = target != null ? target.GetComponentInChildren<Collider2D>() : null;
        }

        void Update()
        {
            if (_target == null) return;

            Vector3 pos = _tf.position;
            Vector2 goal = _targetCollider != null
                ? _targetCollider.ClosestPoint(pos)
                : (Vector2)_target.position;

            Vector3 next = Vector2.MoveTowards(pos, goal, speed * Time.deltaTime);
            next.z = pos.z;
            _tf.position = next;
        }
    }
}
