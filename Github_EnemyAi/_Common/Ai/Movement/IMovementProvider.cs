namespace _Common.Ai.Movement {
    public interface IMovementProvider {
        public void TickMovement(bool run);
        public void StopMovement();
    }
}