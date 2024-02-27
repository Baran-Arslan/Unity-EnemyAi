using _Common.Ai.Target;
using UnityEngine;

namespace _Common {
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(TargetFinder))]
    public class IKLookAt : MonoBehaviour {
        
        
        private ITarget _target => _targetFinder.Get();


        private Vector3 _ikTargetPosition;
        private float _weight;
        
        private Animator _animator;
        private Transform _transform;
        private TargetFinder _targetFinder;


        private void Awake() {
            _animator = GetComponent<Animator>();
            _targetFinder = GetComponent<TargetFinder>();

            _transform = transform;
        }

        private void Update() {
            var _hasTarget = _target != null;
            _weight = Mathf.Lerp(_weight, _hasTarget ? 1 : 0, Time.deltaTime * 2);
            var targetPos = _hasTarget ? _target.Position : _transform.position + _transform.forward * 10;

            _ikTargetPosition = Vector3.Lerp(_ikTargetPosition, targetPos, 10 * Time.deltaTime);
        }


        private void OnAnimatorIK(int layerIndex) {
            _animator.SetLookAtWeight(_weight, 1, 1);
            _animator.SetLookAtPosition(_ikTargetPosition);
        }
    }
}