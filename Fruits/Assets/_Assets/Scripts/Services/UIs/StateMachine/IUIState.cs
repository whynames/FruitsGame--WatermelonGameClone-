using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.UIs.StateMachine
{
    public interface IUIState
    {
        UniTask Enter();
        void Exit();
    }
}