using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace cs.HoLMod.Tool
{
    // 插件信息类
    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "cs.HoLMod.Tool.AnZhi20";
        public const string PLUGIN_NAME = "HoLMod.Tool";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PLUGIN_CONFIGPATH = "HoLMod.Tool.cfg";
        public const string PLUGIN_CONFIGDIR = "Tool";
    }
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Main : BaseUnityPlugin
    {
        // 存储所有加载的数据数组
        private static Dictionary<string, object> _dataArrays = new Dictionary<string, object>();
        
        // 存储字段映射
        private static Dictionary<string, string> _fieldMappings = new Dictionary<string, string>();
        
        // 存储主加载类型
        private static Type _mainloadType;
        
        // 存储字段缓存
        private static Dictionary<string, FieldInfo> _mainloadFieldCache = new Dictionary<string, FieldInfo>();
        
        // 存储字段名称列表
        private static List<string> _mainloadFieldNames = new List<string>();
        
        private static Main _instance;
        private WindowMain _windowMain;
        
        public static Main Instance
        {
            get { return _instance; }
        }
        
        public static Dictionary<string, object> DataArrays
        {
            get { return _dataArrays; }
        }
        
        public static Dictionary<string, string> FieldMappings
        {
            get { return _fieldMappings; }
        }
        
        public static List<string> MainloadFieldNames
        {
            get { return _mainloadFieldNames; }
        }
        
        private void Awake()
        {
            _instance = this;
            Logger.LogInfo("HoLMod.Tool 已加载");
            
            // 初始化主加载类型
            _mainloadType = Type.GetType("Mainload, Assembly-CSharp");
            
            // 初始化字段
            LoadFieldsFromConfigFile();
            LoadFieldMappings();
            ValidateAndCacheFields();
            
            // 初始化窗口组件
            _windowMain = gameObject.AddComponent<WindowMain>();
            _windowMain.Initialize(Logger);
            
            // 添加按键监听以显示/隐藏窗口
            AddLog("按 [Tab] 键显示/隐藏工具窗口");
        }
        
        private void Update()
        {
            // 按Tab键显示/隐藏窗口
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _windowMain?.ToggleWindow();
            }
        }
        
        public void AddLog(string message)
        {
            Logger.LogInfo("[HolMod] " + message);
        }
        
        private void LoadFieldsFromConfigFile()
        {
            try
            {
                string configDir = Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_CONFIGDIR);
                Directory.CreateDirectory(configDir);
                string configPath = Path.Combine(configDir, PluginInfo.PLUGIN_CONFIGPATH);
                
                if (!File.Exists(configPath))
                {
                    CreateDefaultConfigFile(configPath);
                }
                
                string[] lines = File.ReadAllLines(configPath);
                _mainloadFieldNames.Clear();
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("#"))
                    {
                        _mainloadFieldNames.Add(trimmedLine);
                    }
                }
                
                Logger.LogInfo(string.Format("从配置文件加载了 {0} 个字段", _mainloadFieldNames.Count));
            }
            catch (Exception ex)
            {
                Logger.LogError("加载配置文件时出错: " + ex.Message);
                UseFallbackFields();
            }
        }
        
        private void CreateDefaultConfigFile(string fullPath)
        {
            try
            {
                string[] defaultFields = new string[]
                {
                    "Member_now",
                    "Member_qu",
                    "MenKe_Now",
                    "CityData_XueYuan_now",
                    "ZhengLing_City_now",
                    "XunXing_King",
                    "ZiBei_Now",
                    "Cost_King",
                    "PerData",
                    "ZhengLing_Now",
                    "WangGData_now",
                    "JiHuiMeet_now",
                    "TaskHD_Now",
                    "TaskYanShi_Now",
                    "TaskOrderData_Now",
                    "PropPrice_Now",
                    "ShijiaOutXianPoint",
                    "WarEvent_Now",
                    "Trade_Playershop",
                    "JieDai_now",
                    "Buluo_Now",
                    "KingCityData_now",
                    "ShiJia_king",
                    "ShiJia_Now",
                    "JunYing_now",
                    "Zhen_now",
                    "Cun_now",
                    "Kong_now",
                    "Sen_Now",
                    "Hu_Now",
                    "Shan_now",
                    "Kuang_now",
                    "NongZ_now",
                    "Fengdi_now",
                    "Fudi_now",
                    "ZhuangTou_now",
                    "Member_First",
                    "Doctor_now",
                    "Member_Qinglou",
                    "Member_Hanmen",
                    "Member_Other_qu",
                    "Member_other",
                    "Member_King_qu",
                    "Member_King",
                    "Member_Ci",
                    "CGNum",
                    "FamilyData",
                    "Time_now",
                    "Prop_have",
                    "Horse_Have",
                    "IdIndex",
                    "ShangHui_now",
                    "CityData_now",
                    "Guan_JingCheng",
                    "PlayTimeRun",
                    "MemberNumWillDead",
                    "NuLiNum",
                    "VisionIndex",
                    "SceneID",
                    "CityID_CanAttack",
                    "ZuZhangJueCeID_now",
                    "BuildTaoZhuangID_SelectNow",
                    "GetMoney_Month",
                    "SelectNumPer_Shop",
                    "SelectNumPerAct",
                    "CitySawSetData",
                    "CangShuGeData_Now",
                    "XiQuHave_Now",
                    "ZhiZeData_ZhangMu",
                    "VersionID"
                };
                
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.WriteLine("# Mainload字段配置文件");
                    writer.WriteLine("# 每行一个字段名，支持#开头的注释行");
                    writer.WriteLine("# 修改后无需重启游戏，按Tab关闭再打开菜单即可生效");
                    writer.WriteLine();
                    foreach (string field in defaultFields)
                    {
                        writer.WriteLine(field);
                    }
                }
                
                Logger.LogInfo("已创建默认配置文件: " + fullPath);
            }
            catch (Exception ex)
            {
                Logger.LogError("创建默认配置文件失败: " + ex.Message);
            }
        }
        
        private void UseFallbackFields()
        {
            _mainloadFieldNames = new List<string>
            {
                "Member_now",
                "MenKe_Now",
                "Member_qu"
            };
            
            Logger.LogWarning(string.Format("使用后备字段列表，共 {0} 个字段", _mainloadFieldNames.Count));
        }
        
        private void LoadFieldMappings()
        {
            try
            {
                string configDir = Path.Combine(Paths.ConfigPath, PluginInfo.PLUGIN_CONFIGDIR);
                string mappingPath = Path.Combine(configDir, "Map.cfg");
                
                Directory.CreateDirectory(configDir);
                
                if (!File.Exists(mappingPath))
                {
                    Logger.LogWarning("映射配置文件不存在，创建默认文件: " + mappingPath);
                    CreateDefaultMappingFile(mappingPath);
                }
                else
                {
                    string[] lines = File.ReadAllLines(mappingPath);
                    _fieldMappings.Clear();
                    
                    foreach (string line in lines)
                    {
                        string trimmedLine = line.Trim();
                        if (!string.IsNullOrEmpty(trimmedLine) && !trimmedLine.StartsWith("#"))
                        {
                            int equalsIndex = trimmedLine.IndexOf('=');
                            if (equalsIndex > 0 && equalsIndex < trimmedLine.Length - 1)
                            {
                                string fieldPattern = trimmedLine.Substring(0, equalsIndex).Trim();
                                string displayName = trimmedLine.Substring(equalsIndex + 1).Trim();
                                
                                if (!_fieldMappings.ContainsKey(fieldPattern))
                                {
                                    _fieldMappings.Add(fieldPattern, displayName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("加载字段映射失败: " + ex.Message);
            }
        }
        
        private void CreateDefaultMappingFile(string path)
        {
            List<string> contents = new List<string>
            {
                "# 数据字段映射配置文件",
                "# 格式: 字段路径模式 = 显示名称",
                "# 使用*作为通配符匹配任意数字索引",
                "",
                "# 示例配置",
                "Member_now = 族人",
                "MenKe_Now[*][0] = ID"
            };
            
            File.WriteAllLines(path, contents);
            Logger.LogInfo("已创建默认映射配置文件: " + path);
        }
        
        private void ValidateAndCacheFields()
        {
            try
            {
                _mainloadFieldCache.Clear();
                List<string> validFields = new List<string>();
                
                foreach (string fieldName in _mainloadFieldNames)
                {
                    FieldInfo field = _mainloadType?.GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
                    if (field != null)
                    {
                        validFields.Add(fieldName);
                        _mainloadFieldCache[fieldName] = field;
                    }
                    else
                    {
                        Logger.LogWarning("Mainload中不存在字段: " + fieldName + " - 将被忽略");
                    }
                }
                
                _mainloadFieldNames = validFields;
                Logger.LogInfo(string.Format("成功加载并验证 {0} 个有效字段", _mainloadFieldNames.Count));
            }
            catch (Exception ex)
            {
                Logger.LogError("验证字段时出错: " + ex.Message);
            }
        }
        
        // 根据字段名获取数据
        public static object GetDataByFieldName(string fieldName)
        {
            try
            {
                if (_dataArrays.ContainsKey(fieldName))
                {
                    return _dataArrays[fieldName];
                }
                
                FieldInfo field;
                if (_mainloadFieldCache.TryGetValue(fieldName, out field))
                {
                    object data = field.GetValue(null);
                    _dataArrays[fieldName] = data;
                    return data;
                }
                
                return null;
            }
            catch (Exception ex)
            {
                if (_instance != null)
                {
                    _instance.Logger.LogError("获取数据时出错: " + ex.Message);
                }
                return null;
            }
        }
        
        // 从路径获取对象（支持多层嵌套）
        public static object GetObjectFromPath(string fieldName, List<int> indices)
        {
            try
            {
                FieldInfo field;
                if (!_mainloadFieldCache.TryGetValue(fieldName, out field))
                {
                    return null;
                }
                
                object obj = field.GetValue(null);
                foreach (int index in indices)
                {
                    System.Collections.IList list = obj as System.Collections.IList;
                    if (list == null)
                    {
                        return null;
                    }
                    
                    if (index >= 0 && index < list.Count)
                    {
                        obj = list[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                
                return obj;
            }
            catch (Exception ex)
            {
                if (_instance != null)
                {
                    _instance.Logger.LogError("获取对象路径时出错: " + ex.Message);
                }
                return null;
            }
        }
        
        // 构建路径字符串
        public static string BuildPath(string fieldName, List<int> indices)
        {
            if (indices.Count == 0)
            {
                return fieldName;
            }
            
            string path = fieldName;
            foreach (int index in indices)
            {
                path += string.Format("[{0}]", index);
            }
            
            return path;
        }
        
        // 检查对象是否有子数据
        public static bool HasSubData(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            if (obj.GetType().IsValueType || obj is string)
            {
                return false;
            }
            
            System.Collections.IEnumerable enumerable = obj as System.Collections.IEnumerable;
            if (enumerable != null && !(obj is string))
            {
                return true;
            }
            
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length > 0)
            {
                return true;
            }
            
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            return fields.Length > 0;
        }
        
        // 获取显示名称
        public static string GetDisplayName(string fieldPath)
        {
            if (string.IsNullOrEmpty(fieldPath))
            {
                return "";
            }
            
            string displayName;
            if (_fieldMappings.TryGetValue(fieldPath, out displayName))
            {
                return displayName;
            }
            
            // 检查模式匹配
            foreach (KeyValuePair<string, string> mapping in _fieldMappings)
            {
                if (IsPatternMatch(fieldPath, mapping.Key))
                {
                    return mapping.Value;
                }
            }
            
            // 返回简化后的路径
            return SimplifyPath(fieldPath);
        }
        
        // 检查路径是否匹配模式
        private static bool IsPatternMatch(string fieldPath, string pattern)
        {
            string[] pathSegments = SplitPathIntoSegments(fieldPath);
            string[] patternSegments = SplitPathIntoSegments(pattern);
            
            if (pathSegments.Length != patternSegments.Length)
            {
                return false;
            }
            
            for (int i = 0; i < pathSegments.Length; i++)
            {
                string pathSeg = pathSegments[i];
                string patternSeg = patternSegments[i];
                
                if (patternSeg == "*")
                {
                    int index;
                    if (!int.TryParse(pathSeg, out index))
                    {
                        return false;
                    }
                }
                else if (pathSeg != patternSeg)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        // 将路径分割成段
        private static string[] SplitPathIntoSegments(string path)
        {
            List<string> segments = new List<string>();
            System.Text.StringBuilder currentSegment = new System.Text.StringBuilder();
            bool inBracket = false;
            
            foreach (char c in path)
            {
                if (c == '[')
                {
                    if (currentSegment.Length > 0)
                    {
                        segments.Add(currentSegment.ToString());
                        currentSegment.Clear();
                    }
                    inBracket = true;
                }
                else if (c == ']')
                {
                    if (currentSegment.Length > 0)
                    {
                        segments.Add(currentSegment.ToString());
                        currentSegment.Clear();
                    }
                    inBracket = false;
                }
                else
                {
                    if (inBracket)
                    {
                        currentSegment.Append(c);
                    }
                    else
                    {
                        if (c == '.')
                        {
                            if (currentSegment.Length > 0)
                            {
                                segments.Add(currentSegment.ToString());
                                currentSegment.Clear();
                            }
                        }
                        else
                        {
                            currentSegment.Append(c);
                        }
                    }
                }
            }
            
            if (currentSegment.Length > 0)
            {
                segments.Add(currentSegment.ToString());
            }
            
            return segments.ToArray();
        }
        
        // 简化路径
        private static string SimplifyPath(string path)
        {
            return path;
        }
    }
}