using UnityEngine;

namespace _Common.Core.Extensions {
    public static class DistanceExtensions {
        /// <summary>
        /// Determines if the distance between two positions is less than or equal to the specified range.
        /// </summary>
        public static bool IsInRangeOfSqr(this Vector3 firstPos, Vector3 secondPos, float range, bool ignoreY = false) {
            if (ignoreY) firstPos.y = secondPos.y;
            
            var distance = (firstPos - secondPos).sqrMagnitude;
            return distance <= range * range;
        }

    }
}