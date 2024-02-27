namespace _Common.iCare.Core.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick();

    }
}