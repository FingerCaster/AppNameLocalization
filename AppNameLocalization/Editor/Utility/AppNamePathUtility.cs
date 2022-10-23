namespace AppNameLocalization.Editor
{
    public enum Platform
    {
        Android,
        IOS,
    }
    public static class AppNamePathUtility
    {
        private const string ConfigFolder = "Assets/AppNameLocalization/Editor/Res";

        public static string GetAppNameConfigsFolder(Platform platform)
        {
            return $"{ConfigFolder}/{platform}";
        }
        public static string GetAppNameConfigsPath(Platform platform)
        {
            return $"{ConfigFolder}/{platform}/AppNameConfigs.asset";
        }
    }
}