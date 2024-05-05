using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.StateMachine
{
    public interface IGameState : IExitState
    {
        UniTask Enter();
    }

    public interface IExitState
    {
        void Exit();
    }
}