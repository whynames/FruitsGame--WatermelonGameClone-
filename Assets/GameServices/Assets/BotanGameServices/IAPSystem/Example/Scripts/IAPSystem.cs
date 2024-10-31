
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BotanGameServices.IAPSystem.Local
{
    public class IAPSystem : MonoBehaviour
    {



        void Start()
        {
            BotanGameServices.IAPSystem.CodeBase.Initialize(InitializationComplete);
        }


        void RefreshUI()
        {


        }


        private void InitializationComplete(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
        {
            if (status == IAPOperationStatus.Success)
            {
                //IAP was successfully initialized
                //loop through all products
                for (int i = 0; i < shopProducts.Count; i++)
                {
                    //if remove ads was bought before, mark it as owned.
                    if (shopProducts[i].productName == "RemoveAds")
                    {
                        if (shopProducts[i].active)
                        {
                            PlayerPrefs.SetInt("RemoveAds", 1);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Error occurred: " + message);
            }
            // RefreshUI();
        }


        public void BuyCoins()
        {
            //this is the normal implementation
            ////but since your products will not have the same names, we will use the string version to avoid compile errors
            //BotanGameServices.EasyIAP.API.BuyProduct(ShopProductNames.Coins, ProductBought);


            ShopProductNames coinsProduct = BotanGameServices.IAPSystem.CodeBase.ConvertNameToShopProduct("Coins");
            BotanGameServices.IAPSystem.CodeBase.BuyProduct(coinsProduct, ProductBought);
        }


        public void BuyRemoveAds()
        {
            // this is the normal implementation
            //but since your products will not have the same names, we will use the string version to avoid compile errors
            //BotanGameServices.EasyIAP.API.BuyProduct(ShopProductNames.RemoveAds, ProductBought);

            ShopProductNames adsProduct = BotanGameServices.IAPSystem.CodeBase.ConvertNameToShopProduct("RemoveAds");
            BotanGameServices.IAPSystem.CodeBase.BuyProduct(adsProduct, ProductBought);
        }

        public void BuyProduct(string product)
        {
            ShopProductNames adsProduct = BotanGameServices.IAPSystem.CodeBase.ConvertNameToShopProduct(product);
            BotanGameServices.IAPSystem.CodeBase.BuyProduct(adsProduct, ProductBought);
        }
        private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                //since all consumable products reward the same coin, a simple type check is enough 
                if (product.productType == ProductType.Consumable)
                {

                }

                if (product.productName == "RemoveAds")
                {
                    PlayerPrefs.SetInt("RemoveAds", 1);

                    //disable ads here
                }
            }
            else
            {
                Debug.Log("Error occurred: " + message);
            }
            // RefreshUI();
        }

        public void RestorePurchases(UnityEngine.Events.UnityAction<IAPOperationStatus, string, StoreProduct> OnRestored, UnityAction OnRestoreCompleted)
        {
            BotanGameServices.IAPSystem.CodeBase.RestorePurchases(OnRestored, OnRestoreCompleted);

            //the product bought method can also be used as a callback for restore.
            //The callback is triggered for each product to be restored
            //This example is also valid, if you are using a single product bought example for all your purchases like this
            //BotanGameServices.EasyIAP.API.RestorePurchases(ProductBought);
        }

        public void RestorePurchases(UnityEngine.Events.UnityAction<IAPOperationStatus, string, StoreProduct> OnRestored)
        {
            BotanGameServices.IAPSystem.CodeBase.RestorePurchases(OnRestored);

            //the product bought method can also be used as a callback for restore.
            //The callback is triggered for each product to be restored
            //This example is also valid, if you are using a single product bought example for all your purchases like this
            //BotanGameServices.EasyIAP.API.RestorePurchases(ProductBought);
        }


        private void ProductRestored(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                if (product.productName == "RemoveAds")
                {
                    PlayerPrefs.SetInt("RemoveAds", 1);
                    //disable ads here
                }
            }
            else
            {
                Debug.Log("Error occurred: " + message);
            }
            // RefreshUI();
        }
    }
}
