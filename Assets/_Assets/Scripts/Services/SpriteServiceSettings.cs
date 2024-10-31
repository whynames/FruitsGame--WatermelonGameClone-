using UnityEngine;

namespace _Assets.Scripts.Sprites
{
    [CreateAssetMenu(fileName = "SpriteServiceSettings", menuName = "Configs/SpriteServiceSettings", order = 0)]
    public class SpriteServiceSettings : ScriptableObject
    {
        [field: SerializeField]
        public Sprite PlayerSprite { get; set; }
        [field: SerializeField]
        public Sprite BackgroundSprite { get; set; }
        [field: SerializeField]
        public Sprite FieldSprite { get; set; }

        [field: SerializeField]
        public Sprite PauseMenu { get; set; }
    }
}