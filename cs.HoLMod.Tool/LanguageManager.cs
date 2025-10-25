using System.Collections.Generic;

namespace cs.HoLMod.Tool
{
    public class LanguageManager
    {
        public enum Language
        {
            Chinese,
            English
        }

        private static LanguageManager _instance;
        public static LanguageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LanguageManager();
                }
                return _instance;
            }
        }

        private Language _currentLanguage = Language.Chinese;
        public Language CurrentLanguage
        {
            get { return _currentLanguage; }
            set { _currentLanguage = value; }
        }

        private Dictionary<string, Dictionary<Language, string>> _languageDictionary = new Dictionary<string, Dictionary<Language, string>>();

        private LanguageManager()
        {
            // 初始化多语言字典
            InitializeLanguageDictionary();
        }

        private void InitializeLanguageDictionary()
        {
            // 添加基础语言条目
            AddLanguageEntry("windowTitle", "数据编辑器", "Data Editor");
            AddLanguageEntry("selectArray", "选择数组", "Select Array");
            AddLanguageEntry("originalValue", "原始值", "Original Value");
            AddLanguageEntry("newValue", "新值", "New Value");
            AddLanguageEntry("modify", "修改", "Modify");
            AddLanguageEntry("emptyArray", "数组为空", "Array is empty");
            AddLanguageEntry("chinese", "中文", "Chinese");
            AddLanguageEntry("english", "英文", "English");
            // 可以在这里添加更多的语言条目
        }

        // 添加新的语言条目
        public void AddLanguageEntry(string key, string chineseText, string englishText)
        {
            if (!_languageDictionary.ContainsKey(key))
            {
                _languageDictionary[key] = new Dictionary<Language, string>();
            }

            _languageDictionary[key][Language.Chinese] = chineseText;
            _languageDictionary[key][Language.English] = englishText;
        }

        // 添加新的语言支持
        public void AddNewLanguage(Language language, Dictionary<string, string> translations)
        {
            foreach (var kvp in translations)
            {
                if (!_languageDictionary.ContainsKey(kvp.Key))
                {
                    _languageDictionary[kvp.Key] = new Dictionary<Language, string>();
                }

                _languageDictionary[kvp.Key][language] = kvp.Value;
            }
        }

        // 获取翻译文本
        public string GetText(string key)
        {
            if (_languageDictionary.ContainsKey(key) && _languageDictionary[key].ContainsKey(_currentLanguage))
            {
                return _languageDictionary[key][_currentLanguage];
            }
            
            // 如果找不到对应的翻译，返回键本身
            return key;
        }

        // 切换语言
        public void ToggleLanguage()
        {
            if (_currentLanguage == Language.Chinese)
            {
                _currentLanguage = Language.English;
            }
            else
            {
                _currentLanguage = Language.Chinese;
            }
        }
    }
}
