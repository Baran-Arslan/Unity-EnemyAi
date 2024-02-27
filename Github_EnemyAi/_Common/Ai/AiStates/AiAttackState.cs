using UnityEngine;

namespace _Common.Ai.AiStates {
    public class AiAttackState : AiBaseState {
        public AiAttackState(AiBrain brain) : base(brain) { }


        private float _currentTime = 5;

        public override void Tick() {
            _currentTime += Time.deltaTime;
            if (!(_currentTime >= Brain.AiData.AttackCooldown)) return;
            _currentTime = 0;
            Brain.Attack();
        }
    }
}