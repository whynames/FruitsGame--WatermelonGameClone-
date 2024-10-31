using UnityEngine;

namespace _Assets.Scripts.Sprites
{
    public class SpriteService : MonoBehaviour
    {
        [SerializeField]
        SpriteServiceSettings spriteServiceSettings;

        public SpriteServiceSettings SpriteServiceSettings { get => spriteServiceSettings; set => spriteServiceSettings = value; }
    }
}