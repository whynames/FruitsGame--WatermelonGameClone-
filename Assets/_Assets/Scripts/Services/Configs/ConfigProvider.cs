using UnityEngine;

namespace _Assets.Scripts.Services.Configs
{
    public class ConfigProvider : MonoBehaviour
    {
        [SerializeField] private UIConfig uiConfig;
        [SerializeField] private FruitsConfig fruitsConfig;
        public UIConfig UIConfig => uiConfig;
        public FruitsConfig FruitsConfig => fruitsConfig;
    }
}