using UnityEngine;

namespace _Common.Ai.Movement {
    public interface IMovementProvider {
        public void TickMovement(Vector3 targetPos ,bool run);
        public void StopMovement();
    }
}