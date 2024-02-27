using _Common.Ai.Target;
using UnityEngine;

namespace _Common.Ai {
    public class Health : MonoBehaviour, ITarget {
        public TargetFaction Faction { get; }

        public Vector3 Position {
            get {
                Debug.Log("returning " + transform.position);
                return transform.position;
            }
        }

        public void Damage(Transform damageSource, int damageAmount, int ragdollForce) { }
    }
}