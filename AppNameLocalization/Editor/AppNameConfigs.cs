using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AppNameLocalization.Editor
{
    [Serializable]
    public class AppNameConfig
    {
        [SerializeField] private string m_Code;
        [SerializeField] private string m_AppName;

        public string Code
        {
            get => m_Code;
            set => m_Code = value;
        }

        public string AppName
        {
            get => m_AppName;
            set => m_AppName = value;
        }
    }

    public class AppNameConfigs : ScriptableObject
    {
        [SerializeField] private string m_NormalConfig;
        [SerializeField]
        private List<AppNameConfig> m_Configs = new List<AppNameConfig>();

        public List<AppNameConfig> Configs => m_Configs;

        public List<AppNameConfig> GetConfigsWithOutNormal()
        {
            return m_Configs.Where(_ => _.Code != m_NormalConfig).ToList();
        }

        public AppNameConfig GetNormalConfig()
        {
            return m_Configs.Find(_=>_.Code == m_NormalConfig);
        }
    }
}