namespace _Common.Ai.AiStates {
    public class AiRunState : AiBaseState {
        public AiRunState(AiBrain brain) : base(brain) { }
        
        public override void Tick() {
            Brain.MoveToTarget(true);
        }

        public override void OnExit() {
            Brain.StopMovement();
        }
    }
}