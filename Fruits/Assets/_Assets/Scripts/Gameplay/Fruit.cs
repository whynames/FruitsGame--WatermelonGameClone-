using System;
using _Assets.Scripts.Services;
using _Assets.Scripts.Services.Factories;
using _Assets.Scripts.Services.StateMachine;
using UnityEngine;
using VContainer;
using PrimeTween;
using Cysharp.Threading.Tasks;
using _Assets.Scripts.Services.Audio;
using _Assets.Scripts.Services.Effects;

namespace _Assets.Scripts.Gameplay
{
    public class Fruit : MonoBehaviour
    {

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private PolygonColliderOptimizer polygonColliderOptimizer;
        public PolygonColliderOptimizer PolygonColliderOptimizer => polygonColliderOptimizer;
        public bool HasLanded => _landed;
        public bool HadDropped => _dropped;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
        protected internal bool Collided;
        protected internal int Index;
        private bool _dropped;
        private bool _landed;
        [Inject] protected FruitsFactory FruitsFactory;
        [Inject] protected AudioService AudioService;
        [Inject] protected FxService FxService;
        [Inject] protected ScoreService ScoreService;
        [Inject] protected ResetService ResetService;
        [Inject] protected ResumeGameService ResumeGameService;
        private Vector3 scale;

        public void SetIndex(int index) => Index = index;

        public void Drop()
        {
            _dropped = true;
            ResumeGameService.AddFruit(this);

        }


        internal void Initialize()
        {

        }
        private void Awake()
        {
            scale = transform.localScale;
        }

        public void Land() => _landed = true;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_dropped)
            {
                var rb = GetComponent<Rigidbody2D>();
                // var impulse = Vector3.Dot(other.contacts[0].normal, other.relativeVelocity);
                if (!rb.IsSleeping() && (Vector2.Dot(other.contacts[0].normal, Vector2.up) > 0))
                {
                    var impulse = Mathf.Clamp(Vector3.Magnitude(rb.velocity), 0, 3f);
                    if (impulse > 2f)
                    {
                        AudioService.PlayCollision();
                        Bounce(0.5f + impulse / 3);
                    }
                }

                _landed = true;
                if (other.gameObject.TryGetComponent(out Fruit fruit))
                {
                    if (fruit.HadDropped)
                    {
                        OnCollision(fruit);
                    }
                }
            }
        }

        protected virtual void OnCollision(Fruit fruit)
        {

            if (fruit.Index == Index)
            {
                if (Collided || fruit.Collided) return;
                Collided = true;
                fruit.Collided = true;
                var middle = (transform.position + fruit.transform.position) / 2f;


                FruitsFactory.Create(Index, middle);
                ResetService.RemoveFruit(this);
                ResetService.RemoveFruit(fruit);
                ResumeGameService.RemoveFruit(this);
                ResumeGameService.RemoveFruit(fruit);
                FxService.PlayDestroy(this.transform.position);
                FxService.PlayDestroy(fruit.transform.position);
                Destroy(gameObject);
                Destroy(fruit.gameObject);
            }
        }
        public void Bounce(float violence = 1)
        {

            var random = UnityEngine.Random.Range(-1, 1) > 0 ? 1 : -1;
            Sequence.Create(1)
                .Group(
                    Tween.PunchScale(this.transform, new Vector3(0.1f * random * violence, -0.2f * random * violence, 0.1f), 0.1f).OnComplete(() => transform.localScale = scale, warnIfTargetDestroyed: false)
                );
        }
        public void OnCreation()
        {
            Bounce(1.2f);

        }
        public void SetSprite(Sprite sprite)
        {
            // spriteRenderer.sprite = sprite;
            spriteRenderer.size = new Vector2(256, 256);
        }

        private void OnDestroy() => ResumeGameService.RemoveFruit(this);

    }
}