using _Assets.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Misc
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject] private GameStateMachine _gameStateMachine;
        
        private void Start() => _gameStateMachine.SwitchState(GameStateType.LoadSavedData).Forget();
    }
}