using _Assets.Scripts.Gameplay;
using _Assets.Scripts.Misc;
using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Configs;
using _Assets.Scripts.Services.Datas.GameConfigs;
using _Assets.Scripts.Services.Effects;
using _Assets.Scripts.Services.StateMachine;
using _Assets.Scripts.Services.Vibrations;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Assets.Scripts.Services.Factories
{
    public class FruitsFactory
    {
        private readonly IObjectResolver _objectResolver;
        private readonly ConfigProvider _configProvider;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ScoreService _scoreService;
        private readonly ResetService _resetService;
        private readonly AudioService _audioService;
        private readonly FxService _fxService;

        private readonly SpritesCacheService _spritesCacheService;
        private readonly VibrationService _vibrationService;
        [SerializeField]
        private int totalLength = 7;

        private FruitsFactory(IObjectResolver objectResolver, ConfigProvider configProvider, RandomNumberGenerator randomNumberGenerator, ScoreService scoreService, ResetService resetService, AudioService audioService, SpritesCacheService spritesCacheService, VibrationService vibrationService, FxService fxService)
        {
            _objectResolver = objectResolver;
            _configProvider = configProvider;
            _randomNumberGenerator = randomNumberGenerator;
            _scoreService = scoreService;
            _resetService = resetService;
            _audioService = audioService;
            _spritesCacheService = spritesCacheService;
            _vibrationService = vibrationService;
            _fxService = fxService;
        }

        public Rigidbody2D CreateToDrop(Vector3 position, Transform parent)
        {
            var index = _randomNumberGenerator.PickRandomFruit();

            var fruitPrefab = _configProvider.FruitsConfig.GetPrefab(index);
            var fruitInstance = _objectResolver.Instantiate(fruitPrefab.gameObject, position, Quaternion.identity, parent).GetComponent<Fruit>();
            fruitInstance.SetIndex(index);

            var rigidbody = fruitInstance.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;


            var sprite = fruitInstance.SpriteRenderer.sprite;
            fruitInstance.SetSprite(sprite);

            AddToResetService(fruitInstance);
            AddPolygonCollider(fruitInstance);

            return rigidbody;
        }

        public async UniTask Create(int index, Vector3 position)
        {
            index++;

            if (!_configProvider.FruitsConfig.HasPrefab(index))
            {
                AddScore(index--);
                Debug.LogError($"FruitsFactory: Index is out of range {index}");
                return;
            }
            var fruitPrefab = _configProvider.FruitsConfig.GetPrefab(index);
            var fruitInstance = _objectResolver.Instantiate(fruitPrefab.gameObject, position, Quaternion.identity)
                .GetComponent<Fruit>();
            var scale = fruitInstance.transform.localScale;
            Tween.Scale(fruitInstance.transform, Vector3.zero, scale, 0.2f, Ease.InSine).ToUniTask(coroutineRunner: fruitInstance);
            fruitInstance.SetIndex(index);
            fruitInstance.Drop();

            var sprite = fruitInstance.SpriteRenderer.sprite;
            fruitInstance.SetSprite(sprite);

            AddScore(index);
            AddToResetService(fruitInstance);
            AddPolygonCollider(fruitInstance);
            _audioService.PlayMerge(index);
            _fxService.PlayMerge(index, fruitInstance.transform.position, index);

            _audioService.PlaySong(index);
            _vibrationService.Vibrate();

            fruitInstance.Bounce();

            if (index >= totalLength)
            {
                fruitInstance.GetComponent<Collider2D>().enabled = false;
                fruitInstance.GetComponent<Rigidbody2D>().isKinematic = true;
                fruitInstance.SpriteRenderer.sortingOrder = fruitInstance.SpriteRenderer.sortingOrder + 3;
                _audioService.PlayWin();
                Tween.LocalPosition(fruitInstance.transform, Vector3.zero, 1, Ease.OutSine).ToUniTask();
                await Tween.Scale(fruitInstance.transform, Vector3.one * 2, 1, Ease.OutQuad).ToUniTask();
                await UniTask.Delay(1100);
                await Tween.Scale(fruitInstance.transform, Vector3.zero, 1, Ease.InQuad).ToUniTask();
                GameObject.Destroy(fruitInstance.gameObject);
            }



        }

        public void CreateContinue(int index, Vector3 position)
        {
            var fruitPrefab = _configProvider.FruitsConfig.GetPrefab(index);
            var fruitInstance = _objectResolver.Instantiate(fruitPrefab.gameObject, position, Quaternion.identity).GetComponent<Fruit>();
            fruitInstance.SetIndex(index);
            fruitInstance.Drop();
            fruitInstance.Land();

            var sprite = fruitInstance.SpriteRenderer.sprite;
            fruitInstance.SetSprite(sprite);

            AddToResetService(fruitInstance);
            AddPolygonCollider(fruitInstance);
        }

        public Rigidbody2D CreatePlayerContinue(Vector3 position, Transform parent)
        {
            var index = _randomNumberGenerator.Current;
            var fruitPrefab = _configProvider.FruitsConfig.GetPrefab(index);
            var fruitInstance = _objectResolver.Instantiate(fruitPrefab.gameObject, position, Quaternion.identity, parent).GetComponent<Fruit>();
            fruitInstance.SetIndex(index);

            var sprite = fruitInstance.SpriteRenderer.sprite;
            fruitInstance.SetSprite(sprite);

            AddToResetService(fruitInstance);
            AddPolygonCollider(fruitInstance);

            var rigidbody2D = fruitInstance.GetComponent<Rigidbody2D>();
            rigidbody2D.isKinematic = true;

            return rigidbody2D;
        }

        private void AddScore(int index)
        {
            //Previous + level (index) + points? 
            var currentLevel = index;
            var previousPoints = _configProvider.FruitsConfig.GetPoints(Mathf.Clamp(index - 1, 0, 1000));
            var totalPoints = currentLevel + previousPoints;
            _scoreService.AddScore(totalPoints);
        }

        private void AddToResetService(Fruit fruit) => _resetService.AddFruit(fruit);

        private void AddPolygonCollider(Fruit fruit)
        {

            //Maybe automatic polygon collider is not a good idea, custom polygon colliders or circle colliders will be considered.

            //Causing lag spikes
            //It's not a noticeable lag, so ignoring it for now
            // var collider = fruit.SpriteRenderer.gameObject.AddComponent<CircleCollider2D>();
            // var optimizer = fruit.PolygonColliderOptimizer;
            // optimizer.GetInitPaths(collider);
            // optimizer.OptimizePolygonCollider(2f);
        }
    }
}