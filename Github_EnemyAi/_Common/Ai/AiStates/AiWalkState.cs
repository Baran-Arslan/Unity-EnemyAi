namespace _Common.Ai.AiStates {
    public class AiWalkState : AiBaseState {
        public AiWalkState(AiBrain brain) : base(brain) { }

        public override void OnEnter() {
            base.OnEnter();
            Brain.EnableIK(true);
        }

        public override void Tick() {
            Brain.MoveToTarget(false);
        }

        public override void OnExit() {
            Brain.StopMovement();
            Brain.EnableIK(false);
        }
    }
}