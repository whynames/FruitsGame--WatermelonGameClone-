using UnityEngine;
using UnityEngine.UI;

namespace BotanGameServices.AdsPackage.Local
{
    public class AdSystem : MonoBehaviour
    {


        /// <summary>
        /// Initialize the ads
        /// </summary>
        void Awake()
        {
            BotanGameServices.AdsPackage.API.Initialize(BannerStart);
        }
        public void BannerStart()
        {
            if (TryGetComponent<AdBannerStarter>(out var bannerStarter))
            {
                bannerStarter.Initialize();
            }

        }

        void Start()
        {
            // coinsText.text = coins.ToString();
        }

        /// <summary>
        /// Show banner, assigned from inspector
        /// </summary>
        public void ShawBanner()
        {
            BotanGameServices.AdsPackage.API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
        }

        /// <summary>
        /// Hide banner assigned from inspector
        /// </summary>
        public void HideBanner()
        {
            BotanGameServices.AdsPackage.API.HideBanner();
        }


        /// <summary>
        /// Show Interstitial, assigned from inspector
        /// </summary>
        public void ShowInterstitial()
        {
            BotanGameServices.AdsPackage.API.ShowInterstitial();
        }

        /// <summary>
        /// Show rewarded video, assigned from inspector
        /// </summary>
        public void ShowRewardedVideo()
        {
            BotanGameServices.AdsPackage.API.ShowRewardedVideo(CompleteMethod);
        }


        /// <summary>
        /// This is for testing purpose
        /// </summary>


        /// <summary>
        /// Complete method called every time a rewarded video is closed
        /// </summary>
        /// <param name="completed">if true, the video was watched until the end</param>
        private void CompleteMethod(bool completed)
        {

            // coinsText.text = coins.ToString();
        }
    }
}
