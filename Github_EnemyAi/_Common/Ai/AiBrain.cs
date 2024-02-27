using System;
using _Common.Ai.AiStates;
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
    public class AiBrain : MonoBehaviour {
#if UNITY_EDITOR
        [ShowInInspector, ReadOnly, GUIColor(0, 1, 0)]
        private string _currentState => _stateMachine?.CurrentState?.ToString();
#endif
        [Required] public AiSO AiData;


        private ITarget _target => _targetFinder.Get();

        private IMovementProvider _movementProvider;
        private TargetFinder _targetFinder;
        private StateMachine _stateMachine;
        private Animator _animator;

        private void Awake() {
            _targetFinder = GetComponent<TargetFinder>();
            _movementProvider = GetComponent<IMovementProvider>();
            _animator = GetComponent<Animator>();

            var leftWeaponHolder = transform.GetChild("LeftWeaponHolder");
            var rightWeaponHolder = transform.GetChild("RightWeaponHolder");

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
            bool InAttackRange() => _target.Position.IsInRangeOfSqr(transform.position, AiData.AttackRange);
            bool InWalkingRange() => _target.Position.IsInRangeOfSqr(transform.position, AiData.StartWalkingRange);

            void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
            void Any(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);
        }


        public void Move(bool run) => _movementProvider.TickMovement(run);
        public void StopMovement() => _movementProvider.StopMovement();

        public void Attack() {
            _animator.CrossFadeInFixedTime("Attack", 0.1f);
        }

        public void OnAttackEvent() {
            AiData.AttackStrategy.PerformAttack(transform, _target, 0, 0);
        }
    }
}