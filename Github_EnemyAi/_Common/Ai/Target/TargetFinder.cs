using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Ai.Target {
    public class TargetFinder : SerializedMonoBehaviour {
        [SerializeField] private ITarget target;

        public ITarget Get() {
            return target;
        }
    }
}