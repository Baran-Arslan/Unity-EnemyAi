using _Common.Ai.Target;
using UnityEngine;
using UnityEngine.AI;

namespace _Common.Ai.Movement {
    [RequireComponent(typeof(TargetFinder))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class NavmeshMovement : MonoBehaviour, IMovementProvider {
        private ITarget _target => _targetFinder.Get();

        private TargetFinder _targetFinder;
        private Animator _animator;
        private NavMeshAgent _agent;

        private static readonly int _speedParameter = Animator.StringToHash("Speed");


        private void Awake() {
            _targetFinder = GetComponent<TargetFinder>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();

            _agent.updatePosition = false;
        }
        private void Update() => _animator.SetFloat(_speedParameter, _agent.velocity.magnitude, 0.1f, Time.deltaTime);

        public void TickMovement(bool run) {
            _agent.SetDestination(_target.Position);
            _agent.speed = run ? 1 : 0.5f;
        }
        public void StopMovement() => _agent.ResetPath();


        private void OnAnimatorMove() {
            var position = _animator.rootPosition;
            position.y = _agent.nextPosition.y;
            transform.position = position;
            _agent.nextPosition = transform.position;
        }
    }
}