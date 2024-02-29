using Lean.Pool;
using UnityEngine;

namespace _Common.Core.Vfx {
    public static class VfxManager {
        public static void PlayAndRelease(this VFX vfx, Vector3 pos, Quaternion rot = default, Transform parent = null) {
            if (vfx == null || vfx.Vfx == null) return;
            vfx.Vfx.PlayAndRelease(pos, rot, parent, vfx.Scale);
        }
        private static void PlayAndRelease(this VfxSO vfx, Vector3 pos, Quaternion rot = default, Transform parent = null, Vector3 customScale = default) {
            var vfxParticle = LeanPool.Spawn(vfx.VfxPrefab);
            var vfxTransform = vfxParticle.transform;

            if (parent != null) {
                vfxTransform.SetParent(parent);
                vfxTransform.localPosition = pos;
            }
            else
                vfxTransform.position = pos;

            vfxTransform.rotation = rot;
            vfxTransform.localScale = customScale;
            LeanPool.Despawn(vfxParticle, vfx.VfxPrefab.main.duration);
        }
    }
}