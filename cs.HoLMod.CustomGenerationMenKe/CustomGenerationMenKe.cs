using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;



namespace MenKeConverter
{
    [BepInPlugin("cs.HoLMod.CustomGenerationMenKe.AnZhi20", "HoLMod.CustomGenerationMenKe", "1.2.0")]
    public class CustomGenerationMenKe : BaseUnityPlugin
    {
        // 窗口及配置文件设置
        private ConfigEntry<float> menuWidth;
        private ConfigEntry<float> menuHeight;
        private ConfigEntry<string> menKeSalary; // 门客工资配置
        private ConfigEntry<string> MenKeMaxSkillPoints; // 门客超模技能点配置
        private static Rect windowRect;
        private static bool showMenu = false;
        private static bool blockGameInput = false;
        private static Vector2 scrollPosition;
        
        // 定义常量
        const string A1 = ",";
        const string A2 = "|";
        const string A3 = "null";
        const string A4 = "M13"; 
        const string A5 = "0";
        const string A6 = "-1";
        const string Assignment = "100"; // 赋固定值
        

        // 定义变量
        private string Talent_Points = "0";    // 天赋点数 
        private string Skill_Points = "0";    // 技能点数 
        private string Alentin = "空"; // 天赋
        private string Skillsin = "空"; // 技能
        private int generateCount = 1;
        private string statusMessage = "准备就绪";
        private int customAge = 18;    // 年龄 
        
        // 当前插件版本
        private const string CURRENT_VERSION = "1.2.0"; // 与BepInPlugin属性中定义的版本保持一致


        // 定义数据列表
        static readonly List<string> After = Enumerable.Range(1, 19).Select(i => i.ToString()).ToList();  // 后发
        static readonly List<string> Body = Enumerable.Range(1, 29).Select(i => i.ToString()).ToList();   // 身体
        static readonly List<string> Face = Enumerable.Range(1, 2).Select(i => i.ToString()).ToList();    // 脸部
        static readonly List<string> Before = Enumerable.Range(1, 19).Select(i => i.ToString()).ToList(); // 前发
        static readonly List<string> BeforeRandom = Enumerable.Range(1, 9).Select(i => i.ToString()).ToList();  // 前随机
        static readonly List<string> Lifespan = new List<string> { "57", "58", "60", "62", "63", "64", "67", "68", "72", "73", "74", "75", "79", "80", "82" };  // 寿命
        static readonly List<string> Personality = Enumerable.Range(1, 6).Select(i => i.ToString()).ToList();  // 性格
        static readonly List<string> Remuneration = new List<string> { "200", "320", "460", "560", "680", "720", "760", "800", "920", "940", "1000", "1080", "1100", "1120", "1160", "1500", "1640", "2160", "2500" };  // 酬劳

        // 姓氏和名字列表
        static readonly List<string> Surname = new List<string> { "谭", "钱", "文", "金", "顾", "姜", "张", "宋", "王", "郭", "谢", "赵", "邓", "苏", "范", "魏", "余", "白", "贾", "郝", "崔", "钟", "方", "于", "丁", "康", "冯", "李", "许", "孙", "邵", "田", "陆", "孔", "汤", "黎", "林", "孟", "杨", "马", "梁", "高", "朱", "常", "卢", "徐", "段", "武", "戴", "尹", "薛", "郑", "廖", "任", "邱", "龚", "刘", "万", "贺", "秦", "吕", "黄", "沈", "罗", "毛", "蔡", "吴", "易", "萧", "汪", "姚", "潘", "雷", "江", "傅", "胡", "乔", "侯", "曹", "叶", "彭", "蒋", "程", "董", "何", "袁", "周", "熊", "陈", "唐", "杜", "夏" };
        static readonly List<string> Namenan = new List<string> { "德", "浩", "乐", "华", "和", "锦", "博", "天", "弘", "飞", "阳", "文", "景", "振", "高", "宜", "运", "俊", "学", "宏", "意", "彭", "锐", "力", "欣", "志", "耘", "安", "光", "向", "涵", "雅", "鸿", "昊", "恺", "令", "星", "正", "项", "敏", "海", "睿", "咏", "骞", "泰", "修", "鹏", "永", "丰", "烨", "黎", "英", "兴", "蕴", "成", "玉", "昆", "长", "展", "元", "新", "智", "康", "骏", "伟", "雨", "建", "澎", "勇", "宾", "承", "宇", "瀚", "昌", "哲", "震", "波", "嘉", "佑", "毅", "自", "翰", "良", "同", "远", "茂", "致", "经", "温", "维", "思", "泽", "濮", "明", "苑", "昂", "坚", "凯", "绍", "允", "庆", "信", "职", "奇", "彦", "祺", "作", "君", "刚", "晨", "曦", "彬", "寅", "卓", "峻", "立", "斌", "辰", "国", "斯", "心", "曜", "驰", "巍", "雪", "季", "胤", "朋", "旭", "冠", "浦", "炫", "皓", "翔", "溥", "风", "稷", "琪", "开", "晟", "书", "颜", "理", "晗", "池", "昶", "程", "延", "禄", "逸", "中", "煦", "荣", "寒", "然", "诚", "义", "容", "爽", "辉", "专", "涛", "澜", "灿", "亮", "盛", "健", "苍", "生", "歌", "锋", "汉", "魄", "赋", "阵", "青", "禹", "怿", "民", "叡", "贤", "福", "翱", "羽", "磊", "昕", "澄", "业", "山", "琦", "平", "武", "觉", "量", "魁", "超", "雄", "沉", "飒", "楚", "淼", "湃", "基", "采", "白", "宁", "硕", "壮", "恩", "化", "朗", "宕", "荫", "畅", "旻", "用", "慨", "玥", "熙", "圣", "琛", "厚", "轩", "懿", "拔", "珍", "畴", "渊", "言", "达", "甲", "春", "通", "洲", "木", "焱", "邈", "筠", "濯", "旷", "衍", "深", "利", "冰", "举", "存", "晖", "燎", "祯", "真", "钧", "图", "发", "枫", "鸥", "耀", "勤", "美", "载", "云", "纬", "豪", "鑫", "祥", "彩", "舒", "济", "甫", "莱", "章", "翼", "广", "石", "之", "郁", "腾", "名", "瑜", "墨", "弼", "蔚", "纶", "北", "实", "格", "钊", "仁", "映", "年", "秋", "卿", "勋", "望", "杰", "禧", "栋", "庸", "典", "烁", "宝", "迈", "聪", "朝", "峰", "洁", "赡", "清", "寰", "航", "谊", "语", "宣", "嗣", "茗", "羲", "邦", "尧", "为", "颂", "霖", "峯", "初", "赐", "骥", "赫", "穹", "捷", "舟", "锟", "龙", "煊", "津", "知", "河", "薄", "怀", "空", "忍", "裕", "林", "行", "晋", "彰", "材", "楠", "路", "益", "鹍", "懋", "惬", "誉", "旺", "卉", "树", "亘", "东", "精", "藏", "邃", "钰", "阔", "易", "璧", "润", "艺", "伯", "湛", "胜", "铄", "群", "日", "穰", "源", "策", "梓", "寿", "默", "尘", "慈" };
        static readonly List<string> Namenv = new List<string> { "清", "嘉", "逸", "平", "雪", "安", "琛", "思", "初", "静", "慧", "淑", "晨", "岚", "涵", "秀", "翠", "琳", "绮", "代", "欢", "夜", "以", "悦", "心", "书", "半", "和", "妙", "梓", "天", "飞", "山", "丹", "萍", "昕", "绿", "芫", "雅", "冰", "灵", "玲", "尔", "念", "婉", "蔓", "欣", "雯", "娅", "轩", "千", "新", "寄", "吉", "笑", "芷", "芝", "晴", "醉", "谷", "智", "觅", "湛", "绣", "芳", "幼", "惜", "米", "凝", "鸣", "紫", "雨", "合", "莹", "丝", "含", "凡", "今", "珍", "舒", "听", "兰", "语", "格", "梦", "怡", "丽", "易", "忆", "艳", "雁", "晓", "香", "宛", "古", "润", "如", "碧", "媛", "水", "望", "霞", "子", "秋", "巧", "从", "驰", "玛", "沛", "寒", "友", "诗", "迎", "瑾", "慕", "春", "畅", "依", "明", "任", "琬", "采", "白", "菱", "亦", "素", "冷", "盼", "云", "献", "家", "幻", "星", "柔", "忻", "宜", "可", "冬", "芃", "向", "美", "北", "问", "娟", "长", "言", "红", "寻", "运", "元", "孤", "瑜", "惠", "英", "斐", "隽", "洛", "蕴", "奕", "典", "妞", "夏", "琼", "南", "晶", "梅", "月", "杰", "浩", "熙", "弘", "朵", "河", "密", "若", "梧", "燕", "君", "暄", "修", "玉", "恬", "好", "芮", "洁", "玮", "菁", "施", "松", "芸", "彦", "哲", "映", "情", "湘", "彤", "彩", "品", "轶", "菀", "布", "华", "茉", "偲", "琲", "琴", "湉", "佳", "青", "音", "歌", "喜", "贝", "晗", "怿", "方", "怀", "乐", "杏", "浓", "楚", "悠", "流", "皓", "苇", "馨", "洮", "仪", "端", "韵", "之", "倩", "沙", "淳", "朝", "沈", "令", "蓓", "贞", "蓉", "骏", "芬", "琇", "娴", "齐", "彗", "金", "雍", "蝶", "笛", "真", "心", "淑", "美", "晓", "荷", "云", "蓝", "雅", "丽", "贤", "风", "岚", "绿", "阳", "溪", "南", "天", "悦", "霁", "卉", "冬", "怡", "宜", "雁", "雪", "静", "梦", "蓓", "媛", "兰", "英", "琴", "韵", "丹", "月", "竹", "华", "春", "妍", "柏", "珊", "双", "秋", "然", "槐", "蕾", "枫", "凡", "清", "菁", "笑", "玟", "慧", "秀", "素", "梅", "灵", "桃", "寒", "蝶", "波", "惠", "玉", "宸", "晴", "娟", "蓉", "菱", "远", "帆", "莉", "彤", "乐", "馨", "意", "容", "琇", "卓", "翠", "琪", "爱", "巧", "雨", "莲", "豫", "芹", "泽", "涵", "蕊", "格", "易", "洁", "语", "珠", "逸", "芳", "露", "凝", "和", "瑶", "曦", "辞", "微", "菡", "文", "丝", "欣", "白", "青", "莹", "暖", "霜", "薇", "芬", "荣", "煦", "恩", "星", "真", "山", "飞", "若", "婕", "歌", "妙", "慕", "松", "旋", "楠", "愉", "燕", "芃", "安", "怀", "蕙", "辰", "娥", "绮", "宁", "萱", "夏", "柔", "俊", "宇", "旭", "苗", "君", "敏", "义", "娴", "畅", "骄", "妞", "之", "烟", "辉", "茵", "香", "朗", "芙", "颖", "思", "臻", "桐", "冰", "婷", "舞", "珏", "艺", "亦", "璟", "琲", "欢", "奇", "霏", "晨", "照", "红", "艳", "仪", "艾", "嘉", "霞", "珺", "恺", "芝", "菀", "采", "偲", "湉", "佳", "筠", "融", "蔓", "农", "虹", "诗", "可", "念", "彩", "音", "丰", "明", "如", "画", "洮", "瑜", "婉", "珑", "暎", "优", "昕", "阑", "婧", "枝", "花" };

        // 天赋和技能列表
        static readonly List<string> Alent = new List<string> { "空", "文", "武", "商", "艺" };
        static readonly List<string> Skills = new List<string> { "空", "巫", "医", "相", "卜", "魅", "工" };

        
        // 界面控制变量
        private int selectedSex = 0; // 0: 女, 1: 男
        private int selectedAlent = 0; // 0: 空, 1: 文, 2: 武, 3: 商, 4: 艺
        private int selectedSkill = 0; // 0: 空, 1: 巫, 2: 医, 3: 相, 4: 卜, 5: 魅, 6: 工
        
        private void Awake()
        {
            Logger.LogInfo("门客自定义生成器已加载！");
            
            // 版本检查配置
            var loadedVersion = Config.Bind("内部配置（Internal Settings）", "已加载版本（Loaded Version）", "", "用于跟踪插件版本，请勿手动修改").Value;
            
            // 检查是否需要更新配置
            bool isVersionUpdated = loadedVersion != CURRENT_VERSION;
            
            // 配置窗口大小
            menuWidth = Config.Bind<float>("窗口设置（Window Settings）", "窗口宽度（Menu Width）", 400f, "修改器主菜单宽度");
            menuHeight = Config.Bind<float>("窗口设置（Window Settings）", "窗口高度（Menu Height）", 700f, "修改器主菜单高度");
            
            // 配置门客工资
            menKeSalary = Config.Bind<string>("门客设置（MenKe Settings）", "门客月薪（MenKe Salary）", "50000", "门客的属性都这样了，月薪50k不过分吧？当然你也可以修改一下让门客当黑奴！（注意：门客的月薪不能超过100w，因为它不值这个价）");
            
            // 配置门客超模技能点
            MenKeMaxSkillPoints = Config.Bind<string>("门客设置（MenKe Settings）", "门客超模技能点（MenKe Max Skill Points）", "10", "门客属性点，默认值为10");
            
            // 如果版本更新，重新生成配置文件
            if (isVersionUpdated)
            {
                Logger.LogInfo($"检测到插件版本更新至 {CURRENT_VERSION}，正在重新生成配置文件...");
                
                try
                {
                    // 重置所有配置项
                    Config.Clear();
                    
                    // 重新绑定所有配置项
                    menuWidth = Config.Bind<float>("窗口设置（Window Settings）", "窗口宽度（Menu Width）", 400f, "修改器主菜单宽度");
                    menuHeight = Config.Bind<float>("窗口设置（Window Settings）", "窗口高度（Menu Height）", 700f, "修改器主菜单高度");
                    menKeSalary = Config.Bind<string>("门客设置（MenKe Settings）", "门客月薪（MenKe Salary）", "50000", "门客的属性都这样了，月薪50k不过分吧？当然你也可以修改一下让门客当黑奴！（注意：门客的月薪不能超过100w，因为它不值这个价）");
                    MenKeMaxSkillPoints = Config.Bind<string>("门客设置（MenKe Settings）", "门客超模技能点（MenKe Max Skill Points）", "10", "门客属性点，默认值为10");
                    
                    // 保存当前版本号
                    Config.Bind("内部配置（Internal Settings）", "已加载版本（Loaded Version）", CURRENT_VERSION, "用于跟踪插件版本，请勿手动修改");
                    
                    // 保存新的配置文件
                    Config.Save();
                    
                    Logger.LogInfo("配置文件已成功重新生成。");
                }
                catch (Exception ex)
                {
                    Logger.LogError("重新生成配置文件时出错: " + ex.Message);
                }
            }
            
            windowRect = new Rect(20f, 20f, menuWidth.Value, menuHeight.Value);
            
            // 应用Harmony补丁
            Harmony harmony = new Harmony("CustomGenerationMenKe");
            harmony.PatchAll();
        }
        
        private void Update()
        {
            // 按F1键切换菜单显示
            if (UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                ToggleMenu();
            }
            
            // 按ESC键关闭菜单
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape) && showMenu)
            {
                CloseMenu();
            }
            
            // 阻止游戏输入
            if (blockGameInput)
            {
                UnityEngine.Input.ResetInputAxes();
            }
        }
        
        /// <summary>
        /// 切换菜单显示状态
        /// </summary>
        private void ToggleMenu()
        {
            showMenu = !showMenu;
            blockGameInput = showMenu;
        }
        
        /// <summary>
        /// 关闭菜单
        /// </summary>
        private void CloseMenu()
        {
            showMenu = false;
            blockGameInput = false;
        }
        
        private void OnGUI()
        {
            if (showMenu)
            {
                windowRect = UnityEngine.GUI.Window(0, windowRect, DrawWindow, "门客自定义生成器", UnityEngine.GUI.skin.window);
            }
        }
        
        private void DrawWindow(int windowID)
        {
            UnityEngine.GUILayout.BeginVertical();
            UnityEngine.GUILayout.Space(10f);
            
            // 显示状态信息
            UnityEngine.GUILayout.Label("状态: " + statusMessage, UnityEngine.GUI.skin.box, new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Height(50f) });
            UnityEngine.GUILayout.Space(10f);
            
            // 性别选择
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("门客性别:", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(80f) });
            selectedSex = UnityEngine.GUILayout.Toolbar(selectedSex, new string[] { "女", "男" });
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(10f);
            
            // 天赋选择
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("门客天赋:", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(80f) });
            selectedAlent = UnityEngine.GUILayout.SelectionGrid(selectedAlent, Alent.ToArray(), 3);
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(10f);
            
            // 技能选择
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("门客技能:", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(80f) });
            selectedSkill = UnityEngine.GUILayout.SelectionGrid(selectedSkill, Skills.ToArray(), 3);
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(10f);
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("门客年龄:", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(80f) });
            customAge = (int)UnityEngine.GUILayout.HorizontalSlider(customAge, 18, 60, new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(200f) });
            UnityEngine.GUILayout.Label(customAge.ToString(), new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(40f) });
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(10f);
            
            // 数量输入
            UnityEngine.GUILayout.BeginHorizontal();
            UnityEngine.GUILayout.Label("生成数量:", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(80f) });
            string countInput = UnityEngine.GUILayout.TextField(generateCount.ToString(), new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Width(100f) });
            if (int.TryParse(countInput, out int count))
            {
                generateCount = UnityEngine.Mathf.Clamp(count, 1, 1000);
            }
            UnityEngine.GUILayout.EndHorizontal();
            UnityEngine.GUILayout.Space(20f);
            

            // 实时添加按钮
            if (UnityEngine.GUILayout.Button("实时添加到游戏", new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Height(40f) }))
            {
                if (generateCount < 1 || generateCount > 1000)
                {
                    statusMessage = "错误: 生成数量必须在1到1000之间！";
                    Logger.LogWarning("生成数量超出范围: " + generateCount);
                }
                else if (customAge < 18 || customAge > 60)
                {
                    statusMessage = "错误: 门客年龄必须在18到60之间！";
                    Logger.LogWarning("门客年龄超出范围: " + customAge);
                }
                else
                {
                    RealTimeAddMenKeData();
                }
            }
            UnityEngine.GUILayout.Space(20f);
            
            // 使用说明
            UnityEngine.GUILayout.Label("使用说明:", UnityEngine.GUI.skin.box);
            scrollPosition = UnityEngine.GUILayout.BeginScrollView(scrollPosition, new UnityEngine.GUILayoutOption[] { UnityEngine.GUILayout.Height(210f) });
            UnityEngine.GUILayout.Label("1. 请在点击添加前先保存游戏，以便回档");
            UnityEngine.GUILayout.Label("2. 按F1键显示 / 隐藏菜单"); 
            UnityEngine.GUILayout.Label("3. 选择门客性别、天赋和技能");
            UnityEngine.GUILayout.Label("4. 选择门客年龄");
            UnityEngine.GUILayout.Label("5. 输入要生成的门客数量");
            UnityEngine.GUILayout.Label("6. 点击'实时添加到游戏'按钮直接添加到当前游戏中");
            UnityEngine.GUILayout.Label("7. 本mod作者：AnZhi20"); 
            UnityEngine.GUILayout.Label("8. 本mod版本：1.2.0");
            UnityEngine.GUILayout.EndScrollView();
            
            UnityEngine.GUILayout.EndVertical();
            
            // 允许拖动窗口
            UnityEngine.GUI.DragWindow();
        }
        

        
        /// <summary>
        /// 实时添加门客数据到游戏中（不修改GameData.es3）
        /// </summary>
        private void RealTimeAddMenKeData()
        {
            try
            {
                statusMessage = "正在生成门客数据...";
                Logger.LogInfo("开始实时添加门客数据");
                
                List<string> newMenKeData = new List<string>();
                
                for (int i = 0; i < generateCount; i++)
                {
                    // 生成随机ID
                    string Mrandom = (UnityEngine.Random.Range(1000000, 9999999) + DateTime.Now.Ticks).ToString();
                    Logger.LogInfo($"生成门客 {i+1}/{generateCount}，随机ID: {Mrandom}");
                    
                    // 随机选择姓氏、名字前缀和后缀
                    string RanSurname = Surname[UnityEngine.Random.Range(0, Surname.Count)];
                    string RanBefore = Before[UnityEngine.Random.Range(0, Before.Count)];
                    string RanAfter = After[UnityEngine.Random.Range(0, After.Count)];
                    string RanBody = Body[UnityEngine.Random.Range(0, Body.Count)];
                    string RanFace = Face[UnityEngine.Random.Range(0, Face.Count)];
                    string RanBeforeRandom = BeforeRandom[UnityEngine.Random.Range(0, BeforeRandom.Count)];
                    
                    // 生成年龄
                    int age = UnityEngine.Random.Range(16, 50);
                    string Age = age.ToString();
                    
                    // 随机选择性格和寿命
                    string RanPersonality = Personality[UnityEngine.Random.Range(0, Personality.Count)];
                    string RanLifespan = Lifespan[UnityEngine.Random.Range(0, Lifespan.Count)];
                    
                    // 薪酬设置并确保其有效
                    string RanRemuneration = "50000"; // 默认值
                    if (int.TryParse(menKeSalary.Value, out int salaryValue))
                    {
                        if (salaryValue >= 0 && salaryValue <= 1000000)
                        {
                            RanRemuneration = salaryValue.ToString();
                        }
                    }

                    if (int.TryParse(menKeSalary.Value, out int salaryValueCheck))
                        {
                            if (salaryValueCheck < 0)
                            {
                                RanRemuneration = "50000";
                            }
                            else if (salaryValueCheck > 1000000)
                            {
                                RanRemuneration = "50000";
                            }
                            else
                            {
                                RanRemuneration = salaryValueCheck.ToString();
                            }
                        }
                        
                    
                    // 根据性别选择名字
                    string RanName1, RanName2;
                    if (selectedSex == 1)  // 男
                    {
                        RanName1 = Namenan[UnityEngine.Random.Range(0, Namenan.Count)];
                        RanName2 = Namenan[UnityEngine.Random.Range(0, Namenan.Count)];
                    }
                    else  // 女
                    {
                        RanName1 = Namenv[UnityEngine.Random.Range(0, Namenv.Count)];
                        RanName2 = Namenv[UnityEngine.Random.Range(0, Namenv.Count)];
                    }


                    // 将选择的天赋赋值给对应变量
                    switch (Alent[selectedAlent])
                    {
                        case "空":
                            Alentin = "0";
                            Talent_Points = "0";
                            break;
                        case "文":
                            Alentin = "1";
                            Talent_Points = "100";
                            break;
                        case "武":
                            Alentin = "2";
                            Talent_Points = "100";
                            break;
                        case "商":
                            Alentin = "3";
                            Talent_Points = "100";
                            break;
                        case "艺":
                            Alentin = "4";
                            Talent_Points = "100";
                            break;
                        default:
                            Alentin = "0";
                            Talent_Points = "0";
                            break;
                    }


                    // 将选择的技能赋值给对应变量
                    switch (Skills[selectedSkill])
                    {
                        case "空":
                            Skillsin = "0";
                            Skill_Points = "0";
                            break;
                        case "巫":
                            Skillsin = "1";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        case "医":
                            Skillsin = "2";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        case "相":
                            Skillsin = "3";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        case "卜":
                            Skillsin = "4";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        case "魅":
                            Skillsin = "5";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        case "工":
                            Skillsin = "6";
                            Skill_Points = MenKeMaxSkillPoints.Value;
                            break;
                        default:
                            Skillsin = "0";
                            Skill_Points = "0";
                            break;
                    }
                    

                    // 构建门客角色数据字符串
                    // 角色ID
                    string MenKe_ID = $"{A4}{Mrandom}";
                    // 角色合并 后发|身体|脸部|前发
                    string MenKe_JueSe = $"{RanAfter}{A2}{RanBody}{A2}{RanFace}{A2}{RanBefore}";
                    // 属性合并 姓名合并_姓辈字|前随机|天赋|天赋点数|性别|寿命|技能|幸运|性格|null
                    string MenKe_ShuXing = $"{RanSurname}{RanName1}{RanName2}{A2}{RanBeforeRandom}{A2}{Alentin}{A2}{Talent_Points}{A2}{selectedSex}{A2}{RanLifespan}{A2}{Skillsin}{A2}{Assignment}{A2}{RanPersonality}{A2}{A3}";
                    // 年龄
                    string MenKe_Age = customAge.ToString();
                    // 常规属性"文","武","商","艺","心情","声誉","魅力","健康","计谋","健康","体力"
                    string MenKe_ChangGui = $"{Assignment}";
                    // 住房属性
                    string MenKe_ZhuFang = $"{A5}{A2}{A3}{A2}{A3}";
                    // 位置属性 0
                    string MenKe_unknown = $"{A5}";


                    // 将门客数据直接添加到游戏的内存数据中
                    Logger.LogInfo($"检查Mainload.MenKe_Now是否为空: {(Mainload.MenKe_Now == null ? "是" : "否")}");
                    if (Mainload.MenKe_Now != null)
                    {
                        // 检查Mainload.HanMenMemberData_Click是否为空，如果为空则使用默认值
                        Logger.LogInfo($"检查Mainload.HanMenMemberData_Click是否为空: {(Mainload.HanMenMemberData_Click == null ? "是" : "否")}");
                        string hanMenData18 = "0";
                        if (Mainload.HanMenMemberData_Click != null)
                        {
                            Logger.LogInfo($"Mainload.HanMenMemberData_Click.Count = {Mainload.HanMenMemberData_Click.Count}");
                            if (Mainload.HanMenMemberData_Click.Count > 18)
                            {
                                hanMenData18 = Mainload.HanMenMemberData_Click[18];
                                Logger.LogInfo($"获取HanMenMemberData_Click[18] = {hanMenData18}");
                            }
                        }

                        
                        Mainload.MenKe_Now.Add(new List<string>
                        {
                            MenKe_ID,
                            MenKe_JueSe,
                            MenKe_ShuXing,
                            MenKe_Age,
                            MenKe_ChangGui,//文
                            MenKe_ChangGui,//武
                            MenKe_ChangGui,//商
                            MenKe_ChangGui,//艺
                            MenKe_ChangGui,//心情
                            "0|null|null",
                            "0",//状态
                            MenKe_ChangGui,//声誉
                            hanMenData18,
                            MenKe_ChangGui,//魅力
                            MenKe_ChangGui,//健康
                            MenKe_ChangGui,//计谋
                            Skill_Points,//技能点
                            "-1",//怀孕月份
                            RanRemuneration,//工资，根据配置文件判断
                            MenKe_ChangGui,//体力
                            "0",
                            "null"//特殊标签，Mainload.HanMenMemberData_Click[26]
                        });
                        Logger.LogInfo("成功添加门客到游戏中：" + MenKe_JueSe);
                    }
                }

                statusMessage = "正在添加到游戏中...";
                
                if (Mainload.MenKe_Now != null)
                {
                    statusMessage = $"成功添加 {generateCount} 条门客数据到游戏中";
                }
                else
                {
                    statusMessage = "游戏数据未加载，无法实时添加门客";
                    Logger.LogWarning("Mainload.MenKe_Now 为空，无法实时添加门客");
                }
            }
            catch (Exception e)
            {
                statusMessage = "添加失败: " + e.Message;
                Logger.LogError("实时添加门客失败: " + e.Message);
                Logger.LogError("异常堆栈: " + e.StackTrace);
            }
        }
    }
}