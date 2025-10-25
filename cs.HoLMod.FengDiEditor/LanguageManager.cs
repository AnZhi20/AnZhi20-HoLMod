using System.Collections.Generic;
using System.Linq;

namespace cs.HoLMod.FengDiEditor
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
            // 中文翻译（代码中编写的中文，翻译的中文）
            translations["zh-CN"] = new Dictionary<string, string>
            {
                { "封地编辑器", "封地编辑器" },
                { "增加", "增加" },
                { "删除", "删除" },
                { "修改", "修改" },
                { "确认删除", "确认删除" },
                { "沃野", "沃野" },
                { "村落", "村落" },
                { "城镇", "城镇" },
                { "军营", "军营" },
                { "深湖", "深湖" },
                { "农庄", "农庄" },
                { "林场", "林场" },
                { "荒山", "荒山" },
                { "层级索引 (0-12):", "层级索引 (0-12):" },
                { "项目索引:", "项目索引:" },
                { "请输入要修改的字段值:", "请输入要修改的字段值:" },
                { "确认修改", "确认修改" },
                { "取消", "取消" },
                { "坐标", "坐标" },
                { "面积", "面积" },
                { "人口", "人口" },
                { "幸福", "幸福" },
                { "商业", "商业" },
                { "农业", "农业" },
                { "私兵数量", "私兵数量" },
                { "忠诚", "忠诚" },
                { "低级武器装备率", "低级武器装备率" },
                { "高级武器装备率", "高级武器装备率" },
                { "名字", "名字" },
                { "军饷", "军饷" },
                { "工程量", "工程量" },
                { "流民", "流民" },
                { "土地肥力", "土地肥力" },
                { "环境", "环境" },
                { "安全", "安全" },
                { "便捷", "便捷" },
                { "种植农户", "种植农户" },
                { "养殖农户", "养殖农户" },
                { "手工农户", "手工农户" },
                { "注：修改面积时会自动调整肥沃度", "注：修改面积时会自动调整肥沃度" },
                { "注：只有所属世家为-1时才可修改", "注：只有所属世家为-1时才可修改" },
                { "选择子分类:", "选择子分类:" }
            };

            // 英文翻译（代码中编写的中文，翻译的英文）
            translations["en-US"] = new Dictionary<string, string>
            {
                { "封地编辑器", "Territory Editor" },
                { "增加", "Add" },
                { "删除", "Delete" },
                { "修改", "Modify" },
                { "确认删除", "Confirm Delete" },
                { "沃野", "Field" },
                { "村落", "Village" },
                { "城镇", "Town" },
                { "军营", "Camp" },
                { "深湖", "Lake" },
                { "农庄", "Farm" },
                { "林场", "Forest" },
                { "荒山", "Mountain" },
                { "层级索引 (0-12):", "Level Index (0-12):" },
                { "项目索引:", "Item Index:" },
                { "请输入要修改的字段值:", "Please enter field values to modify:" },
                { "确认修改", "Confirm Modify" },
                { "取消", "Cancel" },
                { "坐标", "Coordinates" },
                { "面积", "Area" },
                { "人口", "Population" },
                { "幸福", "Happiness" },
                { "商业", "Commerce" },
                { "农业", "Agriculture" },
                { "私兵数量", "Private Soldiers" },
                { "忠诚", "Loyalty" },
                { "低级武器装备率", "Basic Equipment Rate" },
                { "高级武器装备率", "Advanced Equipment Rate" },
                { "名字", "Name" },
                { "军饷", "Military Pay" },
                { "工程量", "Construction Work" },
                { "流民", "Refugees" },
                { "土地肥力", "Soil Fertility" },
                { "环境", "Environment" },
                { "安全", "Safety" },
                { "便捷", "Convenience" },
                { "种植农户", "Farming Households" },
                { "养殖农户", "Livestock Households" },
                { "手工农户", "Artisan Households" },
                { "注：修改面积时会自动调整肥沃度", "Note: Fertility will adjust automatically when changing area" },
                { "注：只有所属世家为-1时才可修改", "Note: Can only modify when house owner is -1" },
                { "选择子分类:", "Select SubCategory:" }
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