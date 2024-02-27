using UnityEngine;

namespace _Common.Ai.Target {
    public interface ITarget {
        TargetFaction Faction { get; }
        Vector3 Position { get; }
        void Damage(Transform damageSource, int damageAmount, int ragdollForce);
    }
}