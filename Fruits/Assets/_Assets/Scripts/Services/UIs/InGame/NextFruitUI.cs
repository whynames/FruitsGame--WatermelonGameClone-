using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class NextFruitUI : MonoBehaviour
    {
        [SerializeField] private Image nextFruitImage;
        [Inject] private RandomNumberGenerator _randomNumberGenerator;
        [Inject] private FruitUIDataProvider _fruitUIDataProvider;

        public void Init() => _randomNumberGenerator.OnFruitPicked += NextFruitPicked;

        private void Start() => nextFruitImage.sprite = _fruitUIDataProvider.GetNextFruit();

        private void NextFruitPicked(int previous, int current, int next)
        {
            nextFruitImage.sprite = _fruitUIDataProvider.GetNextFruit();
        }

        private void OnDestroy() => _randomNumberGenerator.OnFruitPicked -= NextFruitPicked;
    }
}