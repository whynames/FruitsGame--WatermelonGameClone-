#if BOTANGAMESERVICES_IAP_IOS|| BotanGameServices_IAP_GOOGLEPLAY || BotanGameServices_IAP_AMAZON || BotanGameServices_IAP_MACOS || BotanGameServices_IAP_WINDOWS
#define BotanGameServicesIAPEnabled
#endif

using BotanGameServices.Main;
using BotanGameServices.IAPSystem.Local;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace BotanGameServices.IAPSystem.Editor
{
    public class SettingsWindow : EditorWindow
    {
        private static string rootFolder;
        private static string rootWithoutAssets;

        private List<StoreProduct> localShopProducts;
        private IAPSystemData easyIAPData;
        private Vector2 scrollPosition;
        private string errorText = "";
        private bool useForGooglePlay;
        private bool useForAmazon;
        private bool useForIos;
        private bool useForMac;
        private bool useForWindows;
        private bool debug;
        private bool useReceiptValidation;
        private bool usePlaymaker;
        private bool useUVS;


        [MenuItem(SettingsWindowProperties.menuItem, false, 40)]
        private static void Init()
        {
            WindowLoader.LoadWindow<SettingsWindow>(new SettingsWindowProperties(), out rootFolder, out rootWithoutAssets);
        }


        private void OnEnable()
        {
            if (rootFolder == null)
            {
                rootFolder = WindowLoader.GetRootFolder(new SettingsWindowProperties(), out rootWithoutAssets);
            }


            easyIAPData = EditorUtilities.LoadOrCreateDataAsset<IAPSystemData>(rootFolder, Local.Constants.RESOURCES_FOLDER, Local.Constants.DATA_NAME_RUNTIME);

            debug = easyIAPData.debugIAP;
            useReceiptValidation = easyIAPData.useReceiptValidate;

            useForGooglePlay = easyIAPData.useForGooglePlay;
            useForAmazon = easyIAPData.useForAmazon;
            useForIos = easyIAPData.useForIos;
            useForMac = easyIAPData.useForMac;
            useForWindows = easyIAPData.useForWindows;

            localShopProducts = new List<StoreProduct>();
            for (int i = 0; i < easyIAPData.shopProducts.Count; i++)
            {
                localShopProducts.Add(easyIAPData.shopProducts[i]);
            }
        }


        private void SaveSettings()
        {
            if (useForGooglePlay)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_GOOGLEPLAY, false, BuildTargetGroup.Android);
#if BotanGameServicesIAPEnabled
                try
                {
                    UnityEditor.Purchasing.UnityPurchasingEditor.TargetAndroidStore(UnityEngine.Purchasing.AppStore.GooglePlay);
                }
                catch
                {
                    Debug.LogError("Enable In-App Purchases from Services");
                }
#endif
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_GOOGLEPLAY, true, BuildTargetGroup.Android);
            }

            if (useForAmazon)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_AMAZON, false, BuildTargetGroup.Android);
#if BotanGameServicesIAPEnabled
                try
                {
                    UnityEditor.Purchasing.UnityPurchasingEditor.TargetAndroidStore(UnityEngine.Purchasing.AppStore.AmazonAppStore);
                }
                catch
                {
                    Debug.LogError("Enable In-App Purchases from Services");
                }
#endif
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_AMAZON, true, BuildTargetGroup.Android);
            }

            if (useForIos)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_IOS, false, BuildTargetGroup.iOS);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_IOS, true, BuildTargetGroup.iOS);
            }

            if (useForMac)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_MACOS, false, BuildTargetGroup.Standalone);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_MACOS, true, BuildTargetGroup.Standalone);
            }

            if (useForWindows)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_WINDOWS, false, BuildTargetGroup.WSA);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_WINDOWS, true, BuildTargetGroup.WSA);
            }


            if (useReceiptValidation)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_VALIDATION, false, BuildTargetGroup.Android);
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_VALIDATION, false, BuildTargetGroup.iOS);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_VALIDATION, true, BuildTargetGroup.Android);
                BotanGameServices.Main.PreprocessorDirective.AddToPlatform(SettingsWindowProperties.BOTANGAMESERVICES_IAP_VALIDATION, true, BuildTargetGroup.iOS);
            }

            if (usePlaymaker)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToCurrent(BotanGameServices.Main.Constants.BOTANGAMESERVICES_PLAYMAKER_SUPPORT, false);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToCurrent(BotanGameServices.Main.Constants.BOTANGAMESERVICES_PLAYMAKER_SUPPORT, true);
            }

            if (useUVS)
            {
                BotanGameServices.Main.PreprocessorDirective.AddToCurrent(BotanGameServices.Main.Constants.BOTANGAMESERVICES_UVS_SUPPORT, false);
            }
            else
            {
                BotanGameServices.Main.PreprocessorDirective.AddToCurrent(BotanGameServices.Main.Constants.BOTANGAMESERVICES_UVS_SUPPORT, true);
            }

            easyIAPData.debugIAP = debug;
            easyIAPData.useReceiptValidate = useReceiptValidation;

            easyIAPData.useForGooglePlay = useForGooglePlay;
            easyIAPData.useForIos = useForIos;
            easyIAPData.useForAmazon = useForAmazon;
            easyIAPData.useForMac = useForMac;
            easyIAPData.useForWindows = useForWindows;

            easyIAPData.shopProducts = new List<StoreProduct>();
            for (int i = 0; i < localShopProducts.Count; i++)
            {
                easyIAPData.shopProducts.Add(localShopProducts[i]);
            }

            CreateEnumFile();

            EditorUtility.SetDirty(easyIAPData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height));
            EditorGUILayout.Space();
            debug = EditorGUILayout.Toggle("Debug", debug);
            useReceiptValidation = EditorGUILayout.Toggle("Use Receipt Validation", useReceiptValidation);
            if (useReceiptValidation)
            {
                GUILayout.Label("Go to Window > Unity IAP > IAP Receipt Validation Obfuscator,\nand paste your GooglePlay public key and click Obfuscate.");
            }
            EditorGUILayout.Space();
            GUILayout.Label("Enable Visual Scripting Tool:", EditorStyles.boldLabel);
            usePlaymaker = EditorGUILayout.Toggle("Playmaker", usePlaymaker);
            useUVS = EditorGUILayout.Toggle("Unity Visual Scripting", useUVS);
            EditorGUILayout.Space();
            GUILayout.Label("Select your platforms:", EditorStyles.boldLabel);
            useForGooglePlay = EditorGUILayout.Toggle("Google Play", useForGooglePlay);
            if (useForGooglePlay == true)
            {
                useForAmazon = false;
            }
            useForAmazon = EditorGUILayout.Toggle("Amazon", useForAmazon);
            if (useForAmazon)
            {
                useForGooglePlay = false;
            }
            useForIos = EditorGUILayout.Toggle("iOS", useForIos);
            useForMac = EditorGUILayout.Toggle("MacOS", useForMac);
            useForWindows = EditorGUILayout.Toggle("Windows Store", useForWindows);
            EditorGUILayout.Space();

            if (GUILayout.Button("Import Unity IAP SDK"))
            {
                BotanGameServices.Main.ImportRequiredPackages.ImportPackage("com.unity.purchasing", CompleteMethod);
            }
            EditorGUILayout.Space();

            if (useForGooglePlay || useForIos || useForAmazon || useForMac || useForWindows)
            {
                GUILayout.Label("In App Products Setup", EditorStyles.boldLabel);

                for (int i = 0; i < localShopProducts.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    localShopProducts[i].productName = EditorGUILayout.TextField("Product Name:", localShopProducts[i].productName);
                    localShopProducts[i].productName = Regex.Replace(localShopProducts[i].productName, @"^[\d-]*\s*", "");
                    localShopProducts[i].productName = localShopProducts[i].productName.Replace(" ", "");
                    localShopProducts[i].productName = localShopProducts[i].productName.Trim();
                    localShopProducts[i].productType = (ProductType)EditorGUILayout.EnumPopup("Product Type:", localShopProducts[i].productType);
                    localShopProducts[i].value = EditorGUILayout.IntField("Reward Value:", localShopProducts[i].value);

                    if (useForGooglePlay)
                    {
                        localShopProducts[i].idGooglePlay = EditorGUILayout.TextField("Google Play ID:", localShopProducts[i].idGooglePlay);
                    }

                    if (useForAmazon)
                    {
                        localShopProducts[i].idAmazon = EditorGUILayout.TextField("Amazon SKU:", localShopProducts[i].idAmazon);
                    }

                    if (useForIos)
                    {
                        localShopProducts[i].idIOS = EditorGUILayout.TextField("App Store (iOS) ID:", localShopProducts[i].idIOS);
                    }

                    if (useForMac)
                    {
                        localShopProducts[i].idMac = EditorGUILayout.TextField("Mac Store ID:", localShopProducts[i].idMac);
                    }

                    if (useForWindows)
                    {
                        localShopProducts[i].idWindows = EditorGUILayout.TextField("Windows Store ID:", localShopProducts[i].idWindows);
                    }

                    if (GUILayout.Button("Remove Product"))
                    {
                        localShopProducts.RemoveAt(i);
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUILayout.Space();
                }
                EditorGUILayout.Space();

                if (GUILayout.Button("Add new product"))
                {
                    localShopProducts.Add(new StoreProduct());
                }
            }

            GUILayout.Label(errorText);
            if (GUILayout.Button("Save"))
            {
                if (CheckForNull() == false)
                {
                    SaveSettings();
                    errorText = "Save Success";
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Example Scene"))
            {
                EditorSceneManager.OpenScene($"{rootFolder}/{SettingsWindowProperties.exampleScene}");
            }

            if (GUILayout.Button("Open Test Scene"))
            {
                EditorSceneManager.OpenScene($"{rootFolder}/{SettingsWindowProperties.testScene}");
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Documentation"))
            {

            }
            EditorGUILayout.Space();

            GUILayout.EndScrollView();
        }


        private void CompleteMethod(string message)
        {
            Debug.Log(message);
        }


        private bool CheckForNull()
        {
            for (int i = 0; i < localShopProducts.Count; i++)
            {
                if (String.IsNullOrEmpty(localShopProducts[i].productName))
                {
                    errorText = "Product name cannot be empty! Please fill all of them";
                    return true;
                }

                if (useForGooglePlay)
                {
                    if (String.IsNullOrEmpty(localShopProducts[i].idGooglePlay))
                    {
                        errorText = "Google Play ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }

                if (useForAmazon)
                {
                    if (String.IsNullOrEmpty(localShopProducts[i].idAmazon))
                    {
                        errorText = "Amazon SKU cannot be empty! Please fill all of them";
                        return true;
                    }
                }

                if (useForIos)
                {
                    if (String.IsNullOrEmpty(localShopProducts[i].idIOS))
                    {
                        errorText = "App Store ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }

                if (useForMac)
                {
                    if (String.IsNullOrEmpty(localShopProducts[i].idMac))
                    {
                        errorText = "Mac Store ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }

                if (useForWindows)
                {
                    if (String.IsNullOrEmpty(localShopProducts[i].idWindows))
                    {
                        errorText = "Windows Store ID cannot be empty! Please fill all of them";
                        return true;
                    }
                }
            }
            return false;
        }


        private void CreateEnumFile()
        {
            string text =
            "namespace BotanGameServices.IAPSystem\n" +
            "{\n" +
            "\tpublic enum ShopProductNames\n" +
            "\t{\n";
            for (int i = 0; i < localShopProducts.Count; i++)
            {
                text += $"\t\t{localShopProducts[i].productName},\n";
            }
            text += "\t}\n";
            text += "}";
            File.WriteAllText(Application.dataPath + $"/{rootWithoutAssets}/{Local.Constants.PRODUCT_NAMES_FILE}", text);
        }
    }
}
