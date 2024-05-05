using UnityEngine;
using UnityEngine.EventSystems;

namespace _Assets.Scripts.Services.UIs
{
    public class SetSelectedUIElement : MonoBehaviour
    {
        [SerializeField] private GameObject selected;

        private void Awake() => EventSystem.current.SetSelectedGameObject(selected.gameObject);
    }
}