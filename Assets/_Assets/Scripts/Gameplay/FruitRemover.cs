namespace _Assets.Scripts.Gameplay
{
    public class FruitRemover : Fruit
    {
        protected override void OnCollision(Fruit fruit)
        {
            ResetService.RemoveFruit(this);
            ResetService.RemoveFruit(fruit);
            ResumeGameService.RemoveFruit(this);
            ResumeGameService.RemoveFruit(fruit);
            Destroy(gameObject);
            Destroy(fruit.gameObject);
        }
    }
}