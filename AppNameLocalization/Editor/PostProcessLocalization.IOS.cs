using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace AppNameLocalization.Editor
{
    public partial class PostProcessLocalization 
    {
        [PostProcessBuild(10000)]
        private static void OnPostProcess(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return;
            }
            string configPath = AppNamePathUtility.GetAppNameConfigsPath(Platform.IOS);
            AppNameConfigs appNameConfigs = AssetDatabase.LoadAssetAtPath<AppNameConfigs>(configPath);
            if (appNameConfigs == null)
            {
                throw new Exception($"Can not find AppNameConfigs form {configPath}");
            }
            if (string.IsNullOrEmpty(appNameConfigs.GetNormalConfig().Code))
            {
                throw new Exception($"Normal Config Can not be empty!");
            }
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict root = plist.root;
            root.SetString("CFBundleDisplayName", "${CFBundleDisplayName}");
            root.SetString("CFBundleDevelopmentRegion",appNameConfigs.GetNormalConfig().Code);
            File.WriteAllText(plistPath, plist.WriteToString());
            AddLocalizedStrings(appNameConfigs, pathToBuiltProject);
        }
        private static void AddLocalizedStrings(AppNameConfigs appNameConfigs, string path)
        {
            string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
            ChillyRoom.UnityEditor.iOS.Xcode.PBXProject proj = new ChillyRoom.UnityEditor.iOS.Xcode.PBXProject();
            proj.ReadFromFile(projPath);
            proj.ClearVariantGroupEntries("InfoPlist.strings");
            proj.WriteToFile(projPath);
            string localizationDir = path + "/Localization";
            if (!Directory.Exists(localizationDir))
            {
                Directory.CreateDirectory(localizationDir);
            }

            string content = "\"CFBundleDisplayName\" = \"{0}\";";
            foreach (AppNameConfig productNameConfig in appNameConfigs.Configs)
            {
                string dir = Path.Combine(localizationDir,productNameConfig.Code + ".lproj");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string stringsPath =Path.Combine(dir,"InfoPlist.strings");
                File.WriteAllText(stringsPath, string.Format(content, productNameConfig.AppName));
            }

            NativeLocale.AddLocalizedStringsIOS(path, Path.Combine(Application.dataPath, localizationDir));
            Directory.Delete(localizationDir,true);
        }
    }
}