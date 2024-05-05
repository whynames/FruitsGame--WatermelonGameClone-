using UnityEngine;

namespace _Assets.Scripts.Services.Configs
{
    [CreateAssetMenu(fileName = "UI Config", menuName = "Configs/UI")]
    public class UIConfig : ScriptableObject
    {
        [SerializeField] private GameObject loadingCurtain;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject modsMenu;
        [SerializeField] private GameObject inGameMenu;
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private GameObject pauseMenu;
        public GameObject LoadingCurtain => loadingCurtain;
        public GameObject MainMenu => mainMenu;
        public GameObject ModsMenu => modsMenu;
        public GameObject InGameMenu => inGameMenu;
        public GameObject GameOverMenu => gameOverMenu;
        public GameObject SettingMenu => settingsMenu;
        public GameObject PauseMenu => pauseMenu;
    }
}