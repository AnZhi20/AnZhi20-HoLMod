using System.Collections.Generic;
using UnityEngine;

namespace cs.HoLMod.NongZ_nowData
{
    /// <summary>
    /// 农庄数据结构及工具类
    /// </summary>
    public class NongZData
    {
        /// <summary>
        /// 农庄数据数组索引定义
        /// </summary>
        public class NongZIndexes
        {
            public const int BELONG_TO_CLAN = 0;     // 所属世家
            public const int UNKNOWN_1 = 1;          // 未知
            public const int LAND_FERTILITY = 2;     // 土地肥力
            public const int RENT = 3;               // 地租
            public const int COORDINATES = 4;        // 坐标（如果是大地图：郡索引|县索引；如果是封地：x坐标|y坐标）
            public const int AREA = 5;               // 面积
            public const int FARM_NAME = 6;          // 农庄名字
            public const int MAX_FARMERS = 7;        // 可居住农户数量
            public const int STATUS = 8;             // 状态
            public const int UNKNOWN_9 = 9;          // 未知
            public const int ENVIRONMENT = 10;       // 环境
            public const int SECURITY = 11;          // 安全
            public const int CONVENIENCE = 12;       // 便捷
            public const int UNKNOWN_13 = 13;        // 未知
            public const int FARM_HEAD_ID = 14;      // 庄头的人物编号
            public const int UNKNOWN_15 = 15;        // 未知
            public const int UNKNOWN_16 = 16;        // 未知
            public const int UNKNOWN_17 = 17;        // 未知
            public const int UNKNOWN_18 = 18;        // 未知
            public const int UNKNOWN_19 = 19;        // 未知
            public const int UNKNOWN_20 = 20;        // 未知
            public const int UNKNOWN_21 = 21;        // 未知
            public const int UNKNOWN_22 = 22;        // 未知
            public const int UNKNOWN_23 = 23;        // 未知
            public const int FARMER_COUNT = 24;      // 农户数量（种植|养殖|手工）
            public const int UNKNOWN_25 = 25;        // 未知
            public const int FARMER_RATIO = 26;      // 农户比例（种植|养殖|手工）
        }

        /// <summary>
        /// 检查Mainload.NongZ_now是否存在并初始化（如果不存在）
        /// </summary>
        public static void EnsureNongZNowExists()
        {
            try
            {
                // 尝试访问Mainload.NongZ_now，如果不存在则创建
                if (Mainload.NongZ_now == null)
                {
                    // 索引0对应大地图的所有农庄数组，索引1-12对应各郡（郡索引的0-11）的所有农庄数组
                    Mainload.NongZ_now = new List<List<List<string>>>();
                    // 初始化13个层级（索引0-12）
                    for (int i = 0; i < 13; i++)
                    {
                        Mainload.NongZ_now.Add(new List<List<string>>());
                    }
                    Debug.Log("已初始化Mainload.NongZ_now");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("初始化Mainload.NongZ_now时发生错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 创建一个新的农庄数据实例
        /// </summary>
        /// <param name="belongToClan">所属世家</param>
        /// <param name="landFertility">土地肥力</param>
        /// <param name="rent">地租</param>
        /// <param name="coordinates">坐标（郡索引|县索引）</param>
        /// <param name="area">面积</param>
        /// <param name="farmName">农庄名字</param>
        /// <param name="maxFarmers">可居住农户数量</param>
        /// <param name="status">状态</param>
        /// <param name="environment">环境</param>
        /// <param name="security">安全</param>
        /// <param name="convenience">便捷</param>
        /// <param name="farmHeadId">庄头的人物编号</param>
        /// <param name="farmerCount">农户数量（种植|养殖|手工）</param>
        /// <param name="farmerRatio">农户比例（种植|养殖|手工）</param>
        /// <returns>农庄数据列表</returns>
        public static List<string> CreateNongZInstance(
            string belongToClan = "0",
            string landFertility = "50",
            string rent = "10",
            string coordinates = "0|0",
            string area = "10",
            string farmName = "未知农庄",
            string maxFarmers = "10",
            string status = "0",
            string environment = "50",
            string security = "50",
            string convenience = "50",
            string farmHeadId = "0",
            string farmerCount = "0|0|0",
            string farmerRatio = "0|0|0")
        {
            List<string> farmData = new List<string>();
            // 填充0-26的索引，确保数组长度足够
            for (int i = 0; i < 27; i++)
            {
                farmData.Add("0"); // 默认值
            }

            farmData[NongZIndexes.BELONG_TO_CLAN] = belongToClan;    // 0: 所属世家
            farmData[NongZIndexes.LAND_FERTILITY] = landFertility;    // 2: 土地肥力
            farmData[NongZIndexes.RENT] = rent;                      // 3: 地租
            farmData[NongZIndexes.COORDINATES] = coordinates;        // 4: 坐标（郡索引|县索引）
            farmData[NongZIndexes.AREA] = area;                      // 5: 面积
            farmData[NongZIndexes.FARM_NAME] = farmName;             // 6: 农庄名字
            farmData[NongZIndexes.MAX_FARMERS] = maxFarmers;         // 7: 可居住农户数量
            farmData[NongZIndexes.STATUS] = status;                  // 8: 状态
            farmData[NongZIndexes.ENVIRONMENT] = environment;        // 10: 环境
            farmData[NongZIndexes.SECURITY] = security;              // 11: 安全
            farmData[NongZIndexes.CONVENIENCE] = convenience;        // 12: 便捷
            farmData[NongZIndexes.FARM_HEAD_ID] = farmHeadId;        // 14: 庄头的人物编号
            farmData[NongZIndexes.FARMER_COUNT] = farmerCount;       // 24: 农户数量（种植|养殖|手工）
            farmData[NongZIndexes.FARMER_RATIO] = farmerRatio;       // 26: 农户比例（种植|养殖|手工）

            return farmData;
        }

        /// <summary>
        /// 获取指定层级和索引的农庄数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="farmIndex">农庄索引</param>
        /// <returns>农庄数据，如果索引无效则返回null</returns>
        public static List<string> GetNongZByIndex(int levelIndex, int farmIndex)
        {
            EnsureNongZNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count)
            {
                if (farmIndex >= 0 && farmIndex < Mainload.NongZ_now[levelIndex].Count)
                {
                    return Mainload.NongZ_now[levelIndex][farmIndex];
                }
            }
            return null;
        }

        /// <summary>
        /// 添加新的农庄到指定层级
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <param name="farmData">农庄数据</param>
        /// <returns>添加是否成功</returns>
        public static bool AddNongZ(int levelIndex, List<string> farmData)
        {
            try
            {
                EnsureNongZNowExists();
                
                if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count)
                {
                    if (farmData != null && farmData.Count >= 27) // 确保数据结构完整性
                    {
                        Mainload.NongZ_now[levelIndex].Add(farmData);
                        // 尝试获取农庄名称进行日志记录
                        string farmName = farmData[NongZIndexes.FARM_NAME];
                        Debug.Log($"已添加新农庄: {farmName} (层级索引: {levelIndex})");
                        return true;
                    }
                    else
                    {
                        Debug.LogError("添加农庄失败：数据结构不完整");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError("添加农庄失败：层级索引无效");
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("添加农庄时发生错误: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取指定农庄的简要信息字符串
        /// </summary>
        /// <param name="farmData">农庄数据</param>
        /// <returns>农庄简要信息</returns>
        public static string GetNongZSummary(List<string> farmData)
        {
            if (farmData == null || farmData.Count < 27)
            {
                return "无效的农庄数据";
            }
            
            string farmName = farmData[NongZIndexes.FARM_NAME];
            string area = farmData[NongZIndexes.AREA];
            string landFertility = farmData[NongZIndexes.LAND_FERTILITY];
            string rent = farmData[NongZIndexes.RENT];
            string coordinates = farmData[NongZIndexes.COORDINATES];
            string farmerCount = farmData[NongZIndexes.FARMER_COUNT];
            string farmHeadId = farmData[NongZIndexes.FARM_HEAD_ID];
            
            return $"{farmName} (面积: {area}, 肥力: {landFertility}, 地租: {rent}, 坐标: {coordinates}, 农户数: {farmerCount}, 庄头ID: {farmHeadId})";
        }

        /// <summary>
        /// 加载指定层级的农庄数据
        /// </summary>
        /// <param name="levelIndex">层级索引（0:大地图, 1-12:各郡）</param>
        /// <returns>农庄数据数组</returns>
        public static List<List<string>> LoadNongZDataByLevel(int levelIndex)
        {
            EnsureNongZNowExists();
            
            if (levelIndex >= 0 && levelIndex < Mainload.NongZ_now.Count)
            {
                return Mainload.NongZ_now[levelIndex];
            }
            return new List<List<string>>();
        }

        /// <summary>
        /// 获取指定农庄的名称
        /// </summary>
        /// <param name="farmData">农庄数据</param>
        /// <returns>农庄名称</returns>
        public static string GetNongZName(List<string> farmData)
        {
            if (farmData != null && farmData.Count > NongZIndexes.FARM_NAME)
            {
                return farmData[NongZIndexes.FARM_NAME];
            }
            return "未知农庄";
        }

        /// <summary>
        /// 获取指定农庄的坐标信息
        /// </summary>
        /// <param name="farmData">农庄数据</param>
        /// <returns>坐标数组 [郡索引, 县索引]</returns>
        public static int[] GetNongZCoordinates(List<string> farmData)
        {
            int[] coordinates = new int[] { -1, -1 };
            if (farmData != null && farmData.Count > NongZIndexes.COORDINATES)
            {
                string coordinatesStr = farmData[NongZIndexes.COORDINATES];
                string[] parts = coordinatesStr.Split('|');
                
                if (parts.Length >= 2)
                {
                    int.TryParse(parts[0], out coordinates[0]);
                    int.TryParse(parts[1], out coordinates[1]);
                }
            }
            return coordinates;
        }
    }
}