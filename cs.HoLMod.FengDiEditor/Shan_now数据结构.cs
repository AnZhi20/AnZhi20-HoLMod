using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Shan_nowData
{
    /// <summary>
    /// 荒山数据结构及工具类
    /// </summary>
    public class ShanData
    {
        /// <summary>
        /// 荒山数据数组索引定义（暂定10个索引）
        /// </summary>
        public class ShanIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 未知值，一般是0
            public const int INDEX_3 = 3;     // 未知值，一般是0|0
            public const int INDEX_4 = 4;     // 工程量
            public const int INDEX_5 = 5;     // 流民
        }

        /// <summary>
        /// 检查Mainload.Shan_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureShanNowExists()
        {
            try
            {
                // 尝试访问Mainload.Shan_now，如果不存在则创建
                if (Mainload.Shan_now == null)
                {
                    // 索引0对应大地图的所有荒山数组，索引1-12对应各郡（郡索引的0-11）的所有荒山数组
                    Mainload.Shan_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Shan_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Shan_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Shan_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的荒山数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是0|0</param>
        /// <param name="workAmount">工程量</param>
        /// <param name="homeless">流民</param>
        /// <returns>荒山数据列表</returns>
        public static List<string> CreateShanInstance(string coordinates = "0", string area = "0", string unknown1 = "0", 
                                                    string unknown2 = "0|0", string workAmount = "0", string homeless = "0")
        {
            List<string> shanData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                shanData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            shanData[ShanIndexes.INDEX_0] = coordinates;
            shanData[ShanIndexes.INDEX_1] = area;
            shanData[ShanIndexes.INDEX_2] = unknown1;
            shanData[ShanIndexes.INDEX_3] = unknown2;
            shanData[ShanIndexes.INDEX_4] = workAmount;
            shanData[ShanIndexes.INDEX_5] = homeless;

            return shanData;
        }

        /// <summary>
        /// 获取指定层级和索引的荒山数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="shanIndex">荒山索引</param>
        /// <returns>荒山数据，如果索引无效则返回null</returns>
        public static List<string> GetShanByIndex(int levelIndex, int shanIndex)
        {
            EnsureShanNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count)
            {
                if (shanIndex >= 0 && shanIndex < Mainload.Shan_now[levelIndex].Count)
                {
                    return Mainload.Shan_now[levelIndex][shanIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的荒山到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="shanData">荒山数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddShan(int levelIndex, List<string> shanData)
        {
            try
            {
                EnsureShanNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count)
                {
                    if (shanData != null && shanData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Shan_now[levelIndex].Add(shanData);
                    string coordinates = GetShanCoordinates(shanData);
                    Debug.Log($"已添加新荒山 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加荒山失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加荒山失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加荒山时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的荒山数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>荒山数据数组</returns>
        public static List<List<string>> LoadShanDataByLevel(int levelIndex)
        {
            EnsureShanNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Shan_now.Count)
            {
                return Mainload.Shan_now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取荒山的简要信息字符串
        /// </summary>
        /// <param name="shanData">荒山数据</param>
        /// <returns>包含荒山关键信息的字符串</returns>
        public static string GetShanSummary(List<string> shanData)
        {
            if (shanData == null || shanData.Count < 6)
                return "无效的荒山数据";

            string coordinates = shanData[ShanIndexes.INDEX_0];
            string area = shanData[ShanIndexes.INDEX_1];
            string workAmount = shanData[ShanIndexes.INDEX_4];
            string homeless = shanData[ShanIndexes.INDEX_5];

            return $"荒山 (坐标: {coordinates}, 面积: {area}, 工程量: {workAmount}, 流民: {homeless})";
        }

        /// <summary>
        /// 获取荒山的坐标
        /// </summary>
        /// <param name="shanData">荒山数据</param>
        /// <returns>荒山坐标字符串</returns>
        public static string GetShanCoordinates(List<string> shanData)
        {
            if (shanData == null || shanData.Count <= ShanIndexes.INDEX_0)
                return "未知";
            return shanData[ShanIndexes.INDEX_0];
        }
    }
}