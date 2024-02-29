using UnityEngine;

namespace _Common.Core.Extensions {
    public static class TransformExtensions {
        public static void LookAtSmooth(this Transform transform, Vector3 targetPos, float speed, bool ignoreY = false) {
            if (ignoreY) targetPos.y = transform.position.y;
            var direction = targetPos - transform.position;
            if (direction == Vector3.zero) return;
            var rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
        

    }
}