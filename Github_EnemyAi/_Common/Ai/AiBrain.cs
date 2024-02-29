using System;
using _Common.Ai.AiStates;
using _Common.Ai.Animation;
using _Common.Ai.Movement;
using _Common.Ai.Target;
using _Common.Core.Extensions;
using _Common.iCare.Core.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Ai {
    [RequireComponent(typeof(IMovementProvider))]
    [RequireComponent(typeof(TargetFinder))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(IKLookAt))]
    public class AiBrain : MonoBehaviour {
#if UNITY_EDITOR
        [ShowInInspector, ReadOnly, GUIColor(0, 1, 0)]
        private string _currentState => _stateMachine?.CurrentState?.ToString();
#endif
        public AiSO AiData { get; set; }
        public Transform AttackHand { get; set; }


        private Target.ITarget _target => _targetFinder.Get();

        private IMovementProvider _movementProvider;
        private TargetFinder _targetFinder;
        private StateMachine _stateMachine;
        private Animator _animator;
        private IKLookAt _ikLookAt;

        private void Start() {
            _targetFinder = GetComponent<TargetFinder>();
            _movementProvider = GetComponent<IMovementProvider>();
            _animator = GetComponent<Animator>();
            _ikLookAt = GetComponent<IKLookAt>();

            SetupStateMachine();
        }

        private void Update() => _stateMachine.Tick();

        private void SetupStateMachine() {
            _stateMachine = new StateMachine();

            var idleState = new AiIdleState(this);
            var walkState = new AiWalkState(this);
            var runState = new AiRunState(this);
            var attackState = new AiAttackState(this);

            Any(idleState, () => !HasTarget());
            At(idleState, runState, HasTarget);
            At(runState, walkState, InWalkingRange);
            At(walkState, runState, () => !InWalkingRange());
            At(walkState, attackState, InAttackRange);
            At(attackState, walkState, () => !InAttackRange());


            _stateMachine.SetState(idleState);

            return;
            bool HasTarget() => _target != null;

            bool InAttackRange() =>
                _target.GetTransform().position.IsInRangeOfSqr(transform.position, AiData.AttackSettings.AttackRange, true);

            bool InWalkingRange() =>
                _target.GetTransform().position.IsInRangeOfSqr(transform.position, AiData.StartWalkingRange, true);

            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            void Any(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
        }


        public void MoveToTarget(bool canRun) => _movementProvider.TickMovement(_target.GetTransform().position ,canRun);
        public void StopMovement() => _movementProvider.StopMovement();
        public void RotateTowardsTarget() => transform.LookAtSmooth(_target.GetTransform().position, 2, true);
        public void EnableIK(bool enable) => _ikLookAt.Target = enable ? _target : null;

        public void Attack() {
            _animator.CrossFadeInFixedTime(AiData.AnimationSo.AttackAnimationName, 0.25f);
        }

        public void OnAttackEvent() {
            if (_target == null) {
                _animator.CrossFadeInFixedTime(AnimationSO.IdleAnimationName, 0.2f);
                return;
            }
            AiData.AttackSettings.AttackStrategy.PerformAttack(AttackHand, _target, AiData.AttackSettings.AttackDamage,
                AiData.AttackSettings.RagdollPushForce);
        }
    }
}