using UnityEngine;
using UnityEngine.UI;

namespace BotanGameServices.IAPSystem.Local
{
    public class IAPButton : MonoBehaviour
    {
        [SerializeField]
        string product;

        Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            var iap = FindFirstObjectByType<IAPSystem>();
            button.onClick.AddListener(() => { Debug.Log(product); iap.BuyProduct(product); });
        }
        private void OnDisable()
        {
            var iap = FindFirstObjectByType<IAPSystem>();
            button.onClick.RemoveListener(() => { iap.BuyProduct(product); });
        }
    }
}
