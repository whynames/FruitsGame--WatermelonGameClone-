using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.Datas.GameConfigs;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.UIs.StateMachine;
using _Assets.Scripts.Sprites;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs.Mods
{
    public class ModsMenu : MonoBehaviour
    {
        [SerializeField] private Transform slotParent;
        [SerializeField] private Button close;
        [SerializeField] private Image background;
        private ModSlot[] _slots;
        [Inject] private ModSlotFactory _modSlotFactory;
        [Inject] private IConfigLoader _configLoader;
        [Inject] private UIStateMachine _uiStateMachine;
        [Inject] private SpriteServiceSettings _spriteServiceSettings;

        public void Init(Sprite sprite) => background.sprite = sprite;

        private async void Start()
        {
            //Not the best solution, but it works.
            _configLoader.ConfigChanged += ChangeBackground;
            close.onClick.AddListener(SwitchToMainMenu);

            var sprite = _spriteServiceSettings.BackgroundSprite;
            Init(sprite);

            _slots = new ModSlot[_configLoader.AllConfigs.Count];

            for (int i = 0; i < _configLoader.AllConfigs.Count; i++)
            {
                await CreateSlot(i);
            }

            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots.Length == 1)
                {
                    _slots[0].SetNavigation(null, null, close);
                    continue;
                }

                if (i == 0)
                {
                    _slots[0].SetNavigation(_slots[^1].Selectable, _slots[1].Selectable, close);
                }
                else if (i == _slots.Length - 1)
                {
                    _slots[i].SetNavigation(_slots[i - 1].Selectable, _slots[0].Selectable, close);
                }
                else
                {
                    _slots[i].SetNavigation(_slots[i - 1].Selectable, _slots[i + 1].Selectable, close);
                }
            }

            var navigation = close.navigation;
            navigation.mode = Navigation.Mode.Explicit;
            navigation.selectOnUp = _slots[0].Selectable;
            navigation.selectOnDown = _slots[0].Selectable;
            close.navigation = navigation;
        }

        private async void ChangeBackground(GameConfig config)
        {
            var sprite = _spriteServiceSettings.BackgroundSprite;
            Init(sprite);
        }

        private void SwitchToMainMenu() => _uiStateMachine.SwitchState(UIStateType.MainMenu).Forget();

        private async UniTask CreateSlot(int index)
        {
            var iconPath = _configLoader.AllConfigs[index].ModIconPath;

            Sprite iconSprite;

            if (index == 0)
            {
                iconSprite = await SpriteHelper.CreateSpriteFromStreamingAssests(iconPath);
            }
            else
            {
                iconSprite = await SpriteHelper.CreateSprite(iconPath);
            }

            var slot = _modSlotFactory.CreateSlot();
            slot.transform.SetParent(slotParent);
            slot.transform.localScale = Vector3.one;
            slot.SetSlotData(iconSprite, _configLoader.AllConfigs[index].ModName);
            _slots[index] = slot;
        }

        private void OnDestroy() => _configLoader.ConfigChanged -= ChangeBackground;
    }
}