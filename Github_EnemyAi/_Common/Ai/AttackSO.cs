using _Common.Ai.Target;
using UnityEngine;

namespace _Common.Ai {
    public abstract class AttackSO : ScriptableObject {
        public abstract void PerformAttack(Transform attacker, ITarget target, int damage, int ragdollPushForce);
    }
}