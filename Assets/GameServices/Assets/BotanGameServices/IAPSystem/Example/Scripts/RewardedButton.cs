
using UnityEngine;
using UnityEngine.UI;

public class RewardedButton : MonoBehaviour
{
    // [SerializeField]
    // string product;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        // var iap = FindFirstObjectByType<Ad>();
        button.onClick.AddListener(() =>
        {
            BotanGameServices.AdsPackage.API.ShowRewardedVideo((x) => { });
        });

    }
    private void OnDisable()
    {
        // var iap = FindFirstObjectByType<IAPSystem>();
        button.onClick.RemoveListener(() =>
        {
            BotanGameServices.AdsPackage.API.ShowRewardedVideo((x) => { });
        });
    }
}
