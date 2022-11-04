using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.Android;
using UnityEngine;

namespace AppNameLocalization.Editor
{
    public partial class PostProcessLocalization : IPostGenerateGradleAndroidProject
    {
        
        public int callbackOrder { get; } = 10000;

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log(path);
#if UNITY_2019_1_OR_NEWER
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            string projPath = directoryInfo?.Parent?.FullName;
            if (string.IsNullOrEmpty(projPath))
            {
                throw new Exception("Path Parent folder is not exist.");
            }
            string launcherPath = projPath+"/launcher";
#else
            string launcherPath = path;
#endif
            string configPath = AppNamePathUtility.GetAppNameConfigsPath(Platform.Android);
            AppNameConfigs appNameConfigs = AssetDatabase.LoadAssetAtPath<AppNameConfigs>(configPath);
            if (appNameConfigs == null)
            {
                throw new Exception($"Can not find AppNameConfigs form {configPath}");
            }
            if (string.IsNullOrEmpty(appNameConfigs.GetNormalConfig().Code))
            {
                throw new Exception($"Normal Config Can not be empty!");
            }
            string stringXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
                               "<resources>\n" +
                               "    <string name=\"app_name\">{0}</string>\n" +
                               "</resources>";
            string normalStringsPath = launcherPath + "/src/main/res/values/strings.xml";
            if (File.Exists(normalStringsPath))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(normalStringsPath);
                XmlNode xmlNode = doc.SelectSingleNode("resources");
                XmlNodeList xnl = xmlNode?.ChildNodes;
                if (xnl == null)
                {
                    XmlElement stringElement = doc.CreateElement("string");
                    XmlAttribute nameAttribute = doc.CreateAttribute("name");
                    nameAttribute.InnerText = "app_name";
                    stringElement.SetAttributeNode(nameAttribute);
                    stringElement.InnerText = appNameConfigs.GetNormalConfig().AppName;
                }
                else
                {
                    XmlNode stringNameNode = null;
                    foreach (XmlNode xn1 in xnl)
                    {
                        if (xn1.Name == "string" && xn1.Attributes != null && xn1.Attributes[0].InnerText == "app_name")
                        {
                            stringNameNode = xn1;
                            break;
                        }
                    }

                    if (stringNameNode != null)
                    {
                        stringNameNode.InnerText = appNameConfigs.GetNormalConfig().AppName;
                    }
                }

                doc.Save(normalStringsPath);
            }

            List<AppNameConfig> configs = appNameConfigs.Configs;
            foreach (AppNameConfig appNameConfig in configs)
            {
                string dir = launcherPath + "/src/main/res/values-" + appNameConfig.Code;
                SetStringsFile(dir, "strings.xml", stringXML, appNameConfig.AppName);
            }
        }

        static void SetStringsFile(string folder, string fileName, string stringXML, string appName)
        {
            try
            {
                appName = appName.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "\\\"")
                    .Replace("'", "\\'");
                appName = appName.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                if (!File.Exists(folder + "/" + fileName))
                {
                    stringXML = string.Format(stringXML, appName);
                }
                else
                {
                    stringXML = File.ReadAllText(folder + "/" + fileName);

                    var pattern = "\"app_name\">(.*)<\\/string>";
                    var regexPattern = new System.Text.RegularExpressions.Regex(pattern);
                    if (regexPattern.IsMatch(stringXML))
                    {
                        stringXML = regexPattern.Replace(stringXML,
                            $"\"app_name\">{appName}</string>");
                    }
                    else
                    {
                        int idx = stringXML.IndexOf("<resources>", StringComparison.Ordinal);
                        if (idx > 0)
                            stringXML = stringXML.Insert(idx + "</resources>".Length,
                                $"\n    <string name=\"app_name\">{appName}</string>\n");
                    }
                }

                File.WriteAllText(folder + "/" + fileName, stringXML);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}