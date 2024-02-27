using UnityEngine;

namespace _Common.Core.Extensions {
    public static class GetExtensions {
        public static Transform GetChild(this Transform parent, string name) {
            foreach (Transform child in parent) {
                if (child.name == name) return child;
                var result = child.GetChild(name);
                if (result != null) return result;
            }

            return null;
        }
        
    }
}