using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace cs.HoLMod.TaskCheat
{
    // 插件信息类
    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "cs.HoLMod.TaskCheat.AnZhi20";
        public const string PLUGIN_NAME = "HoLMod.TaskCheat";
        public const string PLUGIN_VERSION = "2.2.0";
    }
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class TaskCheat : BaseUnityPlugin
    {
        // GUI窗口位置和大小
        private Rect windowRect = new Rect(20, 20, 300, 600);
        // 是否显示窗口
        private bool showWindow = false;
        // 窗口ID
        private int windowID = 1000;
        // 插件日志
        internal static ManualLogSource Log;
        // 单例实例
        public static TaskCheat Instance { get; private set; }
        // UI缩放因子
        private float scaleFactor = 1f;
        
        // 任务选择窗口相关
        private bool showTaskSelectionWindow = false;
        private Rect taskSelectionWindowRect = new Rect(100, 100, 700, 600);
        private int taskSelectionWindowID = 2000;
        private Vector2 taskScrollPosition = Vector2.zero;
        private List<bool> taskSelectionStates = new List<bool>();
        private List<List<string>> selectedTasks = new List<List<string>>();
        
        // 任务添加窗口相关
        private bool showAddTaskWindow = false;
        private Rect addTaskWindowRect = new Rect(100, 100, 700, 600);
        private int addTaskWindowID = 3000;
        private Vector2 addTaskScrollPosition = Vector2.zero;
        private List<bool> addTaskSelectionStates = new List<bool>();
        
        // 确认对话框相关
        private bool showConfirmClearAllDialog = false;
        private Rect confirmDialogRect = new Rect(200, 200, 300, 300);
        private int confirmDialogID = 4000;
        
        // 使用BepInEx配置系统管理任务检查间隔
        public static ConfigEntry<int> TaskCheckInterval { get; private set; }

        private void Awake()
        {
            // 保存实例引用
            Instance = this;
            
            Log = Logger;
            TaskCheat.Log?.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
            // 使用BepInEx配置系统初始化任务检查间隔
            TaskCheckInterval = Config.Bind<int>(LanguageManager.GetText("重复任务设置"), LanguageManager.GetText("任务检查间隔（秒）"), 10, LanguageManager.GetText("设置重复任务的检查时间间隔，范围：1-10秒"));
            
            // 初始化Harmony补丁
            Harmony.CreateAndPatchAll(typeof(TaskCheat));
            
        }
        
        /// <summary>
        /// 加载重复任务配置
        /// </summary>
        public void LoadRepetitiveTaskConfig()
        {
            try
            {
                // 获取当前的存储标识
                string storageIdentifier = TaskCheatConfig.GetStorageIdentifier();
                
                // 检查存储标识是否有效且不为默认值
                if (string.IsNullOrEmpty(storageIdentifier) || storageIdentifier == TaskCheatConfig.DefaultStorageIdentifier)
                {
                    TaskCheat.Log?.LogWarning("存储标识无效或为默认值，不加载重复任务配置");
                    // 清空待添加任务列表，防止使用旧的任务配置
                    RepetitiveTaskHandler.ToBeAdded.Clear();
                    return;
                }
                
                // 从配置文件加载选中的任务索引
                List<int> selectedTaskIndices = TaskCheatConfig.LoadRepetitiveTaskSelection();
                
                if (selectedTaskIndices.Count > 0)
                {
                    // 转换为RepetitiveTaskHandler需要的格式
                    List<List<string>> tasksToAdd = new List<List<string>>();
                    foreach (int index in selectedTaskIndices)
                    {
                        tasksToAdd.Add(new List<string> { index.ToString(), "0" });
                    }
                    
                    // 更新ToBeAdded
                    RepetitiveTaskHandler.AddSelectedTasks(tasksToAdd);
                    TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("已根据配置更新待重复任务列表，共加载{0}个任务", selectedTaskIndices.Count));

                    // 直接调用TaskAdd方法立即添加任务
                    RepetitiveTaskHandler.TaskAdd();
                }
                else
                {
                    TaskCheat.Log?.LogInfo("未加载到重复任务配置，ToBeAdded列表为空");
                    // 确保待添加任务列表为空
                    RepetitiveTaskHandler.ToBeAdded.Clear();
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError("加载重复任务配置时出错: " + ex.Message);
                TaskCheat.Log?.LogError(ex.StackTrace);
            }

            
        }

        private void Update()
        {
            // 按F4键开关窗口
            if (Input.GetKeyDown(KeyCode.F4))
            {
                showWindow = !showWindow;
                // 关闭所有子窗口
                showTaskSelectionWindow = false;
                showAddTaskWindow = false;
            }
            
            // 调用重复任务添加方法，根据时间间隔自动添加任务
            RepetitiveTaskHandler.RepetitiveTaskAdd();
        }

        private void OnGUI()
        {
            if (showWindow)
            {
                // 创建无边框窗口
                windowRect = GUI.Window(windowID, windowRect, DrawWindow, "", GUI.skin.window);
            }
            
            if (showTaskSelectionWindow)
            {
                // 创建任务选择窗口
                taskSelectionWindowRect = GUI.Window(taskSelectionWindowID, taskSelectionWindowRect, DrawTaskSelectionWindow, "", GUI.skin.window);
            }
            
            if (showAddTaskWindow)
            {
                // 创建任务添加窗口
                addTaskWindowRect = GUI.Window(addTaskWindowID, addTaskWindowRect, DrawAddTaskWindow, "", GUI.skin.window);
            }
            
            if (showConfirmClearAllDialog)
            {
                // 创建确认对话框
                confirmDialogRect = GUI.Window(confirmDialogID, confirmDialogRect, DrawConfirmClearAllDialog, "", GUI.skin.window);
            }
        }


        //窗口UI设计
        private void DrawWindow(int windowID)
        {
            // 标题和关闭按钮行
            GUILayout.BeginHorizontal();
            // 占位符，确保关闭按钮在右侧
            GUILayout.FlexibleSpace();
            // 关闭按钮
            GUIStyle closeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 30,
                fixedHeight = 30,
                alignment = TextAnchor.MiddleCenter
            };
            if (GUILayout.Button(LanguageManager.GetText("关闭"), closeButtonStyle))
            {
                showWindow = false;
            }
            GUILayout.EndHorizontal();
            
            // 任务编辑器标题
            GUILayout.BeginVertical();
            GUILayout.Space(5);
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(LanguageManager.GetText("任务编辑器"), titleStyle);
            GUILayout.Space(15);

            // 分类标题
            GUIStyle categoryStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold
            };

            // 一键清除分类
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(LanguageManager.GetText("一键清除"), categoryStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            if (GUILayout.Button(LanguageManager.GetText("清除所有任务"), GUILayout.Width(200), GUILayout.Height(30)))
            {
                ClearAllTasks();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(15);

            // 任务添加分类
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(LanguageManager.GetText("任务添加"), categoryStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            if (GUILayout.Button(LanguageManager.GetText("选择需要添加的任务"), GUILayout.Width(200), GUILayout.Height(30)))
            {
                OpenAddTaskWindow();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(15);

            // 重复任务分类
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label(LanguageManager.GetText("重复任务"), categoryStyle);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            if (GUILayout.Button(LanguageManager.GetText("选择需要重复的任务"), GUILayout.Width(200), GUILayout.Height(30)))
            {
                OpenTaskSelectionWindow();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(40);
            if (GUILayout.Button(LanguageManager.GetText("清除配置文件"), GUILayout.Width(200), GUILayout.Height(30)))
            {
                TryClearConfig();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            // 使用说明
            GUILayout.Space(20f * scaleFactor);
            GUILayout.BeginVertical();
            
            // 使用说明标题
            GUILayout.Label(LanguageManager.GetText("使用说明:"), UnityEngine.GUI.skin.box);
            
            // 使用说明
            GUILayout.Label(LanguageManager.GetText("1. 请在修改前先保存游戏，以便回档"));
            GUILayout.Label(LanguageManager.GetText("2. 按F4键显示/隐藏窗口"));
            GUILayout.Label(LanguageManager.GetText("3. 切换模式选择：一键清除/任务添加/重复任务"));
            GUILayout.Label("");
            
            // MOD作者及版本号说明
            GUILayout.Label(LanguageManager.GetText("Mod作者：AnZhi20"));
            GUILayout.Label(LanguageManager.GetFormattedText("Mod版本：{0}", PluginInfo.PLUGIN_VERSION));
            GUILayout.EndVertical();
            
            // 允许拖动窗口
            GUI.DragWindow(new Rect(0, 0, windowRect.width, windowRect.height));
        }

        //清除所有任务
        private void ClearAllTasks()
        {
            try
            {
                TaskClearer.AllTaskClear();
                TaskCheat.Log?.LogInfo(LanguageManager.GetText("所有任务已清除!"));
                // 显示成功提示
                ShowNotification(LanguageManager.GetText("任务已清除成功!"));
            }
            catch (System.Exception ex)
            {
                TaskCheat.Log?.LogError("清除任务时出错: " + ex.Message);
                ShowNotification(LanguageManager.GetText("清除失败: ") + ex.Message);
            }
        }

        // 显示提示信息到游戏界面
        public void ShowNotification(string message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message))
                {
                    Mainload.Tip_Show.Add(new List<string>
                    {
                        "1",
                        message
                    });
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("显示提示信息时出错: {0}", ex.Message));
            }
        }
        
        // 打开任务选择窗口
        private void OpenTaskSelectionWindow()
        {
            try
            {
                // 重置选择状态
                selectedTasks.Clear();
                taskSelectionStates.Clear();
                
                // 根据Text_AllTaskOrder的数量初始化选择状态列表
                if (TaskList.Text_AllTaskOrder != null)
                {
                    for (int i = 0; i < TaskList.Text_AllTaskOrder.Count; i++)
                    {
                        taskSelectionStates.Add(false);
                    }
                    
                    // 加载配置文件中的选择状态
                    List<int> savedSelections = TaskCheatConfig.LoadRepetitiveTaskSelection();
                    foreach (int index in savedSelections)
                    {
                        if (index >= 0 && index < taskSelectionStates.Count)
                        {
                            taskSelectionStates[index] = true;
                        }
                    }
                }
                
                // 显示任务选择窗口
                showTaskSelectionWindow = true;
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError("打开任务选择窗口时出错: " + ex.Message);
                ShowNotification(LanguageManager.GetText("打开任务选择窗口失败!"));
            }
        }
        
        // 绘制任务选择窗口
        private void DrawTaskSelectionWindow(int windowID)
        {
            GUILayout.BeginVertical();
            
            // 标题和关闭按钮行
            // 第一行：关闭按钮在右上
            GUILayout.BeginHorizontal();
            // 占位符，确保关闭按钮在右侧
            GUILayout.FlexibleSpace();
            // 关闭按钮
            GUIStyle closeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 30,
                fixedHeight = 30,
                alignment = TextAnchor.MiddleCenter
            };
            if (GUILayout.Button(LanguageManager.GetText("关闭"), closeButtonStyle))
            {
                showTaskSelectionWindow = false;
            }
            GUILayout.EndHorizontal();
            
            // 第二行：居中显示标题
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // 窗口标题
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(LanguageManager.GetText("选择要重复添加的任务"), titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            
            // 任务列表滚动视图
            taskScrollPosition = GUILayout.BeginScrollView(taskScrollPosition, GUILayout.Height(400));
            
            if (TaskList.Text_AllTaskOrder != null)
            {
                for (int i = 0; i < TaskList.Text_AllTaskOrder.Count; i++)
                {
                    if (i < taskSelectionStates.Count)
                    {
                        // 获取当前语言的任务文本
                            string taskText = LanguageManager.GetTaskText(TaskList.Text_AllTaskOrder[i]);
                            // 提取以|分隔的前半部分
                            string baseDisplayText = taskText.Split('|')[0];

                        // 初始化显示文本，默认使用原始文本
                        string displayText = baseDisplayText;
                        
                        // 尝试从AllTaskOrderData中获取数据并格式化显示文本
                        try
                        {
                            if (TaskList.AllTaskOrderData != null && i < TaskList.AllTaskOrderData.Count)
                            {
                                var taskData = TaskList.AllTaskOrderData[i];
                                if (taskData != null && taskData.Count >= 3)
                                {
                                    // 获取任务所需次数
                                    string times = taskData[1];
                                    // 获取声望值
                                    string reputation = taskData[0];
                                    // 解析铜钱和元宝值
                                    string[] rewardData = taskData[2].Split('|');
                                    string money = rewardData.Length > 0 ? rewardData[0] : "0";
                                    string gold = rewardData.Length > 1 ? rewardData[1] : "0";
                                    
                                    // 替换^符号为实际次数
                                    displayText = baseDisplayText.Replace("^", times);
                                    // 添加奖励信息
                                    displayText += LanguageManager.GetFormattedText("，任务奖励：声望+{0}、铜钱+{1}、元宝+{2}", reputation, money, gold);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("格式化任务显示文本时出错: {0}", ex.Message));
                        }
                        
                        // 绘制多选框，使文本居中显示
                        GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        taskSelectionStates[i] = GUILayout.Toggle(taskSelectionStates[i], displayText, GUILayout.Width(600));
                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    }
                }
            }
            
            GUILayout.EndScrollView();
            
            GUILayout.Space(20);
            
            // 确认选择按钮
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(LanguageManager.GetText("确认选择"), GUILayout.Width(150), GUILayout.Height(30)))
            {
                ConfirmTaskSelection();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            // 允许拖动窗口
            GUI.DragWindow(new Rect(0, 0, taskSelectionWindowRect.width, taskSelectionWindowRect.height));
        }
        
        /// <summary>
        /// 尝试清除配置文件
        /// </summary>
        private void TryClearConfig()
        {
            try
            {
                string storageIdentifier = TaskCheatConfig.GetStorageIdentifier();
                
                // 增强检查：确保存储标识不为空或无效
                if (string.IsNullOrEmpty(storageIdentifier))
                {
                    TaskCheat.Log?.LogWarning(LanguageManager.GetText("存储标识为空，显示确认清除所有配置对话框"));
                    showConfirmClearAllDialog = true;
                    return;
                }
                
                // 尝试加载配置，检查存储标识是否有效
                List<int> selectedTasks = TaskCheatConfig.LoadRepetitiveTaskSelection();
                
                // 增强检查：如果selectedTasks为null或为空列表，都认为存储标识无效
                if (selectedTasks != null && selectedTasks.Count > 0)
                {
                    // 存储标识有效，只清除当前存储标识的配置
                    bool success = TaskCheatConfig.ClearConfigByStorageIdentifier(storageIdentifier);
                    if (success)
                    {
                        // 清除配置文件后，立即清空内存中的待添加任务列表
                        RepetitiveTaskHandler.ToBeAdded.Clear();
                        TaskCheat.Log?.LogInfo(LanguageManager.GetText("已清空内存中的待添加任务列表"));
                        ShowNotification(LanguageManager.GetText("配置文件已清除成功！"));
                    }
                    else
                    {
                        ShowNotification(LanguageManager.GetText("配置文件清除失败！"));
                    }
                }
                else
                {
                    // 存储标识无效，显示确认清除所有配置的对话框
                    TaskCheat.Log?.LogWarning(LanguageManager.GetText("存储标识无效或对应配置不存在，显示确认清除所有配置对话框"));
                    showConfirmClearAllDialog = true;
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("尝试清除配置文件时出错: {0}", ex.Message));
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("异常堆栈: {0}", ex.StackTrace));
                ShowNotification(LanguageManager.GetFormattedText("操作失败: {0}", ex.Message));
                // 出错时也显示确认对话框，让用户选择是否清除所有配置
                showConfirmClearAllDialog = true;
            }
        }
        
        /// <summary>
        /// 绘制确认清除所有配置的对话框
        /// </summary>
        /// <param name="windowID">窗口ID</param>
        private void DrawConfirmClearAllDialog(int windowID)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(20);
            
            // 标题样式
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            
            // 内容样式
            GUIStyle contentStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
            
            GUILayout.Label(LanguageManager.GetText("确认操作"), titleStyle);
            GUILayout.Space(10);
            GUILayout.Label(LanguageManager.GetText("未找到本存档的配置，可能未生成或已删除。"), contentStyle, GUILayout.Height(40));
            GUILayout.Label(LanguageManager.GetText("是否清除所有配置？"), contentStyle, GUILayout.Height(40));
            
            GUILayout.Space(10);
            
            // 按钮区域
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(LanguageManager.GetText("是"), GUILayout.Width(80), GUILayout.Height(30)))
                {
                    // 清除所有配置
                    bool success = TaskCheatConfig.ClearAllConfig();
                    if (success)
                    {
                        // 清除所有配置后，立即清空内存中的待添加任务列表
                        RepetitiveTaskHandler.ToBeAdded.Clear();
                        TaskCheat.Log?.LogInfo(LanguageManager.GetText("已清空内存中的待添加任务列表"));
                        ShowNotification(LanguageManager.GetText("所有配置已清除成功！"));
                    }
                    else
                    {
                        ShowNotification(LanguageManager.GetText("清除所有配置失败！"));
                    }
                    showConfirmClearAllDialog = false;
                }
                
                GUILayout.Space(20);
                
                if (GUILayout.Button(LanguageManager.GetText("否"), GUILayout.Width(80), GUILayout.Height(30)))
                {
                    // 取消操作
                    showConfirmClearAllDialog = false;
                }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            // 允许拖动窗口
            GUI.DragWindow(new Rect(0, 0, confirmDialogRect.width, confirmDialogRect.height));
        }
        
        // 确认任务选择
        private void ConfirmTaskSelection()
        {
            try
            {
                // 收集选中的任务
                List<List<string>> tasksToAdd = new List<List<string>>();
                List<int> selectedIndices = new List<int>();
                
                if (TaskList.Text_AllTaskOrder != null && taskSelectionStates.Count > 0)
                {
                    for (int i = 0; i < taskSelectionStates.Count && i < TaskList.Text_AllTaskOrder.Count; i++)
                    {
                        if (taskSelectionStates[i])
                        {
                            // 添加对应的任务数据（如 {0, 0}）
                            tasksToAdd.Add(new List<string> { i.ToString(), "0" });
                            selectedIndices.Add(i);
                        }
                    }
                }
                
                // 将选中的任务添加到RepetitiveTask的ToBeAdded中
                if (tasksToAdd.Count > 0)
                {
                    RepetitiveTaskHandler.AddSelectedTasks(tasksToAdd);
                    ShowNotification(LanguageManager.GetFormattedText("成功选择了 {0} 个任务！", tasksToAdd.Count));
                }
                else
                {
                    ShowNotification(LanguageManager.GetText("未选择任何任务！"));
                }
                
                // 保存选择状态到配置文件
                TaskCheatConfig.SaveRepetitiveTaskSelection(selectedIndices);
                
                // 关闭任务选择窗口
                showTaskSelectionWindow = false;
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("确认任务选择时出错: {0}", ex.Message));
                ShowNotification(LanguageManager.GetText("确认任务选择失败!"));
            }
        }

        // 打开任务添加窗口
        private void OpenAddTaskWindow()
        {
            try
            {
                // 重置选择状态
                addTaskSelectionStates.Clear();
                
                // 根据Text_AllTaskOrder的数量初始化选择状态列表
                if (TaskList.Text_AllTaskOrder != null)
                {
                    for (int i = 0; i < TaskList.Text_AllTaskOrder.Count; i++)
                    {
                        addTaskSelectionStates.Add(false);
                    }
                }
                
                // 显示任务添加窗口
                showAddTaskWindow = true;
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("打开任务添加窗口时出错: {0}", ex.Message));
                ShowNotification(LanguageManager.GetText("打开任务添加窗口失败!"));
            }
        }
        
        // 绘制任务添加窗口
        private void DrawAddTaskWindow(int windowID)
        {
            GUILayout.BeginVertical();

            // 标题和关闭按钮行
            // 第一行：关闭按钮在右上
            GUILayout.BeginHorizontal();
            // 占位符，确保关闭按钮在右侧
            GUILayout.FlexibleSpace();
            // 关闭按钮
            GUIStyle closeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 30,
                fixedHeight = 30,
                alignment = TextAnchor.MiddleCenter
            };
            if (GUILayout.Button(LanguageManager.GetText("关闭"), closeButtonStyle))
            {
                showAddTaskWindow = false;
            }
            GUILayout.EndHorizontal();
            
            // 第二行：居中显示标题
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // 窗口标题
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label(LanguageManager.GetText("选择要添加的任务"), titleStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            
            // 任务列表滚动视图
            addTaskScrollPosition = GUILayout.BeginScrollView(addTaskScrollPosition, GUILayout.Height(400));
            
            if (TaskList.Text_AllTaskOrder != null)
            {
                for (int i = 0; i < TaskList.Text_AllTaskOrder.Count; i++)
                {
                    if (i < addTaskSelectionStates.Count)
                    {
                        // 获取当前语言的任务文本
                            string taskText = LanguageManager.GetTaskText(TaskList.Text_AllTaskOrder[i]);
                            // 提取以|分隔的前半部分
                            string baseDisplayText = taskText.Split('|')[0];

                        // 初始化显示文本，默认使用原始文本
                        string displayText = baseDisplayText;
                        
                        // 尝试从AllTaskOrderData中获取数据并格式化显示文本
                        try
                        {
                            if (TaskList.AllTaskOrderData != null && i < TaskList.AllTaskOrderData.Count)
                            {
                                var taskData = TaskList.AllTaskOrderData[i];
                                if (taskData != null && taskData.Count >= 3)
                                {
                                    // 获取任务所需次数
                                    string times = taskData[1];
                                    // 获取声望值
                                    string reputation = taskData[0];
                                    // 解析铜钱和元宝值
                                    string[] rewardData = taskData[2].Split('|');
                                    string money = rewardData.Length > 0 ? rewardData[0] : "0";
                                    string gold = rewardData.Length > 1 ? rewardData[1] : "0";
                                    
                                    // 替换^符号为实际次数
                                    displayText = baseDisplayText.Replace("^", times);
                                    // 添加奖励信息
                                    displayText += $"，任务奖励：声望+{reputation}、铜钱+{money}、元宝+{gold}";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("格式化任务显示文本时出错: {0}", ex.Message));
                        }
                        
                        // 绘制多选框
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        addTaskSelectionStates[i] = GUILayout.Toggle(addTaskSelectionStates[i], displayText, GUILayout.Width(600));
                        GUILayout.EndHorizontal();
                        GUILayout.Space(5);
                    }
                }
            }
            
            GUILayout.EndScrollView();
            
            GUILayout.Space(20);
            
            // 确认选择按钮
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(LanguageManager.GetText("确认添加"), GUILayout.Width(150), GUILayout.Height(30)))
            {
                ConfirmAddTaskSelection();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            // 允许拖动窗口
            GUI.DragWindow(new Rect(0, 0, addTaskWindowRect.width, addTaskWindowRect.height));
        }
        
        // 确认任务添加选择
        private void ConfirmAddTaskSelection()
        {
            try
            {
                // 收集选中的任务
                List<List<string>> tasksToAdd = new List<List<string>>();
                
                if (TaskList.Text_AllTaskOrder != null && addTaskSelectionStates.Count > 0)
                {
                    for (int i = 0; i < addTaskSelectionStates.Count && i < TaskList.Text_AllTaskOrder.Count; i++)
                    {
                        if (addTaskSelectionStates[i])
                        {
                            // 添加对应的任务数据（如 {0, 0}）
                            tasksToAdd.Add(new List<string> { i.ToString(), "0" });
                        }
                    }
                }
                
                // 调用AddTasks类中的方法添加任务到当前任务列表
                if (tasksToAdd.Count > 0)
                {
                    AddTaskHandler.AddTasksToCurrent(tasksToAdd);
                }
                
                // 关闭任务添加窗口
                showAddTaskWindow = false;
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("确认任务添加时出错: {0}", ex.Message));
                ShowNotification(LanguageManager.GetText("确认任务添加失败!"));
            }
        }
    }
    
    // SaveData.ReadGameData方法的补丁
    [HarmonyPatch(typeof(SaveData))]
    [HarmonyPatch("ReadGameData")]
    public static class SaveDataReadGameDataPatch
    {
        private static void Postfix()
        {
            try
            {
                // 获取当前存档位置（不再作为存储标识使用）
                string archiveLocation = Mainload.CunDangIndex_now;
                
                if (TaskCheat.Instance != null)
                {
                    // 验证存档位置是否有效
                    if (string.IsNullOrEmpty(archiveLocation))
                    {
                        TaskCheat.Log?.LogWarning(LanguageManager.GetText("存档位置为空，无法加载特定存档的配置文件"));
                        return;
                    }
                    
                    TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("读取存档数据后，根据存档位置 {0} 开始查找匹配的配置文件", archiveLocation));
                    
                    // 清空待添加任务列表，准备根据配置重新添加
                    RepetitiveTaskHandler.ToBeAdded = new List<List<int>>();
                    
                    // 遍历查找所有可能的存储标识，寻找与当前存档匹配的配置
                    bool foundMatchingConfig = false;

                    // 获取当前完整存储标识（包含郡县、家族等信息，但与存档位置明确区分）
                    string storageIdentifier = TaskCheatConfig.GetStorageIdentifier();
                    
                    // 记录当前存储标识信息
                    TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("当前存储标识: '{0}'", storageIdentifier));
                    
                    // 如果存储标识不为空且不为默认值，则检查是否包含当前存档位置信息
                    if (!string.IsNullOrEmpty(storageIdentifier) && 
                        storageIdentifier != TaskCheatConfig.DefaultStorageIdentifier &&
                        storageIdentifier.Contains(archiveLocation))
                    {
                        TaskCheat.Log?.LogInfo(LanguageManager.GetText("找到与当前存档位置匹配的存储标识，尝试加载配置"));
                        // 尝试加载配置
                        try
                        {
                            List<int> selectedTaskIndices = TaskCheatConfig.LoadRepetitiveTaskSelection();
                            
                            if (selectedTaskIndices.Count > 0)
                            {
                                // 将索引转换为RepetitiveTaskHandler.ToBeAdded所需的List<List<int>>格式
                                List<List<string>> tasksToAdd = new List<List<string>>();
                                foreach (int index in selectedTaskIndices)
                                {
                                    tasksToAdd.Add(new List<string> { index.ToString() });
                                }
                                
                                // 添加到待添加列表
                                RepetitiveTaskHandler.AddSelectedTasks(tasksToAdd);
                                foundMatchingConfig = true;
                                TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("成功加载匹配的配置文件，添加了 {0} 个任务", RepetitiveTaskHandler.ToBeAdded.Count));
                                TaskCheat.Instance?.ShowNotification(LanguageManager.GetFormattedText("已加载 {0} 个重复任务配置", RepetitiveTaskHandler.ToBeAdded.Count));
                            }
                            else
                            {
                                TaskCheat.Log?.LogInfo(LanguageManager.GetText("存储标识存在，但没有找到选中的任务配置"));
                            }
                        }
                        catch (Exception ex)
                        {
                            // 如果格式错误，则删除该标识的配置
                            TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("配置格式错误，删除存储标识 '{0}' 的配置: {1}", storageIdentifier, ex.Message));
                            TaskCheat.Log?.LogError(ex.StackTrace);
                            bool cleared = TaskCheatConfig.ClearConfigByStorageIdentifier(storageIdentifier);
                            if (cleared)
                            {
                                TaskCheat.Log?.LogInfo(LanguageManager.GetText("配置已成功清除，下次启动将使用默认设置"));
                                TaskCheat.Instance?.ShowNotification(LanguageManager.GetText("配置格式错误，已清除，请重新配置任务"));
                            }
                        }
                    }
                    else
                    {
                        // 记录为什么没有匹配的原因
                        if (string.IsNullOrEmpty(storageIdentifier))
                        {
                            TaskCheat.Log?.LogWarning(LanguageManager.GetText("存储标识为空"));
                        }
                        else if (storageIdentifier == TaskCheatConfig.DefaultStorageIdentifier)
                        {
                            TaskCheat.Log?.LogWarning(LanguageManager.GetText("当前使用默认存储标识，不会加载特定存档的配置"));
                        }
                        else if (!storageIdentifier.Contains(archiveLocation))
                        {
                            TaskCheat.Log?.LogWarning(LanguageManager.GetFormattedText("存储标识 '{0}' 不包含当前存档位置 '{1}'", storageIdentifier, archiveLocation));
                        }
                    }
                    
                    // 如果没有找到匹配的配置
                    if (!foundMatchingConfig)
                    {
                        // 返回默认空数组给tobeadded（已在开头清空）
                        TaskCheat.Log?.LogInfo(LanguageManager.GetText("未找到匹配的存储标识配置，ToBeAdded保持为空"));
                        // 显示通知给用户
                        TaskCheat.Instance?.ShowNotification("未找到匹配的任务配置，请在任务编辑器中设置");
                    }
                }
                else
                {
                    TaskCheat.Log?.LogError(LanguageManager.GetText("TaskCheat.Instance为null，无法加载配置"));
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetFormattedText("在ReadGameData后加载配置时出错: {0}", ex.Message));
                TaskCheat.Log?.LogError(ex.StackTrace);
                
                // 出错时确保ToBeAdded为空
                RepetitiveTaskHandler.ToBeAdded = new List<List<int>>();
                
                // 显示错误通知
                if (TaskCheat.Instance != null)
                {
                    TaskCheat.Instance?.ShowNotification(LanguageManager.GetText("加载任务配置时出错，请查看日志了解详情"));
                }
            }
        }
    }
}
