using _Assets.Scripts.Misc;
using _Assets.Scripts.Sprites;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private Image fruitIcon;
        [SerializeField] private Image background;
        [SerializeField] private Transform fruitTransform;
        [SerializeField] private float fruitRotationSpeed;
        [Inject] private SpriteServiceSettings _spriteServiceSettings;

        public void Init(Sprite background, Sprite icon)
        {
            this.background.sprite = background;
            // fruitIcon.sprite = icon;
        }

        // private void Update() => fruitTransform.Rotate(Vector3.forward * (fruitRotationSpeed * Time.deltaTime));

        public void Show() => gameObject.SetActive(true);

        public void Hide() => Destroy(gameObject);
    }
}