using UnityEngine;

namespace _Common.Ai.Target {
    public interface ITarget {
        TargetFaction Faction { get; }
        void Damage(Transform damageSource, int damageAmount, int ragdollForce);
        Transform GetTransform();
    }
}