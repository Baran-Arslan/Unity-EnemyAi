using _Common.Ai.Target;
using UnityEngine;

namespace _Common.Ai {
    [CreateAssetMenu(menuName = "iCare/Attacks/Direct Damage")]
    public class MeleeAttackSO : AttackSO {
        public override void PerformAttack(Transform attacker, ITarget target, int damage, int ragdollPushForce) {
            target.Damage(attacker, damage, ragdollPushForce);
        }
    }
}