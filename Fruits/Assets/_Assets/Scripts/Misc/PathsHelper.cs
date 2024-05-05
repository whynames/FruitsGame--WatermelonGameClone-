using System.IO;
using UnityEngine;

namespace _Assets.Scripts.Misc
{
    public static class PathsHelper
    {
        public static string DataPath => Path.Combine(Application.persistentDataPath, "Data");
        public static string ModsPath => Path.Combine(Application.persistentDataPath, "Mods");
        public static string StreamingAssetsPath
        {
            get
            {
                string path = Application.streamingAssetsPath;
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
                path = "file://" + path;
#endif
                return path;
            }
        }
        public static string ModDataJson => "modData.json";
        public static string ConfigJson => "config.json";
        public static string PlayerDataJson => "playerData.json";
        public static string AudioSettingsDataJson => "audioSettingsData.json";
        public static string VibrationSettingsDataJson => "vibrationSettingsData.json";
        public static string ContinueDataJson => "continueData.json";
    }
}