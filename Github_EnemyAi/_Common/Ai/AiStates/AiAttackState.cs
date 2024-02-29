using UnityEngine;

namespace _Common.Ai.AiStates {
    public class AiAttackState : AiBaseState {
        public AiAttackState(AiBrain brain) : base(brain) { }


        private float _currentTime = .5f;
        private float _attackCooldown = .5f;

        public override void Tick() {
            Brain.RotateTowardsTarget();
            
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= _attackCooldown)) return;
            _currentTime = 0;
            _attackCooldown = Brain.AiData.AttackSettings.AttackCooldown;
            Brain.Attack();
        }
    }
}