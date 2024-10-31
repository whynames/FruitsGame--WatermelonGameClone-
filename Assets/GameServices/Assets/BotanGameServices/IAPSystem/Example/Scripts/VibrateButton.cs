using UnityEngine;
using UnityEngine.UI;

public class VibrateButton : MonoBehaviour
{
    // [SerializeField]
    // string product;
    [SerializeField]
    string sceneName;
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
#if UNITY_IPHONE
            
                Vibration.VibrateIOS_SelectionChanged();
#endif
        });

    }
    private void OnDisable()
    {
        // var iap = FindFirstObjectByType<IAPSystem>();
        button.onClick.RemoveListener(() =>
        {
#if UNITY_IPHONE
            
                Vibration.VibrateIOS_SelectionChanged();
#endif

        });
    }
}

