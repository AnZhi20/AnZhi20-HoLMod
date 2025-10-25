using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Cun_nowData
{
    /// <summary>
    /// 村落数据结构及工具类
    /// </summary>
    public class CunData
    {
        /// <summary>
        /// 村落数据数组索引定义（暂定10个索引）
        /// </summary>
        public class CunIndexes
        {
            public const int INDEX_0 = 0;     // 坐标
            public const int INDEX_1 = 1;     // 面积
            public const int INDEX_2 = 2;     // 人口
            public const int INDEX_3 = 3;     // 幸福
            public const int INDEX_4 = 4;     // 商业
            public const int INDEX_5 = 5;     // 农业
            public const int INDEX_6 = 6;     // 未知值，一般是0
            public const int INDEX_7 = 7;     // 未知值，一般是22
        }

        /// <summary>
        /// 检查Mainload.Cun_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureCunNowExists()
        {
            try
            {
                // 尝试访问Mainload.Cun_now，如果不存在则创建
                if (Mainload.Cun_now == null)
                {
                    // 索引0对应大地图的所有村落数组，索引1-12对应各郡（郡索引的0-11）的所有村落数组
                    Mainload.Cun_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Cun_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Cun_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Cun_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的村落数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="population">人口</param>
        /// <param name="happiness">幸福</param>
        /// <param name="commerce">商业</param>
        /// <param name="agriculture">农业</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是22</param>
        /// <returns>村落数据列表</returns>
        public static List<string> CreateCunInstance(string coordinates = "0", string area = "0", string population = "0", 
                                                    string happiness = "0", string commerce = "0", string agriculture = "0",
                                                    string unknown1 = "0", string unknown2 = "22")
        {
            List<string> cunData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                cunData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            cunData[CunIndexes.INDEX_0] = coordinates;
            cunData[CunIndexes.INDEX_1] = area;
            cunData[CunIndexes.INDEX_2] = population;
            cunData[CunIndexes.INDEX_3] = happiness;
            cunData[CunIndexes.INDEX_4] = commerce;
            cunData[CunIndexes.INDEX_5] = agriculture;
            cunData[CunIndexes.INDEX_6] = unknown1;
            cunData[CunIndexes.INDEX_7] = unknown2;

            return cunData;
        }

        /// <summary>
        /// 获取指定层级和索引的村落数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="cunIndex">村落索引</param>
        /// <returns>村落数据，如果索引无效则返回null</returns>
        public static List<string> GetCunByIndex(int levelIndex, int cunIndex)
        {
            EnsureCunNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count)
            {
                if (cunIndex >= 0 && cunIndex < Mainload.Cun_now[levelIndex].Count)
                {
                    return Mainload.Cun_now[levelIndex][cunIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的村落到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="cunData">村落数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddCun(int levelIndex, List<string> cunData)
        {
            try
            {
                EnsureCunNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count)
                {
                    if (cunData != null && cunData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Cun_now[levelIndex].Add(cunData);
                    string coordinates = GetCunCoordinates(cunData);
                    Debug.Log($"已添加新村落 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加村落失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加村落失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加村落时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的村落数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>村落数据数组</returns>
        public static List<List<string>> LoadCunDataByLevel(int levelIndex)
        {
            EnsureCunNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Cun_now.Count)
            {
                return Mainload.Cun_now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取村落的简要信息字符串
        /// </summary>
        /// <param name="cunData">村落数据</param>
        /// <returns>包含村落关键信息的字符串</returns>
        public static string GetCunSummary(List<string> cunData)
        {
            if (cunData == null || cunData.Count < 8)
                return "无效的村落数据";

            string coordinates = cunData[CunIndexes.INDEX_0];
            string area = cunData[CunIndexes.INDEX_1];
            string population = cunData[CunIndexes.INDEX_2];
            string happiness = cunData[CunIndexes.INDEX_3];
            string commerce = cunData[CunIndexes.INDEX_4];
            string agriculture = cunData[CunIndexes.INDEX_5];

            return $"村落 (坐标: {coordinates}, 面积: {area}, 人口: {population}, 幸福: {happiness}, 商业: {commerce}, 农业: {agriculture})";
        }

        /// <summary>
        /// 获取村落的坐标
        /// </summary>
        /// <param name="cunData">村落数据</param>
        /// <returns>村落坐标字符串</returns>
        public static string GetCunCoordinates(List<string> cunData)
        {
            if (cunData == null || cunData.Count <= CunIndexes.INDEX_0)
                return "未知";
            return cunData[CunIndexes.INDEX_0];
        }
    }
}