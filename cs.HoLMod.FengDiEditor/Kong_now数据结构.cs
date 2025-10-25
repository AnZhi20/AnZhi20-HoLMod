using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Kong_nowData
{
    /// <summary>
    /// 沃野数据结构及工具类
    /// </summary>
    public class KongData
    {
        /// <summary>
        /// 沃野数据数组索引定义（暂定10个索引）
        /// </summary>
        public class KongIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 未知值，一般是0
            public const int INDEX_3 = 3;     // 未知值，一般是0|0
            public const int INDEX_4 = 4;     // 工程量
            public const int INDEX_5 = 5;     // 肥沃度，以|作为分隔的25个数分别代表25个地块的肥沃度，例如面积为9时通常为1|1|1|1|1|1|1|1|1|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0
        }

        /// <summary>
        /// 检查Mainload.Kong_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureKongNowExists()
        {
            try
            {
                // 尝试访问Mainload.Kong_now，如果不存在则创建
                if (Mainload.Kong_now == null)
                {
                    // 索引0对应大地图的所有沃野数组，索引1-12对应各郡（郡索引的0-11）的所有沃野数组
                    Mainload.Kong_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Kong_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Kong_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Kong_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的沃野数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是0|0</param>
        /// <param name="workAmount">工程量</param>
        /// <param name="fertility">肥沃度，以|作为分隔的25个数</param>
        /// <returns>沃野数据列表</returns>
        public static List<string> CreateKongInstance(string coordinates = "0", string area = "0", string unknown1 = "0", 
                                                    string unknown2 = "0|0", string workAmount = "0", string fertility = "0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0|0")
        {
            List<string> kongData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                kongData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            kongData[KongIndexes.INDEX_0] = coordinates;
            kongData[KongIndexes.INDEX_1] = area;
            kongData[KongIndexes.INDEX_2] = unknown1;
            kongData[KongIndexes.INDEX_3] = unknown2;
            kongData[KongIndexes.INDEX_4] = workAmount;
            kongData[KongIndexes.INDEX_5] = fertility;

            return kongData;
        }

        /// <summary>
        /// 获取指定层级和索引的沃野数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="kongIndex">沃野索引</param>
        /// <returns>沃野数据，如果索引无效则返回null</returns>
        public static List<string> GetKongByIndex(int levelIndex, int kongIndex)
        {
            EnsureKongNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count)
            {
                if (kongIndex >= 0 && kongIndex < Mainload.Kong_now[levelIndex].Count)
                {
                    return Mainload.Kong_now[levelIndex][kongIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的沃野到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="kongData">沃野数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddKong(int levelIndex, List<string> kongData)
        {
            try
            {
                EnsureKongNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count)
                {
                    if (kongData != null && kongData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Kong_now[levelIndex].Add(kongData);
                    string coordinates = GetKongCoordinates(kongData);
                    Debug.Log($"已添加新沃野 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加沃野失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加沃野失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加沃野时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的沃野数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>沃野数据数组</returns>
        public static List<List<string>> LoadKongDataByLevel(int levelIndex)
        {
            EnsureKongNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Kong_now.Count)
            {
                return Mainload.Kong_now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取沃野的简要信息字符串
        /// </summary>
        /// <param name="kongData">沃野数据</param>
        /// <returns>包含沃野关键信息的字符串</returns>
        public static string GetKongSummary(List<string> kongData)
        {
            if (kongData == null || kongData.Count < 6)
                return "无效的沃野数据";

            string coordinates = kongData[KongIndexes.INDEX_0];
            string area = kongData[KongIndexes.INDEX_1];
            string workAmount = kongData[KongIndexes.INDEX_4];
            
            // 简化肥沃度显示，只显示前几个值
            string fertility = kongData[KongIndexes.INDEX_5];
            string[] fertilityParts = fertility.Split('|');
            string simplifiedFertility = fertilityParts.Length > 5 ? $"{fertilityParts[0]}|{fertilityParts[1]}|{fertilityParts[2]}|..." : fertility;

            return $"沃野 (坐标: {coordinates}, 面积: {area}, 工程量: {workAmount}, 肥沃度: {simplifiedFertility})";
        }

        /// <summary>
        /// 获取沃野的坐标
        /// </summary>
        /// <param name="kongData">沃野数据</param>
        /// <returns>沃野坐标字符串</returns>
        public static string GetKongCoordinates(List<string> kongData)
        {
            if (kongData == null || kongData.Count <= KongIndexes.INDEX_0)
                return "未知";
            return kongData[KongIndexes.INDEX_0];
        }
    }
}