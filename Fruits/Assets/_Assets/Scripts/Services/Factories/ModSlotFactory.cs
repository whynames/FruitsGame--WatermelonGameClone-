using _Assets.Scripts.Services.UIs.Mods;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.Services.Factories
{
    public class ModSlotFactory : MonoBehaviour
    {
        [SerializeField] private ModSlot modSlot;
        [Inject] private IObjectResolver _objectResolver;

        public ModSlot CreateSlot() => _objectResolver.Instantiate(modSlot);
    }
}