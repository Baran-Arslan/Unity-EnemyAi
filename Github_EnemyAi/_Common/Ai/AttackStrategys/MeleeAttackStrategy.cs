using _Common.Ai.Target;
using UnityEngine;

namespace _Common.Ai.AttackStrategys {
    [CreateAssetMenu(menuName = "iCare/Attacks/Melee Attack")]
    public class MeleeAttackStrategy : AttackStrategy {
        public override void PerformAttack(Transform attackSender, Target.ITarget target, int damage, int ragdollPushForce) {
            target.Damage(attackSender, damage, ragdollPushForce);
        }
    }
}