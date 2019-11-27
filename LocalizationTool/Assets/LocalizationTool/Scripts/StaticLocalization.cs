using System;
using System.Collections.Generic;

namespace Ultility.Localization
{
    /// <summary>
    /// Store static values for Editor only
    /// </summary>
    public static class StaticLocalization
    {
        public static Dictionary<string, string> DictStaticLocalizationData = new Dictionary<string, string>();
    }

    public static class LocalizationConfig
    {
        public static string DetectValue = "<?>";
    }
}

public enum ELocalizationLanguage
{
    EN = 0,
    VN = 1,
    DEU = 2,
    JP = 3,
    RU = 4,
    CH = 5,
    FR = 6,
    KO = 7,
}
