using _Common.iCare.Core.StateMachine;

namespace _Common.Ai.AiStates {
    public abstract class AiBaseState : IState {
        protected readonly AiBrain Brain;
        protected AiBaseState(AiBrain brain) => Brain = brain;
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Tick() { }
    }
}