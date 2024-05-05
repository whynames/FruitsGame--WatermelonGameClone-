using UnityEngine;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class CanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas inGameCanvas;
        [SerializeField] private int sortingOrder;

        private void Awake()
        {
            inGameCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            inGameCanvas.worldCamera = Camera.main;
            inGameCanvas.sortingOrder = sortingOrder;
            inGameCanvas.planeDistance = 10;
        }
    }
}