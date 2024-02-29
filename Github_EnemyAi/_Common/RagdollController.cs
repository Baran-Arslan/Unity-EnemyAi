using UnityEngine;

namespace _Common {
    [RequireComponent(typeof(Animator))]
    public class RagdollController : MonoBehaviour {
        private Rigidbody[] _rigidbodys;
        private Collider[] _colliders;
        private Animator _animator;

        private void Awake() {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            _rigidbodys = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
            SetRagdollEnabled(false);
        }

        private void SetRagdollEnabled(bool isEnabled) {
            foreach (var rb in _rigidbodys) {
                rb.isKinematic = !isEnabled;
                rb.detectCollisions = isEnabled;
            }

            foreach (var col in _colliders) {
                col.enabled = isEnabled;
            }

            _animator.enabled = !isEnabled;
        }

        public void TriggerRagdoll(Transform attackSender, int ragdollPushForce) {
            SetRagdollEnabled(true);
            //TODO - PUSH RAGDOLL
        }
    }
}