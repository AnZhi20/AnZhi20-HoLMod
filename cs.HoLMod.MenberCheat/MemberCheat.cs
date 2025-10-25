using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HoLMod;
using UnityEngine;

namespace cs.HoLMod.MenberCheat
{
    [BepInPlugin("cs.HoLMod.MemberCheat.AnZhi20", "HoLMod.MemberCheat", "2.0.0")]
    public class MenberCheat : BaseUnityPlugin
    {
        // 窗口设置
        private ConfigEntry<float> menuWidth;
        private ConfigEntry<float> menuHeight;
        private static Rect windowRect;
        private static bool showMenu = false;
        private static Vector2 scrollPosition = Vector2.zero;
        private static Vector2 familyScrollPosition = Vector2.zero; // 世家模式家族选择框的滚动位置
        private static bool blockGameInput = false;
        private static bool hasSelectedCategory = false; // 控制是否已经选择了分类的变量，初始为false
        private int currentMode = 0; // 0: 族人修改, 1: 门客修改, 2: 皇室修改, 3: 世家修改, 4: 农庄修改
        private string searchText = "";
        private string selectedMemberName = "";
        private int currentMemberSubMode = 0; // 0: 所有族人, 1: 族人家族, 2: 族人妻妾
        private int currentRoyaltySubMode = 0; // 0: 所有皇室, 1: 皇室主脉, 2: 皇家妻妾
        private int currentNobleSubMode = 0; // 0: 所有世家, 1: 世家主脉, 2: 世家妻妾
        private int currentFarmAreaIndex = 0; // 农庄区域索引，0: 大地图, 1-12: 各郡
        private int currentFarmIndex = 0; // 各农庄索引
        private List<string> currentFarmData = null; // 当前农庄数据引用
        private List<string> currentZhuangTouData = null; // 当前庄头数据引用
        private int currentFamilyIndex = 0; //世家索引

        // 用于显示工具提示的变量
        private bool showTooltip = false;
        private string tooltipText = "";
        private Vector2 tooltipPosition = Vector2.zero;

        // 定义族人家族模式变量 (前缀A_)
        private string A_reputation = "0"; // 声誉 
        private string A_reputationOriginal = "0"; // 原始声誉
        private string A_title = "0"; // 功名
        private string A_titleOriginal = "0"; // 原始功名
        private string A_power = "0"; // 体力
        private string A_powerOriginal = "0"; // 原始体力
        private string A_age = "0"; // 年龄
        private string A_ageOriginal = "0"; // 原始年龄
        private string A_health = "0"; // 健康
        private string A_healthOriginal = "0"; // 原始健康
        private string A_mood = "0"; // 心情
        private string A_moodOriginal = "0"; // 原始心情
        private string A_charm = "0"; // 魅力
        private string A_charmOriginal = "0"; // 原始魅力
        private string A_luck = "0"; // 幸运
        private string A_luckOriginal = "0"; // 原始幸运
        private string A_preference = "0"; // 喜好
        private string A_preferenceOriginal = "0"; // 原始喜好
        private string A_character = "0"; // 品性
        private string A_characterOriginal = "0"; // 原始品性
        private string A_talent = "0"; // 天赋
        private string A_talentOriginal = "0"; // 原始天赋
        private string A_talentPoint = "0"; // 天赋点
        private string A_talentPointOriginal = "0"; // 原始天赋点
        private string A_skill = "0"; // 技能
        private string A_skillOriginal = "0"; // 原始技能
        private string A_skillPoint = "0"; // 技能点
        private string A_skillPointOriginal = "0"; // 原始技能点
        private string A_intelligence = "0"; // 文才
        private string A_intelligenceOriginal = "0"; // 原始文才
        private string A_weapon = "0"; // 武才
        private string A_weaponOriginal = "0"; // 原始武才
        private string A_business = "0"; // 商才
        private string A_businessOriginal = "0"; // 原始商才
        private string A_art = "0"; // 艺才
        private string A_artOriginal = "0"; // 原始艺才
        private string A_strategy = "0"; // 计谋
        private string A_strategyOriginal = "0"; // 原始计谋
        private string A_JueWei = "0"; // 爵位
        private string A_JueWeiOriginal = "0"; // 原始爵位
        private string A_FengDi = "0"; // 封地
        private string A_FengDiOriginal = "0"; // 原始封地
        private string A_officialRank = "0@0@0@-1@-1"; // 官职
        private string A_officialRankOriginal = "0@0@0@-1@-1"; // 原始官职
        
        // 定义族人妻妾模式变量 (前缀B_)
        private string B_reputation = "0"; // 声誉 
        private string B_reputationOriginal = "0"; // 原始声誉
        private string B_power = "0"; // 体力
        private string B_powerOriginal = "0"; // 原始体力
        private string B_age = "0"; // 年龄
        private string B_ageOriginal = "0"; // 原始年龄
        private string B_health = "0"; // 健康
        private string B_healthOriginal = "0"; // 原始健康
        private string B_mood = "0"; // 心情
        private string B_moodOriginal = "0"; // 原始心情
        private string B_charm = "0"; // 魅力
        private string B_charmOriginal = "0"; // 原始魅力
        private string B_luck = "0"; // 幸运
        private string B_luckOriginal = "0"; // 原始幸运
        private string B_preference = "0"; // 喜好
        private string B_preferenceOriginal = "0"; // 原始喜好
        private string B_character = "0"; // 品性
        private string B_characterOriginal = "0"; // 原始品性
        private string B_talent = "0"; // 天赋
        private string B_talentOriginal = "0"; // 原始天赋
        private string B_talentPoint = "0"; // 天赋点
        private string B_talentPointOriginal = "0"; // 原始天赋点
        private string B_skill = "0"; // 技能
        private string B_skillOriginal = "0"; // 原始技能
        private string B_skillPoint = "0"; // 技能点
        private string B_skillPointOriginal = "0"; // 原始技能点
        private string B_intelligence = "0"; // 文才
        private string B_intelligenceOriginal = "0"; // 原始文才
        private string B_weapon = "0"; // 武才
        private string B_weaponOriginal = "0"; // 原始武才
        private string B_business = "0"; // 商才
        private string B_businessOriginal = "0"; // 原始商才
        private string B_art = "0"; // 艺才
        private string B_artOriginal = "0"; // 原始艺才
        private string B_strategy = "0"; // 计谋
        private string B_strategyOriginal = "0"; // 原始计谋
        
        // 定义门客模式变量 (前缀C_)
        private string C_reputation = "0"; // 声誉 
        private string C_reputationOriginal = "0"; // 原始声誉
        private string C_power = "0"; // 体力
        private string C_powerOriginal = "0"; // 原始体力
        private string C_age = "0"; // 年龄
        private string C_ageOriginal = "0"; // 原始年龄
        private string C_health = "0"; // 健康
        private string C_healthOriginal = "0"; // 原始健康
        private string C_mood = "0"; // 心情
        private string C_moodOriginal = "0"; // 原始心情
        private string C_charm = "0"; // 魅力
        private string C_charmOriginal = "0"; // 原始魅力
        private string C_luck = "0"; // 幸运
        private string C_luckOriginal = "0"; // 原始幸运
        private string C_character = "0"; // 品性
        private string C_characterOriginal = "0"; // 原始品性
        private string C_talent = "0"; // 天赋
        private string C_talentOriginal = "0"; // 原始天赋
        private string C_talentPoint = "0"; // 天赋点
        private string C_talentPointOriginal = "0"; // 原始天赋点
        private string C_skill = "0"; // 技能
        private string C_skillOriginal = "0"; // 原始技能
        private string C_skillPoint = "0"; // 技能点
        private string C_skillPointOriginal = "0"; // 原始技能点
        private string C_intelligence = "0"; // 文才
        private string C_intelligenceOriginal = "0"; // 原始文才
        private string C_weapon = "0"; // 武才
        private string C_weaponOriginal = "0"; // 原始武才
        private string C_business = "0"; // 商才
        private string C_businessOriginal = "0"; // 原始商才
        private string C_art = "0"; // 艺才
        private string C_artOriginal = "0"; // 原始艺才
        private string C_strategy = "0"; // 计谋
        private string C_strategyOriginal = "0"; // 原始计谋
        private string C_MenKe_Reward = "0"; // 门客酬劳
        private string C_MenKe_RewardOriginal = "0"; // 原始门客酬劳
        
        // 定义皇室主脉模式变量 (前缀D_)
        private string D_reputation = "0"; // 声誉 
        private string D_reputationOriginal = "0"; // 原始声誉
        private string D_age = "0"; // 年龄
        private string D_ageOriginal = "0"; // 原始年龄
        private string D_health = "0"; // 健康
        private string D_healthOriginal = "0"; // 原始健康
        private string D_mood = "0"; // 心情
        private string D_moodOriginal = "0"; // 原始心情
        private string D_charm = "0"; // 魅力
        private string D_charmOriginal = "0"; // 原始魅力
        private string D_luck = "0"; // 幸运
        private string D_luckOriginal = "0"; // 原始幸运
        private string D_preference = "0"; // 喜好
        private string D_preferenceOriginal = "0"; // 原始喜好
        private string D_character = "0"; // 品性
        private string D_characterOriginal = "0"; // 原始品性
        private string D_talent = "0"; // 天赋
        private string D_talentOriginal = "0"; // 原始天赋
        private string D_talentPoint = "0"; // 天赋点
        private string D_talentPointOriginal = "0"; // 原始天赋点
        private string D_skill = "0"; // 技能
        private string D_skillOriginal = "0"; // 原始技能
        private string D_skillPoint = "0"; // 技能点
        private string D_skillPointOriginal = "0"; // 原始技能点
        private string D_intelligence = "0"; // 文才
        private string D_intelligenceOriginal = "0"; // 原始文才
        private string D_weapon = "0"; // 武才
        private string D_weaponOriginal = "0"; // 原始武才
        private string D_business = "0"; // 商才
        private string D_businessOriginal = "0"; // 原始商才
        private string D_art = "0"; // 艺才
        private string D_artOriginal = "0"; // 原始艺才
        private string D_strategy = "0"; // 计谋
        private string D_strategyOriginal = "0"; // 原始计谋
        
        // 定义皇家妻妾模式变量 (前缀E_)
        private string E_reputation = "0"; // 声誉 
        private string E_reputationOriginal = "0"; // 原始声誉
        private string E_age = "0"; // 年龄
        private string E_ageOriginal = "0"; // 原始年龄
        private string E_health = "0"; // 健康
        private string E_healthOriginal = "0"; // 原始健康
        private string E_mood = "0"; // 心情
        private string E_moodOriginal = "0"; // 原始心情
        private string E_charm = "0"; // 魅力
        private string E_charmOriginal = "0"; // 原始魅力
        private string E_luck = "0"; // 幸运
        private string E_luckOriginal = "0"; // 原始幸运
        private string E_preference = "0"; // 喜好
        private string E_preferenceOriginal = "0"; // 原始喜好
        private string E_character = "0"; // 品性
        private string E_characterOriginal = "0"; // 原始品性
        private string E_talent = "0"; // 天赋
        private string E_talentOriginal = "0"; // 原始天赋
        private string E_talentPoint = "0"; // 天赋点
        private string E_talentPointOriginal = "0"; // 原始天赋点
        private string E_skill = "0"; // 技能
        private string E_skillOriginal = "0"; // 原始技能
        private string E_skillPoint = "0"; // 技能点
        private string E_skillPointOriginal = "0"; // 原始技能点
        private string E_intelligence = "0"; // 文才
        private string E_intelligenceOriginal = "0"; // 原始文才
        private string E_weapon = "0"; // 武才
        private string E_weaponOriginal = "0"; // 原始武才
        private string E_business = "0"; // 商才
        private string E_businessOriginal = "0"; // 原始商才
        private string E_art = "0"; // 艺才
        private string E_artOriginal = "0"; // 原始艺才
        private string E_strategy = "0"; // 计谋
        private string E_strategyOriginal = "0"; // 原始计谋
        
        // 定义世家主脉模式变量 (前缀F_)
        private string F_reputation = "0"; // 声誉 
        private string F_reputationOriginal = "0"; // 原始声誉
        private string F_age = "0"; // 年龄
        private string F_ageOriginal = "0"; // 原始年龄
        private string F_health = "0"; // 健康
        private string F_healthOriginal = "0"; // 原始健康
        private string F_mood = "0"; // 心情
        private string F_moodOriginal = "0"; // 原始心情
        private string F_charm = "0"; // 魅力
        private string F_charmOriginal = "0"; // 原始魅力
        private string F_luck = "0"; // 幸运
        private string F_luckOriginal = "0"; // 原始幸运
        private string F_preference = "0"; // 喜好
        private string F_preferenceOriginal = "0"; // 原始喜好
        private string F_character = "0"; // 品性
        private string F_characterOriginal = "0"; // 原始品性
        private string F_talent = "0"; // 天赋
        private string F_talentOriginal = "0"; // 原始天赋
        private string F_talentPoint = "0"; // 天赋点
        private string F_talentPointOriginal = "0"; // 原始天赋点
        private string F_skill = "0"; // 技能
        private string F_skillOriginal = "0"; // 原始技能
        private string F_skillPoint = "0"; // 技能点
        private string F_skillPointOriginal = "0"; // 原始技能点
        private string F_intelligence = "0"; // 文才
        private string F_intelligenceOriginal = "0"; // 原始文才
        private string F_weapon = "0"; // 武才
        private string F_weaponOriginal = "0"; // 原始武才
        private string F_business = "0"; // 商才
        private string F_businessOriginal = "0"; // 原始商才
        private string F_art = "0"; // 艺才
        private string F_artOriginal = "0"; // 原始艺才
        private string F_strategy = "0"; // 计谋
        private string F_strategyOriginal = "0"; // 原始计谋
        
        // 定义世家妻妾模式变量 (前缀G_)
        private string G_reputation = "0"; // 声誉 
        private string G_reputationOriginal = "0"; // 原始声誉
        private string G_age = "0"; // 年龄
        private string G_ageOriginal = "0"; // 原始年龄
        private string G_health = "0"; // 健康
        private string G_healthOriginal = "0"; // 原始健康
        private string G_mood = "0"; // 心情
        private string G_moodOriginal = "0"; // 原始心情
        private string G_charm = "0"; // 魅力
        private string G_charmOriginal = "0"; // 原始魅力
        private string G_luck = "0"; // 幸运
        private string G_luckOriginal = "0"; // 原始幸运
        private string G_preference = "0"; // 喜好
        private string G_preferenceOriginal = "0"; // 原始喜好
        private string G_character = "0"; // 品性
        private string G_characterOriginal = "0"; // 原始品性
        private string G_talent = "0"; // 天赋
        private string G_talentOriginal = "0"; // 原始天赋
        private string G_talentPoint = "0"; // 天赋点
        private string G_talentPointOriginal = "0"; // 原始天赋点
        private string G_skill = "0"; // 技能
        private string G_skillOriginal = "0"; // 原始技能
        private string G_skillPoint = "0"; // 技能点
        private string G_skillPointOriginal = "0"; // 原始技能点
        private string G_intelligence = "0"; // 文才
        private string G_intelligenceOriginal = "0"; // 原始文才
        private string G_weapon = "0"; // 武才
        private string G_weaponOriginal = "0"; // 原始武才
        private string G_business = "0"; // 商才
        private string G_businessOriginal = "0"; // 原始商才
        private string G_art = "0"; // 艺才
        private string G_artOriginal = "0"; // 原始艺才
        private string G_strategy = "0"; // 计谋
        private string G_strategyOriginal = "0"; // 原始计谋

        // 定义农庄修改项 (前缀H_)
        private string H_age = "0"; // 年龄
        private string H_management = "0"; // 管理
        private string H_loyalty = "0"; //忠诚
        private string H_character = "0"; // 品性
        private string H_ageOriginal = "0"; // 原始年龄
        private string H_managementOriginal = "0"; // 原始管理
        private string H_loyaltyOriginal = "0"; // 原始忠诚
        private string H_characterOriginal = "0"; // 原始品性
        private string H_area = "0"; // 区域
        private string H_areaOriginal = "0"; // 原始区域
        private string H_farmersPlanting = "0"; // 种植农户数量
        private string H_farmersPlantingOriginal = "0"; // 原始种植农户数量
        private string H_farmersBreeding = "0"; // 养殖农户数量
        private string H_farmersBreedingOriginal = "0"; // 原始养殖农户数量
        private string H_farmersCrafting = "0"; // 手工农户数量
        private string H_farmersCraftingOriginal = "0"; // 原始手工农户数量
        private string H_zhuangTouAge = "0"; // 庄头年龄
        private string H_zhuangTouAgeOriginal = "0"; // 原始庄头年龄
        private string H_zhuangTouManagement = "0"; // 庄头管理
        private string H_zhuangTouManagementOriginal = "0"; // 原始庄头管理
        private string H_zhuangTouLoyalty = "0"; // 庄头忠诚
        private string H_zhuangTouLoyaltyOriginal = "0"; // 原始庄头忠诚
        private string H_zhuangTouCharacter = "0"; // 庄头品性
        private string H_zhuangTouCharacterOriginal = "0"; // 原始庄头品性
        private string H_maxResidents = "0"; // 可居住上限
        private string H_maxResidentsOriginal = "0"; // 原始可居住上限
        
        // 保存庄头人物编号
        private string currentZhuangTouId = ""; // 庄头人物编号，用于判断是否存在庄头
        
        // 从游戏中获取实际数据
        private List<string> memberList = new List<string>(); // 初始化空列表，实际数据在Update或OnGUI中加载
        private List<string> filteredMembers = new List<string>();

        private void Awake()
        {
            Logger.LogInfo("族人门客修改器已加载！");
            
            // 窗口配置
            menuWidth = Config.Bind("界面设置", "窗口宽度", 1000f, "族人门客修改器窗口的宽度");
            menuHeight = Config.Bind("界面设置", "窗口高度", 1400f, "族人门客修改器窗口的高度");
            
            windowRect = new Rect(100, 200, menuWidth.Value, menuHeight.Value);
            filteredMembers = memberList;
        }        
        private void Update()
        {
            // 按F3键切换窗口显示
            if (UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                showMenu = !showMenu;
                blockGameInput = showMenu;
                // 当打开窗口时，重置分类选择状态
                if (showMenu)
                {
                    hasSelectedCategory = false;
                }
                Logger.LogInfo(showMenu ? "族人门客修改器窗口已打开" : "族人门客修改器窗口已关闭");
            }
            
            // 阻止游戏输入当窗口显示时
            if (blockGameInput)
            {
                // 阻止鼠标滚轮
                if (UnityEngine.Input.mouseScrollDelta.y != 0)
                {
                    UnityEngine.Input.ResetInputAxes();
                }
                
                // 阻止鼠标点击
                if (UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButton(1) || UnityEngine.Input.GetMouseButton(2))
                {
                    UnityEngine.Input.ResetInputAxes();
                }
                
                // 阻止键盘输入（保留F3键用于关闭窗口）
                if (UnityEngine.Input.anyKeyDown && !UnityEngine.Input.GetKeyDown(KeyCode.F3))
                {
                    UnityEngine.Input.ResetInputAxes();
                }
            }
        }
        
        private void OnGUI()
        {
            // 重置工具提示变量
            showTooltip = false;
            
            if (!showMenu)
                return;
            
            // 允许窗口拖拽出游戏窗口 - 移除边界限制
            
            // 设置GUI样式
            GUI.skin.window.fontSize = 12;
            GUI.skin.window.padding = new RectOffset(20, 20, 10, 10);
            
            GUI.skin.label.fontSize = 12;
            GUI.skin.button.fontSize = 12;
            GUI.skin.textField.fontSize = 12;
            
            // 保存窗口背景色并设置为半透明
            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);
            
            // 显示一个半透明的背景遮罩，防止操作游戏界面
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(0, 0, 0, 0.1f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.EndGroup();
            
            // 创建窗口（无标题栏）
            windowRect = GUI.Window(0, windowRect, DrawWindow, "", UnityEngine.GUI.skin.window);
            windowRect.width = menuWidth.Value;
            windowRect.height = menuHeight.Value;
            
            // 显示工具提示（在主窗口之外绘制）
            if (showTooltip && !string.IsNullOrEmpty(tooltipText))
            {
                // 确保工具提示不会超出屏幕边界
                Vector2 adjustedPosition = tooltipPosition;
                adjustedPosition.y -= 30; // 窗口显示在鼠标上方
                
                // 设置工具提示样式
                GUI.backgroundColor = new Color(0.95f, 0.95f, 0.95f, 0.95f);
                GUI.Window(999, new Rect(adjustedPosition, new Vector2(100, 30)), (id) => 
                {
                    GUI.Label(new Rect(5, 5, 90, 20), tooltipText);
                }, "");
            }
            
            // 恢复原始背景色
            GUI.backgroundColor = originalBackgroundColor;
        }
        
        private void DrawWindow(int windowID)
        {
            // 设置统一的字体大小
            GUI.skin.label.fontSize = 14;
            GUI.skin.button.fontSize = 14;
            GUI.skin.textField.fontSize = 14;
            GUI.skin.window.fontSize = 14;
            
            GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
            
            // 标题
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("成员修改器", new GUILayoutOption[] { GUILayout.ExpandWidth(false) });
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            
            // 分类按钮
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
            if (GUILayout.Button("族人修改", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
            {
                currentMode = 0;
                hasSelectedCategory = true;
                // 清空属性值，确保显示与当前模式一致
                ResetAttributeValues();
                // 清空搜索文本框
                ClearSearchText();
                // 过滤显示族人员列表
                FilterMembers();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("门客修改", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
            {
                currentMode = 1;
                hasSelectedCategory = true;
                // 清空属性值，确保显示与当前模式一致
                ResetAttributeValues();
                // 清空搜索文本框
                ClearSearchText();
                // 过滤显示门客列表
                FilterMembers();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("皇室修改", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
            {
                currentMode = 2;
                hasSelectedCategory = true;
                // 清空属性值，确保显示与当前模式一致
                ResetAttributeValues();
                // 清空搜索文本框
                ClearSearchText();
                // 过滤显示皇室列表
                FilterMembers();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("世家修改", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
            {
                currentMode = 3;
                hasSelectedCategory = true;
                // 清空属性值，确保显示与当前模式一致
                ResetAttributeValues();
                // 清空搜索文本框
                ClearSearchText();
                // 过滤显示世家列表
                FilterMembers();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("农庄修改", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
            {
                currentMode = 4;
                hasSelectedCategory = true;
                // 清空属性值，确保显示与当前模式一致
                ResetAttributeValues();
                // 清空搜索文本框
                ClearSearchText();
                // 过滤显示农庄列表
                FilterMembers();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
            
            // 族人模式支项选择（只有选择族人模式时才显示）
                if (currentMode == 0)
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    if (GUILayout.Button("族人家族", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentMemberSubMode = 1;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    if (GUILayout.Button("族人妻妾", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentMemberSubMode = 2;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                }
                
                // 皇室模式支项选择（只有选择皇室模式时才显示）
                if (currentMode == 2)
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    if (GUILayout.Button("皇室主脉", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentRoyaltySubMode = 1;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    if (GUILayout.Button("皇家妻妾", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentRoyaltySubMode = 2;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                }
                
                // 世家模式支项选择（只有选择世家模式时才显示）
                if (currentMode == 3)
                {
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    if (GUILayout.Button("世家主脉", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentNobleSubMode = 1;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    if (GUILayout.Button("世家妻妾", new GUILayoutOption[] { GUILayout.Width(150f), GUILayout.Height(30f) }))
                    {
                        currentNobleSubMode = 2;
                        hasSelectedCategory = true;
                        // 清空属性值，确保显示与当前子模式一致
                        ResetAttributeValues();
                        // 清空搜索文本框
                        ClearSearchText();
                        FilterMembers();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                }
            
            // 根据是否选择了分类显示不同内容
            if (hasSelectedCategory)
            {
                // 选择分类后显示：搜索框、成员列表、属性编辑区域、修改按钮
                
                // 搜索文本框和清空按钮
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                GUILayout.Label("名字搜索:", new GUILayoutOption[] { GUILayout.Width(80f) });
                string newSearchText = GUILayout.TextField(searchText, new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                if (newSearchText != searchText)
                {
                    searchText = newSearchText;
                    FilterMembers();
                }
                if (GUILayout.Button("清空", new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) }))
                {
                    ClearSearchText();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
                
                // 在农庄修改模式和世家模式下显示郡选择
                if (currentMode == 4 || currentMode == 3) // 农庄修改模式或世家模式
                {
                    // 选择郡标签和大地图按钮单独一行
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("选择郡: ", new GUILayoutOption[] { GUILayout.Width(80f) });
                    if (GUILayout.Button("大地图", new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(25f) }))
                    {
                        currentFarmAreaIndex = 0;
                        ClearSearchText();
                        FilterMembers();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    
                    // 第一行郡按钮 (南郡、三川郡、蜀郡、丹阳郡、陈留郡、长沙郡)
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Space(80f); // 与上方标签对齐
                    string[] countyNames1 = new string[] { "南郡", "三川郡", "蜀郡", "丹阳郡", "陈留郡", "长沙郡" };
                    for (int i = 0; i < countyNames1.Length; i++)
                    {
                        if (GUILayout.Button(countyNames1[i], new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(25f) }))
                        {
                            currentFarmAreaIndex = i + 1; // 索引1-6
                            ClearSearchText();
                            FilterMembers();
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    
                    // 第二行郡按钮 (会稽郡、广陵郡、太原郡、益州郡、南海郡、云南郡)
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Space(80f); // 与上方标签对齐
                    string[] countyNames2 = new string[] { "会稽郡", "广陵郡", "太原郡", "益州郡", "南海郡", "云南郡" };
                    for (int i = 0; i < countyNames2.Length; i++)
                    {
                        if (GUILayout.Button(countyNames2[i], new GUILayoutOption[] { GUILayout.Width(80f), GUILayout.Height(25f) }))
                        {
                            currentFarmAreaIndex = i + 7; // 索引7-12
                            ClearSearchText();
                            FilterMembers();
                        }
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                }
                
                // 世家模式特殊界面设计
                if (currentMode == 3) // 世家模式
                {
                    // 家族选择框
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("选择家族: ", new GUILayoutOption[] { GUILayout.Width(100f) });
                    // 从游戏数据中动态获取世家列表
                    List<string> familyOptionsList = new List<string> { "全部家族" };
                    Dictionary<int, int> familyIndexMap = new Dictionary<int, int> { { 0, -1 } }; // UI索引到实际索引的映射，0表示全部家族
                    int currentFamilyIndex = 1;
                    
                    if (Mainload.ShiJia_Now != null)
                    {
                        for (int i = 0; i < Mainload.ShiJia_Now.Count; i++)
                        {
                            if (Mainload.ShiJia_Now[i] != null && Mainload.ShiJia_Now[i].Count > 1)
                            {
                                string familyName = Mainload.ShiJia_Now[i][1]; // 获取世家姓氏
                                string locationText = "";
                                
                                //获取郡和县的索引
                                int junIndex = -1;
                                int xianIndex = -1;
                                string[] locationParts = Mainload.ShiJia_Now[i][5].Split('|');
                                int.TryParse(locationParts[0], out junIndex);
                                int.TryParse(locationParts[1], out xianIndex);

                                // 解析位置信息
                                if (Mainload.ShiJia_Now[i].Count > 5 && !string.IsNullOrEmpty(Mainload.ShiJia_Now[i][5]))
                                {
                                    if (locationParts.Length >= 2)
                                    {
                                        // 从郡数组和二维县数组获取对应的郡名和县名
                                        if (junIndex >= 0 && junIndex < JunList.Length)
                                        {
                                            locationText = JunList[junIndex];
                                            if (xianIndex >= 0 && xianIndex < XianList[junIndex].Length)
                                            {
                                                locationText += "-" + XianList[junIndex][xianIndex];
                                            }
                                        }
                                    }
                                }
                                
                                // 应用郡筛选条件
                                bool isCurrentCounty = currentFarmAreaIndex <= 0; // 0表示全部郡
                                if (!isCurrentCounty && junIndex >= 0 && junIndex < JunList.Length)
                                {
                                    // currentFarmAreaIndex是1-based索引，JunList是0-based
                                    isCurrentCounty = (currentFarmAreaIndex - 1) == junIndex;
                                }

                                if (isCurrentCounty)
                                {
                                    familyOptionsList.Add($"{familyName}家 ({locationText})");
                                    familyIndexMap[currentFamilyIndex] = i;
                                    currentFamilyIndex++;
                                }
                            }
                        }
                    }
                    
                    // 改为滚动列表形式显示家族
                    GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(120f) });
                    familyScrollPosition = GUILayout.BeginScrollView(familyScrollPosition, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
                    
                    for (int i = 0; i < familyOptionsList.Count; i++)
                    {
                        if (GUILayout.Button(familyOptionsList[i], new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(25f) }))
                        {
                            // 处理家族选择
                            int selectedFamilyIndex = familyIndexMap[i];
                            this.currentFamilyIndex = selectedFamilyIndex;
                            ClearSearchText();
                            FilterMembers();
                        }
                    }
                    
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                    
                    // 家族成员选择框
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("选择成员: ", new GUILayoutOption[] { GUILayout.Width(100f) });
                    // 搜索结果列表
                    GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(120f) });
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
                    
                    foreach (string member in filteredMembers)
                    {
                        if (GUILayout.Button(member, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(25f) }))
                        {
                            selectedMemberName = member;
                            LoadMemberData(member);
                        }
                    }
                    
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10f);
                    
                    // 已选择信息显示文本
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.FlexibleSpace();
                    // 创建居中显示的样式
                    GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                    centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                    centeredLabelStyle.fontStyle = FontStyle.Bold;
                    
                    // 显示已选择信息
                    if (!string.IsNullOrEmpty(selectedMemberName))
                    {
                        string selectedCounty = "全郡";
                        if (currentFarmAreaIndex > 0)
                        {
                            string[] countyNames = new string[] { "", "南郡", "三川郡", "蜀郡", "丹阳郡", "陈留郡", "长沙郡", "会稽郡", "广陵郡", "太原郡", "益州郡", "南海郡", "云南郡" };
                            if (currentFarmAreaIndex < countyNames.Length)
                            {
                                selectedCounty = countyNames[currentFarmAreaIndex];
                            }
                        }
                        
                        // 获取当前选择的家族名称
                        string familyName = "全部家族";
                        if (this.currentFamilyIndex >= 0 && Mainload.ShiJia_Now != null && this.currentFamilyIndex < Mainload.ShiJia_Now.Count)
                        {
                            if (Mainload.ShiJia_Now[this.currentFamilyIndex] != null && Mainload.ShiJia_Now[this.currentFamilyIndex].Count > 1)
                            {
                                familyName = Mainload.ShiJia_Now[this.currentFamilyIndex][1] + "家";
                            }
                        }
                        
                        GUILayout.Label("已选择：" + selectedCounty + "-" + familyName + "-" + selectedMemberName, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(400f) });
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(15f);
                }
                else // 其他模式保持原列表显示
                {
                    // 搜索结果列表
                    GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(150f) });
                    scrollPosition = GUILayout.BeginScrollView(scrollPosition, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
                    
                    foreach (string member in filteredMembers)
                    {
                        if (GUILayout.Button(member, new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(30f) }))
                        {
                            selectedMemberName = member;
                            LoadMemberData(member);
                        }
                    }
                    
                    GUILayout.EndScrollView();
                    GUILayout.EndVertical();
                }
                GUILayout.Space(10f);
                
                // 属性编辑区域 - 根据选择的分类显示不同的属性
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                Vector2 scrollPos = GUILayout.BeginScrollView(Vector2.zero, new GUILayoutOption[] { GUILayout.Height(750f) });
                
                // 根据不同分类显示对应的属性
                if (currentMode == 0) // 族人模式
                {
                    if (currentMemberSubMode == 1) // 族人家族
                    {
                        // 族人家族基本属性
                        // 官职选择按钮
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        GUILayout.Label("官职: ", new GUILayoutOption[] { GUILayout.Width(100f) });
                        
                        // 创建居中显示的样式
                        GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                        centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                        
                        // 第二列显示原始值对应的中文，与其他属性行保持一致
                        GUILayout.Label(OfficialRankCodeToChinese(A_officialRankOriginal), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                        
                        // 第二列和第三列之间添加至少4个字符缩进
                        GUILayout.Space(32f);
                        
                        // 第三列显示固定文本：→，与其他属性行保持一致
                        GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                        
                        // 第三列和第四列之间添加5个字符缩进
                        GUILayout.Space(40f);
                        
                        // 第四列显示待修改值对应的中文，与其他属性行保持一致
                        GUILayout.Label(OfficialRankCodeToChinese(A_officialRank), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                        
                        // 第四列和第五列之间添加至少4个字符缩进
                        GUILayout.Space(32f);
                        
                        // 第五列改为按钮形式（所有按钮放在同一行）
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(450f) });
                        string[] officialRankOptions = new string[] { "6", "5", "4", "3", "2", "1", "0" };
                        string[] officialRankLabels = new string[] { "一品", "二品", "三品", "四品", "五品", "六品", "七品" };
                        
                        for (int i = 0; i < officialRankOptions.Length; i++)
                        {
                            if (GUILayout.Button(officialRankLabels[i], new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) }))
                            {
                                // 生成完整的散官数据格式：5@品阶@0@-1@-1
                                int a=5, b=0, c =-1;
                                A_officialRank = $"{a}@{officialRankOptions[i]}@{b}@{c}@{c}";
                                // 验证生成的官职代码
                                UnityEngine.Debug.Log("生成的官职代码: " + A_officialRank);
                                UnityEngine.Debug.Log("对应的中文官职: " + OfficialRankCodeToChinese(A_officialRank));
                            }
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndHorizontal();

                        DrawAttributeRow("声誉:", ref A_reputationOriginal, ref A_reputation);
                        DrawAttributeRow("体力:", ref A_powerOriginal, ref A_power);
                        DrawAttributeRow("年龄:", ref A_ageOriginal, ref A_age);
                        DrawAttributeRow("健康:", ref A_healthOriginal, ref A_health);
                        DrawAttributeRow("心情:", ref A_moodOriginal, ref A_mood);
                        DrawAttributeRow("魅力:", ref A_charmOriginal, ref A_charm);
                        DrawAttributeRow("幸运:", ref A_luckOriginal, ref A_luck);
                        DrawAttributeRow("品性:", ref A_characterOriginal, ref A_character);
                        DrawAttributeRow("天赋:", ref A_talentOriginal, ref A_talent);
                        DrawAttributeRow("天赋点:", ref A_talentPointOriginal, ref A_talentPoint);
                        DrawAttributeRow("技能:", ref A_skillOriginal, ref A_skill);
                        DrawAttributeRow("技能点:", ref A_skillPointOriginal, ref A_skillPoint);
                        DrawAttributeRow("喜好:", ref A_preferenceOriginal, ref A_preference);
                        DrawAttributeRow("文才:", ref A_intelligenceOriginal, ref A_intelligence);
                        DrawAttributeRow("武才:", ref A_weaponOriginal, ref A_weapon);
                        DrawAttributeRow("商才:", ref A_businessOriginal, ref A_business);
                        DrawAttributeRow("艺才:", ref A_artOriginal, ref A_art);
                        DrawAttributeRow("计谋:", ref A_strategyOriginal, ref A_strategy);
                        // 族人家族特有属性：爵位和封地
                        DrawAttributeRow("爵位:", ref A_JueWeiOriginal, ref A_JueWei);
                        DrawAttributeRow("封地:", ref A_FengDiOriginal, ref A_FengDi);
                    }
                    else if (currentMemberSubMode == 2) // 族人妻妾
                    {
                        // 族人妻妾基本属性
                        DrawAttributeRow("声誉:", ref B_reputationOriginal, ref B_reputation);
                        DrawAttributeRow("体力:", ref B_powerOriginal, ref B_power);
                        DrawAttributeRow("年龄:", ref B_ageOriginal, ref B_age);
                        DrawAttributeRow("健康:", ref B_healthOriginal, ref B_health);
                        DrawAttributeRow("心情:", ref B_moodOriginal, ref B_mood);
                        DrawAttributeRow("魅力:", ref B_charmOriginal, ref B_charm);
                        DrawAttributeRow("幸运:", ref B_luckOriginal, ref B_luck);
                        DrawAttributeRow("品性:", ref B_characterOriginal, ref B_character);
                        DrawAttributeRow("天赋:", ref B_talentOriginal, ref B_talent);
                        DrawAttributeRow("天赋点:", ref B_talentPointOriginal, ref B_talentPoint);
                        DrawAttributeRow("技能:", ref B_skillOriginal, ref B_skill);
                        DrawAttributeRow("技能点:", ref B_skillPointOriginal, ref B_skillPoint);
                        DrawAttributeRow("喜好:", ref B_preferenceOriginal, ref B_preference);
                        DrawAttributeRow("文才:", ref B_intelligenceOriginal, ref B_intelligence);
                        DrawAttributeRow("武才:", ref B_weaponOriginal, ref B_weapon);
                        DrawAttributeRow("商才:", ref B_businessOriginal, ref B_business);
                        DrawAttributeRow("艺才:", ref B_artOriginal, ref B_art);
                        DrawAttributeRow("计谋:", ref B_strategyOriginal, ref B_strategy);
                        
                    }
                } else if (currentMode == 1) // 门客模式
                {
                    // 门客基本属性
                    DrawAttributeRow("声誉:", ref C_reputationOriginal, ref C_reputation);
                    DrawAttributeRow("体力:", ref C_powerOriginal, ref C_power);
                    DrawAttributeRow("年龄:", ref C_ageOriginal, ref C_age);
                    DrawAttributeRow("健康:", ref C_healthOriginal, ref C_health);
                    DrawAttributeRow("心情:", ref C_moodOriginal, ref C_mood);
                    DrawAttributeRow("魅力:", ref C_charmOriginal, ref C_charm);
                    DrawAttributeRow("幸运:", ref C_luckOriginal, ref C_luck);
                    DrawAttributeRow("品性:", ref C_characterOriginal, ref C_character);
                    DrawAttributeRow("天赋:", ref C_talentOriginal, ref C_talent);
                    DrawAttributeRow("天赋点:", ref C_talentPointOriginal, ref C_talentPoint);
                    DrawAttributeRow("技能:", ref C_skillOriginal, ref C_skill);
                    DrawAttributeRow("技能点:", ref C_skillPointOriginal, ref C_skillPoint);
                    DrawAttributeRow("文才:", ref C_intelligenceOriginal, ref C_intelligence);
                    DrawAttributeRow("武才:", ref C_weaponOriginal, ref C_weapon);
                    DrawAttributeRow("商才:", ref C_businessOriginal, ref C_business);
                    DrawAttributeRow("艺才:", ref C_artOriginal, ref C_art);
                    DrawAttributeRow("计谋:", ref C_strategyOriginal, ref C_strategy);
                    DrawAttributeRow("门客酬劳:", ref C_MenKe_RewardOriginal, ref C_MenKe_Reward);
                }
                else if (currentMode == 2) // 皇室模式
                {
                    if (currentRoyaltySubMode == 1) // 皇室主脉
                    {
                        // 皇室主脉基本属性
                        DrawAttributeRow("声誉:", ref D_reputationOriginal, ref D_reputation);
                        DrawAttributeRow("年龄:", ref D_ageOriginal, ref D_age);
                        DrawAttributeRow("健康:", ref D_healthOriginal, ref D_health);
                        DrawAttributeRow("心情:", ref D_moodOriginal, ref D_mood);
                        DrawAttributeRow("魅力:", ref D_charmOriginal, ref D_charm);
                        DrawAttributeRow("幸运:", ref D_luckOriginal, ref D_luck);
                        DrawAttributeRow("品性:", ref D_characterOriginal, ref D_character);
                        DrawAttributeRow("天赋:", ref D_talentOriginal, ref D_talent);
                        DrawAttributeRow("天赋点:", ref D_talentPointOriginal, ref D_talentPoint);
                        DrawAttributeRow("技能:", ref D_skillOriginal, ref D_skill);
                        DrawAttributeRow("技能点:", ref D_skillPointOriginal, ref D_skillPoint);
                        DrawAttributeRow("喜好:", ref D_preferenceOriginal, ref D_preference);
                        DrawAttributeRow("文才:", ref D_intelligenceOriginal, ref D_intelligence);
                        DrawAttributeRow("武才:", ref D_weaponOriginal, ref D_weapon);
                        DrawAttributeRow("商才:", ref D_businessOriginal, ref D_business);
                        DrawAttributeRow("艺才:", ref D_artOriginal, ref D_art);
                        DrawAttributeRow("计谋:", ref D_strategyOriginal, ref D_strategy);
                    }
                    else if (currentRoyaltySubMode == 2) // 皇家妻妾
                    {
                        // 皇家妻妾基本属性
                        DrawAttributeRow("声誉:", ref E_reputationOriginal, ref E_reputation);
                        DrawAttributeRow("年龄:", ref E_ageOriginal, ref E_age);
                        DrawAttributeRow("健康:", ref E_healthOriginal, ref E_health);
                        DrawAttributeRow("心情:", ref E_moodOriginal, ref E_mood);
                        DrawAttributeRow("魅力:", ref E_charmOriginal, ref E_charm);
                        DrawAttributeRow("幸运:", ref E_luckOriginal, ref E_luck);
                        DrawAttributeRow("品性:", ref E_characterOriginal, ref E_character);
                        DrawAttributeRow("天赋:", ref E_talentOriginal, ref E_talent);
                        DrawAttributeRow("天赋点:", ref E_talentPointOriginal, ref E_talentPoint);
                        DrawAttributeRow("技能:", ref E_skillOriginal, ref E_skill);
                        DrawAttributeRow("技能点:", ref E_skillPointOriginal, ref E_skillPoint);
                        DrawAttributeRow("喜好:", ref E_preferenceOriginal, ref E_preference);
                        DrawAttributeRow("文才:", ref E_intelligenceOriginal, ref E_intelligence);
                        DrawAttributeRow("武才:", ref E_weaponOriginal, ref E_weapon);
                        DrawAttributeRow("商才:", ref E_businessOriginal, ref E_business);
                        DrawAttributeRow("艺才:", ref E_artOriginal, ref E_art);
                        DrawAttributeRow("计谋:", ref E_strategyOriginal, ref E_strategy);
                    }
                }
                else if (currentMode == 3) // 世家模式
                {
                    if (currentNobleSubMode == 1) // 世家主脉
                    {
                        // 世家主脉基本属性
                        DrawAttributeRow("声誉:", ref F_reputationOriginal, ref F_reputation);
                        DrawAttributeRow("年龄:", ref F_ageOriginal, ref F_age);
                        DrawAttributeRow("健康:", ref F_healthOriginal, ref F_health);
                        DrawAttributeRow("心情:", ref F_moodOriginal, ref F_mood);
                        DrawAttributeRow("魅力:", ref F_charmOriginal, ref F_charm);
                        DrawAttributeRow("幸运:", ref F_luckOriginal, ref F_luck);
                        DrawAttributeRow("品性:", ref F_characterOriginal, ref F_character);
                        DrawAttributeRow("天赋:", ref F_talentOriginal, ref F_talent);
                        DrawAttributeRow("天赋点:", ref F_talentPointOriginal, ref F_talentPoint);
                        DrawAttributeRow("技能:", ref F_skillOriginal, ref F_skill);
                        DrawAttributeRow("技能点:", ref F_skillPointOriginal, ref F_skillPoint);
                        DrawAttributeRow("喜好:", ref F_preferenceOriginal, ref F_preference);
                        DrawAttributeRow("文才:", ref F_intelligenceOriginal, ref F_intelligence);
                        DrawAttributeRow("武才:", ref F_weaponOriginal, ref F_weapon);
                        DrawAttributeRow("商才:", ref F_businessOriginal, ref F_business);
                        DrawAttributeRow("艺才:", ref F_artOriginal, ref F_art);
                        DrawAttributeRow("计谋:", ref F_strategyOriginal, ref F_strategy);
                    }
                    else if (currentNobleSubMode == 2) // 世家妻妾
                    {
                        // 世家妻妾基本属性
                        DrawAttributeRow("声誉:", ref G_reputationOriginal, ref G_reputation);
                        DrawAttributeRow("年龄:", ref G_ageOriginal, ref G_age);
                        DrawAttributeRow("健康:", ref G_healthOriginal, ref G_health);
                        DrawAttributeRow("心情:", ref G_moodOriginal, ref G_mood);
                        DrawAttributeRow("魅力:", ref G_charmOriginal, ref G_charm);
                        DrawAttributeRow("幸运:", ref G_luckOriginal, ref G_luck);
                        DrawAttributeRow("品性:", ref G_characterOriginal, ref G_character);
                        DrawAttributeRow("天赋:", ref G_talentOriginal, ref G_talent);
                        DrawAttributeRow("天赋点:", ref G_talentPointOriginal, ref G_talentPoint);
                        DrawAttributeRow("技能:", ref G_skillOriginal, ref G_skill);
                        DrawAttributeRow("技能点:", ref G_skillPointOriginal, ref G_skillPoint);
                        DrawAttributeRow("喜好:", ref G_preferenceOriginal, ref G_preference);
                        DrawAttributeRow("文才:", ref G_intelligenceOriginal, ref G_intelligence);
                        DrawAttributeRow("武才:", ref G_weaponOriginal, ref G_weapon);
                        DrawAttributeRow("商才:", ref G_businessOriginal, ref G_business);
                        DrawAttributeRow("艺才:", ref G_artOriginal, ref G_art);
                        DrawAttributeRow("计谋:", ref G_strategyOriginal, ref G_strategy);
                    }
                }
                else if (currentMode == 4) // 农庄修改模式
                {
                    // 郡选择已移至搜索框下方
                    GUILayout.Space(10f);
                    
                    // 创建居中显示的样式
                    GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                    centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                    
                    // 农庄属性编辑区域
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("农庄面积: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                    GUILayout.Label(H_areaOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                    GUILayout.Space(32f);
                    GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                    GUILayout.Space(40f);
                    GUILayout.Label(H_area, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                    GUILayout.Space(32f);
                    
                    // 面积选择按钮
                    if (GUILayout.Button("4", new GUILayoutOption[] { GUILayout.Width(50f), GUILayout.Height(30f) }))
                    {
                        H_area = "4";
                    }
                    if (GUILayout.Button("9", new GUILayoutOption[] { GUILayout.Width(50f), GUILayout.Height(30f) }))
                    {
                        H_area = "9";
                    }
                    if (GUILayout.Button("16", new GUILayoutOption[] { GUILayout.Width(50f), GUILayout.Height(30f) }))
                    {
                        H_area = "16";
                    }
                    if (GUILayout.Button("25", new GUILayoutOption[] { GUILayout.Width(50f), GUILayout.Height(30f) }))
                    {
                        H_area = "25";
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("农户数量-种植: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                    GUILayout.Label(H_farmersPlantingOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                    GUILayout.Space(32f);
                    GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                    GUILayout.Space(40f);
                    GUILayout.Label(H_farmersPlanting, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                    GUILayout.Space(32f);
                    string newH_farmersPlanting = GUILayout.TextField(H_farmersPlanting, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newH_farmersPlanting != H_farmersPlanting)
                    {
                        H_farmersPlanting = newH_farmersPlanting;
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("农户数量-养殖: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                    GUILayout.Label(H_farmersBreedingOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                    GUILayout.Space(32f);
                    GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                    GUILayout.Space(40f);
                    GUILayout.Label(H_farmersBreeding, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                    GUILayout.Space(32f);
                    string newH_farmersBreeding = GUILayout.TextField(H_farmersBreeding, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newH_farmersBreeding != H_farmersBreeding)
                    {
                        H_farmersBreeding = newH_farmersBreeding;
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                    GUILayout.Label("农户数量-手工: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                    GUILayout.Label(H_farmersCraftingOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                    GUILayout.Space(32f);
                    GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                    GUILayout.Space(40f);
                    GUILayout.Label(H_farmersCrafting, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                    GUILayout.Space(32f);
                    string newH_farmersCrafting = GUILayout.TextField(H_farmersCrafting, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newH_farmersCrafting != H_farmersCrafting)
                    {
                        H_farmersCrafting = newH_farmersCrafting;
                    }
                    GUILayout.EndHorizontal();
                    
                    // 只有当存在庄头时才显示和允许修改庄头属性
                    if (!string.IsNullOrEmpty(currentZhuangTouId))
                    {
                        GUILayout.Space(20f);
                        GUILayout.Label("庄头属性:", new GUILayoutOption[] { GUILayout.Width(100f) });
                        
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        GUILayout.Label("年龄: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                        GUILayout.Label(H_zhuangTouAgeOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                        GUILayout.Space(32f);
                        GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                        GUILayout.Space(40f);
                        GUILayout.Label(H_zhuangTouAge, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                        GUILayout.Space(32f);
                        string newH_zhuangTouAge = GUILayout.TextField(H_zhuangTouAge, new GUILayoutOption[] { GUILayout.Width(200f) });
                        if (newH_zhuangTouAge != H_zhuangTouAge)
                        {
                            H_zhuangTouAge = newH_zhuangTouAge;
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        GUILayout.Label("管理: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                        GUILayout.Label(H_zhuangTouManagementOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                        GUILayout.Space(32f);
                        GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                        GUILayout.Space(40f);
                        GUILayout.Label(H_zhuangTouManagement, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                        GUILayout.Space(32f);
                        string newH_zhuangTouManagement = GUILayout.TextField(H_zhuangTouManagement, new GUILayoutOption[] { GUILayout.Width(200f) });
                        if (newH_zhuangTouManagement != H_zhuangTouManagement)
                        {
                            H_zhuangTouManagement = newH_zhuangTouManagement;
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        GUILayout.Label("忠诚: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                        GUILayout.Label(H_zhuangTouLoyaltyOriginal, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                        GUILayout.Space(32f);
                        GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                        GUILayout.Space(40f);
                        GUILayout.Label(H_zhuangTouLoyalty, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                        GUILayout.Space(32f);
                        string newH_zhuangTouLoyalty = GUILayout.TextField(H_zhuangTouLoyalty, new GUILayoutOption[] { GUILayout.Width(200f) });
                        if (newH_zhuangTouLoyalty != H_zhuangTouLoyalty)
                        {
                            H_zhuangTouLoyalty = newH_zhuangTouLoyalty;
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                        GUILayout.Label("品性: ", new GUILayoutOption[] { GUILayout.Width(150f) });
                        GUILayout.Label(CharacterCodeToChinese(H_zhuangTouCharacterOriginal), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) });
                        GUILayout.Space(32f);
                        GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                        GUILayout.Space(40f);
                        GUILayout.Label(CharacterCodeToChinese(H_zhuangTouCharacter), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) });
                        GUILayout.Space(32f);
                        
                        // 第五列改为按钮形式（分两行显示，每行7个，0不设按钮）
                        GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(300f) });
                        
                        // 第一行按钮 (1-7)
                        GUILayout.BeginHorizontal();
                        string[] characterOptions1 = new string[] { "1", "2", "3", "4", "5", "6", "7" };
                        string[] characterLabels1 = new string[] { "傲娇", "刚正", "活泼", "善良", "真诚", "洒脱", "高冷" };
                        
                        for (int i = 0; i < characterOptions1.Length; i++)
                        {
                            if (GUILayout.Button(characterLabels1[i], new GUILayoutOption[] { GUILayout.Width(40f), GUILayout.Height(20f) }))
                            {
                                H_zhuangTouCharacter = characterOptions1[i];
                            }
                        }
                        GUILayout.EndHorizontal();
                        
                        // 第二行按钮 (8-14)
                        GUILayout.BeginHorizontal();
                        string[] characterOptions2 = new string[] { "8", "9", "10", "11", "12", "13", "14" };
                        string[] characterLabels2 = new string[] { "自卑", "怯懦", "腼腆", "凶狠", "善变", "忧郁", "多疑" };
                        
                        for (int i = 0; i < characterOptions2.Length; i++)
                        {
                            if (GUILayout.Button(characterLabels2[i], new GUILayoutOption[] { GUILayout.Width(40f), GUILayout.Height(20f) }))
                            {
                                H_zhuangTouCharacter = characterOptions2[i];
                            }
                        }
                        GUILayout.EndHorizontal();
                        
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.Space(20f);
                        GUILayout.Label("当前农庄暂无庄头", new GUILayoutOption[] { GUILayout.Width(200f) });
                    }
                }
                
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
                GUILayout.Space(10f);

                // 修改按钮
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("修改", new GUILayoutOption[] { GUILayout.Width(200f), GUILayout.Height(50f) }))
                {
                    ApplyChanges();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);
            }
            else
            {
                // 未选择分类时显示：使用说明
                
                // 说明区域
                GUILayout.Space(20f);
                GUILayout.BeginVertical();
                
                // 创建大字体样式
                GUIStyle largeFontStyle = new GUIStyle(UnityEngine.GUI.skin.label);
                largeFontStyle.fontSize = largeFontStyle.fontSize * 2; // 设置字体大小为原来的2倍
                
                GUIStyle largeBoxStyle = new GUIStyle(UnityEngine.GUI.skin.box);
                largeBoxStyle.fontSize = largeBoxStyle.fontSize * 2; // 设置字体大小为原来的2倍
                
                // 使用说明标题
                GUILayout.Label("使用说明:", largeBoxStyle);
                
                // 使用说明
                GUILayout.Label("1. 请在点击修改前先保存游戏，以便回档", largeFontStyle);
                GUILayout.Label("2. 按F3键显示/隐藏窗口", largeFontStyle);
                GUILayout.Label("3. 选择修改类型：族人修改/门客修改/皇室修改/世家修改/农庄修改", largeFontStyle);
                GUILayout.Label("4. 族人模式下可选择：族人家族/族人妻妾", largeFontStyle);
                GUILayout.Label("5. 皇室模式下可选择：皇室主脉/皇室妻妾", largeFontStyle);
                GUILayout.Label("6. 世家模式下可选择：世家主脉/世家妻妾", largeFontStyle);
                GUILayout.Label("7. 输入部分字符可搜索角色", largeFontStyle);
                GUILayout.Label("8. 选择角色后可以通过点击按钮或者直接在文本框中输入来修改对应的属性值", largeFontStyle);
                GUILayout.Label("9. 点击修改按钮应用更改", largeFontStyle);
                
                // MOD作者及版本号说明
                GUILayout.Label("Mod作者：AnZhi20", largeFontStyle);
                GUILayout.Label("Mod版本：2.0.0", largeFontStyle);
                GUILayout.EndVertical();
            }
            
            GUILayout.EndVertical();
            
            // 允许拖动窗口
            GUI.DragWindow();
        }
        
        // 技能代码转中文对照表
        private string SkillCodeToChinese(string skillCode)
        {
            if (string.IsNullOrEmpty(skillCode))
                return "";
            
            string result = "";
            char[] codes = skillCode.ToCharArray();
            
            foreach (char code in codes)
            {
                switch (code)
                {
                    case '0': result += "空";
                        break;
                    case '1': result += "巫";
                        break;
                    case '2': result += "医";
                        break;
                    case '3': result += "相";
                        break;
                    case '4': result += "卜";
                        break;
                    case '5': result += "魅";
                        break;
                    case '6': result += "工";
                        break;
                    default: result += code;
                        break;
                }
            }
            
            return result;
        }
        
        // 天赋代码转中文对照表
        private string TalentCodeToChinese(string talentCode)
        {
            if (string.IsNullOrEmpty(talentCode))
                return "";
            
            string result = "";
            char[] codes = talentCode.ToCharArray();
            
            foreach (char code in codes)
            {
                switch (code)
                {
                    case '0': result += "空";
                        break;
                    case '1': result += "文";
                        break;
                    case '2': result += "武";
                        break;
                    case '3': result += "商";
                        break;
                    case '4': result += "艺";
                        break;
                    default: result += code;
                        break;
                }
            }
            
            return result;
        }
        
        // 喜好代码转中文对照表
        private string PreferenceCodeToChinese(string preferenceCode)
        {
            if (string.IsNullOrEmpty(preferenceCode))
                return "";
            
            string result = "";
            char[] codes = preferenceCode.ToCharArray();
            
            foreach (char code in codes)
            {
                switch (code)
                {
                    case '0': result += "香粉、";
                        break;
                    case '1': result += "书法、";
                        break;
                    case '2': result += "丹青、";
                        break;
                    case '3': result += "文玩、";
                        break;
                    case '4': result += "茶具、";
                        break;
                    case '5': result += "香具、";
                        break;
                    case '6': result += "瓷器、";
                        break;
                    case '7': result += "美酒、";
                        break;
                    case '8': result += "琴瑟、";
                        break;
                    case '9': result += "皮毛、";
                        break;
                    default: result += code + "、";
                        break;
                }
            }
            
            // 移除最后一个顿号
            if (result.EndsWith("、"))
            {
                result = result.Substring(0, result.Length - 1);
            }
            
            return result;
        }
        
        // 品性代码转中文对照表
        private string CharacterCodeToChinese(string characterCode)
        {
            if (string.IsNullOrEmpty(characterCode))
                return "";
            
            // 品性代码0-14对应中文
            switch (characterCode)
            {
                case "0": return "不明";
                case "1": return "傲娇";
                case "2": return "刚正";
                case "3": return "活泼";
                case "4": return "善良";
                case "5": return "真诚";
                case "6": return "洒脱";
                case "7": return "高冷";
                case "8": return "自卑";
                case "9": return "怯懦";
                case "10": return "腼腆";
                case "11": return "凶狠";
                case "12": return "善变";
                case "13": return "忧郁";
                case "14": return "多疑";
                default: return characterCode;
            }
        }
        
        // 官职代码转中文对照表
        private string OfficialRankCodeToChinese(string officialRankCode)
        {
            // 添加调试日志
            UnityEngine.Debug.Log("OfficialRankCodeToChinese 输入: " + officialRankCode);
            
            // 解析官职代码，只取|前面的部分（如果有）
            string positionPart = officialRankCode;
            if (officialRankCode.Contains("|"))
            {
                positionPart = officialRankCode.Split('|')[0];
                UnityEngine.Debug.Log("提取|前面部分: " + positionPart);
            }

            // 直接根据格式解析并返回对应的中文官职
            int i = 0, j = 0, k = 0, l = 0, m = 0;
            try
            {
                string[] parts = positionPart.Split('@');
                UnityEngine.Debug.Log("分割后部分数量: " + parts.Length);
                
                if (parts.Length >= 5)
                {
                    i = int.Parse(parts[0]);
                    j = int.Parse(parts[1]);
                    k = int.Parse(parts[2]);
                    l = int.Parse(parts[3]);
                    m = int.Parse(parts[4]);
                    
                    UnityEngine.Debug.Log(string.Format("解析后参数: i={0}, j={1}, k={2}, l={3}, m={4}", i, j, k, l, m));
                    
                    string result = GuanZhiData.GetOfficialPosition(i, j, k, l, m);
                    UnityEngine.Debug.Log("GetOfficialPosition 返回: " + result);
                    return result;
                }
                else
                {
                    UnityEngine.Debug.Log("部分数量不足5个");
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("解析官职代码时出错: " + ex.Message);
            }
            
            UnityEngine.Debug.Log("返回默认值: 未知官职");
            return "未知官职";
        }

        // 爵位代码转中文对照表
        private string JueWeiCodeToChinese(string jueWeiCode)
        {
            if (string.IsNullOrEmpty(jueWeiCode))
                return "";
            
            // 爵位代码0-4对应中文
            switch (jueWeiCode)
            {
                case "0": return "空";
                case "1": return "伯爵";
                case "2": return "侯爵";
                case "3": return "公爵";
                case "4": return "王爵";
                default: return jueWeiCode;
            }
        }
        
       
        // 封地代码转中文对照表
        private string FengDiCodeToChinese(string fengDiCode)
        {
            if (string.IsNullOrEmpty(fengDiCode))
                return "";
            
            // 封地代码1-12对应中文
            switch (fengDiCode)
            {
                case "0": return "无封地";
                case "1": return "南郡-汉阳";
                case "2": return "三川郡-左亭";
                case "3": return "蜀郡-华阳";
                case "4": return "丹阳郡-宛陵";
                case "5": return "陈留郡-长罗";
                case "6": return "长沙郡-安成";
                case "7": return "会稽郡-太末";
                case "8": return "广陵郡-盐渎";
                case "9": return "太原郡-霍人";
                case "10": return "益州郡-比苏";
                case "11": return "南海郡-新会";
                case "12": return "云南郡-越隽";
                default: return fengDiCode;
            }
        }
        
        private void DrawAttributeRow(string label, ref string originalValue, ref string currentValue)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.ExpandWidth(true) });
            GUILayout.Label(label, new GUILayoutOption[] { GUILayout.Width(100f) });
            
            // 特殊处理喜好显示和修改
                if (label == "喜好:")
                {
                    // 创建居中显示的样式
                    GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                    centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                    
                    // 第二列显示中文
                    GUILayout.Label(PreferenceCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                    
                    // 第二列和第三列之间添加至少4个字符缩进
                    GUILayout.Space(32f);
                    
                    // 第三列显示固定文本：→
                    GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                    
                    // 第三列和第四列之间添加5个字符缩进
                    GUILayout.Space(40f);
                    
                    // 第四列显示待修改值
                    GUILayout.Label(PreferenceCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第四列和第五列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第五列改为按钮形式
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(350f) });
                
                // 第一行按钮 (0-4)
                GUILayout.BeginHorizontal();
                string[] preferenceOptions1 = new string[] { "0", "1", "2", "3", "4" };
                string[] preferenceLabels1 = new string[] { "香粉", "书法", "丹青", "文玩", "茶具" };
                
                for (int i = 0; i < preferenceOptions1.Length; i++)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(preferenceLabels1[i]), GUI.skin.button, new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) });
                    if (GUI.Button(buttonRect, preferenceLabels1[i]))
                    {
                        currentValue = preferenceOptions1[i];
                    }
                    
                    // 检查鼠标是否悬停在按钮上
                    if (buttonRect.Contains(Event.current.mousePosition))
                    {
                        // 设置工具提示变量
                        showTooltip = true;
                        tooltipText = PreferenceCodeToChinese(preferenceOptions1[i]);
                        tooltipPosition = Event.current.mousePosition;
                    }
                }
                GUILayout.EndHorizontal();
                
                // 第二行按钮 (5-9)
                GUILayout.BeginHorizontal();
                string[] preferenceOptions2 = new string[] { "5", "6", "7", "8", "9" };
                string[] preferenceLabels2 = new string[] { "香具", "瓷器", "美酒", "琴瑟", "皮毛" };
                
                for (int i = 0; i < preferenceOptions2.Length; i++)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(preferenceLabels2[i]), GUI.skin.button, new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) });
                    if (GUI.Button(buttonRect, preferenceLabels2[i]))
                    {
                        currentValue = preferenceOptions2[i];
                    }
                    
                    // 检查鼠标是否悬停在按钮上
                    if (buttonRect.Contains(Event.current.mousePosition))
                    {
                        // 设置工具提示变量
                        showTooltip = true;
                        tooltipText = PreferenceCodeToChinese(preferenceOptions2[i]);
                        tooltipPosition = Event.current.mousePosition;
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            // 特殊处理爵位显示和修改
            else if (label == "爵位:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(JueWeiCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                
                // 第二列和第三列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                
                // 第三列和第四列之间添加5个字符缩进
                GUILayout.Space(40f);
                
                // 第四列显示待修改值
                GUILayout.Label(JueWeiCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第四列和第五列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第五列改为按钮形式
                GUILayout.BeginHorizontal();
                string[] jueWeiOptions = new string[] { "0", "1", "2", "3", "4" };
                string[] jueWeiLabels = new string[] { "空", "伯爵", "侯爵", "公爵", "王爵" };
                
                for (int i = 0; i < jueWeiOptions.Length; i++)
                {
                    if (GUILayout.Button(jueWeiLabels[i], new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) }))
                    {
                        currentValue = jueWeiOptions[i];
                    }
                }
                GUILayout.EndHorizontal();
                
            }
            // 特殊处理封地显示和修改
            else if (label == "封地:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(FengDiCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                
                // 第二列和第三列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                
                // 第三列和第四列之间添加5个字符缩进
                GUILayout.Space(40f);
                
                // 第四列显示待修改值
                GUILayout.Label(FengDiCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第五列改为按钮形式
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(380f) });
                
                // 空按钮单独成行
                GUILayout.BeginHorizontal();
                Rect emptyButtonRect = GUILayoutUtility.GetRect(new GUIContent("空"), GUI.skin.button, new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) });
                if (GUI.Button(emptyButtonRect, "空"))
                {
                    currentValue = "0";
                }
                
                // 检查鼠标是否悬停在空按钮上
                if (emptyButtonRect.Contains(Event.current.mousePosition))
                {
                    // 设置工具提示变量
                    showTooltip = true;
                    tooltipText = FengDiCodeToChinese("0");
                    tooltipPosition = Event.current.mousePosition;
                }
                GUILayout.EndHorizontal();
                
                // 第一行按钮 (1-6)
                GUILayout.BeginHorizontal();
                string[] fengDiOptions1 = new string[] { "1", "2", "3", "4", "5", "6" };
                string[] fengDiLabels1 = new string[] { "汉阳", "左亭", "华阳", "宛陵", "长罗", "安成" };
                
                for (int i = 0; i < fengDiOptions1.Length; i++)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(fengDiLabels1[i]), GUI.skin.button, new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) });
                    if (GUI.Button(buttonRect, fengDiLabels1[i]))
                    {
                        currentValue = fengDiOptions1[i];
                    }
                    
                    // 检查鼠标是否悬停在按钮上
                    if (buttonRect.Contains(Event.current.mousePosition))
                    {
                        // 设置工具提示变量
                        showTooltip = true;
                        tooltipText = FengDiCodeToChinese(fengDiOptions1[i]);
                        tooltipPosition = Event.current.mousePosition;
                    }
                }
                GUILayout.EndHorizontal();
                
                // 第二行按钮 (7-12)
                GUILayout.BeginHorizontal();
                string[] fengDiOptions2 = new string[] { "7", "8", "9", "10", "11", "12" };
                string[] fengDiLabels2 = new string[] { "太末", "盐渎", "霍人", "比苏", "新会", "越隽" };
                
                for (int i = 0; i < fengDiOptions2.Length; i++)
                {
                    Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(fengDiLabels2[i]), GUI.skin.button, new GUILayoutOption[] { GUILayout.Width(60f), GUILayout.Height(20f) });
                    if (GUI.Button(buttonRect, fengDiLabels2[i]))
                    {
                        currentValue = fengDiOptions2[i];
                    }
                    
                    // 检查鼠标是否悬停在按钮上
                    if (buttonRect.Contains(Event.current.mousePosition))
                    {
                        // 设置工具提示变量
                        showTooltip = true;
                        tooltipText = FengDiCodeToChinese(fengDiOptions2[i]);
                        tooltipPosition = Event.current.mousePosition;
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            // 特殊处理品性显示
            else if (label == "品性:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(CharacterCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                
                // 第二列和第三列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                
                // 第三列和第四列之间添加5个字符缩进
                GUILayout.Space(40f);
                
                // 第四列显示待修改值
                GUILayout.Label(CharacterCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第五列改为按钮形式（分两行显示，每行7个）
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(300f) });
                
                // 第一行按钮 (1-7)
                GUILayout.BeginHorizontal();
                string[] characterOptions1 = new string[] { "1", "2", "3", "4", "5", "6", "7" };
                string[] characterLabels1 = new string[] { "傲娇", "刚正", "活泼", "善良", "真诚", "洒脱", "高冷" };
                
                for (int i = 0; i < characterOptions1.Length; i++)
                {
                    if (GUILayout.Button(characterLabels1[i], new GUILayoutOption[] { GUILayout.Width(40f), GUILayout.Height(20f) }))
                    {
                        currentValue = characterOptions1[i];
                        // 直接更新对应的模式特定变量
                        if (label == "品性:")
                        {
                            if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                            {
                                A_character = characterOptions1[i];
                            }
                            else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                            {
                                B_character = characterOptions1[i];
                            }
                            else if (currentMode == 1) // 门客模式
                            {
                                C_character = characterOptions1[i];
                            }
                            else if (currentMode == 2) // 皇室模式
                            {
                                if (currentRoyaltySubMode == 1) // 皇室主脉
                                {
                                    D_character = characterOptions1[i];
                                }
                                else if (currentRoyaltySubMode == 2) // 皇家妻妾
                                {
                                    E_character = characterOptions1[i];
                                }
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
                
                // 第二行按钮 (8-14)
                GUILayout.BeginHorizontal();
                string[] characterOptions2 = new string[] { "8", "9", "10", "11", "12", "13", "14" };
                string[] characterLabels2 = new string[] { "自卑", "怯懦", "腼腆", "凶狠", "善变", "忧郁", "多疑" };
                
                for (int i = 0; i < characterOptions2.Length; i++)
                {
                    if (GUILayout.Button(characterLabels2[i], new GUILayoutOption[] { GUILayout.Width(40f), GUILayout.Height(20f) }))
                    {
                        currentValue = characterOptions2[i];
                        // 直接更新对应的模式特定变量
                        if (label == "品性:")
                        {
                            if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                            {
                                A_character = characterOptions2[i];
                            }
                            else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                            {
                                B_character = characterOptions2[i];
                            }
                            else if (currentMode == 1) // 门客模式
                            {
                                C_character = characterOptions2[i];
                            }
                            else if (currentMode == 2) // 皇室模式
                            {
                                if (currentRoyaltySubMode == 1) // 皇室主脉
                                {
                                    D_character = characterOptions2[i];
                                }
                                else if (currentRoyaltySubMode == 2) // 皇家妻妾
                                {
                                    E_character = characterOptions2[i];
                                }
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            // 特殊处理技能显示
            else if (label == "技能:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(SkillCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                
                // 第二列和第三列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                
                // 第三列和第四列之间添加5个字符缩进
                GUILayout.Space(40f);
                
                // 第四列显示待修改值
                GUILayout.Label(SkillCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第五列改为按钮形式
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(200f) });
                string[] skillOptions = new string[] { "0", "1", "2", "3", "4", "5", "6" };
                string[] skillLabels = new string[] { "空", "巫", "医", "相", "卜", "魅", "工" };
                
                for (int i = 0; i < skillOptions.Length; i++)
                {
                    if (GUILayout.Button(skillLabels[i], new GUILayoutOption[] { GUILayout.Width(30f), GUILayout.Height(25f) }))
                    {
                        // 直接赋值选中的技能值
                        currentValue = skillOptions[i];
                        
                        // 直接更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_skill = skillOptions[i];
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_skill = skillOptions[i];
                        }
                        else if (currentMode == 1) // 门客模式
                        {
                            C_skill = skillOptions[i];
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_skill = skillOptions[i];
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_skill = skillOptions[i];
                            }
                        }
                        
                        // 如果技能为空，重置技能点为0
                        if (string.IsNullOrEmpty(currentValue) || currentValue == "0")
                        {
                            if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                            {
                                A_skillPoint = "0";
                                A_skillPointOriginal = "0";
                            }
                            else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                            {
                                B_skillPoint = "0";
                                B_skillPointOriginal = "0";
                            }
                            else if (currentMode == 1) // 门客模式
                            {
                                C_skillPoint = "0";
                                C_skillPointOriginal = "0";
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            // 特殊处理技能点显示
            else if (label == "技能点:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示原始值
                GUILayout.Label(originalValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                
                // 第二列和第三列之间添加至少4个字符缩进
                GUILayout.Space(32f);
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                
                // 第三列和第四列之间添加5个字符缩进
                GUILayout.Space(40f);
                
                // 第四列显示待修改值
                GUILayout.Label(currentValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第五列改为输入框
                // 如果技能为空或选择空，技能点默认为0且不可修改
                bool isSkillEmpty = false;
                if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                {
                    isSkillEmpty = string.IsNullOrEmpty(A_skill) || A_skill == "0";
                }
                else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                {
                    isSkillEmpty = string.IsNullOrEmpty(B_skill) || B_skill == "0";
                }
                else if (currentMode == 1) // 门客模式
                {
                    isSkillEmpty = string.IsNullOrEmpty(C_skill) || C_skill == "0";
                }
                
                if (isSkillEmpty)
                {
                    GUI.enabled = false;
                    GUILayout.TextField("0", new GUILayoutOption[] { GUILayout.Width(200f) });
                    GUI.enabled = true;
                }
                else
                {
                    string newValue = GUILayout.TextField(currentValue, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newValue != currentValue)
                    {
                        currentValue = newValue;
                        // 更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_skillPoint = newValue;
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_skillPoint = newValue;
                        }
                        else if (currentMode == 1) // 门客模式
                        {
                            C_skillPoint = newValue;
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_skillPoint = newValue;
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_skillPoint = newValue;
                            }
                        }
                    }
                }
            }
            // 特殊处理天赋显示
            else if (label == "天赋:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(TalentCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                GUILayout.Space(32f); // 第二列和第三列之间至少4个字符缩进
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                GUILayout.Space(40f); // 第三列和第四列之间添加5个字符缩进
                
                // 第四列显示待修改值
                GUILayout.Label(TalentCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                GUILayout.Space(32f); // 第四列和第五列之间至少4个字符缩进
                
                // 第五列改为按钮形式
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(200f) });
                string[] talentOptions = new string[] { "0", "1", "2", "3", "4" };
                string[] talentLabels = new string[] { "空", "文", "武", "商", "艺" };
                
                for (int i = 0; i < talentOptions.Length; i++)
                {
                    if (GUILayout.Button(talentLabels[i], new GUILayoutOption[] { GUILayout.Width(40f), GUILayout.Height(25f) }))
                    {
                        // 直接赋值选中的天赋值
                        currentValue = talentOptions[i];
                        
                        // 直接更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_talent = talentOptions[i];
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_talent = talentOptions[i];
                        }
                        else if (currentMode == 1) // 门客模式
                        {
                            C_talent = talentOptions[i];
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_talent = talentOptions[i];
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_talent = talentOptions[i];
                            }
                        }
                        
                        // 如果天赋为空，重置天赋点为0
                        if (string.IsNullOrEmpty(currentValue) || currentValue == "0")
                        {
                            if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                            {
                                A_talentPoint = "0";
                                A_talentPointOriginal = "0";
                            }
                            else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                            {
                                B_talentPoint = "0";
                                B_talentPointOriginal = "0";
                            }
                            else if (currentMode == 1) // 门客模式
                            {
                                C_talentPoint = "0";
                                C_talentPointOriginal = "0";
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            // 特殊处理天赋点显示
            else if (label == "天赋点:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示原始值
                GUILayout.Label(originalValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                GUILayout.Space(32f); // 第二列和第三列之间至少4个字符缩进
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                GUILayout.Space(40f); // 第三列和第四列之间添加5个字符缩进
                
                // 第四列显示待修改值
                GUILayout.Label(currentValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                GUILayout.Space(32f); // 第四列和第五列之间至少4个字符缩进
                
                // 第五列改为输入框
                // 如果天赋为空或选择空，天赋点默认为0且不可修改
                bool isTalentEmpty = false;
                if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                {
                    isTalentEmpty = string.IsNullOrEmpty(A_talent) || A_talent == "0";
                }
                else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                {
                    isTalentEmpty = string.IsNullOrEmpty(B_talent) || B_talent == "0";
                }
                else if (currentMode == 1) // 门客模式
                {
                    isTalentEmpty = string.IsNullOrEmpty(C_talent) || C_talent == "0";
                }
                
                if (isTalentEmpty)
                {
                    GUI.enabled = false;
                    GUILayout.TextField("0", new GUILayoutOption[] { GUILayout.Width(200f) });
                    GUI.enabled = true;
                }
                else
                {
                    string newValue = GUILayout.TextField(currentValue, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newValue != currentValue)
                    {
                        currentValue = newValue;
                        // 更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_talentPoint = newValue;
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_talentPoint = newValue;
                        }
                        else if (currentMode == 1) // 门客模式
                        {
                            C_talentPoint = newValue;
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_talentPoint = newValue;
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_talentPoint = newValue;
                            }
                        }
                    }
                }
            }
            // 特殊处理喜好点显示
            else if (label == "喜好点:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示原始值
                GUILayout.Label(originalValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                GUILayout.Space(32f); // 第二列和第三列之间至少4个字符缩进
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                GUILayout.Space(40f); // 第三列和第四列之间添加5个字符缩进
                
                // 第四列显示待修改值
                GUILayout.Label(currentValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                GUILayout.Space(32f); // 第四列和第五列之间至少4个字符缩进
                
                // 第五列改为输入框
                // 如果喜好为空或选择空，喜好点默认为0且不可修改
                bool isPreferenceEmpty = false;
                if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                {
                    isPreferenceEmpty = string.IsNullOrEmpty(A_preference);
                }
                else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                {
                    isPreferenceEmpty = string.IsNullOrEmpty(B_preference);
                }
                else if (currentMode == 2) // 皇室模式
                {
                    if (currentRoyaltySubMode == 1) // 皇室主脉
                    {
                        isPreferenceEmpty = string.IsNullOrEmpty(D_preference);
                    }
                    else if (currentRoyaltySubMode == 2) // 皇家妻妾
                    {
                        isPreferenceEmpty = string.IsNullOrEmpty(E_preference);
                    }
                }
                
                if (isPreferenceEmpty)
                {
                    GUI.enabled = false;
                    GUILayout.TextField("0", new GUILayoutOption[] { GUILayout.Width(200f) });
                    GUI.enabled = true;
                }
                else
                {
                    string newValue = GUILayout.TextField(currentValue, new GUILayoutOption[] { GUILayout.Width(200f) });
                    if (newValue != currentValue)
                    {
                        currentValue = newValue;
                        // 更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_preference = newValue;
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_preference = newValue;
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_preference = newValue;
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_preference = newValue;
                            }
                        }
                    }
                }
            }
            // 特殊处理喜好显示
            else if (label == "喜好:")
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 第二列显示中文
                GUILayout.Label(PreferenceCodeToChinese(originalValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                GUILayout.Space(32f); // 第二列和第三列之间至少4个字符缩进
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                GUILayout.Space(40f); // 第三列和第四列之间添加5个字符缩进
                
                // 第四列显示待修改值
                GUILayout.Label(PreferenceCodeToChinese(currentValue), centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                
                // 第五列改为按钮形式（分两行显示）
                GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.Width(200f) });
                
                // 第一行按钮
                GUILayout.BeginHorizontal();
                string[] preferenceOptions1 = new string[] { "0", "1", "2", "3", "4" };
                string[] preferenceLabels1 = new string[] { "香粉", "书法", "丹青", "文玩", "茶具" };
                
                for (int i = 0; i < preferenceOptions1.Length; i++)
                {
                    if (GUILayout.Button(preferenceLabels1[i], new GUILayoutOption[] { GUILayout.Width(35f), GUILayout.Height(20f) }))
                    {
                        if (string.IsNullOrEmpty(currentValue) || !currentValue.Contains(preferenceOptions1[i]))
                        {
                            currentValue += preferenceOptions1[i];
                        }
                        else
                        {
                            // 移除已存在的喜好
                            currentValue = currentValue.Replace(preferenceOptions1[i], "");
                        }
                        
                        // 直接更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_preference = currentValue;
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_preference = currentValue;
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_preference = currentValue;
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_preference = currentValue;
                            }
                        }
                        else if (currentMode == 2) // 皇室模式
                        {
                            if (currentRoyaltySubMode == 1) // 皇室主脉
                            {
                                D_preference = currentValue;
                            }
                            else if (currentRoyaltySubMode == 2) // 皇家妻妾
                            {
                                E_preference = currentValue;
                            }
                        }
                    }
                }
                GUILayout.EndHorizontal();
                
                // 第二行按钮
                GUILayout.BeginHorizontal();
                string[] preferenceOptions2 = new string[] { "5", "6", "7", "8", "9" };
                string[] preferenceLabels2 = new string[] { "香具", "瓷器", "美酒", "琴瑟", "皮毛" };
                
                for (int i = 0; i < preferenceOptions2.Length; i++)
                {
                    if (GUILayout.Button(preferenceLabels2[i], new GUILayoutOption[] { GUILayout.Width(35f), GUILayout.Height(20f) }))
                    {
                        if (string.IsNullOrEmpty(currentValue) || !currentValue.Contains(preferenceOptions2[i]))
                        {
                            currentValue += preferenceOptions2[i];
                        }
                        else
                        {
                            // 移除已存在的喜好
                            currentValue = currentValue.Replace(preferenceOptions2[i], "");
                        }
                        
                        // 直接更新对应的模式特定变量
                        if (currentMode == 0 && currentMemberSubMode == 1) // 族人家族模式
                        {
                            A_preference = currentValue;
                        }
                        else if (currentMode == 0 && currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            B_preference = currentValue;
                        }
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            else
            {
                // 创建居中显示的样式
                GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label);
                centeredLabelStyle.alignment = TextAnchor.MiddleCenter;
                
                // 其他属性修改为新的布局方式
                GUILayout.Label(originalValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(60f) }); // 第二列显示宽度改为6个字符，并居中显示
                GUILayout.Space(32f); // 第二列和第三列之间至少4个字符缩进
                
                // 第三列显示固定文本：→
                GUILayout.Label("→", new GUILayoutOption[] { GUILayout.Width(20f) });
                GUILayout.Space(40f); // 第三列和第四列之间改为5个字符缩进
                
                // 第四列显示待修改值
                GUILayout.Label(currentValue, centeredLabelStyle, new GUILayoutOption[] { GUILayout.Width(80f) }); // 第四列显示宽度改为8个字符，并居中显示
                GUILayout.Space(32f); // 第四列和第五列之间至少4个字符缩进
                
                // 第五列改为输入框
                string newValue = GUILayout.TextField(currentValue, new GUILayoutOption[] { GUILayout.Width(200f) });
                if (newValue != currentValue)
                {
                    currentValue = newValue;
                    
                    // 根据当前模式和标签更新对应的模式特定变量
                    if (currentMode == 0)
                    {
                        if (currentMemberSubMode == 1) // 族人家族模式
                        {
                            if (label == "幸运:") A_luck = currentValue;
                            else if (label == "天赋:") A_talent = currentValue;
                            else if (label == "技能:") A_skill = currentValue;
                            else if (label == "喜好:") A_preference = currentValue;
                            else if (label == "品性:") A_character = currentValue;
                            else if (label == "天赋点:") A_talentPoint = currentValue;
                            else if (label == "技能点:") A_skillPoint = currentValue;
                        }
                        else if (currentMemberSubMode == 2) // 族人妻妾模式
                        {
                            if (label == "幸运:") B_luck = currentValue;
                            else if (label == "天赋:") B_talent = currentValue;
                            else if (label == "技能:") B_skill = currentValue;
                            else if (label == "喜好:") B_preference = currentValue;
                            else if (label == "品性:") B_character = currentValue;
                            else if (label == "天赋点:") B_talentPoint = currentValue;
                            else if (label == "技能点:") B_skillPoint = currentValue;
                        }
                    }
                    else if (currentMode == 1) // 门客模式
                    {
                        if (label == "幸运:") C_luck = currentValue;
                        else if (label == "天赋:") C_talent = currentValue;
                        else if (label == "技能:") C_skill = currentValue;
                        else if (label == "品性:") C_character = currentValue;
                        else if (label == "天赋点:") C_talentPoint = currentValue;
                        else if (label == "技能点:") C_skillPoint = currentValue;
                    }
                }
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void FilterMembers()
        {
            try
            {
                filteredMembers.Clear();
                
                if (currentMode == 0) // 族人模式
                {
                    // 只有当选择了具体的子模式（族人家族或族人妻妾）时才显示列表
                    if (currentMemberSubMode == 0)
                    {
                        return; // 不显示任何列表
                    }
                    
                    // 检查数据是否可用
                    bool hasMemberNowData = Mainload.Member_now != null && Mainload.Member_now.Count > 0;
                    bool hasMemberQuData = Mainload.Member_qu != null && Mainload.Member_qu.Count > 0;
                    
                    if (!hasMemberNowData && !hasMemberQuData)
                    {
                        Logger.LogWarning("没有找到任何族人数据");
                        return;
                    }
                    
                    // 搜索族人家族
                    if (currentMemberSubMode == 1)
                    {
                        SearchMemberData(Mainload.Member_now, 4);
                    }
                    
                    // 搜索族人妻妾
                    if (currentMemberSubMode == 2)
                    {
                        SearchMemberData(Mainload.Member_qu, 2); 
                    }
                }
                else if (currentMode == 1) // 门客模式
                {
                    SearchMemberData(Mainload.MenKe_Now, 2);
                }
                else if (currentMode == 2) // 皇室模式
                {
                    // 只有当选择了具体的子模式（皇室主脉或皇家妻妾）时才显示列表
                    if (currentRoyaltySubMode == 0)
                    {
                        return; // 不显示任何列表
                    }
                    
                    // 检查数据是否可用
                    bool hasRoyalFamilyData = Mainload.Member_King != null && Mainload.Member_King.Count > 0;
                    bool hasRoyalSpouseData = Mainload.Member_King_qu != null && Mainload.Member_King_qu.Count > 0;
                    
                    if (!hasRoyalFamilyData && !hasRoyalSpouseData)
                    {
                        Logger.LogWarning("没有找到任何皇室数据");
                        return;
                    }
                    
                    // 搜索皇室主脉
                    if (currentRoyaltySubMode == 1)
                    {
                        SearchMemberData(Mainload.Member_King, 2);
                    }
                    
                    // 搜索皇家妻妾
                    if (currentRoyaltySubMode == 2)
                    {
                        SearchMemberData(Mainload.Member_King_qu, 2); 
                    }
                }
                else if (currentMode == 3) // 世家模式
                {
                    // 只有当选择了具体的子模式（世家主脉或世家妻妾）时才显示列表
                    if (currentNobleSubMode == 0)
                    {
                        return; // 不显示任何列表
                    }
                    
                    // 检查数据是否可用
                    bool hasNobleFamilyData = Mainload.Member_other != null && Mainload.Member_other.Count > 0;
                    bool hasNobleSpouseData = Mainload.Member_Other_qu != null && Mainload.Member_Other_qu.Count > 0;
                    
                    if (!hasNobleFamilyData && !hasNobleSpouseData)
                    {
                        Logger.LogWarning("没有找到任何世家数据");
                        return;
                    }
                    
                    // 搜索世家主脉
                    if (currentNobleSubMode == 1 && Mainload.Member_other != null)
                    {
                        // 遍历所有世家
                        for (int familyIndex = 0; familyIndex < Mainload.Member_other.Count; familyIndex++)
                        {
                            if (Mainload.Member_other[familyIndex] != null)
                            {
                                // 检查家族选择状态：0表示全部家族，否则根据家族索引筛选
                                if (currentFamilyIndex == 0 || familyIndex == currentFamilyIndex)
                                {
                                    // 检查郡选择状态
                                    string[] countyNames = new string[] { "", "南郡", "三川郡", "蜀郡", "丹阳郡", "陈留郡", "长沙郡", "会稽郡", "广陵郡", "太原郡", "益州郡", "南海郡", "云南郡" };
                                    bool countyMatch = true;
                                    
                                    if (currentFarmAreaIndex > 0 && currentFarmAreaIndex < countyNames.Length && Mainload.ShiJia_Now != null && familyIndex < Mainload.ShiJia_Now.Count && Mainload.ShiJia_Now[familyIndex] != null && Mainload.ShiJia_Now[familyIndex].Count > 5 && !string.IsNullOrEmpty(Mainload.ShiJia_Now[familyIndex][5]))
                                    {
                                        // 解析位置信息
                                        string[] locationParts = Mainload.ShiJia_Now[familyIndex][5].Split('|');
                                        if (locationParts.Length >= 2)
                                        {
                                            int junIndex = -1;
                                            int.TryParse(locationParts[0], out junIndex);
                                            
                                            // 检查是否有对应的郡名数组JunList
                                            if (JunList != null && JunList.Length > 0)
                                            {
                                                // 使用JunList进行比较
                                                if (junIndex >= 0 && junIndex < JunList.Length)
                                                {
                                                    countyMatch = JunList[junIndex] == countyNames[currentFarmAreaIndex];
                                                }
                                                else
                                                {
                                                    countyMatch = false;
                                                }
                                            }
                                            else
                                            {
                                                // 如果没有JunList，则回退到简单的字符串包含比较
                                                string junName = countyNames[currentFarmAreaIndex];
                                                string locationText = Mainload.ShiJia_Now[familyIndex][5];
                                                countyMatch = locationText.Contains(junName);
                                            }
                                        }
                                    }
                                    
                                    if (countyMatch)
                                    {
                                        // 遍历每个世家的所有主脉成员
                                        for (int memberIndex = 0; memberIndex < Mainload.Member_other[familyIndex].Count; memberIndex++)
                                        {
                                            try
                                            {
                                                if (Mainload.Member_other[familyIndex][memberIndex] != null && 
                                                    Mainload.Member_other[familyIndex][memberIndex].Count > 0)
                                                {
                                                    string[] memberInfo = Mainload.Member_other[familyIndex][memberIndex][2].Split('|');
                                                    if (memberInfo.Length > 0 && !string.IsNullOrEmpty(memberInfo[0]))
                                                    {
                                                        AddMemberIfMatches(memberInfo[0]);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.LogError(string.Format("处理世家主脉成员数据时出错: {0}", ex.Message));
                                                // 继续处理下一个成员，不中断搜索
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                    // 搜索世家妻妾
                    if (currentNobleSubMode == 2 && Mainload.Member_Other_qu != null)
                    {
                        // 遍历所有世家
                        for (int familyIndex = 0; familyIndex < Mainload.Member_Other_qu.Count; familyIndex++)
                        {
                            if (Mainload.Member_Other_qu[familyIndex] != null)
                            {
                                // 检查家族选择状态：0表示全部家族，否则根据家族索引筛选
                                if (currentFamilyIndex == 0 || familyIndex == currentFamilyIndex)
                                {
                                    // 检查郡选择状态
                                    string[] countyNames = new string[] { "", "南郡", "三川郡", "蜀郡", "丹阳郡", "陈留郡", "长沙郡", "会稽郡", "广陵郡", "太原郡", "益州郡", "南海郡", "云南郡" };
                                    bool countyMatch = true;
                                    
                                    if (currentFarmAreaIndex > 0 && currentFarmAreaIndex < countyNames.Length && Mainload.ShiJia_Now != null && familyIndex < Mainload.ShiJia_Now.Count && Mainload.ShiJia_Now[familyIndex] != null && Mainload.ShiJia_Now[familyIndex].Count > 5 && !string.IsNullOrEmpty(Mainload.ShiJia_Now[familyIndex][5]))
                                    {
                                        // 解析位置信息
                                        string[] locationParts = Mainload.ShiJia_Now[familyIndex][5].Split('|');
                                        if (locationParts.Length >= 2)
                                        {
                                            int junIndex = -1;
                                            int.TryParse(locationParts[0], out junIndex);
                                            
                                            // 检查是否有对应的郡名数组JunList
                                            if (JunList != null && JunList.Length > 0)
                                            {
                                                // 使用JunList进行比较
                                                if (junIndex >= 0 && junIndex < JunList.Length)
                                                {
                                                    countyMatch = JunList[junIndex] == countyNames[currentFarmAreaIndex];
                                                }
                                                else
                                                {
                                                    countyMatch = false;
                                                }
                                            }
                                            else
                                            {
                                                // 如果没有JunList，则回退到简单的字符串包含比较
                                                string junName = countyNames[currentFarmAreaIndex];
                                                string locationText = Mainload.ShiJia_Now[familyIndex][5];
                                                countyMatch = locationText.Contains(junName);
                                            }
                                        }
                                    }
                                    
                                    if (countyMatch)
                                    {
                                        // 遍历每个世家的所有妻妾成员
                                        for (int memberIndex = 0; memberIndex < Mainload.Member_Other_qu[familyIndex].Count; memberIndex++)
                                        {
                                            try
                                            {
                                                if (Mainload.Member_Other_qu[familyIndex][memberIndex] != null && 
                                                    Mainload.Member_Other_qu[familyIndex][memberIndex].Count > 0)
                                                {
                                                    string[] memberInfo = Mainload.Member_Other_qu[familyIndex][memberIndex][2].Split('|');
                                                    if (memberInfo.Length > 0 && !string.IsNullOrEmpty(memberInfo[0]))
                                                    {
                                                        AddMemberIfMatches(memberInfo[0]);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Logger.LogError(string.Format("处理世家妻妾成员数据时出错: {0}", ex.Message));
                                                // 继续处理下一个成员，不中断搜索
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (currentMode == 4) // 农庄修改模式
                {
                    SearchFarmData();
                }
                
                // 按名称排序
                filteredMembers.Sort();
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("筛选成员时出错: {0}", ex.Message));
                filteredMembers.Clear();
            }
        }
        
        // 通用的成员搜索方法，提高代码复用性
        private void SearchMemberData(List<List<string>> dataList, int nameFieldIndex)
        {
            if (dataList == null || dataList.Count == 0)
                return;
            
            foreach (var memberData in dataList)
            {
                try
                {
                    if (memberData != null && memberData.Count > nameFieldIndex)
                    {
                        string nameField = memberData[nameFieldIndex];
                        if (!string.IsNullOrEmpty(nameField))
                        {
                            string[] memberInfo = nameField.Split('|');
                            if (memberInfo.Length > 0 && !string.IsNullOrEmpty(memberInfo[0]))
                            {
                                AddMemberIfMatches(memberInfo[0]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(string.Format("处理成员数据时出错: {0}", ex.Message));
                    // 继续处理下一个成员，不中断搜索
                }
            }
        }
        
        private void AddMemberIfMatches(string name)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                filteredMembers.Add(name);
            }
            else
            {
                // 不区分大小写的模糊搜索
                if (name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    filteredMembers.Add(name);
                }
            }
        }
        
        // 搜索农庄数据的方法
        private void SearchFarmData()
        {
            try
            {
                if (Mainload.NongZ_now == null)
                {
                    Logger.LogWarning("没有找到任何农庄数据");
                    return;
                }
                
                // 获取当前选择的区域索引（0-12，0为大地图，1-12为各郡）
                int areaIndex = currentFarmAreaIndex;
                
                if (Mainload.NongZ_now.Count > areaIndex)
                {
                    List<List<string>> areaFarmData = Mainload.NongZ_now[areaIndex];
                    if (areaFarmData != null)
                    {
                        foreach (var farmData in areaFarmData)
                        {
                            try
                            {
                                //farmData[0] == "-1"是判断农庄归属属于玩家
                                if (farmData != null && farmData.Count > 6 && farmData[0] == "-1")
                                {
                                    string farmName = farmData[6]; // 农庄名字，索引6
                                    if (!string.IsNullOrEmpty(farmName))
                                    {
                                        AddMemberIfMatches(farmName);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(string.Format("处理农庄数据时出错: {0}", ex.Message));
                                // 继续处理下一个农庄，不中断搜索
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("搜索农庄数据时出错: {0}", ex.Message));
            }
        }
        
        // 清空搜索文本
        private void ClearSearchText()
        {
            searchText = "";
            FilterMembers();
        }

        private void ResetAttributeValues()
        {
            // 重置族人家族属性
            A_reputation = "0";
            A_reputationOriginal = "0";
            A_title = "0";
            A_titleOriginal = "0";
            A_power = "0";
            A_powerOriginal = "0";
            A_age = "0";
            A_ageOriginal = "0";
            A_health = "0";
            A_healthOriginal = "0";
            A_mood = "0";
            A_moodOriginal = "0";
            A_charm = "0";
            A_charmOriginal = "0";
            A_luck = "0";
            A_luckOriginal = "0";
            A_preference = "0";
            A_preferenceOriginal = "0";
            A_character = "0";
            A_characterOriginal = "0";
            A_talent = "0";
            A_talentOriginal = "0";
            A_talentPoint = "0";
            A_talentPointOriginal = "0";
            A_skill = "0";
            A_skillOriginal = "0";
            A_skillPoint = "0";
            A_skillPointOriginal = "0";
            A_JueWei = "0";
            A_JueWeiOriginal = "0";
            A_FengDi = "0";
            A_FengDiOriginal = "0";
            A_officialRank = "0@0@0@-1@-1";
            A_officialRankOriginal = "0@0@0@-1@-1";
            
            // 重置族人妻妾属性
            B_reputation = "0";
            B_reputationOriginal = "0";
            B_power = "0";
            B_powerOriginal = "0";
            B_age = "0";
            B_ageOriginal = "0";
            B_health = "0";
            B_healthOriginal = "0";
            B_mood = "0";
            B_moodOriginal = "0";
            B_charm = "0";
            B_charmOriginal = "0";
            B_luck = "0";
            B_luckOriginal = "0";
            B_preference = "0";
            B_preferenceOriginal = "0";
            B_character = "0";
            B_characterOriginal = "0";
            B_talent = "0";
            B_talentOriginal = "0";
            B_talentPoint = "0";
            B_talentPointOriginal = "0";
            B_skill = "0";
            B_skillOriginal = "0";
            B_skillPoint = "0";
            B_skillPointOriginal = "0";
            
            // 重置门客属性
            C_reputation = "0";
            C_reputationOriginal = "0";
            C_power = "0";
            C_powerOriginal = "0";
            C_age = "0";
            C_ageOriginal = "0";
            C_health = "0";
            C_healthOriginal = "0";
            C_mood = "0";
            C_moodOriginal = "0";
            C_charm = "0";
            C_charmOriginal = "0";
            C_luck = "0";
            C_luckOriginal = "0";
            C_character = "0";
            C_characterOriginal = "0";
            C_talent = "0";
            C_talentOriginal = "0";
            C_talentPoint = "0";
            C_talentPointOriginal = "0";
            C_skill = "0";
            C_skillOriginal = "0";
            C_skillPoint = "0";
            C_skillPointOriginal = "0";
            C_intelligence = "0";
            C_intelligenceOriginal = "0";
            C_weapon = "0";
            C_weaponOriginal = "0";
            C_business = "0";
            C_businessOriginal = "0";
            C_art = "0";
            C_artOriginal = "0";
            C_strategy = "0";
            C_strategyOriginal = "0";
            C_MenKe_Reward = "0";
            C_MenKe_RewardOriginal = "0";
            
            // 重置皇室主脉属性
            D_reputation = "0";
            D_reputationOriginal = "0";
            D_age = "0";
            D_ageOriginal = "0";
            D_health = "0";
            D_healthOriginal = "0";
            D_mood = "0";
            D_moodOriginal = "0";
            D_charm = "0";
            D_charmOriginal = "0";
            D_luck = "0";
            D_luckOriginal = "0";
            D_preference = "0";
            D_preferenceOriginal = "0";
            D_character = "0";
            D_characterOriginal = "0";
            D_talent = "0";
            D_talentOriginal = "0";
            D_talentPoint = "0";
            D_talentPointOriginal = "0";
            D_skill = "0";
            D_skillOriginal = "0";
            D_skillPoint = "0";
            D_skillPointOriginal = "0";
            D_intelligence = "0";
            D_intelligenceOriginal = "0";
            D_weapon = "0";
            D_weaponOriginal = "0";
            D_business = "0";
            D_businessOriginal = "0";
            D_art = "0";
            D_artOriginal = "0";
            D_strategy = "0";
            D_strategyOriginal = "0";
            
            // 重置皇家妻妾属性
            E_reputation = "0";
            E_reputationOriginal = "0";
            E_age = "0";
            E_ageOriginal = "0";
            E_health = "0";
            E_healthOriginal = "0";
            E_mood = "0";
            E_moodOriginal = "0";
            E_charm = "0";
            E_charmOriginal = "0";
            E_luck = "0";
            E_luckOriginal = "0";
            E_preference = "0";
            E_preferenceOriginal = "0";
            E_character = "0";
            E_characterOriginal = "0";
            E_talent = "0";
            E_talentOriginal = "0";
            E_talentPoint = "0";
            E_talentPointOriginal = "0";
            E_skill = "0";
            E_skillOriginal = "0";
            E_skillPoint = "0";
            E_skillPointOriginal = "0";
            E_intelligence = "0";
            E_intelligenceOriginal = "0";
            E_weapon = "0";
            E_weaponOriginal = "0";
            E_business = "0";
            E_businessOriginal = "0";
            E_art = "0";
            E_artOriginal = "0";
            E_strategy = "0";
            E_strategyOriginal = "0";
            
            // 重置世家主脉属性
            F_reputation = "0";
            F_reputationOriginal = "0";
            F_age = "0";
            F_ageOriginal = "0";
            F_health = "0";
            F_healthOriginal = "0";
            F_mood = "0";
            F_moodOriginal = "0";
            F_charm = "0";
            F_charmOriginal = "0";
            F_luck = "0";
            F_luckOriginal = "0";
            F_preference = "0";
            F_preferenceOriginal = "0";
            F_character = "0";
            F_characterOriginal = "0";
            F_talent = "0";
            F_talentOriginal = "0";
            F_talentPoint = "0";
            F_talentPointOriginal = "0";
            F_skill = "0";
            F_skillOriginal = "0";
            F_skillPoint = "0";
            F_skillPointOriginal = "0";
            F_intelligence = "0";
            F_intelligenceOriginal = "0";
            F_weapon = "0";
            F_weaponOriginal = "0";
            F_business = "0";
            F_businessOriginal = "0";
            F_art = "0";
            F_artOriginal = "0";
            F_strategy = "0";
            F_strategyOriginal = "0";
            
            // 重置世家妻妾属性
            G_reputation = "0";
            G_reputationOriginal = "0";
            G_age = "0";
            G_ageOriginal = "0";
            G_health = "0";
            G_healthOriginal = "0";
            G_mood = "0";
            G_moodOriginal = "0";
            G_charm = "0";
            G_charmOriginal = "0";
            G_luck = "0";
            G_luckOriginal = "0";
            G_preference = "0";
            G_preferenceOriginal = "0";
            G_character = "0";
            G_characterOriginal = "0";
            G_talent = "0";
            G_talentOriginal = "0";
            G_talentPoint = "0";
            G_talentPointOriginal = "0";
            G_skill = "0";
            G_skillOriginal = "0";
            G_skillPoint = "0";
            G_skillPointOriginal = "0";
            G_intelligence = "0";
            G_intelligenceOriginal = "0";
            G_weapon = "0";
            G_weaponOriginal = "0";
            G_business = "0";
            G_businessOriginal = "0";
            G_art = "0";
            G_artOriginal = "0";
            G_strategy = "0";
            G_strategyOriginal = "0";
            
            // 重置选择状态
            selectedMemberName = "";
            currentDataIndex = -1;
            
            // 重置数据索引变量
            Member_nowData = null;
            Member_quData = null;
            Member_MenKeData = null;
            
            // 重置农庄修改属性
            H_area = "0";
            H_areaOriginal = "0";
            H_farmersPlanting = "0";
            H_farmersPlantingOriginal = "0";
            H_farmersBreeding = "0";
            H_farmersBreedingOriginal = "0";
            H_farmersCrafting = "0";
            H_farmersCraftingOriginal = "0";
            H_zhuangTouAge = "0";
            H_zhuangTouAgeOriginal = "0";
            H_zhuangTouManagement = "0";
            H_zhuangTouManagementOriginal = "0";
            H_zhuangTouLoyalty = "0";
            H_zhuangTouLoyaltyOriginal = "0";
            H_zhuangTouCharacter = "0";
            H_zhuangTouCharacterOriginal = "0";
        }
        
        // 为不同模式定义独立的数据索引变量
        private List<string> Member_nowData = null; // 族人家族数据
        private List<string> Member_quData = null; // 族人妻妾数据
        private List<string> Member_MenKeData = null; // 门客数据
        private int currentDataIndex = -1; // 当前选中成员的索引

        // 根据选择的各个大模式和小模式加载数据
        private void LoadMemberData(string memberName)
        {
            try
            {
                if (currentMode == 0) // 族人模式
                {
                    // 根据子模式选择正确的数据结构
                    bool found = false;
                    currentDataIndex = -1;

                    // 加载族人家族数据
                    if (currentMemberSubMode == 0 || currentMemberSubMode == 1)
                    {
                        if (Mainload.Member_now != null)
                        {
                            for (int i = 0; i < Mainload.Member_now.Count; i++)
                            {
                                Member_nowData = Mainload.Member_now[i];
                                string[] memberInfo = Member_nowData[4].Split('|');
                                string name = memberInfo[0];

                                if (name == memberName)
                                {
                                    // 加载族人家族成员数据
                                    // 读取完整的官职数据，索引12
                                    if (Member_nowData.Count > 12 && !string.IsNullOrEmpty(Member_nowData[12]))
                                    {
                                        A_officialRank = Member_nowData[12];
                                    }
                                    A_officialRankOriginal = A_officialRank; // 保存原始值

                                    A_reputation = Member_nowData[16]; // 声誉
                                    A_reputationOriginal = A_reputation; // 保存原始值
                                    A_power = Member_nowData[30]; // 体力
                                    A_powerOriginal = A_power; // 保存原始值
                                    A_age = Member_nowData[6]; // 年龄
                                    A_ageOriginal = A_age; // 保存原始值
                                    A_health = Member_nowData[21]; // 健康
                                    A_healthOriginal = A_health; // 保存原始值
                                    A_mood = Member_nowData[11]; // 心情
                                    A_moodOriginal = A_mood; // 保存原始值
                                    A_charm = Member_nowData[20]; // 魅力
                                    A_charmOriginal = A_charm; // 保存原始值
                                    A_luck = memberInfo.Length > 7 ? memberInfo[7] : "0"; // 幸运
                                    A_luckOriginal = A_luck; // 保存原始值
                                    A_character = Member_nowData[5]; // 品性
                                    A_characterOriginal = A_character; // 保存原始值
                                    A_talent = memberInfo.Length > 2 ? memberInfo[2] : "0"; // 天赋
                                    A_talentOriginal = A_talent; // 保存原始值
                                    A_talentPoint = memberInfo.Length > 3 ? memberInfo[3] : "0"; // 天赋点
                                    A_talentPointOriginal = A_talentPoint; // 保存原始值
                                    A_skill = memberInfo.Length > 6 ? memberInfo[6] : "0"; // 技能
                                    A_skillOriginal = A_skill; // 保存原始值
                                    A_skillPoint = Member_nowData[33]; // 技能点
                                    A_skillPointOriginal = A_skillPoint; // 保存原始值
                                    A_preference = memberInfo.Length > 9 ? memberInfo[9] : "0"; // 喜好
                                    A_preferenceOriginal = A_preference; // 保存原始值
                                    A_intelligence = Member_nowData[7]; // 文才
                                    A_intelligenceOriginal = A_intelligence; // 保存原始值
                                    A_weapon = Member_nowData[8]; // 武才
                                    A_weaponOriginal = A_weapon; // 保存原始值
                                    A_business = Member_nowData[9]; // 商才
                                    A_businessOriginal = A_business; // 保存原始值
                                    A_art = Member_nowData[10]; // 艺才
                                    A_artOriginal = A_art; // 保存原始值
                                    A_strategy = Member_nowData[27]; // 计谋
                                    A_strategyOriginal = A_strategy; // 保存原始值
                                    string[] jueWeiFengDi = Member_nowData[14].Split('|');
                                    A_JueWei = jueWeiFengDi.Length > 0 ? jueWeiFengDi[0] : "0"; // 爵位
                                    A_JueWeiOriginal = A_JueWei; // 保存原始值
                                    A_FengDi = jueWeiFengDi.Length > 1 ? jueWeiFengDi[1] : "0"; // 封地
                                    A_FengDiOriginal = A_FengDi; // 保存原始值

                                    found = true;
                                    currentDataIndex = i;
                                    break;
                                }
                            }
                        }
                    }

                    // 加载族人妻妾数据
                    if (!found && currentMode == 0 && currentMemberSubMode == 2)
                    {
                        if (Mainload.Member_qu != null)
                        {
                            for (int i = 0; i < Mainload.Member_qu.Count; i++)
                            {
                                Member_quData = Mainload.Member_qu[i];
                                string[] memberInfo = Member_quData[2].Split('|');
                                string name = memberInfo[0];

                                if (name == memberName)
                                {
                                    // 加载族人妻妾成员数据
                                    B_reputation = Member_quData[12]; // 声誉
                                    B_reputationOriginal = B_reputation; // 保存原始值
                                    B_power = Member_quData[20]; // 体力
                                    B_powerOriginal = B_power; // 保存原始值
                                    B_age = Member_quData[5]; // 年龄
                                    B_ageOriginal = B_age; // 保存原始值
                                    B_health = Member_quData[16]; // 健康
                                    B_healthOriginal = B_health; // 保存原始值
                                    B_mood = Member_quData[10]; // 心情
                                    B_moodOriginal = B_mood; // 保存原始值
                                    B_charm = Member_quData[15]; // 魅力
                                    B_charmOriginal = B_charm; // 保存原始值
                                    B_luck = memberInfo.Length > 7 ? memberInfo[7] : "0"; // 幸运
                                    B_luckOriginal = B_luck; // 保存原始值
                                    B_character = memberInfo.Length > 8 ? memberInfo[8] : "0"; // 品性
                                    B_characterOriginal = B_character; // 保存原始值
                                    B_preference = memberInfo.Length > 10 ? memberInfo[10] : "0"; // 喜好
                                    B_preferenceOriginal = B_preference; // 保存原始值
                                    B_talent = memberInfo.Length > 2 ? memberInfo[2] : "0"; // 天赋
                                    B_talentOriginal = B_talent; // 保存原始值
                                    B_talentPoint = memberInfo.Length > 3 ? memberInfo[3] : "0"; // 天赋点
                                    B_talentPointOriginal = B_talentPoint; // 保存原始值
                                    B_skill = memberInfo.Length > 6 ? memberInfo[6] : "0"; // 技能
                                    B_skillOriginal = B_skill; // 保存原始值
                                    B_skillPoint = Member_quData[23]; // 技能点
                                    B_skillPointOriginal = B_skillPoint; // 保存原始值
                                    B_intelligence = Member_quData[6]; // 文才
                                    B_intelligenceOriginal = B_intelligence; // 保存原始值
                                    B_weapon = Member_quData[7]; // 武才
                                    B_weaponOriginal = B_weapon; // 保存原始值
                                    B_business = Member_quData[8]; // 商才
                                    B_businessOriginal = B_business; // 保存原始值
                                    B_art = Member_quData[9]; // 艺才
                                    B_artOriginal = B_art; // 保存原始值
                                    B_strategy = Member_quData[19]; // 计谋
                                    B_strategyOriginal = B_strategy; // 保存原始值

                                    found = true;
                                    currentDataIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (currentMode == 1) // 门客模式
                {
                    // 加载门客数据
                    if (Mainload.MenKe_Now != null)
                    {
                        for (int i = 0; i < Mainload.MenKe_Now.Count; i++)
                        {
                            Member_MenKeData = Mainload.MenKe_Now[i];
                            string[] menkeInfo = Member_MenKeData[2].Split('|'); // 名字
                            string name = menkeInfo[0];

                            if (name == memberName)
                            {
                                C_reputation = Member_MenKeData[11]; // 声誉
                                C_reputationOriginal = C_reputation; // 保存原始值
                                C_power = Member_MenKeData[19]; // 体力
                                C_powerOriginal = C_power; // 保存原始值
                                C_age = Member_MenKeData[3]; // 年龄
                                C_ageOriginal = C_age; // 保存原始值
                                C_health = Member_MenKeData[14]; // 健康
                                C_healthOriginal = C_health; // 保存原始值
                                C_mood = Member_MenKeData[8]; // 心情
                                C_moodOriginal = C_mood; // 保存原始值
                                C_charm = Member_MenKeData[13]; // 魅力
                                C_charmOriginal = C_charm; // 保存原始值
                                string[] menkeDataArray = Member_MenKeData[2].Split('|');
                                C_luck = menkeDataArray.Length > 7 ? menkeDataArray[7] : "0"; // 幸运
                                C_luckOriginal = C_luck; // 保存原始值
                                C_character = menkeDataArray.Length > 8 ? menkeDataArray[8] : "0"; // 品性
                                C_characterOriginal = C_character; // 保存原始值
                                C_talent = menkeInfo.Length > 2 ? menkeInfo[2] : "0"; // 天赋
                                C_talentOriginal = C_talent; // 保存原始值
                                C_talentPoint = menkeInfo.Length > 3 ? menkeInfo[3] : "0"; // 天赋点
                                C_talentPointOriginal = C_talentPoint; // 保存原始值
                                C_skill = menkeInfo.Length > 6 ? menkeInfo[6] : "0"; // 技能
                                C_skillOriginal = C_skill; // 保存原始值
                                C_skillPoint = Member_MenKeData[16]; // 技能点
                                C_skillPointOriginal = C_skillPoint; // 保存原始值
                                C_intelligence = Member_MenKeData[4]; // 文才
                                C_intelligenceOriginal = C_intelligence; // 保存原始值
                                C_weapon = Member_MenKeData[5]; // 武才
                                C_weaponOriginal = C_weapon; // 保存原始值
                                C_business = Member_MenKeData[6]; // 商才
                                C_businessOriginal = C_business; // 保存原始值
                                C_art = Member_MenKeData[7]; // 艺才
                                C_artOriginal = C_art; // 保存原始值
                                C_strategy = Member_MenKeData[15]; // 计谋
                                C_strategyOriginal = C_strategy; // 保存原始值
                                C_MenKe_Reward = Member_MenKeData[18]; // 门客工资
                                C_MenKe_RewardOriginal = C_MenKe_Reward; // 保存原始值

                                currentDataIndex = i;
                                break;
                            }
                        }
                    }
                }
                else if (currentMode == 2) // 皇室模式
                {
                    // 根据子模式选择正确的数据结构
                    bool found = false;
                    currentDataIndex = -1;

                    if (currentRoyaltySubMode == 1) // 加载皇室主脉数据
                    {
                        if (Mainload.Member_King != null)
                        {
                            for (int i = 0; i < Mainload.Member_King.Count; i++)
                            {
                                List<string> royalData = Mainload.Member_King[i];
                                string[] royalInfo = royalData[2].Split('|');
                                string name = royalInfo[0];

                                if (name == memberName)
                                {
                                    // 加载皇室主脉成员数据
                                    // 基本属性
                                    D_reputation = royalData.Count > 16 && !string.IsNullOrEmpty(royalData[16]) ? royalData[16] : "0"; // 声誉
                                    D_reputationOriginal = D_reputation; // 保存原始值
                                    D_age = royalData.Count > 3 && !string.IsNullOrEmpty(royalData[3]) ? royalData[3] : "0"; // 年龄
                                    D_ageOriginal = D_age; // 保存原始值
                                    D_health = royalData.Count > 19 && !string.IsNullOrEmpty(royalData[19]) ? royalData[19] : "0"; // 健康
                                    D_healthOriginal = D_health; // 保存原始值
                                    D_mood = royalData.Count > 8 && !string.IsNullOrEmpty(royalData[8]) ? royalData[8] : "0"; // 心情
                                    D_moodOriginal = D_mood; // 保存原始值
                                    D_charm = royalData.Count > 18 && !string.IsNullOrEmpty(royalData[18]) ? royalData[18] : "0"; // 魅力
                                    D_charmOriginal = D_charm; // 保存原始值
                                    D_luck = royalInfo.Length > 7 && !string.IsNullOrEmpty(royalInfo[7]) ? royalInfo[7] : "0"; // 幸运
                                    D_luckOriginal = D_luck; // 保存原始值
                                    D_character = royalInfo.Length > 8 && !string.IsNullOrEmpty(royalInfo[8]) ? royalInfo[8] : "0"; // 品性
                                    D_characterOriginal = D_character; // 保存原始值
                                    D_talent = royalInfo.Length > 2 && !string.IsNullOrEmpty(royalInfo[2]) ? royalInfo[2] : "0"; // 天赋
                                    D_talentOriginal = D_talent; // 保存原始值
                                    D_talentPoint = royalInfo.Length > 3 && !string.IsNullOrEmpty(royalInfo[3]) ? royalInfo[3] : "0"; // 天赋点
                                    D_talentPointOriginal = D_talentPoint; // 保存原始值
                                    D_skill = royalInfo.Length > 6 && !string.IsNullOrEmpty(royalInfo[6]) ? royalInfo[6] : "0"; // 技能
                                    D_skillOriginal = D_skill; // 保存原始值
                                    D_skillPoint = royalData.Count > 23 && !string.IsNullOrEmpty(royalData[23]) ? royalData[23] : "0"; // 技能点
                                    D_skillPointOriginal = D_skillPoint; // 保存原始值
                                    D_preference = royalInfo.Length > 1 && !string.IsNullOrEmpty(royalInfo[1]) ? royalInfo[1] : "0"; // 喜好
                                    D_preferenceOriginal = D_preference; // 保存原始值
                                    D_intelligence = royalData.Count > 4 && !string.IsNullOrEmpty(royalData[4]) ? royalData[4] : "0"; // 文才
                                    D_intelligenceOriginal = D_intelligence; // 保存原始值
                                    D_weapon = royalData.Count > 5 && !string.IsNullOrEmpty(royalData[5]) ? royalData[5] : "0"; // 武才
                                    D_weaponOriginal = D_weapon; // 保存原始值
                                    D_business = royalData.Count > 6 && !string.IsNullOrEmpty(royalData[6]) ? royalData[6] : "0"; // 商才
                                    D_businessOriginal = D_business; // 保存原始值
                                    D_art = royalData.Count > 7 && !string.IsNullOrEmpty(royalData[7]) ? royalData[7] : "0"; // 艺才
                                    D_artOriginal = D_art; // 保存原始值
                                    D_strategy = royalData.Count > 21 && !string.IsNullOrEmpty(royalData[21]) ? royalData[21] : "0"; // 计谋
                                    D_strategyOriginal = D_strategy; // 保存原始值

                                    found = true;
                                    currentDataIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                    else if (currentRoyaltySubMode == 2) // 加载皇家妻妾数据
                    {
                        if (Mainload.Member_King_qu != null)
                        {
                            for (int i = 0; i < Mainload.Member_King_qu.Count; i++)
                            {
                                List<string> royalSpouseData = Mainload.Member_King_qu[i];
                                string[] royalSpouseInfo = royalSpouseData[2].Split('|');
                                string name = royalSpouseInfo[0];

                                if (name == memberName)
                                {
                                    // 加载皇家妻妾成员数据
                                    E_reputation = royalSpouseData.Count > 10 && !string.IsNullOrEmpty(royalSpouseData[10]) ? royalSpouseData[10] : "0"; // 声誉
                                    E_reputationOriginal = E_reputation; // 保存原始值
                                    E_age = royalSpouseData.Count > 3 && !string.IsNullOrEmpty(royalSpouseData[3]) ? royalSpouseData[3] : "0"; // 年龄
                                    E_ageOriginal = E_age; // 保存原始值
                                    E_health = royalSpouseData.Count > 16 && !string.IsNullOrEmpty(royalSpouseData[16]) ? royalSpouseData[16] : "0"; // 健康
                                    E_healthOriginal = E_health; // 保存原始值
                                    E_mood = royalSpouseData.Count > 8 && !string.IsNullOrEmpty(royalSpouseData[8]) ? royalSpouseData[8] : "0"; // 心情
                                    E_moodOriginal = E_mood; // 保存原始值
                                    E_charm = royalSpouseData.Count > 15 && !string.IsNullOrEmpty(royalSpouseData[15]) ? royalSpouseData[15] : "0"; // 魅力
                                    E_charmOriginal = E_charm; // 保存原始值
                                    E_luck = royalSpouseInfo.Length > 7 && !string.IsNullOrEmpty(royalSpouseInfo[7]) ? royalSpouseInfo[7] : "0"; // 幸运
                                    E_luckOriginal = E_luck; // 保存原始值
                                    E_character = royalSpouseInfo.Length > 8 && !string.IsNullOrEmpty(royalSpouseInfo[8]) ? royalSpouseInfo[8] : "0"; // 品性
                                    E_characterOriginal = E_character; // 保存原始值
                                    E_talent = royalSpouseInfo.Length > 2 && !string.IsNullOrEmpty(royalSpouseInfo[2]) ? royalSpouseInfo[2] : "0"; // 天赋
                                    E_talentOriginal = E_talent; // 保存原始值
                                    E_talentPoint = royalSpouseInfo.Length > 3 && !string.IsNullOrEmpty(royalSpouseInfo[3]) ? royalSpouseInfo[3] : "0"; // 天赋点
                                    E_talentPointOriginal = E_talentPoint; // 保存原始值
                                    E_skill = royalSpouseInfo.Length > 6 && !string.IsNullOrEmpty(royalSpouseInfo[6]) ? royalSpouseInfo[6] : "0"; // 技能
                                    E_skillOriginal = E_skill; // 保存原始值
                                    E_skillPoint = royalSpouseData.Count > 20 && !string.IsNullOrEmpty(royalSpouseData[20]) ? royalSpouseData[20] : "0"; // 技能点
                                    E_skillPointOriginal = E_skillPoint; // 保存原始值
                                    E_preference = royalSpouseInfo.Length > 10 && !string.IsNullOrEmpty(royalSpouseInfo[10]) ? royalSpouseInfo[10] : "0"; // 喜好
                                    E_preferenceOriginal = E_preference; // 保存原始值
                                    E_intelligence = royalSpouseData.Count > 4 && !string.IsNullOrEmpty(royalSpouseData[4]) ? royalSpouseData[4] : "0"; // 文才
                                    E_intelligenceOriginal = E_intelligence; // 保存原始值
                                    E_weapon = royalSpouseData.Count > 5 && !string.IsNullOrEmpty(royalSpouseData[5]) ? royalSpouseData[5] : "0"; // 武才
                                    E_weaponOriginal = E_weapon; // 保存原始值
                                    E_business = royalSpouseData.Count > 6 && !string.IsNullOrEmpty(royalSpouseData[6]) ? royalSpouseData[6] : "0"; // 商才
                                    E_businessOriginal = E_business; // 保存原始值
                                    E_art = royalSpouseData.Count > 7 && !string.IsNullOrEmpty(royalSpouseData[7]) ? royalSpouseData[7] : "0"; // 艺才
                                    E_artOriginal = E_art; // 保存原始值
                                    E_strategy = royalSpouseData.Count > 17 && !string.IsNullOrEmpty(royalSpouseData[17]) ? royalSpouseData[17] : "0"; // 计谋
                                    E_strategyOriginal = E_strategy; // 保存原始值

                                    found = true;
                                    currentDataIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (currentMode == 3) // 世家模式
                {
                    // 检查数据是否可用
                    bool hasNobleFamilyData = Mainload.Member_other != null && Mainload.Member_other.Count > 0;
                    bool hasNobleSpouseData = Mainload.Member_Other_qu != null && Mainload.Member_Other_qu.Count > 0;
                    
                    if (!hasNobleFamilyData && !hasNobleSpouseData)
                    {
                        Logger.LogWarning("没有找到任何世家数据");
                        return;
                    }
                    
                    // 加载世家主脉成员数据
                    if (currentNobleSubMode == 1 && Mainload.Member_other != null)
                    {
                        for (int familyIndex = 0; familyIndex < Mainload.Member_other.Count; familyIndex++)
                        {
                            if (Mainload.Member_other[familyIndex] != null)
                            {
                                for (int memberIndex = 0; memberIndex < Mainload.Member_other[familyIndex].Count; memberIndex++)
                                {
                                    try
                                    {
                                        if (Mainload.Member_other[familyIndex][memberIndex] != null && 
                                            Mainload.Member_other[familyIndex][memberIndex].Count > 0)
                                        {
                                            string[] memberInfo = Mainload.Member_other[familyIndex][memberIndex][2].Split('|');
                                            string name = memberInfo[0];
                                            
                                            if (name == memberName)
                                            {
                                                // 加载世家主脉成员属性
                                                string[] memberData = Mainload.Member_other[familyIndex][memberIndex].ToArray();
                                                
                                                F_reputation = memberData.Length > 17 && !string.IsNullOrEmpty(memberData[17]) ? memberData[17] : "0"; // 声誉
                                                F_reputationOriginal = F_reputation; // 保存原始值
                                                F_age = memberData.Length > 3 && !string.IsNullOrEmpty(memberData[3]) ? memberData[3] : "0"; // 年龄
                                                F_ageOriginal = F_age; // 保存原始值
                                                F_health = memberData.Length > 20 && !string.IsNullOrEmpty(memberData[20]) ? memberData[20] : "0"; // 健康
                                                F_healthOriginal = F_health; // 保存原始值
                                                F_mood = memberData.Length > 8 && !string.IsNullOrEmpty(memberData[8]) ? memberData[8] : "0"; // 心情
                                                F_moodOriginal = F_mood; // 保存原始值
                                                F_charm = memberData.Length > 19 && !string.IsNullOrEmpty(memberData[19]) ? memberData[19] : "0"; // 魅力
                                                F_charmOriginal = F_charm; // 保存原始值
                                                F_luck = memberInfo.Length > 7 && !string.IsNullOrEmpty(memberInfo[7]) ? memberInfo[7] : "0"; // 幸运
                                                F_luckOriginal = F_luck; // 保存原始值
                                                F_character = memberInfo.Length > 8 && !string.IsNullOrEmpty(memberInfo[8]) ? memberInfo[8] : "0"; // 品性
                                                F_characterOriginal = F_character; // 保存原始值
                                                F_talent = memberInfo.Length > 2 && !string.IsNullOrEmpty(memberInfo[2]) ? memberInfo[2] : "0"; // 天赋
                                                F_talentOriginal = F_talent; // 保存原始值
                                                F_talentPoint = memberInfo.Length > 3 && !string.IsNullOrEmpty(memberInfo[3]) ? memberInfo[3] : "0"; // 天赋点
                                                F_talentPointOriginal = F_talentPoint; // 保存原始值
                                                F_skill = memberInfo.Length > 6 && !string.IsNullOrEmpty(memberInfo[6]) ? memberInfo[6] : "0"; // 技能
                                                F_skillOriginal = F_skill; // 保存原始值
                                                F_skillPoint = memberData.Length > 25 && !string.IsNullOrEmpty(memberData[25]) ? memberData[25] : "0"; // 技能点
                                                F_skillPointOriginal = F_skillPoint; // 保存原始值
                                                F_preference = memberInfo.Length > 1 && !string.IsNullOrEmpty(memberInfo[1]) ? memberInfo[1] : "0"; // 喜好
                                                F_preferenceOriginal = F_preference; // 保存原始值
                                                F_intelligence = memberData.Length > 4 && !string.IsNullOrEmpty(memberData[4]) ? memberData[4] : "0"; // 文才
                                                F_intelligenceOriginal = F_intelligence; // 保存原始值
                                                F_weapon = memberData.Length > 5 && !string.IsNullOrEmpty(memberData[5]) ? memberData[5] : "0"; // 武才
                                                F_weaponOriginal = F_weapon; // 保存原始值
                                                F_business = memberData.Length > 6 && !string.IsNullOrEmpty(memberData[6]) ? memberData[6] : "0"; // 商才
                                                F_businessOriginal = F_business; // 保存原始值
                                                F_art = memberData.Length > 7 && !string.IsNullOrEmpty(memberData[7]) ? memberData[7] : "0"; // 艺才
                                                F_artOriginal = F_art; // 保存原始值
                                                F_strategy = memberData.Length > 22 && !string.IsNullOrEmpty(memberData[22]) ? memberData[22] : "0"; // 计谋
                                                F_strategyOriginal = F_strategy; // 保存原始值
                                                
                                                currentDataIndex = memberIndex;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogError(string.Format("处理世家主脉成员数据时出错: {0}", ex.Message));
                                        // 继续处理下一个成员，不中断搜索
                                    }
                                }
                            }
                        }
                    }
                    
                    // 加载世家妻妾成员数据
                    if (currentNobleSubMode == 2 && Mainload.Member_Other_qu != null)
                    {
                        for (int familyIndex = 0; familyIndex < Mainload.Member_Other_qu.Count; familyIndex++)
                        {
                            if (Mainload.Member_Other_qu[familyIndex] != null)
                            {
                                for (int memberIndex = 0; memberIndex < Mainload.Member_Other_qu[familyIndex].Count; memberIndex++)
                                {
                                    try
                                    {
                                        if (Mainload.Member_Other_qu[familyIndex][memberIndex] != null && 
                                            Mainload.Member_Other_qu[familyIndex][memberIndex].Count > 0)
                                        {
                                            string[] memberInfo = Mainload.Member_Other_qu[familyIndex][memberIndex][2].Split('|');
                                            string name = memberInfo[0];
                                            
                                            if (name == memberName)
                                            {
                                                // 加载世家妻妾成员属性
                                                string[] memberData = Mainload.Member_Other_qu[familyIndex][memberIndex].ToArray();
                                                
                                                G_reputation = memberData.Length > 10 && !string.IsNullOrEmpty(memberData[10]) ? memberData[10] : "0"; // 声誉
                                                G_reputationOriginal = G_reputation; // 保存原始值
                                                G_age = memberData.Length > 3 && !string.IsNullOrEmpty(memberData[3]) ? memberData[3] : "0"; // 年龄
                                                G_ageOriginal = G_age; // 保存原始值
                                                G_health = memberData.Length > 16 && !string.IsNullOrEmpty(memberData[16]) ? memberData[16] : "0"; // 健康
                                                G_healthOriginal = G_health; // 保存原始值
                                                G_mood = memberData.Length > 8 && !string.IsNullOrEmpty(memberData[8]) ? memberData[8] : "0"; // 心情
                                                G_moodOriginal = G_mood; // 保存原始值
                                                G_charm = memberData.Length > 15 && !string.IsNullOrEmpty(memberData[15]) ? memberData[15] : "0"; // 魅力
                                                G_charmOriginal = G_charm; // 保存原始值
                                                G_luck = memberInfo.Length > 7 && !string.IsNullOrEmpty(memberInfo[7]) ? memberInfo[7] : "0"; // 幸运
                                                G_luckOriginal = G_luck; // 保存原始值
                                                G_character = memberInfo.Length > 8 && !string.IsNullOrEmpty(memberInfo[8]) ? memberInfo[8] : "0"; // 品性
                                                G_characterOriginal = G_character; // 保存原始值
                                                G_talent = memberInfo.Length > 2 && !string.IsNullOrEmpty(memberInfo[2]) ? memberInfo[2] : "0"; // 天赋
                                                G_talentOriginal = G_talent; // 保存原始值
                                                G_talentPoint = memberInfo.Length > 3 && !string.IsNullOrEmpty(memberInfo[3]) ? memberInfo[3] : "0"; // 天赋点
                                                G_talentPointOriginal = G_talentPoint; // 保存原始值
                                                G_skill = memberInfo.Length > 6 && !string.IsNullOrEmpty(memberInfo[6]) ? memberInfo[6] : "0"; // 技能
                                                G_skillOriginal = G_skill; // 保存原始值
                                                G_skillPoint = memberData.Length > 19 && !string.IsNullOrEmpty(memberData[19]) ? memberData[19] : "0"; // 技能点
                                                G_skillPointOriginal = G_skillPoint; // 保存原始值
                                                G_preference = memberInfo.Length > 10 && !string.IsNullOrEmpty(memberInfo[10]) ? memberInfo[10] : "0"; // 喜好
                                                G_preferenceOriginal = G_preference; // 保存原始值
                                                G_intelligence = memberData.Length > 4 && !string.IsNullOrEmpty(memberData[4]) ? memberData[4] : "0"; // 文才
                                                G_intelligenceOriginal = G_intelligence; // 保存原始值
                                                G_weapon = memberData.Length > 5 && !string.IsNullOrEmpty(memberData[5]) ? memberData[5] : "0"; // 武才
                                                G_weaponOriginal = G_weapon; // 保存原始值
                                                G_business = memberData.Length > 6 && !string.IsNullOrEmpty(memberData[6]) ? memberData[6] : "0"; // 商才
                                                G_businessOriginal = G_business; // 保存原始值
                                                G_art = memberData.Length > 7 && !string.IsNullOrEmpty(memberData[7]) ? memberData[7] : "0"; // 艺才
                                                G_artOriginal = G_art; // 保存原始值
                                                G_strategy = memberData.Length > 17 && !string.IsNullOrEmpty(memberData[17]) ? memberData[17] : "0"; // 计谋
                                                G_strategyOriginal = G_strategy; // 保存原始值
                                                
                                                currentDataIndex = memberIndex;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogError(string.Format("处理世家妻妾成员数据时出错: {0}", ex.Message));
                                        // 继续处理下一个成员，不中断搜索
                                    }
                                }
                            }
                        }
                    }
                }
                else if (currentMode == 4) // 农庄修改模式
                {
                    // 加载农庄数据
                    if (Mainload.NongZ_now != null)
                    {
                        int areaIndex = currentFarmAreaIndex;
                        if (Mainload.NongZ_now.Count > areaIndex)
                        {
                            List<List<string>> areaFarmData = Mainload.NongZ_now[areaIndex];
                            if (areaFarmData != null)
                            {
                                for (int i = 0; i < areaFarmData.Count; i++)
                                {
                                    var farmData = areaFarmData[i];
                                    if (farmData != null && farmData.Count > 6 && farmData[6] == memberName)
                                    {
                                        // 保存当前农庄数据引用和索引
                                        currentFarmData = farmData;
                                        currentFarmIndex = i;

                                        // 加载农庄属性
                                        H_area = farmData[5]; // 面积，索引5
                                        H_areaOriginal = H_area; // 保存原始值

                                        // 加载可居住上限，索引7
                                        if (farmData.Count > 7 && !string.IsNullOrEmpty(farmData[7]))
                                        {
                                            H_maxResidents = farmData[7];
                                        }
                                        H_maxResidentsOriginal = H_maxResidents; // 保存原始值

                                        // 加载农户数量（种植|养殖|手工），索引24
                                        if (farmData.Count > 24 && !string.IsNullOrEmpty(farmData[24]))
                                        {
                                            string[] farmerCounts = farmData[24].Split('|');
                                            if (farmerCounts.Length > 0) H_farmersPlanting = farmerCounts[0];
                                            if (farmerCounts.Length > 1) H_farmersBreeding = farmerCounts[1];
                                            if (farmerCounts.Length > 2) H_farmersCrafting = farmerCounts[2];
                                        }
                                        H_farmersPlantingOriginal = H_farmersPlanting;
                                        H_farmersBreedingOriginal = H_farmersBreeding;
                                        H_farmersCraftingOriginal = H_farmersCrafting;

                                        // 根据农庄索引里的庄头人物编号去读取和修改相应数据
                                        currentZhuangTouId = ""; // 重置庄头人物编号
                                        currentZhuangTouData = null; // 重置庄头数据引用
                                        H_zhuangTouAge = "0"; // 重置庄头年龄
                                        H_zhuangTouAgeOriginal = "0";
                                        H_zhuangTouManagement = "0"; // 重置庄头管理
                                        H_zhuangTouManagementOriginal = "0";
                                        H_zhuangTouLoyalty = "0"; // 重置庄头忠诚
                                        H_zhuangTouLoyaltyOriginal = "0";
                                        H_zhuangTouCharacter = "0"; // 重置庄头品性
                                        H_zhuangTouCharacterOriginal = "0";
                                        
                                        if (farmData.Count > 14 && !string.IsNullOrEmpty(farmData[14]))
                                        {
                                            currentZhuangTouId = farmData[14];
                                            LoadZhuangTouData(currentZhuangTouId, areaIndex, i);
                                        }

                                        currentDataIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("加载{0}数据时出错: {1}", memberName, ex.Message));
            }
        }
        
        // 加载庄头数据
        private void LoadZhuangTouData(string zhuangTouId, int areaIndex, int farmIndex)
        {
            try
            {
                // 首先重置所有庄头相关数据，确保没有残留
                currentZhuangTouData = null;
                H_zhuangTouAge = "0";
                H_zhuangTouAgeOriginal = "0";
                H_zhuangTouManagement = "0";
                H_zhuangTouManagementOriginal = "0";
                H_zhuangTouLoyalty = "0";
                H_zhuangTouLoyaltyOriginal = "0";
                H_zhuangTouCharacter = "0";
                H_zhuangTouCharacterOriginal = "0";
                
                if (Mainload.ZhuangTou_now == null || Mainload.ZhuangTou_now.Count <= areaIndex)
                {
                    return;
                }
                
                List<List<List<string>>> areaZhuangTouData = Mainload.ZhuangTou_now[areaIndex];
                if (areaZhuangTouData == null || areaZhuangTouData.Count <= farmIndex)
                {
                    return;
                }
                
                // 增加额外检查，确保areaZhuangTouData[farmIndex]不为空且有至少一个元素
                if (areaZhuangTouData[farmIndex] == null || areaZhuangTouData[farmIndex].Count <= 0)
                {
                    return;
                }
                
                List<string> zhuangTouData = areaZhuangTouData[farmIndex][0];
                if (zhuangTouData != null && zhuangTouData.Count >= 6)
                {
                    // 庄头年龄，索引3
                    H_zhuangTouAge = zhuangTouData[3];
                    H_zhuangTouAgeOriginal = H_zhuangTouAge;
                    
                    // 庄头管理，索引4
                    H_zhuangTouManagement = zhuangTouData[4];
                    H_zhuangTouManagementOriginal = H_zhuangTouManagement;
                    
                    // 庄头忠诚，索引5
                    H_zhuangTouLoyalty = zhuangTouData[5];
                    H_zhuangTouLoyaltyOriginal = H_zhuangTouLoyalty;
                    
                    // 庄头品性，从人物信息中提取，索引2
                    if (zhuangTouData.Count > 2 && !string.IsNullOrEmpty(zhuangTouData[2]))
                    {
                        string[] characterInfo = zhuangTouData[2].Split('|');
                        if (characterInfo.Length > 3)
                        {
                            H_zhuangTouCharacter = characterInfo[3];
                        }
                    }
                    H_zhuangTouCharacterOriginal = H_zhuangTouCharacter;
                    
                    // 保存庄头数据引用
                    currentZhuangTouData = zhuangTouData;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("加载庄头数据时出错: {0}", ex.Message));
            }
        }
        
        private void ApplyChanges()
        {
            try
            {
                if (string.IsNullOrEmpty(selectedMemberName) || currentDataIndex < 0)
                    return;

                // 根据当前模式执行对应的修改方法
                switch (currentMode)
                {
                    case 0: // 族人模式
                        ApplyClanMemberChanges();
                        break;
                    case 1: // 门客模式
                        ApplyMenKeModeChanges();
                        break;
                    case 2: // 皇室模式
                        ApplyRoyaltyMemberChanges();
                        break;
                    case 3: // 世家模式
                        ApplyNobleMemberChanges();
                        break;
                    case 4: // 农庄修改模式
                        ApplyFarmChanges();
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用{0}属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 世家模式属性修改主方法
        private void ApplyNobleMemberChanges()
        {
            // 根据子模式执行对应的修改
            switch (currentNobleSubMode)
            {
                case 1: // 世家主脉模式
                    if (Mainload.Member_other != null && currentFamilyIndex >= 0 && currentFamilyIndex < Mainload.Member_other.Count && 
                        currentDataIndex >= 0 && currentDataIndex < Mainload.Member_other[currentFamilyIndex].Count)
                    {
                        // 检查世家主脉模式下的天赋和技能有效性
                        bool isF_TalentEmpty = string.IsNullOrEmpty(F_talent) || F_talent == "0";
                        bool hasF_TalentPoints = !string.IsNullOrEmpty(F_talentPoint) && F_talentPoint != "0";
                         
                        bool isF_SkillEmpty = string.IsNullOrEmpty(F_skill) || F_skill == "0";
                        bool hasF_SkillPoints = !string.IsNullOrEmpty(F_skillPoint) && F_skillPoint != "0";
                         
                        // 有效性检查
                        if (isF_TalentEmpty && hasF_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                         
                        if (isF_SkillEmpty && hasF_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                         
                        // 应用世家主脉模式修改
                        ApplyNobleFamilyChanges();
                    }
                    break;
                case 2: // 世家妻妾模式
                    if (Mainload.Member_Other_qu != null && currentFamilyIndex >= 0 && currentFamilyIndex < Mainload.Member_Other_qu.Count && 
                        currentDataIndex >= 0 && currentDataIndex < Mainload.Member_Other_qu[currentFamilyIndex].Count)
                    {
                        // 检查世家妻妾模式下的天赋和技能有效性
                        bool isG_TalentEmpty = string.IsNullOrEmpty(G_talent) || G_talent == "0";
                        bool hasG_TalentPoints = !string.IsNullOrEmpty(G_talentPoint) && G_talentPoint != "0";
                         
                        bool isG_SkillEmpty = string.IsNullOrEmpty(G_skill) || G_skill == "0";
                        bool hasG_SkillPoints = !string.IsNullOrEmpty(G_skillPoint) && G_skillPoint != "0";
                         
                        // 有效性检查
                        if (isG_TalentEmpty && hasG_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                         
                        if (isG_SkillEmpty && hasG_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                         
                        // 应用世家妻妾模式修改
                        ApplyNobleSpouseChanges();
                    }
                    break;
            }
        }

        // 族人模式属性修改主方法
        private void ApplyClanMemberChanges()
        {
            // 根据子模式执行对应的修改
            switch (currentMemberSubMode)
            {
                case 1: // 族人家族模式
                    if (Mainload.Member_now != null && currentDataIndex < Mainload.Member_now.Count)
                    {
                        // 检查族人家族模式下的天赋和技能有效性
                        bool isA_TalentEmpty = string.IsNullOrEmpty(A_talent) || A_talent == "0";
                        bool hasA_TalentPoints = !string.IsNullOrEmpty(A_talentPoint) && A_talentPoint != "0";
                        
                        bool isA_SkillEmpty = string.IsNullOrEmpty(A_skill) || A_skill == "0";
                        bool hasA_SkillPoints = !string.IsNullOrEmpty(A_skillPoint) && A_skillPoint != "0";
                        
                        // 有效性检查
                        if (isA_TalentEmpty && hasA_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                        
                        if (isA_SkillEmpty && hasA_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                        
                        // 应用族人家族模式修改
                        ApplyClanFamilyChanges();
                    }
                    break;
                case 2: // 族人妻妾模式
                    if (Mainload.Member_qu != null && currentDataIndex < Mainload.Member_qu.Count)
                    {
                        // 检查族人妻妾模式下的天赋和技能有效性
                        bool isB_TalentEmpty = string.IsNullOrEmpty(B_talent) || B_talent == "0";
                        bool hasB_TalentPoints = !string.IsNullOrEmpty(B_talentPoint) && B_talentPoint != "0";
                        
                        bool isB_SkillEmpty = string.IsNullOrEmpty(B_skill) || B_skill == "0";
                        bool hasB_SkillPoints = !string.IsNullOrEmpty(B_skillPoint) && B_skillPoint != "0";
                        
                        // 有效性检查
                        if (isB_TalentEmpty && hasB_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                        
                        if (isB_SkillEmpty && hasB_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                        
                        // 应用族人妻妾模式修改
                        ApplyClanSpouseChanges();
                    }
                    break;
            }
        }

        // 门客模式属性修改主方法
        private void ApplyMenKeModeChanges()
        {
            if (Mainload.MenKe_Now != null && !string.IsNullOrEmpty(selectedMemberName) && currentDataIndex < Mainload.MenKe_Now.Count)
            {
                // 检查门客模式下的天赋和技能有效性
                bool isC_TalentEmpty = string.IsNullOrEmpty(C_talent) || C_talent == "0";
                bool hasC_TalentPoints = !string.IsNullOrEmpty(C_talentPoint) && C_talentPoint != "0";
                
                bool isC_SkillEmpty = string.IsNullOrEmpty(C_skill) || C_skill == "0";
                bool hasC_SkillPoints = !string.IsNullOrEmpty(C_skillPoint) && C_skillPoint != "0";
                
                // 有效性检查
                if (isC_TalentEmpty && hasC_TalentPoints)
                {
                    ShowErrorMessage("天赋为空，天赋点不为零");
                    return;
                }
                
                if (isC_SkillEmpty && hasC_SkillPoints)
                {
                    ShowErrorMessage("技能为空，技能点不为零");
                    return;
                }
                
                // 应用门客模式修改
                ApplyMenKeChanges();
            }
        }

        // 皇室模式属性修改主方法
        private void ApplyRoyaltyMemberChanges()
        {
            // 根据子模式执行对应的修改
            switch (currentRoyaltySubMode)
            {
                case 1: // 皇室主脉模式
                    if (Mainload.Member_King != null && currentDataIndex < Mainload.Member_King.Count)
                    {
                        // 检查皇室主脉模式下的天赋和技能有效性
                        bool isD_TalentEmpty = string.IsNullOrEmpty(D_talent) || D_talent == "0";
                        bool hasD_TalentPoints = !string.IsNullOrEmpty(D_talentPoint) && D_talentPoint != "0";
                        
                        bool isD_SkillEmpty = string.IsNullOrEmpty(D_skill) || D_skill == "0";
                        bool hasD_SkillPoints = !string.IsNullOrEmpty(D_skillPoint) && D_skillPoint != "0";
                        
                        // 有效性检查
                        if (isD_TalentEmpty && hasD_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                        
                        if (isD_SkillEmpty && hasD_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                        
                        // 应用皇室主脉模式修改
                        ApplyRoyalFamilyChanges();
                    }
                    break;
                case 2: // 皇家妻妾模式
                    if (Mainload.Member_King_qu != null && currentDataIndex < Mainload.Member_King_qu.Count)
                    {
                        // 检查皇家妻妾模式下的天赋和技能有效性
                        bool isE_TalentEmpty = string.IsNullOrEmpty(E_talent) || E_talent == "0";
                        bool hasE_TalentPoints = !string.IsNullOrEmpty(E_talentPoint) && E_talentPoint != "0";
                        
                        bool isE_SkillEmpty = string.IsNullOrEmpty(E_skill) || E_skill == "0";
                        bool hasE_SkillPoints = !string.IsNullOrEmpty(E_skillPoint) && E_skillPoint != "0";
                        
                        // 有效性检查
                        if (isE_TalentEmpty && hasE_TalentPoints)
                        {
                            ShowErrorMessage("天赋为空，天赋点不为零");
                            return;
                        }
                        
                        if (isE_SkillEmpty && hasE_SkillPoints)
                        {
                            ShowErrorMessage("技能为空，技能点不为零");
                            return;
                        }
                        
                        // 应用皇家妻妾模式修改
                        ApplyRoyalSpouseChanges();
                    }
                    break;
            }
        }

        // 应用世家主脉属性修改
        private void ApplyNobleFamilyChanges()
        {
            try
            {
                // 检查Mainload.Member_other集合是否有效
                if (Mainload.Member_other != null && currentFamilyIndex >= 0 && currentFamilyIndex < Mainload.Member_other.Count && 
                    currentDataIndex >= 0 && currentDataIndex < Mainload.Member_other[currentFamilyIndex].Count)
                {
                    // 获取当前世家主脉成员数据的副本
                    string[] memberData = Mainload.Member_other[currentFamilyIndex][currentDataIndex].ToArray();
                    string[] memberInfo = memberData[2].Split('|');

                    // 基本属性修改
                    UpdateNobleFamilyBasicAttributes(memberData);

                    // 扩展属性修改
                    UpdateNobleFamilyExtendedAttributes(memberData);

                    // 嵌套属性修改
                    memberInfo = UpdateNobleFamilyNestedAttributes(memberInfo);

                    // 更新嵌套数据
                    memberData[2] = string.Join("|", memberInfo);

                    // 创建新的List<string>对象来保存修改后的数据
                    List<string> updatedData = new List<string>(memberData);
                    Mainload.Member_other[currentFamilyIndex][currentDataIndex] = updatedData;

                    // 修改成功，添加提示信息
                    if (Mainload.Tip_Show != null)
                    {
                        Mainload.Tip_Show.Add(new List<string>
                        {
                            "1",
                            string.Format("【{0}】属性修改成功", selectedMemberName)
                        });
                    }

                    Logger.LogInfo(string.Format("成功应用世家主脉{0}的属性修改", selectedMemberName));
                }
                else
                {
                    Logger.LogError("Mainload.Member_other集合无效或索引超出范围");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用世家主脉{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新世家主脉基本属性
        private void UpdateNobleFamilyBasicAttributes(string[] memberData)
        {
            if (memberData.Length > 3) memberData[3] = F_age;          // 年龄
            if (memberData.Length > 4) memberData[4] = F_intelligence; // 文才
            if (memberData.Length > 5) memberData[5] = F_weapon;       // 武才
            if (memberData.Length > 6) memberData[6] = F_business;     // 商才
            if (memberData.Length > 7) memberData[7] = F_art;          // 艺才
            if (memberData.Length > 8) memberData[8] = F_mood;         // 心情
            if (memberData.Length > 17) memberData[17] = F_reputation; // 声誉
            if (memberData.Length > 19) memberData[19] = F_charm;      // 魅力
            if (memberData.Length > 20) memberData[20] = F_health;     // 健康
            if (memberData.Length > 22) memberData[22] = F_strategy;   // 计谋
        }

        // 更新世家主脉扩展属性
        private void UpdateNobleFamilyExtendedAttributes(string[] memberData)
        {
            if (memberData.Length > 25) memberData[25] = F_skillPoint; // 技能点
        }

        // 更新世家主脉嵌套属性
        private string[] UpdateNobleFamilyNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 10; // 至少需要索引9
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            if (updatedMemberInfo.Length > 2) updatedMemberInfo[2] = F_talent ?? "0";           // 天赋
            if (updatedMemberInfo.Length > 3) updatedMemberInfo[3] = F_talentPoint ?? "0";      // 天赋点
            if (updatedMemberInfo.Length > 6) updatedMemberInfo[6] = F_skill ?? "0";            // 技能
            if (updatedMemberInfo.Length > 7) updatedMemberInfo[7] = F_luck ?? "0";             // 幸运
            if (updatedMemberInfo.Length > 8) updatedMemberInfo[8] = F_character ?? "0";        // 品性
            if (updatedMemberInfo.Length > 9) updatedMemberInfo[1] = F_preference ?? "0";       // 喜好
            
            return updatedMemberInfo;
        }

        // 应用世家妻妾属性修改
        private void ApplyNobleSpouseChanges()
        {
            try
            {
                // 检查Mainload.Member_Other_qu集合是否有效
                if (Mainload.Member_Other_qu != null && currentFamilyIndex >= 0 && currentFamilyIndex < Mainload.Member_Other_qu.Count && 
                    currentDataIndex >= 0 && currentDataIndex < Mainload.Member_Other_qu[currentFamilyIndex].Count)
                {
                    // 获取当前世家妻妾成员数据的副本
                    string[] memberData = Mainload.Member_Other_qu[currentFamilyIndex][currentDataIndex].ToArray();
                    string[] memberInfo = memberData[2].Split('|');

                    // 基本属性修改
                    UpdateNobleSpouseBasicAttributes(memberData);

                    // 扩展属性修改
                    UpdateNobleSpouseExtendedAttributes(memberData);

                    // 嵌套属性修改
                    memberInfo = UpdateNobleSpouseNestedAttributes(memberInfo);

                    // 更新嵌套数据
                    memberData[2] = string.Join("|", memberInfo);

                    // 创建新的List<string>对象来保存修改后的数据
                    List<string> updatedData = new List<string>(memberData);
                    Mainload.Member_Other_qu[currentFamilyIndex][currentDataIndex] = updatedData;

                    // 修改成功，添加提示信息
                    if (Mainload.Tip_Show != null)
                    {
                        Mainload.Tip_Show.Add(new List<string>
                        {
                            "1",
                            string.Format("【{0}】属性修改成功", selectedMemberName)
                        });
                    }

                    Logger.LogInfo(string.Format("成功应用世家妻妾{0}的属性修改", selectedMemberName));
                }
                else
                {
                    Logger.LogError("Mainload.Member_Other_qu集合无效或索引超出范围");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用世家妻妾{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新世家妻妾基本属性
        private void UpdateNobleSpouseBasicAttributes(string[] memberData)
        {
            if (memberData.Length > 3) memberData[3] = G_age;          // 年龄
            if (memberData.Length > 4) memberData[4] = G_intelligence; // 文才
            if (memberData.Length > 5) memberData[5] = G_weapon;       // 武才
            if (memberData.Length > 6) memberData[6] = G_business;     // 商才
            if (memberData.Length > 7) memberData[7] = G_art;          // 艺才
            if (memberData.Length > 8) memberData[8] = G_mood;         // 心情
            if (memberData.Length > 9) memberData[10] = G_reputation;  // 声誉
            if (memberData.Length > 12) memberData[12] = G_reputation; // 声誉
            if (memberData.Length > 15) memberData[15] = G_charm;      // 魅力
            if (memberData.Length > 16) memberData[16] = G_health;     // 健康
            if (memberData.Length > 17) memberData[17] = G_strategy;   // 计谋
        }

        // 更新世家妻妾扩展属性
        private void UpdateNobleSpouseExtendedAttributes(string[] memberData)
        {
            if (memberData.Length > 19) memberData[19] = G_skillPoint; // 技能点
        }

        // 更新世家妻妾嵌套属性
        private string[] UpdateNobleSpouseNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 10; // 至少需要索引9
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            if (updatedMemberInfo.Length > 2) updatedMemberInfo[2] = G_talent ?? "0";           // 天赋
            if (updatedMemberInfo.Length > 3) updatedMemberInfo[3] = G_talentPoint ?? "0";      // 天赋点
            if (updatedMemberInfo.Length > 6) updatedMemberInfo[6] = G_skill ?? "0";            // 技能
            if (updatedMemberInfo.Length > 7) updatedMemberInfo[7] = G_luck ?? "0";             // 幸运 
            if (updatedMemberInfo.Length > 9) updatedMemberInfo[9] = G_preference ?? "0";       // 喜好
            
            return updatedMemberInfo;
        }

        // 应用农庄修改
        private void ApplyFarmChanges()
        {
            try
            {
                if (currentFarmData == null || string.IsNullOrEmpty(selectedMemberName) || currentDataIndex < 0)
                {
                    return;
                }
                
                // 检查农户数量是否超过可居住上限
                int plantingCount = 0;
                int breedingCount = 0;
                int craftingCount = 0;
                int maxResidents = 0;
                
                // 尝试解析农户数量和可居住上限
                int.TryParse(H_farmersPlanting, out plantingCount);
                int.TryParse(H_farmersBreeding, out breedingCount);
                int.TryParse(H_farmersCrafting, out craftingCount);
                int.TryParse(H_maxResidents, out maxResidents);
                
                // 计算总农户数量
                int totalFarmers = plantingCount + breedingCount + craftingCount;
                
                // 如果总农户数量超过可居住上限，阻止修改并显示提示
                if (totalFarmers > maxResidents)
                {
                    ShowErrorMessage("可容纳农户数量不足，请升级或修建更多农房");
                    return;
                }
                
                int areaIndex = currentFarmAreaIndex;
                
                // 更新农庄数据
                if (Mainload.NongZ_now != null && Mainload.NongZ_now.Count > areaIndex)
                {
                    List<List<string>> areaFarmData = Mainload.NongZ_now[areaIndex];
                    if (areaFarmData != null && areaFarmData.Count > currentFarmIndex)
                    {
                        var farmData = areaFarmData[currentFarmIndex];
                        if (farmData != null)
                        {
                            // 更新面积，索引5
                            if (farmData.Count > 5)
                            {
                                farmData[5] = H_area;
                            }
                            
                            // 更新农户数量（种植|养殖|手工），索引24
                            if (farmData.Count > 24)
                            {
                                farmData[24] = $"{H_farmersPlanting}|{H_farmersBreeding}|{H_farmersCrafting}";
                            }
                        }
                    }
                }
                
                // 更新庄头数据 - 只有当存在庄头时才进行修改
                if (currentZhuangTouData != null && !string.IsNullOrEmpty(currentZhuangTouId))
                {
                    // 更新年龄，索引3
                    if (currentZhuangTouData.Count > 3)
                    {
                        currentZhuangTouData[3] = H_zhuangTouAge;
                    }
                    
                    // 更新管理，索引4
                    if (currentZhuangTouData.Count > 4)
                    {
                        currentZhuangTouData[4] = H_zhuangTouManagement;
                    }
                    
                    // 更新忠诚，索引5
                    if (currentZhuangTouData.Count > 5)
                    {
                        currentZhuangTouData[5] = H_zhuangTouLoyalty;
                    }
                    
                    // 更新品性，索引2中的第4个元素
                    if (currentZhuangTouData.Count > 2 && !string.IsNullOrEmpty(currentZhuangTouData[2]))
                    {
                        string[] characterInfo = currentZhuangTouData[2].Split('|');
                        if (characterInfo.Length > 3)
                        {
                            characterInfo[3] = H_zhuangTouCharacter;
                            currentZhuangTouData[2] = string.Join("|", characterInfo);
                        }
                    }
                }
                
                // 刷新数据
                FilterMembers();
                
                // 显示成功消息
                ShowSuccessMessage(string.Format("成功修改农庄 {0} 的属性", selectedMemberName));
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用农庄修改时出错: {0}", ex.Message));
                ShowErrorMessage("应用修改时出错，请查看日志");
            }
        }
        
        // 显示错误信息的辅助方法
        private void ShowErrorMessage(string errorReason)
        {
            Logger.LogWarning(string.Format("{0}的{1}，已阻止修改", selectedMemberName, errorReason));
            
            // 添加提示信息
            if (Mainload.Tip_Show != null)
            {
                Mainload.Tip_Show.Add(new List<string>
                {
                    "1",
                    string.Format("【{0}】的{1}，修改失败", selectedMemberName, errorReason)
                });
            }
        }

        // 应用族人家族成员属性修改
        private void ApplyClanFamilyChanges()
        {
            // 使用族人家族数据索引变量
            Member_nowData = Mainload.Member_now[currentDataIndex];
            string[] memberInfo = Member_nowData[4].Split('|');

            try
            {
                // 基本属性修改
                UpdateClanFamilyBasicAttributes(Member_nowData);

                // 扩展属性修改
                UpdateClanFamilyExtendedAttributes(Member_nowData);

                // 嵌套属性修改
                memberInfo = UpdateClanFamilyNestedAttributes(memberInfo);

                // 更新嵌套数据
                Member_nowData[4] = string.Join("|", memberInfo);

                // 处理爵位和封地
                HandleClanFamilyTitleAndLand(Member_nowData);

                // 检查并解锁封地
                bool fengDiUnlockedSuccessfully = UnlockFengDiIfNeeded();

                // 如果封地解锁失败，阻止修改并显示提示
                if (!fengDiUnlockedSuccessfully)
                {
                    return;
                }

                // 修改成功，添加提示信息
                ShowSuccessMessage("族人家族");
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用族人家族{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新族人家族基本属性
        private void UpdateClanFamilyBasicAttributes(List<string> memberData)
        {
            memberData[5] = A_character;    // 品性
            memberData[6] = A_age;          // 年龄
            memberData[7] = A_intelligence; // 文才
            memberData[8] = A_weapon;       // 武才
            memberData[9] = A_business;     // 商才
            memberData[10] = A_art;         // 艺才
            memberData[11] = A_mood;        // 心情
            // 更新官职数据，索引12，只修改官职部分，保留政绩部分不变
            if (memberData.Count > 12)
            {
                // 解析原数据，保留|后面的政绩部分
                string originalData = memberData[12];
                string zhengJi = "";
                
                if (originalData.Contains("|"))
                {
                    string[] parts = originalData.Split('|');
                    if (parts.Length > 1)
                    {
                        zhengJi = "|" + parts[1];
                    }
                }
                
                // 构建新的官职数据格式：官职数据|政绩
                memberData[12] = A_officialRank + zhengJi;
            }
            memberData[16] = A_reputation;  // 声誉
            memberData[20] = A_charm;       // 魅力
            memberData[21] = A_health;      // 健康
            memberData[27] = A_strategy;    // 计谋
            memberData[30] = A_power;       // 体力
        }

        // 更新族人家族扩展属性
        private void UpdateClanFamilyExtendedAttributes(List<string> memberData)
        {
            memberData[33] = A_skillPoint;// 技能点
        }

        // 更新族人家族嵌套属性
        private string[] UpdateClanFamilyNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 10; // 至少需要索引9
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            updatedMemberInfo[2] = A_talent ?? "0";           // 天赋
            updatedMemberInfo[3] = A_talentPoint ?? "0";      // 天赋点
            updatedMemberInfo[6] = A_skill ?? "0";            // 技能
            updatedMemberInfo[7] = A_luck ?? "0";             // 幸运 
            updatedMemberInfo[9] = A_preference ?? "0";       // 喜好
            
            return updatedMemberInfo;
        }

        // 处理族人家族爵位和封地
        private void HandleClanFamilyTitleAndLand(List<string> memberData)
        {
            memberData[14] = string.IsNullOrEmpty(A_FengDi) ? A_JueWei : A_JueWei + "|" + A_FengDi;
        }

        // 显示修改成功消息
        private void ShowSuccessMessage(string memberType)
        {
            if (Mainload.Tip_Show != null)
            {
                Mainload.Tip_Show.Add(new List<string>
                {
                    "1",
                    string.Format("【{0}】属性修改成功", selectedMemberName)
                });

                // 如果是族人家族模式且有封地解锁，添加封地解锁成功提示
                if (memberType == "族人家族" && !string.IsNullOrEmpty(A_FengDi) && A_FengDi != "0")
                {
                    string fengDiName = FengDiCodeToChinese(A_FengDi);
                    Mainload.Tip_Show.Add(new List<string>
                    {
                        "1",
                        string.Format("封地【{0}】解锁成功", fengDiName)
                    });
                }
            }

            Logger.LogInfo(string.Format("成功应用{0}{1}的属性修改", memberType, selectedMemberName));
        }

        // 应用族人妻妾属性修改
        private void ApplyClanSpouseChanges()
        {
            try
            {
                // 检查Mainload.Member_qu集合是否有效
                if (Mainload.Member_qu != null && currentDataIndex >= 0 && currentDataIndex < Mainload.Member_qu.Count)
                {
                    // 获取当前妻妾数据的副本
                    Member_quData = new List<string>(Mainload.Member_qu[currentDataIndex]);
                    string[] memberInfo = Member_quData[2].Split('|');

                    // 基本属性修改
                    UpdateClanSpouseBasicAttributes(Member_quData);

                    // 扩展属性修改
                    UpdateClanSpouseExtendedAttributes(Member_quData);

                    // 嵌套属性修改
                    memberInfo = UpdateClanSpouseNestedAttributes(memberInfo);

                    // 更新嵌套数据
                    Member_quData[2] = string.Join("|", memberInfo);

                    // 创建新的List<string>对象来保存修改后的数据
                    List<string> updatedData = new List<string>(Member_quData);
                    Mainload.Member_qu[currentDataIndex] = updatedData;

                    // 修改成功，添加提示信息
                    if (Mainload.Tip_Show != null)
                    {
                        Mainload.Tip_Show.Add(new List<string>
                        {
                            "1",
                            string.Format("【{0}】属性修改成功", selectedMemberName)
                        });
                    }

                    Logger.LogInfo(string.Format("成功应用族人妻妾{0}的属性修改", selectedMemberName));
                }
                else
                {
                    Logger.LogError("Mainload.Member_qu集合无效或索引超出范围");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用族人妻妾{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新族人妻妾基本属性
        private void UpdateClanSpouseBasicAttributes(List<string> memberData)
        {
            memberData[5] = B_age;          // 年龄
            memberData[6] = B_intelligence; // 文才
            memberData[7] = B_weapon;       // 武才
            memberData[8] = B_business;     // 商才
            memberData[9] = B_art;          // 艺才
            memberData[10] = B_mood;        // 心情
            memberData[12] = B_reputation;  // 声誉
            memberData[15] = B_charm;       // 魅力
            memberData[16] = B_health;      // 健康
            memberData[19] = B_strategy;    // 计谋
            memberData[20] = B_power;       // 体力
        }

        // 更新族人妻妾扩展属性
        private void UpdateClanSpouseExtendedAttributes(List<string> memberData)
        {
            memberData[23] = B_skillPoint;  // 技能点
        }

        // 更新族人妻妾嵌套属性
        private string[] UpdateClanSpouseNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 11; 
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            updatedMemberInfo[2] = B_talent ?? "0";     // 天赋
            updatedMemberInfo[3] = B_talentPoint ?? "0";// 天赋点
            updatedMemberInfo[6] = B_skill ?? "0";      // 技能
            updatedMemberInfo[7] = B_luck ?? "0";       // 幸运
            updatedMemberInfo[8] = B_character ?? "0";  // 品性
            updatedMemberInfo[10] = B_preference ?? "0"; // 喜好
            
            return updatedMemberInfo;
        }

        // 应用门客属性修改
        private void ApplyMenKeChanges()
        {
            // 使用门客数据索引变量
            Member_MenKeData = Mainload.MenKe_Now[currentDataIndex];
            string[] menkeDataArray = Member_MenKeData[2].Split('|');

            try
            {
                // 基本属性修改
                UpdateMenKeBasicAttributes(Member_MenKeData);

                // 扩展属性修改
                UpdateMenKeExtendedAttributes(Member_MenKeData);

                // 嵌套属性修改
                menkeDataArray = UpdateMenKeNestedAttributes(menkeDataArray);

                // 更新嵌套数据
                Member_MenKeData[2] = string.Join("|", menkeDataArray);

                // 修改成功，添加提示信息
                if (Mainload.Tip_Show != null)
                {
                    Mainload.Tip_Show.Add(new List<string>
                    {
                        "1",
                        string.Format("【{0}】属性修改成功", selectedMemberName)
                    });
                }

                Logger.LogInfo(string.Format("成功应用门客{0}的属性修改", selectedMemberName));
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用门客{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新门客基本属性
        private void UpdateMenKeBasicAttributes(List<string> memberData)
        {
            memberData[11] = C_reputation;  // 声誉
            memberData[19] = C_power;       // 体力
            memberData[3] = C_age;          // 年龄
            memberData[14] = C_health;      // 健康
            memberData[8] = C_mood;         // 心情
            memberData[13] = C_charm;       // 魅力
            memberData[16] = C_skillPoint;  // 技能点
            memberData[18] = C_MenKe_Reward;// 门客工资
        }

        // 更新门客扩展属性
        private void UpdateMenKeExtendedAttributes(List<string> memberData)
        {
            memberData[4] = C_intelligence; // 文才
            memberData[5] = C_weapon;       // 武才
            memberData[6] = C_business;     // 商才
            memberData[7] = C_art;          // 艺才
            memberData[15] = C_strategy;    // 计谋
        }

        // 更新门客嵌套属性
        private string[] UpdateMenKeNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 9; // 至少需要索引8
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            updatedMemberInfo[2] = C_talent ?? "0";     // 天赋
            updatedMemberInfo[3] = C_talentPoint ?? "0";// 天赋点
            updatedMemberInfo[6] = C_skill ?? "0";      // 技能
            updatedMemberInfo[7] = C_luck ?? "0";       // 幸运
            updatedMemberInfo[8] = C_character ?? "0";  // 品性
            
            return updatedMemberInfo;
        }

        // 检查并解锁封地
        private bool UnlockFengDiIfNeeded()
        {
            bool fengDiUnlockedSuccessfully = true;
            
            if (!string.IsNullOrEmpty(A_FengDi) && A_FengDi != "0")
            {
                try
                {
                    int fengDiIndex = int.Parse(A_FengDi);
                    string fengDiName = FengDiCodeToChinese(A_FengDi);
                    // 确保索引在有效范围内 (1-12)
                    if (fengDiIndex >= 1 && fengDiIndex <= 12 && Mainload.Fengdi_now != null && Mainload.Fengdi_now.Count > fengDiIndex)
                    {
                        List<string> fengDiData = Mainload.Fengdi_now[fengDiIndex];
                        if (fengDiData != null && fengDiData.Count > 0)
                        {
                            // 检查封地是否已解锁 (第一个元素为0表示未解锁)
                            if (fengDiData[0] == "0")
                            {
                                // 新增判断逻辑：检查郡城势力是否为-1
                                // 获取封地所在郡的索引（根据用户提供的信息，Member_nowData[14]中包含郡编号）
                                bool canUnlockFengDi = true;
                                try
                                {
                                    if (Member_nowData != null && Member_nowData.Count > 14)
                                    {
                                        string[] jueWeiFengDi = Member_nowData[14].Split('|');
                                        if (jueWeiFengDi.Length > 1)
                                        {
                                            int junIndex = int.Parse(jueWeiFengDi[1]);
                                            // 检查Mainload.CityData_now数组中对应郡城的势力名称
                                            // 由于无法直接访问CityData_now，使用反射来尝试获取
                                            object cityDataNow = typeof(Mainload).GetField("CityData_now", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(null);
                                            if (cityDataNow != null && junIndex >= 0)
                                            {
                                                // 假设CityData_now是一个二维数组，尝试获取对应郡的数据
                                                System.Collections.IList junArray = cityDataNow as System.Collections.IList;
                                                if (junArray != null && junIndex < junArray.Count)
                                                {
                                                    object junData = junArray[junIndex];
                                                    System.Collections.IList junDetails = junData as System.Collections.IList;
                                                    if (junDetails != null && junDetails.Count > 0)
                                                    {
                                                        // 第一个元素是郡城信息
                                                        string junChengInfo = junDetails[0] as string;
                                                        if (!string.IsNullOrEmpty(junChengInfo))
                                                        {
                                                            string[] parts = junChengInfo.Split('|');
                                                            if (parts.Length > 0)
                                                            {
                                                                // 第一个数是势力名称
                                                                string forceName = parts[0];
                                                                // 如果势力名称不为-1，则不解锁封地
                                                                if (forceName != "-1")
                                                                {
                                                                    canUnlockFengDi = false;
                                                                    Logger.LogInfo(string.Format("封地{0}所在郡城已有势力控制，无法解锁", fengDiName));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // 如果检查过程中出错，默认允许解锁
                                    Logger.LogWarning(string.Format("检查郡城势力时出错: {0}，将继续解锁封地", ex.Message));
                                }
                                
                                // 只有当郡城势力为-1时才解锁封地
                                if (canUnlockFengDi)
                                {
                                    // 尝试解锁封地
                                    fengDiData[0] = "1";
                                    // 验证解锁是否成功
                                    if (fengDiData[0] != "1")
                                    {
                                        fengDiUnlockedSuccessfully = false;
                                        Logger.LogError(string.Format("解锁封地失败: {0}", fengDiName));
                                    }
                                    else
                                    {
                                        Logger.LogInfo(string.Format("已解锁封地: {0}", fengDiName));
                                    }
                                }
                                else
                                {
                                    fengDiUnlockedSuccessfully = false;
                                    Logger.LogError(string.Format("郡城已有势力控制，无法解锁封地: {0}", fengDiName));
                                }
                            }
                            else
                            {
                                // 封地已解锁，直接跳出解锁逻辑
                                Logger.LogInfo(string.Format("封地{0}已解锁，无需再次解锁", fengDiName));
                            }
                        }
                        else
                        {
                            fengDiUnlockedSuccessfully = false;
                            Logger.LogError(string.Format("封地数据无效: {0}", fengDiName));
                        }
                    }
                    else
                    {
                        fengDiUnlockedSuccessfully = false;
                        Logger.LogError(string.Format("无效的封地索引: {0}", fengDiIndex));
                    }
                }
                catch (Exception ex)
                {
                    fengDiUnlockedSuccessfully = false;
                    Logger.LogError(string.Format("解锁封地时出错: {0}", ex.Message));
                }
            }

            // 如果封地解锁失败，阻止修改并显示提示
            if (!fengDiUnlockedSuccessfully)
            {
                // 添加提示信息
                string fengDiName = FengDiCodeToChinese(A_FengDi);
                if (Mainload.Tip_Show != null)
                {
                    Mainload.Tip_Show.Add(new List<string>
                    {
                        "1",
                        string.Format("{0}封地解锁失败，修改失败", fengDiName)
                    });
                }
                Logger.LogError(string.Format("{0}封地解锁失败，已阻止属性修改", fengDiName));
            }

            return fengDiUnlockedSuccessfully;
        }

        // 辅助方法：应用族员属性修改（此方法现在已被整合到ApplyChanges中）
        private bool ApplyClanMemberChanges(List<List<string>> memberDataList)
        {
            // 此方法已被整合到ApplyChanges中，保留仅用于兼容性
            return false;
        }

        // 应用皇室主脉属性修改
        private void ApplyRoyalFamilyChanges()
        {
            try
            {
                // 检查Mainload.Member_King集合是否有效
                if (Mainload.Member_King != null && currentDataIndex >= 0 && currentDataIndex < Mainload.Member_King.Count)
                {
                    // 获取当前皇室主脉数据
                    List<string> royalData = Mainload.Member_King[currentDataIndex];
                    string[] royalInfo = royalData[2].Split('|');

                    // 基本属性修改
                    UpdateRoyalFamilyBasicAttributes(royalData);

                    // 扩展属性修改
                    UpdateRoyalFamilyExtendedAttributes(royalData);

                    // 嵌套属性修改
                    royalInfo = UpdateRoyalFamilyNestedAttributes(royalInfo);

                    // 更新嵌套数据
                    royalData[2] = string.Join("|", royalInfo);

                    // 创建新的List<string>对象来保存修改后的数据
                    List<string> updatedData = new List<string>(royalData);
                    Mainload.Member_King[currentDataIndex] = updatedData;

                    // 修改成功，添加提示信息
                    if (Mainload.Tip_Show != null)
                    {
                        Mainload.Tip_Show.Add(new List<string>
                        {
                            "1",
                            string.Format("【{0}】属性修改成功", selectedMemberName)
                        });
                    }

                    Logger.LogInfo(string.Format("成功应用皇室主脉{0}的属性修改", selectedMemberName));
                }
                else
                {
                    Logger.LogError("Mainload.Member_King集合无效或索引超出范围");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用皇室主脉{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新皇室主脉基本属性
        private void UpdateRoyalFamilyBasicAttributes(List<string> memberData)
        {
            
            memberData[3] = D_age;          // 年龄
            memberData[4] = D_intelligence; // 文才
            memberData[5] = D_weapon;       // 武才
            memberData[6] = D_business;     // 商才
            memberData[7] = D_art;          // 艺才
            memberData[8] = D_mood;         // 心情
            memberData[16] = D_reputation;  // 声誉
            memberData[18] = D_charm;       // 魅力
            memberData[19] = D_health;      // 健康
            memberData[21] = D_strategy;    // 计谋
        }

        // 更新皇室主脉扩展属性
        private void UpdateRoyalFamilyExtendedAttributes(List<string> memberData)
        {
            memberData[23] = D_skillPoint;  // 技能点
        }

        // 更新皇室主脉嵌套属性
        private string[] UpdateRoyalFamilyNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 10; // 至少需要索引9
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            updatedMemberInfo[2] = D_talent ?? "0";           // 天赋
            updatedMemberInfo[3] = D_talentPoint ?? "0";      // 天赋点
            updatedMemberInfo[6] = D_skill ?? "0";            // 技能
            updatedMemberInfo[7] = D_luck ?? "0";             // 幸运 
            updatedMemberInfo[8] = D_character ?? "0";        // 品性
            updatedMemberInfo[1] = D_preference ?? "0";       // 喜好
            
            return updatedMemberInfo;
        }

        // 应用皇家妻妾属性修改
        private void ApplyRoyalSpouseChanges()
        {
            try
            {
                // 检查Mainload.Member_King_qu集合是否有效
                if (Mainload.Member_King_qu != null && currentDataIndex >= 0 && currentDataIndex < Mainload.Member_King_qu.Count)
                {
                    // 获取当前皇家妻妾数据
                    List<string> royalSpouseData = Mainload.Member_King_qu[currentDataIndex];
                    string[] royalSpouseInfo = royalSpouseData[2].Split('|');

                    // 基本属性修改
                    UpdateRoyalSpouseBasicAttributes(royalSpouseData);

                    // 扩展属性修改
                    UpdateRoyalSpouseExtendedAttributes(royalSpouseData);

                    // 嵌套属性修改
                    royalSpouseInfo = UpdateRoyalSpouseNestedAttributes(royalSpouseInfo);

                    // 更新嵌套数据
                    royalSpouseData[2] = string.Join("|", royalSpouseInfo);

                    // 创建新的List<string>对象来保存修改后的数据
                    List<string> updatedData = new List<string>(royalSpouseData);
                    Mainload.Member_King_qu[currentDataIndex] = updatedData;

                    // 修改成功，添加提示信息
                    if (Mainload.Tip_Show != null)
                    {
                        Mainload.Tip_Show.Add(new List<string>
                        {
                            "1",
                            string.Format("【{0}】属性修改成功", selectedMemberName)
                        });
                    }

                    Logger.LogInfo(string.Format("成功应用皇家妻妾{0}的属性修改", selectedMemberName));
                }
                else
                {
                    Logger.LogError("Mainload.Member_King_qu集合无效或索引超出范围");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("应用皇家妻妾{0}的属性修改时出错: {1}", selectedMemberName, ex.Message));
            }
        }

        // 更新皇家妻妾基本属性
        private void UpdateRoyalSpouseBasicAttributes(List<string> memberData)
        {
            memberData[3] = E_age;          // 年龄
            memberData[4] = E_intelligence; // 文才
            memberData[5] = E_weapon;       // 武才
            memberData[6] = E_business;     // 商才
            memberData[7] = E_art;          // 艺才
            memberData[8] = E_mood;         // 心情
            memberData[10] = E_reputation;  // 声誉
            memberData[15] = E_charm;       // 魅力
            memberData[16] = E_health;      // 健康
            memberData[17] = E_strategy;    // 计谋
        }

        // 更新皇家妻妾扩展属性
        private void UpdateRoyalSpouseExtendedAttributes(List<string> memberData)
        {
            memberData[20] = E_skillPoint;  // 技能点
        }

        // 更新皇家妻妾嵌套属性
        private string[] UpdateRoyalSpouseNestedAttributes(string[] memberInfo)
        {
            // 确保数组长度足够存储所有属性，不足时扩展数组
            int requiredLength = 11; 
            string[] updatedMemberInfo = memberInfo;
            
            if (memberInfo.Length < requiredLength)
            {
                updatedMemberInfo = new string[requiredLength];
                memberInfo.CopyTo(updatedMemberInfo, 0);
                // 初始化新增元素为默认值
                for (int i = memberInfo.Length; i < requiredLength; i++)
                {
                    updatedMemberInfo[i] = "0";
                }
            }
            
            // 确保所有属性都被正确赋值
            updatedMemberInfo[2] = E_talent ?? "0";     // 天赋
            updatedMemberInfo[3] = E_talentPoint ?? "0"; // 天赋点
            updatedMemberInfo[6] = E_skill ?? "0";      // 技能
            updatedMemberInfo[7] = E_luck ?? "0";       // 幸运
            updatedMemberInfo[8] = E_character ?? "0";  // 品性
            updatedMemberInfo[10] = E_preference ?? "0"; // 喜好
            
            return updatedMemberInfo;
        }
    
        // 郡数组，索引对应郡ID
        public static string[] JunList = new string[]
        {
            "南郡",     // 0
            "三川郡",   // 1
            "蜀郡",     // 2
            "丹阳郡",   // 3
            "陈留郡",   // 4
            "长沙郡",   // 5
            "会稽郡",   // 6
            "广陵郡",   // 7
            "太原郡",   // 8
            "益州郡",   // 9
            "南海郡",   // 10
            "云南郡"    // 11
        };

        // 二维县数组，第一维是郡索引，第二维是县索引
        public static string[][] XianList = new string[][]
        {
            // 南郡 (索引0)
            new string[] { "临沮", "襄樊", "宜城", "麦城", "华容", "郢亭", "江陵", "夷陵" },
            
            // 三川郡 (索引1)
            new string[] { "平阳", "荥阳", "原武", "阳武", "新郑", "宜阳" },
            
            // 蜀郡 (索引2)
            new string[] { "邛崃", "郫县", "什邡", "绵竹", "新都", "成都" },
            
            // 丹阳郡 (索引3)
            new string[] { "秣陵", "江乘", "江宁", "溧阳", "建邺", "永世" },
            
            // 陈留郡 (索引4)
            new string[] { "长垣", "济阳", "成武", "襄邑", "宁陵", "封丘" },
            
            // 长沙郡 (索引5)
            new string[] { "零陵", "益阳", "湘县", "袁州", "庐陵", "衡山", "建宁", "桂阳" },
            
            // 会稽郡 (索引6)
            new string[] { "曲阿", "松江", "山阴", "余暨" },
            
            // 广陵郡 (索引7)
            new string[] { "平安", "射阳", "海陵", "江都" },
            
            // 太原郡 (索引8)
            new string[] { "大陵", "晋阳", "九原", "石城", "阳曲", "魏榆", "孟县", "中都" },
            
            // 益州郡 (索引9)
            new string[] { "连然", "谷昌", "同劳", "昆泽", "滇池", "俞元", "胜休", "南安" },
            
            // 南海郡 (索引10)
            new string[] { "四会", "阳山", "龙川", "揭岭", "罗阳", "善禺" },
            
            // 云南郡 (索引11)
            new string[] { "云平", "叶榆", "永宁", "遂久", "姑复", "蜻陵", "弄栋", "邪龙" }
        };
    }
}
