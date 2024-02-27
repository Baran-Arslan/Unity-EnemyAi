namespace _Common.Ai.AiStates {
    public class AiWalkState : AiBaseState {
        public AiWalkState(AiBrain brain) : base(brain) { }

        public override void Tick() {
            Brain.Move(false);
        }

        public override void OnExit() {
            Brain.StopMovement();
        }
    }
}