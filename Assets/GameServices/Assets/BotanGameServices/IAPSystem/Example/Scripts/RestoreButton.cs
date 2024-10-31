
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace BotanGameServices.IAPSystem.Local
{
    public class RestoreButton : MonoBehaviour
    {


        Button button;
        UnityAction action;
        private void Awake()
        {
            button = GetComponent<Button>();
        }
        private void OnEnable()
        {
            var iap = FindFirstObjectByType<IAPSystem>();
            action = () => { iap.RestorePurchases(OnAttemptCompleted, OnRestored); };
            button.onClick.AddListener(action);
        }

        private void OnAttemptCompleted(IAPOperationStatus arg0, string arg1, StoreProduct arg2)
        {
            var endColor = new Color(0, 0, 0, 0);

            if (arg0 == IAPOperationStatus.Success)
            {

            }
            else
            {
                Tween.Color(GetComponent<Image>(), Color.red, endColor, 1, Ease.OutSine);

            }
        }
        public void OnRestored()
        {
            var endColor = new Color(0, 0, 0, 0);
            Tween.Color(GetComponent<Image>(), Color.green, endColor, 1, Ease.OutSine);
        }

        private void OnDisable()
        {
            var iap = FindFirstObjectByType<IAPSystem>();
            button.onClick.RemoveListener(action);
        }
    }
}
