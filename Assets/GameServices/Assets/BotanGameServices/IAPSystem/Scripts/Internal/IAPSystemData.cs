using System.Collections.Generic;
using UnityEngine;

namespace BotanGameServices.IAPSystem.Local
{
    /// <summary>
    /// Used to save user settings made from Settings Window
    /// </summary>
    public class IAPSystemData : ScriptableObject
    {
        public bool debugIAP;
        public bool useReceiptValidate;

        public bool useForGooglePlay;
        public bool useForAmazon;
        public bool useForIos;
        public bool useForMac;
        public bool useForWindows;
        public List<StoreProduct> shopProducts = new List<StoreProduct>();
    }
}
