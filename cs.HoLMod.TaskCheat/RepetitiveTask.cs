using System;
using System.Collections.Generic;

namespace cs.HoLMod.TaskCheat
{
    /// <summary>
    /// 重复任务处理类
    /// </summary>
    public static class RepetitiveTaskHandler
    {
        /// <summary>
        /// 待重复添加的任务列表
        /// </summary>
        public static List<List<int>> ToBeAdded = new List<List<int>>();

        /// <summary>
        /// 添加选中的任务到待添加列表
        /// </summary>
        /// <param name="selectedTasks">选中的任务列表</param>
        public static void AddSelectedTasks(List<List<string>> selectedTasks)
        {
            try
            {
                // 参数校验
                if (selectedTasks == null)
                {
                    TaskCheat.Log?.LogWarning(LanguageManager.GetText("TaskListIsNull"));
                    return;
                }
                
                // 清空现有任务
                ToBeAdded.Clear();
                
                // 添加选中的任务，将string类型转换为int类型
                if (selectedTasks.Count > 0)
                {
                    int successfullyAdded = 0;
                    int failedConversions = 0;
                    
                    foreach (var task in selectedTasks)
                    {
                        if (task == null || task.Count == 0)
                        {
                            failedConversions++;
                            continue;
                        }
                        
                        List<int> intTask = new List<int>();
                        bool taskValid = false;
                        
                        foreach (var item in task)
                        {
                            if (int.TryParse(item, out int result))
                            {
                                intTask.Add(result);
                                taskValid = true;
                            }
                            else if (!string.IsNullOrEmpty(item))
                            {
                                // 记录转换失败的项
                                TaskCheat.Log?.LogWarning(LanguageManager.GetFormattedText("CannotConvertStringToInt", item));
                            }
                        }
                        
                        if (taskValid)
                        {
                            ToBeAdded.Add(intTask);
                            successfullyAdded++;
                        }
                        else
                        {
                            failedConversions++;
                        }
                    }
                    
                    // 记录添加结果
                    if (successfullyAdded > 0)
                    {
                        TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("SuccessfullyAddedToBeAddedList", successfullyAdded));
                    }
                    
                    if (failedConversions > 0)
                    {
                        TaskCheat.Log?.LogWarning(LanguageManager.GetFormattedText("TaskConversionFailed", failedConversions));
                    }
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError(LanguageManager.GetText("AddSelectedTasksError") + ex.Message);
                TaskCheat.Log?.LogError(ex.StackTrace);
            }
        }
        
        // 保存上一次添加任务的现实时间
        private static DateTime lastAddTime = DateTime.MinValue;
        
        // 保存检查间隔（从配置文件加载）
        private static int checkIntervalSeconds = -1;
        
        /// <summary>
        /// 获取当前各国默认时间（如果获取失败则使用北京时间）
        /// </summary>
        /// <returns>当前时间</returns>
        private static DateTime GetCurrentDateTime()
        {
            try
            {
                // 尝试获取系统默认时区的当前时间
                return DateTime.Now;
            }
            catch
            {
                try
                {
                    // 如果失败，尝试获取北京时间 (UTC+8)
                    TimeZoneInfo chinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
                    return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, chinaTimeZone);
                }
                catch
                {
                    // 如果仍然失败，使用UTC时间加上8小时作为北京时间
                    return DateTime.UtcNow.AddHours(8);
                }
            }
        }

        /// <summary>
        /// 重复添加任务方法 - 根据配置文件中的间隔执行
        /// </summary>
        public static void RepetitiveTaskAdd()
        {
            // 获取当前时间（使用各国默认时间或北京时间）
            DateTime currentTime = GetCurrentDateTime();

            // 距离上次添加已超过配置的间隔时间时执行添加
            if ((currentTime - lastAddTime).TotalSeconds >= checkIntervalSeconds && checkIntervalSeconds > 0)
            {
                // 每次执行添加前先清空ToBeAdded列表
                if (ToBeAdded != null)
                {
                    ToBeAdded.Clear();
                    TaskCheat.Log?.LogInfo(LanguageManager.GetText("ClearedToBeAddedListBeforeRepetitiveAdd"));
                }
                
                // 在时间间隔判断后重新加载配置
                checkIntervalSeconds = TaskCheatConfig.LoadCheckInterval();
                
                // 重新加载任务配置
                ReloadTaskConfig();
                
                // 执行任务添加
                TaskAdd();
                lastAddTime = currentTime;
            }
            else if (checkIntervalSeconds == -1)
            {
                // 首次调用时初始化
                checkIntervalSeconds = TaskCheatConfig.LoadCheckInterval();
                lastAddTime = currentTime;
            }
        }

        /// <summary>
        /// 重新加载任务配置
        /// </summary>
        private static void ReloadTaskConfig()
        {
            try
            {
                // 检查TaskCheat实例是否存在以及Mainload.SceneID是否为空
                if (TaskCheat.Instance != null && !string.IsNullOrEmpty(Mainload.SceneID))
                {
                    // 加载当前场景并用|分割
                    string[] arrayrepetitive = Mainload.SceneID.Split(new char[] { '|' });

                    // 读取场景类型
                    if (arrayrepetitive.Length > 0)
                    {
                        string SceneClass = arrayrepetitive[0];
                        
                        // 根据场景类型重新从配置文件读取任务
                        if (SceneClass == "M" || SceneClass == "Z" || SceneClass == "S" || SceneClass == "H" || SceneClass == "F")
                        {
                            try
                            {
                                TaskCheat.Log?.LogInfo(LanguageManager.GetText("开始重新从配置文件读取任务"));
                                List<int> selectedTaskIndices = TaskCheatConfig.LoadRepetitiveTaskSelection();

                                if (selectedTaskIndices.Count > 0)
                                {
                                    // 转换为所需格式并添加到ToBeAdded列表
                                    foreach (int index in selectedTaskIndices)
                                    {
                                        List<int> intTask = new List<int> { index, 0 };
                                        ToBeAdded.Add(intTask);
                                    }

                                    TaskCheat.Log?.LogInfo(LanguageManager.GetFormattedText("ReloadedTasksToBeAddedList", ToBeAdded.Count));
                                }
                                else
                                {
                                    TaskCheat.Log?.LogInfo(LanguageManager.GetText("NoTasksFoundInConfig"));
                                }
                            }
                            catch (Exception ex)
                            {
                                TaskCheat.Log?.LogError(LanguageManager.GetText("ReloadTasksFromConfigError") + ex.Message);
                                TaskCheat.Log?.LogError(ex.StackTrace);
                            }
                        }
                        
                        // 清空临时变量
                        SceneClass = null;
                    }
                    
                    // 清空数组
                    arrayrepetitive = null;
                }
            }
            catch (Exception ex)
            {
                TaskCheat.Log?.LogError("重新加载任务配置时出错: " + ex.Message);
                TaskCheat.Log?.LogError(ex.StackTrace);
            }
        }

        /// <summary>
        /// 添加待添加列表中的任务
        /// </summary>
        public static void TaskAdd()
        {
            try
            {
                // 检查是否有待添加的任务
                if (ToBeAdded != null && ToBeAdded.Count > 0)
                {
                    // 将List<List<int>>转换为List<List<string>>
                    List<List<string>> stringTasks = new List<List<string>>();
                    foreach (var intTask in ToBeAdded)
                    {
                        List<string> stringTask = new List<string>();
                        foreach (var item in intTask)
                        {
                            stringTask.Add(item.ToString());
                        }
                        stringTasks.Add(stringTask);
                    }

                    // 加载当前场景并用|分割
                    string[] arraycheck = null;
                    string SceneClass = null;
                     
                    // 安全地获取场景信息
                    if (!string.IsNullOrEmpty(Mainload.SceneID))
                    {
                        arraycheck = Mainload.SceneID.Split(new char[] { '|' });
                        if (arraycheck.Length > 0)
                        {
                            SceneClass = arraycheck[0];
                        }
                    }

                    // 使用AddTasks.cs中的添加逻辑
                    if (TaskCheat.Instance != null && !string.IsNullOrEmpty(SceneClass))
                    {
                        AddTaskHandler.AddTasksToCurrent(stringTasks);
                        TaskCheat.Log?.LogInfo(LanguageManager.GetText("SuccessfullyExecutedTaskAdd"));
                    }
                    else
                    {
                        TaskCheat.Log?.LogWarning(LanguageManager.GetText("CannotAddTaskInstanceOrSceneInvalid"));
                    }
                }
                else
                {
                    TaskCheat.Log?.LogInfo(LanguageManager.GetText("ToBeAddedListEmpty"));
                }
            }
            catch (Exception ex)
            {
                // 记录异常信息
                TaskCheat.Log?.LogError(LanguageManager.GetText("TaskAddMethodException") + ex.Message);
                TaskCheat.Log?.LogError(ex.StackTrace);
            }
            finally
            {
                // 每次重复添加执行结束后清空加载后的配置
                if (ToBeAdded != null)
                {
                    ToBeAdded.Clear();
                    TaskCheat.Log?.LogInfo(LanguageManager.GetText("ClearedToBeAddedListAfterRepetitiveAdd"));
                }
                
                // 清空临时变量
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
