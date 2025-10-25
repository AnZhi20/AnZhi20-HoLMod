using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Sen_NowData
{
    /// <summary>
    /// 林场数据结构及工具类
    /// </summary>
    public class SenData
    {
        /// <summary>
        /// 林场数据数组索引定义（暂定10个索引）
        /// </summary>
        public class SenIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 未知值，一般是0
            public const int INDEX_3 = 3;     // 未知值，一般是0|0
            public const int INDEX_4 = 4;     // 工程量
            public const int INDEX_5 = 5;     // 未知值
        }

        /// <summary>
        /// 检查Mainload.Sen_Now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureSenNowExists()
        {
            try
            {
                // 尝试访问Mainload.Sen_Now，如果不存在则创建
                if (Mainload.Sen_Now == null)
                {
                    // 索引0对应大地图的所有林场数组，索引1-12对应各郡（郡索引的0-11）的所有林场数组
                    Mainload.Sen_Now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Sen_Now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Sen_Now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Sen_Now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的林场数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是0|0</param>
        /// <param name="workAmount">工程量</param>
        /// <param name="unknown3">未知值</param>
        /// <returns>林场数据列表</returns>
        public static List<string> CreateSenInstance(string coordinates = "0", string area = "0", string unknown1 = "0", 
                                                    string unknown2 = "0|0", string workAmount = "0", string unknown3 = "0")
        {
            List<string> senData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                senData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            senData[SenIndexes.INDEX_0] = coordinates;
            senData[SenIndexes.INDEX_1] = area;
            senData[SenIndexes.INDEX_2] = unknown1;
            senData[SenIndexes.INDEX_3] = unknown2;
            senData[SenIndexes.INDEX_4] = workAmount;
            senData[SenIndexes.INDEX_5] = unknown3;

            return senData;
        }

        /// <summary>
        /// 获取指定层级和索引的林场数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="senIndex">林场索引</param>
        /// <returns>林场数据，如果索引无效则返回null</returns>
        public static List<string> GetSenByIndex(int levelIndex, int senIndex)
        {
            EnsureSenNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count)
            {
                if (senIndex >= 0 && senIndex < Mainload.Sen_Now[levelIndex].Count)
                {
                    return Mainload.Sen_Now[levelIndex][senIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的林场到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="senData">林场数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddSen(int levelIndex, List<string> senData)
        {
            try
            {
                EnsureSenNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count)
                {
                    if (senData != null && senData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Sen_Now[levelIndex].Add(senData);
                    string coordinates = GetSenCoordinates(senData);
                    Debug.Log($"已添加新林场 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加林场失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加林场失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加林场时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的林场数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>林场数据数组</returns>
        public static List<List<string>> LoadSenDataByLevel(int levelIndex)
        {
            EnsureSenNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Sen_Now.Count)
            {
                return Mainload.Sen_Now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取林场的简要信息字符串
        /// </summary>
        /// <param name="senData">林场数据</param>
        /// <returns>包含林场关键信息的字符串</returns>
        public static string GetSenSummary(List<string> senData)
        {
            if (senData == null || senData.Count < 6)
                return "无效的林场数据";

            string coordinates = senData[SenIndexes.INDEX_0];
            string area = senData[SenIndexes.INDEX_1];
            string workAmount = senData[SenIndexes.INDEX_4];

            return $"林场 (坐标: {coordinates}, 面积: {area}, 工程量: {workAmount})";
        }

        /// <summary>
        /// 获取林场的坐标
        /// </summary>
        /// <param name="senData">林场数据</param>
        /// <returns>林场坐标字符串</returns>
        public static string GetSenCoordinates(List<string> senData)
        {
            if (senData == null || senData.Count <= SenIndexes.INDEX_0)
                return "未知";
            return senData[SenIndexes.INDEX_0];
        }
    }
}