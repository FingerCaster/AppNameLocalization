using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

namespace AppNameLocalization.Editor
{
    public static class AppNameConfigCreateMenu
    {
        [MenuItem("Assets/Create/AppNameConfigs/Android")]
        private static void CreateAndroidAppNameConfigs()
        {
            CreateAppNameConfigs(Platform.Android);
        }
        [MenuItem("Assets/Create/AppNameConfigs/IOS")]
        private static void CreateIOSAppNameConfigs()
        {
            CreateAppNameConfigs(Platform.IOS);
        }
        private static void CreateAppNameConfigs(Platform platform)
        {
            string configPath = AppNamePathUtility.GetAppNameConfigsPath(platform);
            AppNameConfigs appNameConfigs = AssetDatabase.LoadAssetAtPath<AppNameConfigs>(configPath);
            if (appNameConfigs != null) return;
            if (!Directory.Exists(AppNamePathUtility.GetAppNameConfigsFolder(platform)))
            {
                Directory.CreateDirectory(AppNamePathUtility.GetAppNameConfigsFolder(platform));
            }
            appNameConfigs = ScriptableObject.CreateInstance<AppNameConfigs>();
            AssetDatabase.CreateAsset(appNameConfigs,configPath);
            EditorUtility.SetDirty(appNameConfigs);
            AssetDatabase.SaveAssets();
        }

    }
}