using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.JunYing_nowData
{
    /// <summary>
    /// 军营数据结构及工具类
    /// </summary>
    public class JunYingData
    {
        /// <summary>
        /// 军营数据数组索引定义（暂定10个索引）
        /// </summary>
        public class JunYingIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 私兵数量
            public const int INDEX_3 = 3;     // 待定
            public const int INDEX_4 = 4;     // 忠诚
            public const int INDEX_5 = 5;     // 低级武器装备率
            public const int INDEX_6 = 6;     // 高级武器装备率
            public const int INDEX_7 = 7;     // 名字
            public const int INDEX_8 = 8;     // 军饷
        }

        /// <summary>
        /// 检查Mainload.JunYing_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureJunYingNowExists()
        {
            try
            {
                // 尝试访问Mainload.JunYing_now，如果不存在则创建
                if (Mainload.JunYing_now == null)
                {
                    // 索引0对应大地图的所有军营数组，索引1-12对应各郡（郡索引的0-11）的所有军营数组
                    Mainload.JunYing_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.JunYing_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.JunYing_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.JunYing_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的军营数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="privateSoldierCount">私兵数量</param>
        /// <param name="loyalty">忠诚</param>
        /// <param name="lowLevelEquipmentRate">低级武器装备率</param>
        /// <param name="highLevelEquipmentRate">高级武器装备率</param>
        /// <param name="name">军营名字</param>
        /// <param name="salary">军饷</param>
        /// <returns>军营数据列表</returns>
        public static List<string> CreateJunYingInstance(
            string coordinates = "0|0",
            string area = "16",
            string privateSoldierCount = "0",
            string loyalty = "100",
            string lowLevelEquipmentRate = "0",
            string highLevelEquipmentRate = "0",
            string name = "未知军营",
            string salary = "500")
        {
            List<string> junYingData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                junYingData.Add("0"); // 默认值
            }

            junYingData[JunYingIndexes.INDEX_0] = coordinates;              // 坐标
            junYingData[JunYingIndexes.INDEX_1] = area;                    // 面积
            junYingData[JunYingIndexes.INDEX_2] = privateSoldierCount;     // 私兵数量
            junYingData[JunYingIndexes.INDEX_4] = loyalty;                 // 忠诚
            junYingData[JunYingIndexes.INDEX_5] = lowLevelEquipmentRate;   // 低级武器装备率
            junYingData[JunYingIndexes.INDEX_6] = highLevelEquipmentRate;  // 高级武器装备率
            junYingData[JunYingIndexes.INDEX_7] = name;                    // 军营名字
            junYingData[JunYingIndexes.INDEX_8] = salary;                  // 军饷

            return junYingData;
        }

        /// <summary>
        /// 获取指定层级和索引的军营数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="junYingIndex">军营索引</param>
        /// <returns>军营数据，如果索引无效则返回null</returns>
        public static List<string> GetJunYingByIndex(int levelIndex, int junYingIndex)
        {
            EnsureJunYingNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count)
            {
                if (junYingIndex >= 0 && junYingIndex < Mainload.JunYing_now[levelIndex].Count)
                {
                    return Mainload.JunYing_now[levelIndex][junYingIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的军营到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="junYingData">军营数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddJunYing(int levelIndex, List<string> junYingData)
        {
            try
            {
                EnsureJunYingNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count)
                {
                    if (junYingData != null && junYingData.Count >= 10) // 确保数据结构完整性
                    {
                        Mainload.JunYing_now[levelIndex].Add(junYingData);
                        // 尝试获取军营名称进行日志记录
                        string junYingName = GetJunYingName(junYingData);
                        Debug.Log($"已添加新军营: {junYingName} (层级索引: {levelIndex})");
                        return true;
                    }
                    else
                    {
                        Debug.LogError("添加军营失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加军营失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加军营时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取指定军营的简要信息字符串
        /// </summary>
        /// <param name="junYingData">军营数据</param>
        /// <returns>军营简要信息</returns>
        public static string GetJunYingSummary(List<string> junYingData)
        {
            if (junYingData == null || junYingData.Count < 10)
            {
                return "无效的军营数据";
            }
            
            string name = junYingData[JunYingIndexes.INDEX_7];
            string area = junYingData[JunYingIndexes.INDEX_1];
            string privateSoldierCount = junYingData[JunYingIndexes.INDEX_2];
            string loyalty = junYingData[JunYingIndexes.INDEX_4];
            string coordinates = junYingData[JunYingIndexes.INDEX_0];
            string salary = junYingData[JunYingIndexes.INDEX_8];
            
            return $"{name} (面积: {area}, 私兵数: {privateSoldierCount}, 忠诚: {loyalty}, 坐标: {coordinates}, 军饷: {salary})";
        }

        /// <summary>
        /// 获取指定军营的名称
        /// </summary>
        /// <param name="junYingData">军营数据</param>
        /// <returns>军营名称</returns>
        public static string GetJunYingName(List<string> junYingData)
        {
            if (junYingData != null && junYingData.Count > JunYingIndexes.INDEX_7)
            {
                return junYingData[JunYingIndexes.INDEX_7];
            }
            return "未知军营";
        }

        /// <summary>
        /// 获取指定军营的坐标信息
        /// </summary>
        /// <param name="junYingData">军营数据</param>
        /// <returns>坐标数组 [x, y]</returns>
        public static int[] GetJunYingCoordinates(List<string> junYingData)
        {
            int[] coordinates = new int[] { -1, -1 };
            if (junYingData != null && junYingData.Count > JunYingIndexes.INDEX_0)
            {
                string coordinatesStr = junYingData[JunYingIndexes.INDEX_0];
                string[] parts = coordinatesStr.Split('|');
                
                if (parts.Length >= 2)
                {
                    int.TryParse(parts[0], out coordinates[0]);
                    int.TryParse(parts[1], out coordinates[1]);
                }
            }
            return coordinates;
        }

        /// <summary>
        /// 加载指定层级的军营数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>军营数据数组</returns>
        public static List<List<string>> LoadJunYingDataByLevel(int levelIndex)
        {
            EnsureJunYingNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.JunYing_now.Count)
            {
                return Mainload.JunYing_now[levelIndex];
            }
            return new List<List<string>>();
        }
    }
}