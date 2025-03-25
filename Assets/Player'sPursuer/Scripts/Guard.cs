using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Guard : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minDistanceToTarget = 2;

    private Transform _transform;
    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _transform = transform;
        _playerTransform = _target.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if ((_playerTransform.position - _transform.position).sqrMagnitude > _minDistanceToTarget * _minDistanceToTarget)
        {
            FollowToTarget();
        }
    }

    private void Update()
    {
        LookAtTarget();
    }

    private void FollowToTarget()
    {
        Vector3 direction = (_playerTransform.position - _transform.position).normalized;
        Vector3 newPosition = _transform.position + _moveSpeed * Time.fixedDeltaTime * direction;
        _rigidbody.MovePosition(newPosition);
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = _playerTransform.position;
        targetPosition.y = _transform.position.y;
        _transform.LookAt(targetPosition);
    }
}