using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.Zhen_nowData
{
    /// <summary>
    /// 城镇数据结构及工具类
    /// </summary>
    public class ZhenData
    {
        /// <summary>
        /// 城镇数据数组索引定义（暂定10个索引）
        /// </summary>
        public class ZhenIndexes
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
        /// 检查Mainload.Zhen_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureZhenNowExists()
        {
            try
            {
                // 尝试访问Mainload.Zhen_now，如果不存在则创建
                if (Mainload.Zhen_now == null)
                {
                    // 索引0对应大地图的所有城镇数组，索引1-12对应各郡（郡索引的0-11）的所有城镇数组
                    Mainload.Zhen_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.Zhen_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.Zhen_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.Zhen_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的城镇数据实例
        /// </summary>
        /// <param name="coordinates">坐标</param>
        /// <param name="area">面积</param>
        /// <param name="population">人口</param>
        /// <param name="happiness">幸福</param>
        /// <param name="commerce">商业</param>
        /// <param name="agriculture">农业</param>
        /// <param name="unknown1">未知值，一般是0</param>
        /// <param name="unknown2">未知值，一般是22</param>
        /// <returns>城镇数据列表</returns>
        public static List<string> CreateZhenInstance(string coordinates = "0", string area = "0", string population = "0", 
                                                    string happiness = "0", string commerce = "0", string agriculture = "0",
                                                    string unknown1 = "0", string unknown2 = "22")
        {
            List<string> zhenData = new List<string>();
            // 填充0-9的索引，确保数组长度足够
            for (int i = 0; i < 10; i++)
            {
                zhenData.Add("0"); // 默认值
            }

            // 设置已知索引的值
            zhenData[ZhenIndexes.INDEX_0] = coordinates;
            zhenData[ZhenIndexes.INDEX_1] = area;
            zhenData[ZhenIndexes.INDEX_2] = population;
            zhenData[ZhenIndexes.INDEX_3] = happiness;
            zhenData[ZhenIndexes.INDEX_4] = commerce;
            zhenData[ZhenIndexes.INDEX_5] = agriculture;
            zhenData[ZhenIndexes.INDEX_6] = unknown1;
            zhenData[ZhenIndexes.INDEX_7] = unknown2;

            return zhenData;
        }

        /// <summary>
        /// 获取指定层级和索引的城镇数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="zhenIndex">城镇索引</param>
        /// <returns>城镇数据，如果索引无效则返回null</returns>
        public static List<string> GetZhenByIndex(int levelIndex, int zhenIndex)
        {
            EnsureZhenNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count)
            {
                if (zhenIndex >= 0 && zhenIndex < Mainload.Zhen_now[levelIndex].Count)
                {
                    return Mainload.Zhen_now[levelIndex][zhenIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的城镇到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="zhenData">城镇数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddZhen(int levelIndex, List<string> zhenData)
        {
            try
            {
                EnsureZhenNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count)
                {
                    if (zhenData != null && zhenData.Count >= 10) // 确保数据结构完整性
                {
                    Mainload.Zhen_now[levelIndex].Add(zhenData);
                    string coordinates = GetZhenCoordinates(zhenData);
                    Debug.Log($"已添加新城镇 (层级索引: {levelIndex}, 坐标: {coordinates})");
                    return true;
                }
                    else
                    {
                        Debug.LogError("添加城镇失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加城镇失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加城镇时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加载指定层级的城镇数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>城镇数据数组</returns>
        public static List<List<string>> LoadZhenDataByLevel(int levelIndex)
        {
            EnsureZhenNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.Zhen_now.Count)
            {
                return Mainload.Zhen_now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取城镇的简要信息字符串
        /// </summary>
        /// <param name="zhenData">城镇数据</param>
        /// <returns>包含城镇关键信息的字符串</returns>
        public static string GetZhenSummary(List<string> zhenData)
        {
            if (zhenData == null || zhenData.Count < 8)
                return "无效的城镇数据";

            string coordinates = zhenData[ZhenIndexes.INDEX_0];
            string area = zhenData[ZhenIndexes.INDEX_1];
            string population = zhenData[ZhenIndexes.INDEX_2];
            string happiness = zhenData[ZhenIndexes.INDEX_3];
            string commerce = zhenData[ZhenIndexes.INDEX_4];
            string agriculture = zhenData[ZhenIndexes.INDEX_5];

            return $"城镇 (坐标: {coordinates}, 面积: {area}, 人口: {population}, 幸福: {happiness}, 商业: {commerce}, 农业: {agriculture})";
        }

        /// <summary>
        /// 获取城镇的坐标
        /// </summary>
        /// <param name="zhenData">城镇数据</param>
        /// <returns>城镇坐标字符串</returns>
        public static string GetZhenCoordinates(List<string> zhenData)
        {
            if (zhenData == null || zhenData.Count <= ZhenIndexes.INDEX_0)
                return "未知";
            return zhenData[ZhenIndexes.INDEX_0];
        }
    }
}