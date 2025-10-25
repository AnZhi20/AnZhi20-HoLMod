using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace cs.HoLMod.FengDiEditor
{
    // 插件信息类
    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "cs.HoLMod.FengDiEditor.AnZhi20";
        public const string PLUGIN_NAME = "HoLMod.FengDiEditor";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PLUGIN_CONFIG = "cs.HoLMod.FengDiEditor.AnZhi20.cfg";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class FengDiEditor : BaseUnityPlugin
    {
        private FengDiEditorWindow editorWindow;
        private Harmony harmony;

        private void Awake()
        {
            // 初始化语言管理器
            LanguageManager.Instance.SetLanguage("zh-CN"); // 默认使用中文

            // 初始化编辑器窗口
            editorWindow = new FengDiEditorWindow();
            editorWindow.OnSubCategoryButtonClicked += HandleSubCategoryButtonClick;
            editorWindow.OnModifyConfirmed += HandleModifyConfirmed;

            // 初始化Harmony补丁
            harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Logger.LogInfo("HoLMod.FengDiEditor 已加载，按F5显示/隐藏窗口");
        }

        private void OnGUI()
        {    
            // 处理F5键按下事件
            if (Input.GetKeyDown(KeyCode.F5))
            {    
                editorWindow.ToggleWindowVisibility();
                Logger.LogInfo($"窗口状态: {(editorWindow.IsWindowVisible() ? "显示" : "隐藏")}");
            }

            // 绘制窗口
            editorWindow.OnGUI();
        }

        private void HandleSubCategoryButtonClick(string category, string subCategory)
        {    
            // 这里实现具体的逻辑处理
            // 暂时只记录日志
            string categoryName = LanguageManager.Instance.GetTranslation(category);
            string subCategoryName = LanguageManager.Instance.GetTranslation(subCategory);
            
            Logger.LogInfo($"执行操作: {categoryName} - {subCategoryName}");
            
            // 根据不同的分类和子分类执行不同的逻辑
            switch (category)
            {    
                case "CategoryAdd":
                    HandleAddOperation(subCategory);
                    break;
                case "CategoryDelete":
                    HandleDeleteOperation(subCategory);
                    break;
                case "CategoryModify":
                    HandleModifyOperation(subCategory);
                    break;
            }
        }

        private void HandleAddOperation(string subCategory)
        {    
            // 增加操作的具体逻辑
            Logger.LogInfo($"执行增加操作: {subCategory}");
            // 显示提示信息
            Logger.LogInfo("提示: 功能正在开发中...");
        }

        private void HandleDeleteOperation(string subCategory)
        {    
            // 删除操作的具体逻辑
            Logger.LogInfo($"执行删除操作: {subCategory}");
            
            // 打开删除窗口，这里复用修改窗口的结构，但用途是删除
            editorWindow.OpenModifyWindow(subCategory);
        }

        private void HandleModifyOperation(string subCategory)
        {
            // 修改操作的具体逻辑
            Logger.LogInfo($"执行修改操作: {subCategory}");
            // 打开修改窗口
            editorWindow.OpenModifyWindow(subCategory);
        }

        private void HandleModifyConfirmed(string modifyType, int levelIndex, int itemIndex, Dictionary<string, string> values)
        {            
            // 检查当前选中的是哪个大分类
            if (editorWindow.GetSelectedCategory() == "删除")
            {                
                // 执行删除操作
                HandleDeleteConfirmed(modifyType, levelIndex, itemIndex);
                return;
            }
            
            // 否则执行修改操作
            Logger.LogInfo($"确认修改: {modifyType}, 层级: {levelIndex}, 索引: {itemIndex}");
            
            try
            {
                switch (modifyType)
                {
                    case "SubCategoryVillage":// 村落
                    case "SubCategoryTown":    // 城镇
                        ModifyVillageOrTown(modifyType, levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryCamp":    // 军营
                        ModifyCamp(levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryField":    // 沃野
                        ModifyField(levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryLake":     // 深湖
                        ModifyLake(levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryForest":   // 林场
                        ModifyForest(levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryMountain": // 荒山
                        ModifyMountain(levelIndex, itemIndex, values);
                        break;
                    case "SubCategoryFarm":     // 农庄
                        ModifyFarm(levelIndex, itemIndex, values);
                        break;
                }
                
                Logger.LogInfo("修改成功");
            }
            catch (Exception ex)
            {
                Logger.LogError($"修改失败: {ex.Message}");
            }
        }

        private void HandleDeleteConfirmed(string deleteType, int levelIndex, int itemIndex)
        {            
            Logger.LogInfo($"确认删除: {deleteType}, 层级: {levelIndex}, 索引: {itemIndex}");
            
            try
            {
                // 检查层级索引是否有效
                if (levelIndex < 0 || levelIndex > 12)
                {
                    Logger.LogError("无效的层级索引");
                    Logger.LogError("错误: 无效的层级索引，请输入0-12之间的数字");
                    return;
                }
                
                // 获取要删除的地块面积（假设从对应数据中获取）
                int area = GetAreaFromTarget(deleteType, levelIndex, itemIndex);
                
                Logger.LogInfo($"删除{GetSubCategoryName(deleteType)}，层级: {levelIndex}，索引: {itemIndex}，面积: {area}");
                
                // 执行删除操作：删除对应数组
                DeleteTargetArray(deleteType, levelIndex, itemIndex);
                
                // 特殊处理农庄删除时的文件操作
                if (deleteType == "SubCategoryFarm")
                {
                    // 构建存档路径
                    string savePath = Mainload.CunDangIndex_now;
                    string folderPath = $"{savePath}/Z{levelIndex}";
                    string fileToDelete = $"{folderPath}/{itemIndex}.es3";
                    
                    Logger.LogInfo($"删除农庄文件: {fileToDelete}");
                    
                    // 删除对应文件
                    if (ES3.FileExists(fileToDelete))
                    {
                        ES3.DeleteFile(fileToDelete);
                        Logger.LogInfo("成功删除农庄文件");
                    }
                    else
                    {
                        Logger.LogWarning($"农庄文件不存在: {fileToDelete}");
                    }
                    
                    // 判断是否需要重命名后续文件（假设我们可以通过某种方式获取当前层级下农庄的总数）
                    // 这里假设有一个方法可以获取该层级下农庄的总数
                    int totalFarms = GetTotalFarmsAtLevel(levelIndex);
                    
                    // 如果不是最后一个元素，执行重命名操作
                    if (itemIndex < totalFarms - 1)
                    {
                        Logger.LogInfo($"开始重命名后续农庄文件，从索引{itemIndex + 1}开始");
                        
                        // 重命名后续文件，将每个文件的索引减1
                        for (int i = 1; itemIndex + i < totalFarms; i++)
                        {
                            string oldFilePath = $"{folderPath}/{itemIndex + i}.es3";
                            string newFilePath = $"{folderPath}/{itemIndex + i - 1}.es3";
                            
                            if (ES3.FileExists(oldFilePath))
                            {
                                Logger.LogInfo($"重命名文件: {oldFilePath} -> {newFilePath}");
                                ES3.RenameFile(oldFilePath, newFilePath);
                            }
                            else
                            {
                                Logger.LogWarning($"要重命名的文件不存在: {oldFilePath}");
                            }
                        }
                        
                        Logger.LogInfo("完成农庄文件重命名操作");
                    }
                }
                
                
                Logger.LogInfo("删除成功，并添加了相同面积的沃野");
                Logger.LogInfo("成功: 删除成功，并添加了相同面积的沃野");
            }
            catch (Exception ex)
            {
                Logger.LogError($"删除失败: {ex.Message}");
                Logger.LogError($"错误: 删除失败: {ex.Message}");
            }
        }
        
        private int GetTotalFarmsAtLevel(int levelIndex)
        {            
            // 获取指定层级下农庄的总数
            Logger.LogInfo($"获取层级{levelIndex}下农庄的总数");
            
            try
            {
                // 从Mainload.NongZ_now数组中获取对应层级的农庄数量
                // 例如南郡封地(索引1)就读取Mainload.NongZ_now[1].Count
                if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count)
                {
                    int totalFarms = Mainload.NongZ_now[levelIndex].Count;
                    Logger.LogInfo($"层级{levelIndex}下农庄总数为: {totalFarms}");
                    return totalFarms;
                }
                else
                {
                    Logger.LogWarning($"层级索引{levelIndex}超出范围");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"获取农庄数量失败: {ex.Message}");
                return 0;
            }
        }
        
        // 方法：从数组中移除指定索引的元素（元素也是数组）
        private List<string>[] RemoveAt(List<string>[] array, int index)
        {            
            if (array == null || index < 0 || index >= array.Length)
            {                
                return array; // 返回原数组或空数组
            }
            
            List<string>[] newArray = new List<string>[array.Length - 1];
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - index - 1);
            return newArray;
        }
        
        private int GetAreaFromTarget(string type, int levelIndex, int itemIndex)
        {            
            Logger.LogInfo($"获取{type}的面积信息，层级: {levelIndex}，索引: {itemIndex}");
            
            try
            {
                // 根据不同类型从对应的Mainload数据结构中获取面积
                switch (type)
                {                
                    case "SubCategoryCamp":    // 军营 (JunYing_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.JunYing_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.JunYing_now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryTown":    // 城镇 (Zhen_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Zhen_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Zhen_now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryVillage": // 村落 (Cun_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Cun_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Cun_now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryField":   // 沃野 (Kong_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Kong_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Kong_now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryForest":  // 林场 (Sen_Now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Sen_Now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Sen_Now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryLake":    // 深湖 (Hu_Now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Hu_Now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Hu_Now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryMountain":// 荒山 (Shan_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Shan_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.Shan_now[levelIndex][itemIndex][1]);
                        }
                        break;
                    case "SubCategoryFarm":    // 农庄 (NongZ_now)
                        if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.NongZ_now[levelIndex].Count)
                        {
                            return int.Parse(Mainload.NongZ_now[levelIndex][itemIndex][5]);
                        }
                        break;
                }
                
                // 如果索引无效或类型不匹配，返回默认值
                Logger.LogWarning($"无法获取面积信息，可能是索引无效或类型不匹配");
                return 16; // 默认面积
            }
            catch (Exception ex)
            {
                Logger.LogError($"获取面积信息失败: {ex.Message}");
                return 16; // 出错时返回默认面积
            }
        }
        
        private void DeleteTargetArray(string type, int levelIndex, int itemIndex)
        {            
            // 根据类型删除对应数组
            Logger.LogInfo($"删除{type}数组，层级: {levelIndex}，索引: {itemIndex}");
            
            // 实现实际的删除逻辑
            try
            {
                switch (type)
                {                
                    case "SubCategoryCamp":    // 军营 (JunYing_now)
                        Logger.LogInfo($"删除军营数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.JunYing_now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.JunYing_now[levelIndex] = RemoveAt(Mainload.JunYing_now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryTown":    // 城镇 (Zhen_now)
                        Logger.LogInfo($"删除城镇数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Zhen_now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.Zhen_now[levelIndex] = RemoveAt(Mainload.Zhen_now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryVillage": // 村落 (Cun_now)
                        Logger.LogInfo($"删除村落数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Cun_now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.Cun_now[levelIndex] = RemoveAt(Mainload.Cun_now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryField":   // 沃野 (Kong_now)
                        Logger.LogInfo($"删除沃野数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Kong_now[levelIndex].Count)
                        {    
                            Mainload.Kong_now[levelIndex] = RemoveAt(Mainload.Kong_now[levelIndex].ToArray(), itemIndex).ToList();
                        }
                        break;
                    case "SubCategoryForest":  // 林场 (Sen_Now)
                        Logger.LogInfo($"删除林场数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Sen_Now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.Sen_Now[levelIndex] = RemoveAt(Mainload.Sen_Now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryLake":    // 深湖 (Hu_Now)
                        Logger.LogInfo($"删除深湖数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Hu_Now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.Hu_Now[levelIndex] = RemoveAt(Mainload.Hu_Now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryMountain":// 荒山 (Shan_now)
                        Logger.LogInfo($"删除荒山数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Shan_now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.Shan_now[levelIndex] = RemoveAt(Mainload.Shan_now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                    case "SubCategoryFarm":    // 农庄 (NongZ_now)
                        Logger.LogInfo($"删除农庄数组: 层级索引={levelIndex}, 记录索引={itemIndex}");
                        if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.NongZ_now[levelIndex].Count)
                        {    
                            // 在删除前获取面积和坐标
                            int area = GetAreaFromTarget(type, levelIndex, itemIndex);
                            string coordinates = GetCoordinatesFromTarget(type, levelIndex, itemIndex);
                            
                            // 删除项
                            Mainload.NongZ_now[levelIndex] = RemoveAt(Mainload.NongZ_now[levelIndex].ToArray(), itemIndex).ToList();
                            
                            // 在相同位置添加对应面积和坐标的沃野
                            AddSameAreaFieldAtPosition(levelIndex, itemIndex, area, coordinates);
                        }
                        break;
                }
                
                // 隐藏窗口
                Logger.LogInfo($"操作完成，隐藏窗口");
                if (editorWindow != null && editorWindow.IsWindowVisible())
                {                    
                    editorWindow.ToggleWindowVisibility();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"删除操作异常: {ex.Message}\n{ex.StackTrace}");
            }
        }
        
        private void AddSameAreaFieldAtPosition(int levelIndex, int itemIndex, int area, string coordinates)
        {            
            try
            {
                // 在相同位置添加相同面积和坐标的沃野
                Logger.LogInfo($"在层级: {levelIndex}，索引: {itemIndex}，添加面积为: {area}，坐标为: {coordinates}的沃野");
                
                // 检查层级索引有效性
                if (levelIndex < 0 || levelIndex >= Mainload.Kong_now.Count)
                {
                    Logger.LogError($"添加沃野失败：无效的层级索引 {levelIndex}");
                    return;
                }
                
                // 创建新的沃野数据项，使用正确的数据格式
                List<string> newField = new List<string>();
                // 初始化10个索引位置
                for (int i = 0; i < 10; i++)
                {
                    newField.Add("0");
                }
                
                // 设置坐标（索引0）
                newField[0] = coordinates;
                // 设置面积（索引1）
                newField[1] = area.ToString();
                // 设置其他默认值
                newField[2] = "0";
                newField[3] = "0|0";
                newField[4] = "0";
                
                // 生成肥沃度数据，根据面积设置
                string fertility = "";
                for (int i = 0; i < area; i++)
                {
                    fertility += "1";
                    if (i < area - 1)
                    {
                        fertility += "|";
                    }
                }
                // 补足25个地块的肥沃度数据
                int remaining = 25 - area;
                for (int i = 0; i < remaining; i++)
                {
                    if (area > 0 || i > 0)
                    {
                        fertility += "|";
                    }
                    fertility += "0";
                }
                newField[5] = fertility;
                
                // 获取当前沃野数组
                List<List<string>> currentFields = Mainload.Kong_now[levelIndex];
                
                // 在指定位置插入新沃野
                currentFields.Insert(itemIndex, newField);
            }
            catch (Exception ex)
            {
                Logger.LogError($"添加沃野时发生异常: {ex.Message}\n{ex.StackTrace}");
            }
        }
        
        /// <summary>
        /// 从目标地块获取坐标
        /// </summary>
        /// <param name="type">地块类型</param>
        /// <param name="levelIndex">层级索引</param>
        /// <param name="itemIndex">项目索引</param>
        /// <returns>坐标字符串</returns>
        private string GetCoordinatesFromTarget(string type, int levelIndex, int itemIndex)
        {            
            try
            {
                switch (type)
                {
                    case "SubCategoryCamp":
                        if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.JunYing_now[levelIndex].Count)
                        {
                            return Mainload.JunYing_now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryTown":
                        if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Zhen_now[levelIndex].Count)
                        {
                            return Mainload.Zhen_now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryVillage":
                        if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Cun_now[levelIndex].Count)
                        {
                            return Mainload.Cun_now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryForest":
                        if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Sen_Now[levelIndex].Count)
                        {
                            return Mainload.Sen_Now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryLake":
                        if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Hu_Now[levelIndex].Count)
                        {
                            return Mainload.Hu_Now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryMountain":
                        if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Shan_now[levelIndex].Count)
                        {
                            return Mainload.Shan_now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryFarm":
                        if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.NongZ_now[levelIndex].Count)
                        {
                            return Mainload.NongZ_now[levelIndex][itemIndex][0];
                        }
                        break;
                    case "SubCategoryField":
                        if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count && 
                            itemIndex >= 0 && itemIndex < Mainload.Kong_now[levelIndex].Count)
                        {
                            return Mainload.Kong_now[levelIndex][itemIndex][0];
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"获取坐标时发生异常: {ex.Message}\n{ex.StackTrace}");
            }
            return "0";
        }
        
        private string GetSubCategoryName(string subCategoryKey)
        {            
            // 获取子分类的中文名称
            switch (subCategoryKey)
            {                
                case "SubCategoryField": return "沃野";
                case "SubCategoryVillage": return "村落";
                case "SubCategoryTown": return "城镇";
                case "SubCategoryCamp": return "军营";
                case "SubCategoryLake": return "深湖";
                case "SubCategoryFarm": return "农庄";
                case "SubCategoryForest": return "林场";
                case "SubCategoryMountain": return "荒山";
                default: return "未知";
            }
        }

        private void ModifyVillageOrTown(string type, int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 村落和城镇修改索引0-5的内容
            Logger.LogInfo($"修改{(type == "SubCategoryVillage" ? "村落" : "城镇")} - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (type == "SubCategoryVillage" && levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.Cun_now[levelIndex].Count)
            {    
                // 处理坐标
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    // 修改索引0
                    Mainload.Cun_now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    // 修改索引1
                    Mainload.Cun_now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理人口
                if (!string.IsNullOrEmpty(values["人口"]))
                {    
                    // 修改索引2
                    Mainload.Cun_now[levelIndex][itemIndex][2] = values["人口"];
                    Logger.LogInfo($"修改人口为: {values["人口"]}");
                }
                
                // 处理幸福
                if (!string.IsNullOrEmpty(values["幸福"]))
                {    
                    // 修改索引3
                    Mainload.Cun_now[levelIndex][itemIndex][3] = values["幸福"];
                    Logger.LogInfo($"修改幸福为: {values["幸福"]}");
                }
                
                // 处理商业
                if (!string.IsNullOrEmpty(values["商业"]))
                {    
                    // 修改索引4
                    Mainload.Cun_now[levelIndex][itemIndex][4] = values["商业"];
                    Logger.LogInfo($"修改商业为: {values["商业"]}");
                }
                
                // 处理农业
                if (!string.IsNullOrEmpty(values["农业"]))
                {    
                    // 修改索引5
                    Mainload.Cun_now[levelIndex][itemIndex][5] = values["农业"];
                    Logger.LogInfo($"修改农业为: {values["农业"]}");
                }
            }
            else if (type == "SubCategoryTown" && levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count && 
                     itemIndex >= 0 && itemIndex < Mainload.Zhen_now[levelIndex].Count)
            {    
                // 处理坐标
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    // 修改索引0
                    Mainload.Zhen_now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    // 修改索引1
                    Mainload.Zhen_now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理人口
                if (!string.IsNullOrEmpty(values["人口"]))
                {    
                    // 修改索引2
                    Mainload.Zhen_now[levelIndex][itemIndex][2] = values["人口"];
                    Logger.LogInfo($"修改人口为: {values["人口"]}");
                }
                
                // 处理幸福
                if (!string.IsNullOrEmpty(values["幸福"]))
                {    
                    // 修改索引3
                    Mainload.Zhen_now[levelIndex][itemIndex][3] = values["幸福"];
                    Logger.LogInfo($"修改幸福为: {values["幸福"]}");
                }
                
                // 处理商业
                if (!string.IsNullOrEmpty(values["商业"]))
                {    
                    // 修改索引4
                    Mainload.Zhen_now[levelIndex][itemIndex][4] = values["商业"];
                    Logger.LogInfo($"修改商业为: {values["商业"]}");
                }
                
                // 处理农业
                if (!string.IsNullOrEmpty(values["农业"]))
                {    
                    // 修改索引5
                    Mainload.Zhen_now[levelIndex][itemIndex][5] = values["农业"];
                    Logger.LogInfo($"修改农业为: {values["农业"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyCamp(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 军营修改除3外的所有索引的内容
            Logger.LogInfo($"修改军营 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.JunYing_now[levelIndex].Count)
            {    
                // 处理坐标 (索引0)
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积 (索引1)
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 跳过索引3（待定字段）
                
                // 处理私兵数量 (索引2)
                if (!string.IsNullOrEmpty(values["私兵数量"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][2] = values["私兵数量"];
                    Logger.LogInfo($"修改私兵数量为: {values["私兵数量"]}");
                }
                
                // 处理忠诚 (索引4)
                if (!string.IsNullOrEmpty(values["忠诚"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][4] = values["忠诚"];
                    Logger.LogInfo($"修改忠诚为: {values["忠诚"]}");
                }
                
                // 处理低级武器装备率 (索引5)
                if (!string.IsNullOrEmpty(values["低级武器装备率"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][5] = values["低级武器装备率"];
                    Logger.LogInfo($"修改低级武器装备率为: {values["低级武器装备率"]}");
                }
                
                // 处理高级武器装备率 (索引6)
                if (!string.IsNullOrEmpty(values["高级武器装备率"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][6] = values["高级武器装备率"];
                    Logger.LogInfo($"修改高级武器装备率为: {values["高级武器装备率"]}");
                }
                
                // 处理名字 (索引7)
                if (!string.IsNullOrEmpty(values["名字"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][7] = values["名字"];
                    Logger.LogInfo($"修改名字为: {values["名字"]}");
                }
                
                // 处理军饷 (索引8)
                if (!string.IsNullOrEmpty(values["军饷"]))
                {    
                    Mainload.JunYing_now[levelIndex][itemIndex][8] = values["军饷"];
                    Logger.LogInfo($"修改军饷为: {values["军饷"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyField(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 沃野修改索引0、1、4（修改面积时对应修改肥沃度）
            Logger.LogInfo($"修改沃野 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.Kong_now[levelIndex].Count)
            {    
                // 处理坐标 (索引0)
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    Mainload.Kong_now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积 (索引1)，并更新肥沃度
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    int area = int.Parse(values["面积"]);
                    Mainload.Kong_now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                    
                    // 根据面积更新肥沃度（索引5）
                    string fertility = "";
                    for (int i = 0; i < area; i++)
                    {    
                        fertility += "1";
                        if (i < area - 1)
                        {    
                            fertility += "|";
                        }
                    }
                    // 补足25个地块的肥沃度数据
                    int remaining = 25 - area;
                    for (int i = 0; i < remaining; i++)
                    {    
                        if (area > 0 || i > 0)
                        {    
                            fertility += "|";
                        }
                        fertility += "0";
                    }
                    Mainload.Kong_now[levelIndex][itemIndex][5] = fertility;
                    Logger.LogInfo("根据面积更新肥沃度");
                }
                
                // 处理工程量 (索引4)
                if (!string.IsNullOrEmpty(values["工程量"]))
                {    
                    Mainload.Kong_now[levelIndex][itemIndex][4] = values["工程量"];
                    Logger.LogInfo($"修改工程量为: {values["工程量"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyLake(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 深湖修改索引0、1、4
            Logger.LogInfo($"修改深湖 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.Hu_Now[levelIndex].Count)
            {    
                // 处理坐标 (索引0)
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    Mainload.Hu_Now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积 (索引1)
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    Mainload.Hu_Now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理工程量 (索引4)
                if (!string.IsNullOrEmpty(values["工程量"]))
                {    
                    Mainload.Hu_Now[levelIndex][itemIndex][4] = values["工程量"];
                    Logger.LogInfo($"修改工程量为: {values["工程量"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyForest(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 林场修改索引0、1、4
            Logger.LogInfo($"修改林场 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.Sen_Now[levelIndex].Count)
            {    
                // 处理坐标 (索引0)
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    Mainload.Sen_Now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积 (索引1)
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    Mainload.Sen_Now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理工程量 (索引4)
                if (!string.IsNullOrEmpty(values["工程量"]))
                {    
                    Mainload.Sen_Now[levelIndex][itemIndex][4] = values["工程量"];
                    Logger.LogInfo($"修改工程量为: {values["工程量"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyMountain(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 荒山修改索引0、1、4、5
            Logger.LogInfo($"修改荒山 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.Shan_now[levelIndex].Count)
            {    
                // 处理坐标 (索引0)
                if (!string.IsNullOrEmpty(values["坐标"]))
                {    
                    Mainload.Shan_now[levelIndex][itemIndex][0] = values["坐标"];
                    Logger.LogInfo($"修改坐标为: {values["坐标"]}");
                }
                
                // 处理面积 (索引1)
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    Mainload.Shan_now[levelIndex][itemIndex][1] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理工程量 (索引4)
                if (!string.IsNullOrEmpty(values["工程量"]))
                {    
                    Mainload.Shan_now[levelIndex][itemIndex][4] = values["工程量"];
                    Logger.LogInfo($"修改工程量为: {values["工程量"]}");
                }
                
                // 处理流民 (索引5)
                if (!string.IsNullOrEmpty(values["流民"]))
                {    
                    Mainload.Shan_now[levelIndex][itemIndex][5] = values["流民"];
                    Logger.LogInfo($"修改流民为: {values["流民"]}");
                }
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void ModifyFarm(int levelIndex, int itemIndex, Dictionary<string, string> values)
        {    
            // 农庄修改索引2、5、10、11、12、24（索引24分为种植、养殖、手工三个修改）
            // 只有所属世家为-1时才可修改
            Logger.LogInfo($"修改农庄 - 层级: {levelIndex}, 索引: {itemIndex}");
            
            // 检查层级和索引是否有效
            if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count && 
                itemIndex >= 0 && itemIndex < Mainload.NongZ_now[levelIndex].Count)
            {    
                // 检查所属世家是否为-1（假设在索引6）
                bool canModify = true;
                if (Mainload.NongZ_now[levelIndex][itemIndex].Count > 6 && 
                    Mainload.NongZ_now[levelIndex][itemIndex][6] != "-1")
                {    
                    canModify = false;
                }
                
                if (!canModify)
                {    
                    Logger.LogWarning("农庄所属世家不为-1，无法修改");
                    return;
                }
                
                // 处理土地肥力 (索引2)
                if (!string.IsNullOrEmpty(values["土地肥力"]))
                {    
                    Mainload.NongZ_now[levelIndex][itemIndex][2] = values["土地肥力"];
                    Logger.LogInfo($"修改土地肥力为: {values["土地肥力"]}");
                }
                
                // 处理面积 (索引5)
                if (!string.IsNullOrEmpty(values["面积"]))
                {    
                    Mainload.NongZ_now[levelIndex][itemIndex][5] = values["面积"];
                    Logger.LogInfo($"修改面积为: {values["面积"]}");
                }
                
                // 处理环境 (索引10)
                if (!string.IsNullOrEmpty(values["环境"]))
                {    
                    Mainload.NongZ_now[levelIndex][itemIndex][10] = values["环境"];
                    Logger.LogInfo($"修改环境为: {values["环境"]}");
                }
                
                // 处理安全 (索引11)
                if (!string.IsNullOrEmpty(values["安全"]))
                {    
                    Mainload.NongZ_now[levelIndex][itemIndex][11] = values["安全"];
                    Logger.LogInfo($"修改安全为: {values["安全"]}");
                }
                
                // 处理便捷 (索引12)
                if (!string.IsNullOrEmpty(values["便捷"]))
                {    
                    Mainload.NongZ_now[levelIndex][itemIndex][12] = values["便捷"];
                    Logger.LogInfo($"修改便捷为: {values["便捷"]}");
                }
                
                // 处理农户数量 (索引24)，分为种植、养殖、手工
                string farmersStr = $"{values["种植农户"]}|{values["养殖农户"]}|{values["手工农户"]}";
                Mainload.NongZ_now[levelIndex][itemIndex][24] = farmersStr;
                Logger.LogInfo($"修改农户数量为: {farmersStr}");
            }
            else
            {    
                Logger.LogError("修改失败：无效的层级索引或项目索引");
            }
        }

        private void OnDestroy()
        {    
            // 清理Harmony补丁
            if (harmony != null)
            {    
                harmony.UnpatchSelf();
            }
        }
    }
}
