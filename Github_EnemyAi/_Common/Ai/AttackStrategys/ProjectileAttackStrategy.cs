using _Common.Core.Vfx;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

namespace _Common.Ai.AttackStrategys {
    [CreateAssetMenu(menuName = "iCare/Attacks/Projectile Attack")]
    public class ProjectileAttackStrategy : AttackStrategy {
        [SerializeField] private MeshRenderer projectilePrefab;
        [SerializeField] private float projectileSpeed = 35;

        [Header("VFX")]
        
        [SerializeField] private VFX collisionVfx;

        public override void PerformAttack(Transform attackSender, Target.ITarget target, int damage, int ragdollPushForce) {
            var projectile = LeanPool.Spawn(projectilePrefab);
            projectile.enabled = true;
            
            projectile.transform.position = attackSender.position;
            projectile.transform.LookAt(target.GetTransform());
            projectile.transform.DOMove(target.GetTransform().position, projectileSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnComplete(() => OnProjectileCollision(attackSender, target, damage, ragdollPushForce, projectile));
        }

        private void OnProjectileCollision(Transform attackSender, Target.ITarget target, int damage, int ragdollPushForce, MeshRenderer projectileRenderer) {
            target.Damage(attackSender, damage, ragdollPushForce);
            collisionVfx.PlayAndRelease(target.GetTransform().position);
            projectileRenderer.enabled = false;
            LeanPool.Despawn(projectileRenderer, 0.5f);
        }
    }
}