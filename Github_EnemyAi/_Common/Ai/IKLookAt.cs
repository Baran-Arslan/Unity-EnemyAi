using _Common.Ai.Target;
using UnityEngine;

namespace _Common.Ai {
    [RequireComponent(typeof(Animator))]
    public class IKLookAt : MonoBehaviour {
        
        public ITarget Target { get; set; }


        private Vector3 _ikTargetPosition;
        private float _weight;
        
        private Animator _animator;
        private Transform _transform;


        private void Awake() {
            _animator = GetComponent<Animator>();
            _transform = transform;
        }

        private void Update() {
            var _hasTarget = Target != null;
            _weight = Mathf.Lerp(_weight, _hasTarget ? 1 : 0, Time.deltaTime);
            var targetPos = _hasTarget ? Target.GetTransform().position : _transform.position + _transform.forward * 10;

            _ikTargetPosition = Vector3.Lerp(_ikTargetPosition, targetPos, Time.deltaTime * 2);
        }


        private void OnAnimatorIK(int layerIndex) {
            _animator.SetLookAtWeight(_weight, 1, 1);
            _animator.SetLookAtPosition(_ikTargetPosition);
        }
    }
}