using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Guard : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private float _moveSpeed;

    private Transform _thisTransform;
    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _thisTransform = transform;
        _playerTransform = _target.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        FollowToTarget();
    }

    private void Update()
    {
        LookAtTarget();
    }

    private void FollowToTarget()
    {
        Vector3 distance = _playerTransform.position - _thisTransform.position;
        Vector3 direction = distance.normalized;
        Vector3 newPosition = _thisTransform.position + _moveSpeed * Time.fixedDeltaTime * direction;
        _rigidbody.MovePosition(newPosition);
    }

    private void LookAtTarget()
    {
        Vector3 targetPosition = _playerTransform.position;
        targetPosition.y = _thisTransform.position.y;
        _thisTransform.LookAt(targetPosition);
    }
}