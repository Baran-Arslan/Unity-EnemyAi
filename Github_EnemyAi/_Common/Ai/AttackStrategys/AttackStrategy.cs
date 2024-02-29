using UnityEngine;

namespace _Common.Ai.AttackStrategys {
    public abstract class AttackStrategy : ScriptableObject {
        public abstract void PerformAttack(Transform attackSender, Target.ITarget target, int damage, int ragdollPushForce);
    }
}