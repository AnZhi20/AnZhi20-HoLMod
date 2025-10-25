using System;
using System.Collections.Generic;
using System.Linq;

namespace cs.HoLMod.AddItem
{
    internal class LoadGameItemsToDictionary
    {
        /// <summary>
        /// 从Text_AllProp加载游戏物品到itemList
        /// </summary>
        /// <param name="itemList">要添加物品的字典</param>
        /// <param name="isChineseLanguage">是否为中文语言环境</param>
        public static void LoadItems(Dictionary<int, (string, string)> itemList, bool isChineseLanguage)
        {
            try
            {
                // 检查Text_AllProp是否有效
                if (AllText.Text_AllProp == null || AllText.Text_AllProp.Count == 0)
                {
                    UnityEngine.Debug.LogWarning("AllText.Text_AllProp为null或空");
                    return;
                }

                // 遍历AllText.Text_AllProp中的所有物品
                for (int i = 0; i < AllText.Text_AllProp.Count; i++)
                {
                    // 获取当前物品数组
                    List<string> itemInfo = AllText.Text_AllProp[i];
                    if (itemInfo == null || itemInfo.Count < 2)
                    {
                        continue; // 跳过无效的物品信息
                    }

                    // 获取中文名称和英文名称
                    string chineseName = itemInfo[0];
                    string englishName = itemInfo[1];

                    // 根据语言选择显示名称
                    string displayName = isChineseLanguage ? chineseName : englishName;

                    // 检查物品是否已存在于itemList中
                    bool itemExists = itemList.Values.Any(item => item.Item1 == displayName);

                    // 如果物品不存在，则添加到itemList中
                    if (!itemExists)
                    {
                        // 与AllText.Text_AllProp[]的索引保持一致
                        int newId = i;
                         
                        // 如果ID已存在，记录警告并跳过添加，以保持索引一致性
                        if (itemList.ContainsKey(newId))
                        {
                            UnityEngine.Debug.LogWarning("物品ID冲突: 索引 " + i + " (" + displayName + ") 已存在，跳过添加以保持索引一致性");
                            continue;
                        }

                        // 添加新物品，分类为"新增物品"
                        itemList.Add(newId, (displayName, "新增物品"));
                        UnityEngine.Debug.Log("已添加新物品: " + displayName + "索引" + newId);
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("加载游戏物品到字典时出错: " + ex.Message);
            }
        }

        /// <summary>
        /// 从Text_AllProp加载游戏物品到itemList（兼容旧版本调用）
        /// </summary>
        /// <param name="itemList">要添加物品的字典</param>
        public static void LoadItems(Dictionary<int, (string, string)> itemList)
        {
            // 默认使用中文
            LoadItems(itemList, true);
        }
    }
}
