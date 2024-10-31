#if BOTANGAMESERVICES_IAP_IOS|| BotanGameServices_IAP_GOOGLEPLAY || BotanGameServices_IAP_AMAZON || BotanGameServices_IAP_MACOS || BotanGameServices_IAP_WINDOWS
#define BotanGameServicesIAPEnabled
#endif

#if BotanGameServicesIAPEnabled
using UnityEngine.Purchasing;
#endif


namespace BotanGameServices.IAPSystem
{
    [System.Serializable]
    public class StoreProduct
    {
        public string productName;
        public ProductType productType;
        public string idGooglePlay;
        public string idAmazon;
        public string idIOS;
        public string idMac;
        public string idWindows;
        public int value;
        public string localizedPriceString = "-";
        public int price;
        public string isoCurrencyCode;
        public string receipt;
        internal string localizedDescription;
        internal string localizedTitle;
        internal bool active;
        internal SubscriptionInfo subscriptionInfo;



        public StoreProduct(string productName, ProductType productType, int value, string idGooglePlay, string idIOS, string idAmazon, string idMac, string idWindows)
        {
            this.productName = productName;
            this.productType = productType;
            this.value = value;
            this.idGooglePlay = idGooglePlay;
            this.idIOS = idIOS;
            this.idAmazon = idAmazon;
            this.idMac = idMac;
            this.idWindows = idWindows;
        }


        public StoreProduct()
        {
            productName = "";
            idGooglePlay = "";
            idIOS = "";
            idAmazon = "";
            idMac = "";
            idWindows = "";
            productType = ProductType.Consumable;
        }

#if BotanGameServicesIAPEnabled
        internal UnityEngine.Purchasing.ProductType GetProductType()
        {
            return (UnityEngine.Purchasing.ProductType)(int)productType;
        }
#endif

        internal string GetStoreID()
        {
#if BotanGameServices_IAP_MACOS
            return idMac;
#elif BOTANGAMESERVICES_IAP_IOS
            return idIOS;
#elif BotanGameServices_IAP_GOOGLEPLAY
            return idGooglePlay;
#elif BotanGameServices_IAP_AMAZON
            return idAmazon;
#elif BotanGameServices_IAP_WINDOWS
            return idWindows;
#else
            return "";
#endif
        }
    }

#if !BotanGameServicesIAPEnabled
    public class SubscriptionInfo
    {
    }

    public enum Result
    {

    }
#endif
}
