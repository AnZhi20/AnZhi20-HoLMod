using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.FengDiEditor
{
    internal class FengDiEditorWindow
    {
        private Rect windowRect = new Rect(20, 20, 500, 400);
        private Rect modifyWindowRect = new Rect(250, 100, 600, 500);
        private bool isWindowVisible = false;
        private bool isModifyWindowVisible = false;
        private string selectedCategory = "增加"; // 默认选中"增加"分类
        private string currentModifyType = ""; // 当前修改的类型
        private LanguageManager languageManager;
        
        // 修改窗口的输入字段
        private Dictionary<string, string> inputFields = new Dictionary<string, string>();
        private int selectedLevelIndex = 0; // 选中的层级索引
        private int selectedItemIndex = 0; // 选中的项目索引
        
        // 定义委托来处理子分类按钮点击事件
        public delegate void SubCategoryButtonClickHandler(string category, string subCategory);
        public event SubCategoryButtonClickHandler OnSubCategoryButtonClicked;
        
        // 定义委托来处理修改操作
        public delegate void ModifyConfirmHandler(string modifyType, int levelIndex, int itemIndex, Dictionary<string, string> values);
        public event ModifyConfirmHandler OnModifyConfirmed;

        public FengDiEditorWindow()
        {
            languageManager = LanguageManager.Instance;
        }

        public void ToggleWindowVisibility()
        {
            isWindowVisible = !isWindowVisible;
        }

        public bool IsWindowVisible()
        {    
            return isWindowVisible;
        }

        public void OpenModifyWindow(string modifyType)
        {    
            currentModifyType = modifyType;
            isModifyWindowVisible = true;
            // 清空输入字段
            inputFields.Clear();
            // 根据修改类型初始化输入字段
            InitializeInputFields(modifyType);
        }

        public void CloseModifyWindow()
        {    
            isModifyWindowVisible = false;
        }
        
        public string GetSelectedCategory()
        {    
            return selectedCategory;
        }

        private void InitializeInputFields(string modifyType)
        {    
            // 初始化通用字段
            inputFields["LevelIndex"] = "0";
            inputFields["ItemIndex"] = "0";
            
            // 根据不同类型初始化特定字段
            switch (modifyType)
            {    
                case "SubCategoryVillage":// 村落
                case "SubCategoryTown":    // 城镇
                    inputFields["坐标"] = "";
                    inputFields["面积"] = "";
                    inputFields["人口"] = "";
                    inputFields["幸福"] = "";
                    inputFields["商业"] = "";
                    inputFields["农业"] = "";
                    break;
                
                case "SubCategoryCamp":    // 军营
                    inputFields["坐标"] = "";
                    inputFields["面积"] = "";
                    inputFields["私兵数量"] = "";
                    inputFields["忠诚"] = "";
                    inputFields["低级武器装备率"] = "";
                    inputFields["高级武器装备率"] = "";
                    inputFields["名字"] = "";
                    inputFields["军饷"] = "";
                    break;
                
                case "SubCategoryField":    // 沃野
                    inputFields["坐标"] = "";
                    inputFields["面积"] = "";
                    inputFields["工程量"] = "";
                    break;
                
                case "SubCategoryLake":     // 深湖
                case "SubCategoryForest":   // 林场
                    inputFields["坐标"] = "";
                    inputFields["面积"] = "";
                    inputFields["工程量"] = "";
                    break;
                
                case "SubCategoryMountain": // 荒山
                    inputFields["坐标"] = "";
                    inputFields["面积"] = "";
                    inputFields["工程量"] = "";
                    inputFields["流民"] = "";
                    break;
                
                case "SubCategoryFarm":     // 农庄
                    inputFields["土地肥力"] = "";
                    inputFields["面积"] = "";
                    inputFields["环境"] = "";
                    inputFields["安全"] = "";
                    inputFields["便捷"] = "";
                    inputFields["种植农户"] = "";
                    inputFields["养殖农户"] = "";
                    inputFields["手工农户"] = "";
                    break;
            }
        }

        public void OnGUI()
        {
            // 主窗口
            if (isWindowVisible)
            {
                windowRect = GUI.Window(0, windowRect, DrawMainWindow, languageManager.GetTranslation("封地编辑器"), GUI.skin.window);
            }
            
            // 修改窗口
            if (isModifyWindowVisible)
            {
                string windowTitle = "";
                if (selectedCategory == "删除")
                {
                    windowTitle = $"删除 - {languageManager.GetTranslation(GetSubCategoryName(currentModifyType))}";
                }
                else
                {
                    windowTitle = $"修改 - {languageManager.GetTranslation(GetSubCategoryName(currentModifyType))}";
                }
                modifyWindowRect = GUI.Window(1, modifyWindowRect, DrawModifyWindow, windowTitle, GUI.skin.window);
            }
        }

        private void DrawMainWindow(int windowID)
        {
            // 分类按钮行
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(languageManager.GetTranslation("增加"), GUILayout.Width(100)))
            {
                selectedCategory = "增加";
            }
            if (GUILayout.Button(languageManager.GetTranslation("删除"), GUILayout.Width(100)))
            {
                selectedCategory = "删除";
            }
            if (GUILayout.Button(languageManager.GetTranslation("修改"), GUILayout.Width(100)))
            {
                selectedCategory = "修改";
            }
            GUILayout.EndHorizontal();

            // 小分类按钮网格
            GUILayout.Space(20);
            GUILayout.Label($"{languageManager.GetTranslation(selectedCategory)}" + " - {languageManager.GetTranslation('选择子分类:')}", GUILayout.Width(200));
            GUILayout.Space(10);

            GUILayout.BeginVertical();
            // 创建4x2的按钮网格
            for (int i = 0; i < 2; i++)    
            {    
                GUILayout.BeginHorizontal();
                for (int j = 0; j < 4; j++)    
                {    
                    int index = i * 4 + j;
                    string subCategoryKey = GetSubCategoryKey(index);
                    string subCategoryName = GetSubCategoryName(subCategoryKey);
                    if (GUILayout.Button(languageManager.GetTranslation(subCategoryName), GUILayout.Width(100), GUILayout.Height(40)))
                    {
                        OnSubCategoryButtonClick(selectedCategory, subCategoryKey);
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
            GUILayout.EndVertical();

            // 允许窗口拖动
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void DrawModifyWindow(int windowID)
        {    
            GUILayout.Space(10);
            
            // 层级和索引输入
            GUILayout.BeginHorizontal();
            GUILayout.Label(languageManager.GetTranslation("层级索引 (0-12):"), GUILayout.Width(120));
            inputFields["LevelIndex"] = GUILayout.TextField(inputFields["LevelIndex"], GUILayout.Width(100));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(languageManager.GetTranslation("项目索引:"), GUILayout.Width(120));
            inputFields["ItemIndex"] = GUILayout.TextField(inputFields["ItemIndex"], GUILayout.Width(100));
            GUILayout.EndHorizontal();
            
            // 对于删除操作，只显示索引输入，不显示其他字段
            if (selectedCategory != "删除")
            {
                GUILayout.Space(20);
                GUILayout.Label(languageManager.GetTranslation("请输入要修改的字段值:"), GUI.skin.label);
                GUILayout.Space(10);
                
                // 根据不同类型显示不同的字段输入
                switch (currentModifyType)
                {    
                    case "SubCategoryVillage":// 村落
                    case "SubCategoryTown":    // 城镇
                        DrawCommonFields(new string[] { "坐标", "面积", "人口", "幸福", "商业", "农业" });
                        break;
                    
                    case "SubCategoryCamp":    // 军营
                        DrawCommonFields(new string[] { "坐标", "面积", "私兵数量", "忠诚", "低级武器装备率", "高级武器装备率", "名字", "军饷" });
                        break;
                    
                    case "SubCategoryField":    // 沃野
                        DrawCommonFields(new string[] { "坐标", "面积", "工程量" });
                        GUILayout.Label(languageManager.GetTranslation("注：修改面积时会自动调整肥沃度"), GUI.skin.label);
                        break;
                    
                    case "SubCategoryLake":     // 深湖
                    case "SubCategoryForest":   // 林场
                        DrawCommonFields(new string[] { "坐标", "面积", "工程量" });
                        break;
                    
                    case "SubCategoryMountain": // 荒山
                        DrawCommonFields(new string[] { "坐标", "面积", "工程量", "流民" });
                        break;
                    
                    case "SubCategoryFarm":     // 农庄
                        DrawCommonFields(new string[] { "土地肥力", "面积", "环境", "安全", "便捷", "种植农户", "养殖农户", "手工农户" });
                        GUILayout.Label(languageManager.GetTranslation("注：只有所属世家为-1时才可修改"), GUI.skin.label);
                        break;
                }
            }
            else
            {
                // 对于删除操作，显示提示信息
                GUILayout.Space(20);
                GUILayout.Label("此操作将删除选定的地块并在相同位置添加相同面积的沃野。", GUI.skin.label);
                GUILayout.Label("请确认层级索引和项目索引是否正确。", GUI.skin.label);
            }
            
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            string confirmButtonText = selectedCategory == "删除" ? 
                languageManager.GetTranslation("确认删除") : 
                languageManager.GetTranslation("确认修改");
                
            if (GUILayout.Button(confirmButtonText, GUILayout.Width(100)))
            {    
                // 解析索引
                int levelIndex = 0;
                int itemIndex = 0;
                int.TryParse(inputFields["LevelIndex"], out levelIndex);
                int.TryParse(inputFields["ItemIndex"], out itemIndex);
                
                // 触发确认事件
                OnModifyConfirmed?.Invoke(currentModifyType, levelIndex, itemIndex, inputFields);
                
                // 关闭窗口
                CloseModifyWindow();
            }
            
            if (GUILayout.Button(languageManager.GetTranslation("取消"), GUILayout.Width(100)))
            {    
                CloseModifyWindow();
            }
            GUILayout.EndHorizontal();

            // 允许窗口拖动
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
        }

        private void DrawCommonFields(string[] fieldNames)
        {    
            foreach (string fieldName in fieldNames)
            {    
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{languageManager.GetTranslation(fieldName)}:", GUILayout.Width(120));
                if (inputFields.ContainsKey(fieldName))
                {    
                    inputFields[fieldName] = GUILayout.TextField(inputFields[fieldName], GUILayout.Width(300));
                }
                else
                {    
                    inputFields[fieldName] = GUILayout.TextField("", GUILayout.Width(300));
                }
                GUILayout.EndHorizontal();
            }
        }

        private string GetSubCategoryKey(int index)
        {    
            switch (index)
            {    
                case 0: return "SubCategoryField";
                case 1: return "SubCategoryVillage";
                case 2: return "SubCategoryTown";
                case 3: return "SubCategoryCamp";
                case 4: return "SubCategoryLake";
                case 5: return "SubCategoryFarm";
                case 6: return "SubCategoryForest";
                case 7: return "SubCategoryMountain";
                default: return "SubCategoryField";
            }
        }

        private string GetSubCategoryName(string subCategoryKey)
        {    
            switch (subCategoryKey)
            {    
                case "SubCategoryField": return languageManager.GetTranslation("沃野");
                case "SubCategoryVillage": return languageManager.GetTranslation("村落");
                case "SubCategoryTown": return languageManager.GetTranslation("城镇");
                case "SubCategoryCamp": return languageManager.GetTranslation("军营");
                case "SubCategoryLake": return languageManager.GetTranslation("深湖");
                case "SubCategoryFarm": return languageManager.GetTranslation("农庄");
                case "SubCategoryForest": return languageManager.GetTranslation("林场");
                case "SubCategoryMountain": return languageManager.GetTranslation("荒山");
                default: return languageManager.GetTranslation("沃野");
            }
        }

        private void OnSubCategoryButtonClick(string category, string subCategory)    
        {    
            // 触发事件，由Main.cs处理具体逻辑
            OnSubCategoryButtonClicked?.Invoke(category, subCategory);
        }
    }
}
