using UnityEngine;

namespace _Common.Core.Extensions {
    public static class GetExtensions {
        public static Transform FindDeepChildren(this Transform parent, string name) {
            var result = SearchChildren(parent, name);
            if (result == null)
                Debug.LogError("Child '" + name + "' not found under parent '" + parent.name + "'");
            return result;
        }

        private static Transform SearchChildren(Transform parent, string name) {
            foreach (Transform child in parent) {
                if (child.name == name) return child;
                var result = SearchChildren(child, name);
                if (result != null) return result;
            }
            return null;
        }
    }
}