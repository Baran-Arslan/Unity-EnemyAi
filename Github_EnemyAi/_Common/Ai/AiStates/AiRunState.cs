namespace _Common.Ai.AiStates {
    public class AiRunState : AiBaseState {
        public AiRunState(AiBrain brain) : base(brain) { }
        
        public override void Tick() {
            Brain.Move(true);
        }

        public override void OnExit() {
            Brain.StopMovement();
        }
    }
}