using _Common.Ai.Target;
using _Common.Core.Vfx;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable CS0414 // Field is assigned but its value is never used

namespace _Common.Ai {
    public enum SetType {
        Manuel,
        ViaOtherScript
    }

    public class Health : MonoBehaviour, ITarget {
        public TargetFaction Faction { get; set; }
        [SerializeField] private SetType setType = SetType.ViaOtherScript;

        [ShowIf("setType", SetType.Manuel)]
        [SerializeField] private int maxHealth = 1000;

        private int _currentHealth;

        //[ShowIf("setType", SetType.Manuel)]
        public Transform MyBloodSpawnTransform;

        [ShowIf("setType", SetType.Manuel)]
        public VFX HitVFX;

        [ShowIf("setType", SetType.Manuel)]
        public VFX DeathVFX;

        [SerializeField] private UnityEvent onDamaged;
        public UnityEvent<Transform, int> onDead;

        private void OnEnable() {
            _currentHealth = maxHealth;
        }

        public void SetHealth(int newCurrentHealth, int newMaxHealth) {
            _currentHealth = newCurrentHealth;
            maxHealth = newMaxHealth;
        }


        public void Damage(Transform damageSource, int damageAmount, int ragdollForce) {
            if (_currentHealth <= 0) return;
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0) {
                Die(damageSource, ragdollForce);
            }
            else {
                onDamaged.Invoke();
                HitVFX.PlayAndRelease(GetTransform().position);
            }
        }



        private void Die(Transform damageSource, int ragdollForce) {
            onDead.Invoke(damageSource, ragdollForce);
            DeathVFX.PlayAndRelease(GetTransform().position);
        }
        public Transform GetTransform() {
            return MyBloodSpawnTransform == null ? transform : MyBloodSpawnTransform;
        }
    }
}

