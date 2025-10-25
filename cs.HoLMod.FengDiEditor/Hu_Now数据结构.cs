using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Hu_NowData
{
    /// <summary>
    /// 深湖数据结构及工具类
    /// </summary>
    public class HuData
    {
        /// <summary>
        /// 深湖数据数组索引定义（暂定10个索引）
        /// </summary>
        public class HuIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 未知值，一般是0
            public const int INDEX_3 = 3;     // 未知值，一般是0|0
            public const int INDEX_4 = 4;     // 工程量
        }

        /// <summary>
        /// 检查Mainload.Hu_Now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureHuNowExists()
        {
            try
            {
                // 尝试访问Mainload.Hu_Now，如果不存在则创建
                if (Mainload.Hu_Now == null)
                {
                    // 索引0对应大地图的所有深湖数组，索引1-12对应各郡（郡索引的0-11）的所有深湖数组
                    Mainload.Hu_Now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Hu_Now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Hu_Now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Hu_Now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的深湖数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是0|0</param>
        /// <param name="workAmount">工程量</param>
        /// <returns>深湖数据列表</returns>
        public static List<string> CreateHuInstance(string coordinates = "0", string area = "0", string unknown1 = "0", 
                                                  string unknown2 = "0|0", string workAmount = "0")
        {
            List<string> huData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                huData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            huData[HuIndexes.INDEX_0] = coordinates;
            huData[HuIndexes.INDEX_1] = area;
            huData[HuIndexes.INDEX_2] = unknown1;
            huData[HuIndexes.INDEX_3] = unknown2;
            huData[HuIndexes.INDEX_4] = workAmount;

            return huData;
        }

        /// <summary>
        /// 获取指定层级和索引的深湖数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="huIndex">深湖索引</param>
        /// <returns>深湖数据，如果索引无效则返回null</returns>
        public static List<string> GetHuByIndex(int levelIndex, int huIndex)
        {
            EnsureHuNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count)
            {
                if (huIndex >= 0 && huIndex < Mainload.Hu_Now[levelIndex].Count)
                {
                    return Mainload.Hu_Now[levelIndex][huIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的深湖到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="huData">深湖数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddHu(int levelIndex, List<string> huData)
        {
            try
            {
                EnsureHuNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count)
                {
                    if (huData != null && huData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Hu_Now[levelIndex].Add(huData);
                    string coordinates = GetHuCoordinates(huData);
                    Debug.Log($"已添加新深湖 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加深湖失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加深湖失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加深湖时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的深湖数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>深湖数据数组</returns>
        public static List<List<string>> LoadHuDataByLevel(int levelIndex)
        {
            EnsureHuNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Hu_Now.Count)
            {
                return Mainload.Hu_Now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取深湖的简要信息字符串
        /// </summary>
        /// <param name="huData">深湖数据</param>
        /// <returns>包含深湖关键信息的字符串</returns>
        public static string GetHuSummary(List<string> huData)
        {
            if (huData == null || huData.Count < 5)
                return "无效的深湖数据";

            string coordinates = huData[HuIndexes.INDEX_0];
            string area = huData[HuIndexes.INDEX_1];
            string workAmount = huData[HuIndexes.INDEX_4];

            return $"深湖 (坐标: {coordinates}, 面积: {area}, 工程量: {workAmount})";
        }

        /// <summary>
        /// 获取深湖的坐标
        /// </summary>
        /// <param name="huData">深湖数据</param>
        /// <returns>深湖坐标字符串</returns>
        public static string GetHuCoordinates(List<string> huData)
        {
            if (huData == null || huData.Count <= HuIndexes.INDEX_0)
                return "未知";
            return huData[HuIndexes.INDEX_0];
        }
    }
}