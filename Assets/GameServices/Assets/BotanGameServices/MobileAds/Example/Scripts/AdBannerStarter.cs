using System;
using UnityEngine;

public class AdBannerStarter : MonoBehaviour
{
    [SerializeField]
    BotanGameServices.AdsPackage.BannerPosition bannerPosition;

    internal void Initialize()
    {
        BotanGameServices.AdsPackage.API.ShowBanner(bannerPosition, BotanGameServices.AdsPackage.BannerType.Banner);

    }

    private void Awake()
    {
    }
}