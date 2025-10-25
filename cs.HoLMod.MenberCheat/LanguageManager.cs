using System.Collections.Generic;
using System.Linq;

namespace cs.HoLMod.MenberCheat
{
    internal class LanguageManager
    {
        // 单例模式
        private static readonly LanguageManager instance = new LanguageManager();
        private string currentLanguageCode = "zh-CN"; // 默认中文
        private Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>();

        // 私有构造函数
        private LanguageManager()
        {
            // 初始化语言翻译字典
            InitializeTranslations();
        }

        // 获取单例实例
        public static LanguageManager Instance
        {
            get { return instance; }
        }

        // 初始化翻译
        private void InitializeTranslations()
        {
            // 中文翻译
            translations["zh-CN"] = new Dictionary<string, string>
            {
            };

            // 英文翻译
            translations["en-US"] = new Dictionary<string, string>
            {
            };
        }

        // 设置当前语言
        public void SetLanguage(string languageCode)
        {
            if (translations.ContainsKey(languageCode))
            {
                currentLanguageCode = languageCode;
            }
            else
            {
                // 如果请求的语言不存在，使用默认语言
                currentLanguageCode = "zh-CN";
            }
        }

        // 获取翻译
        public string GetTranslation(string key)
        {
            if (translations[currentLanguageCode].ContainsKey(key))
            {
                return translations[currentLanguageCode][key];
            }
            else
            {
                // 如果键不存在，返回键本身
                return key;
            }
        }

        // 获取支持的语言列表
        public List<string> GetSupportedLanguages()
        {
            return translations.Keys.ToList();
        }

        // 获取语言显示名称
        public string GetLanguageDisplayName(string languageCode)
        {
            switch (languageCode)
            {
                case "zh-CN": return "中文简体";
                case "en-US": return "English";
                // 可以在这里添加更多语言的显示名称
                default:
                    return languageCode;
            }
        }
    }
}