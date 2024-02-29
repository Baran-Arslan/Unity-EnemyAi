using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Core.Vfx {
    
    [CreateAssetMenu(menuName = "iCare/Vfx")]
    public class VfxSO : ScriptableObject {
        [AssetsOnly]
        public ParticleSystem VfxPrefab;
    }
}