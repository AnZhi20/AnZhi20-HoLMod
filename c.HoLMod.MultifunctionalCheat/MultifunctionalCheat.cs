using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MultifunctionalCheat
{
    // 插件信息类
    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "cs.HoLMod.MultifunctionalCheat.AnZhi20";
        public const string PLUGIN_NAME = "HoLMod.MultifunctionalCheat";
        public const string PLUGIN_VERSION = "1.7.0";
        public const string PLUGIN_CONFIG = "cs.HoLMod.MultifunctionalCheat.AnZhi20.cfg";
    }
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class PluginMain : BaseUnityPlugin
    {
        // 当前插件版本
        public static readonly string CURRENT_VERSION = PluginInfo.PLUGIN_VERSION; // 与BepInPlugin属性中定义的版本保持一致
        
        private void Awake()
        {
            Logger.LogInfo("多功能修改器已加载！当前版本：" + CURRENT_VERSION);
            Logger.LogInfo("The multifunctional modifier has been loaded! Current version:" + CURRENT_VERSION);
            
            // 版本检测逻辑
            bool isVersionUpdated = false;
            string loadedVersion = null;
            
            // 尝试从配置中读取已加载的版本
            ConfigEntry<string> loadedVersionEntry = base.Config.Bind<string>("内部配置（Internal Settings）", "已加载版本（Loaded Version）", CURRENT_VERSION, "用于跟踪插件版本，请勿手动修改");
            loadedVersion = loadedVersionEntry.Value;
            
            // 检查版本是否更新
            if (loadedVersion != CURRENT_VERSION)
            {
                Logger.LogInfo("检测到插件版本更新：" + loadedVersion + " -> " + CURRENT_VERSION);
                Logger.LogInfo("Plugin version update detected: " + loadedVersion + " -> " + CURRENT_VERSION);
                isVersionUpdated = true;
                
                // 清除旧配置
                base.Config.Clear();
                Logger.LogInfo("已清除旧配置");
                Logger.LogInfo("Old configuration has been cleared");
            }
            
            // 配置游戏倍率（无论是否删除配置文件都会执行，确保配置项存在）
            PluginMain.城中可招门客上限倍数 = base.Config.Bind<int>("倍率调整（Magnification）", "城中可招门客上限倍数（CityMaxRecruitableCustomersMultiplier）", 1, "城中可招募门客的数量=家族等级*2*城中可招门客上限倍数，填1为不修改（The number of potential recruits in the city = family level * 2 * CityMaxRecruitableCustomersMultiplier , default: 1）");
            PluginMain.城中可建民居上限倍数 = base.Config.Bind<int>("倍率调整（Magnification）", "城中可建民居上限倍数（CityMaxBuildableHousesMultiplier）", 1, "所有城中可购买民居数量总和=家族等级*城中可建民居上限倍数，填1为不修改（The total number of potential buildings in the city = family level * CityMaxBuildableHousesMultiplier , default: 1）");
            PluginMain.城中可建商铺上限倍数 = base.Config.Bind<int>("倍率调整（Magnification）", "城中可建商铺上限倍数（CityMaxBuildableShopsMultiplier）", 1, "所有城中可建造商铺数量总和=家族等级*城中可建商铺上限倍数，填1为不修改（The total number of potential shops in the city = family level * CityMaxBuildableShopsMultiplier , default: 1）");
            PluginMain.全角色体力上限倍数 = base.Config.Bind<int>("倍率调整（Magnification）", "全角色体力上限倍数（AllCharactersMaxHealthMultiplier）", 1, "全角色体力上限=默认上限*全角色体力上限倍数，填1为不修改（The maximum health of all characters = default health * AllCharactersMaxHealthMultiplier , default: 1）");
            PluginMain.城中商铺兑换元宝上限 = base.Config.Bind<int>("倍率调整（Magnification）", "城中商铺兑换元宝上限倍数（CityShopExchangeGoldMultiplier）", 1, "所有城中每个商铺可兑换元宝上限=钱庄等级*10*城中商铺兑换元宝上限倍数，填1为不修改（The maximum gold exchange per shop in the city = bank level * 10 * CityShopExchangeGoldMultiplier , default: 1）");
            PluginMain.科举人数上限 = base.Config.Bind<int>("倍率调整（Magnification）", "科举人数上限倍数（ExaminationNumMaxMultiplier）", 1, "科举可选人数上限=家族等级*科举人数上限倍数，填1为不修改（The maximum number of candidates for the exam = family level * ExaminationNumMaxMultiplier , default: 1）");
            PluginMain.最大子嗣上限 = base.Config.Bind<int>("倍率调整（Magnification）", "最大子嗣上限倍数（MaxOffspringNumMultiplier）", 1, "每个女性可以生的子嗣上限=1（或2）*最大子嗣上限倍数，填1为不修改（The maximum number of children that each woman can have = 1（or 2） * MaxOffspringNumMultiplier , default: 1）");
            
            // 折扣相关配置
            PluginMain.民居售价折扣 = base.Config.Bind<float>("折扣（Discount）", "民居售价折扣（HousePriceMultiplier）", 10, "每个民居的售价=默认售价*民居售价折扣/10，填10为不修改（The price of each house = default price * HousePriceMultiplier /10, default: 10）");
            PluginMain.府邸销售折扣 = base.Config.Bind<float>("折扣（Discount）", "府邸销售折扣（HouseSaleMultiplier）", 10, "每个府邸的销售价格=默认售价*府邸销售折扣/10，填10为不修改（The price of each mansion = default price * HouseSaleMultiplier /10, default: 10）");

            // 封地税收倍率配置
            PluginMain.封地农业税收倍数 = base.Config.Bind<float>("封地参数（FengDiParameters）", "封地农业税收（AgriculturalTaxMultiplier）", 1, "每个封地的农业税收=原本计算逻辑*封地农业税收倍数，填1为不修改（Agricultural tax revenue for each fiefdom = Original calculation logic * AgriculturalTaxMultiplier , default: 1）");
            PluginMain.封地人民税收倍数 = base.Config.Bind<float>("封地参数（FengDiParameters）", "封地人民税收（ResidentTaxMultiplier）", 1, "每个封地的人民税收=原本计算逻辑*封地人民税收倍数，填1为不修改（Resident tax revenue for each fiefdom = Original calculation logic * ResidentTaxMultiplier , default: 1）");
            PluginMain.封地商业税收倍数 = base.Config.Bind<float>("封地参数（FengDiParameters）", "封地商业税收（CommercialTaxMultiplier）", 1, "每个封地的商业税收=原本计算逻辑*封地商业税收倍数，填1为不修改（Commercial tax revenue for each fiefdom = Original calculation logic * CommercialTaxMultiplier , default: 1）");
            
            // 生育年龄相关配置
            PluginMain.最小生育年龄 = base.Config.Bind<int>("怀孕参数（PregnancyParameters）", "最小生育年龄（MinPregnancyAge）", 18, "女性可怀孕的最小年龄，最好不要低于18岁，人不能至少不应该（The minimum age at which women can become pregnant,it is best not to be under 18 years old, as people should not be at least）");
            PluginMain.最大生育年龄 = base.Config.Bind<int>("怀孕参数（PregnancyParameters）", "最大生育年龄（MaxPregnancyAge）", 40, "女性可怀孕的最大年龄（The maximum age at which women can become pregnant）");
            
            // 保存当前版本号
            base.Config.Bind("内部配置（Internal Settings）", "已加载版本（Loaded Version）", CURRENT_VERSION, "用于跟踪插件版本，请勿手动修改");
            
            // 保存配置文件
            base.Config.Save();
            
            Harmony.CreateAndPatchAll(typeof(PluginMain), null);
        }

       
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "MenkeNumMax")]
        public static bool MenkeNumMax(FormulaData __instance, ref int __result)
        {
            __result = int.Parse(Mainload.FamilyData[2]) * 2 * PluginMain.城中可招门客上限倍数.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "MinjuNumMaxPerCity")]
        public static bool MinjuNumMaxPerCity(FormulaData __instance, ref int __result)
        {
            __result = int.Parse(Mainload.FamilyData[2]) * PluginMain.城中可建民居上限倍数.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "ShopNumMaxPerCity")]
        public static bool ShopNumMaxPerCity(FormulaData __instance, ref int __result)
        {
            __result = int.Parse(Mainload.FamilyData[2]) * PluginMain.城中可建商铺上限倍数.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "GetTiliMax")]
        public static bool GetTiliMax(FormulaData __instance, ref int __result, ref int OldNum)
        {
            bool flag = OldNum <= 30;
            int num;
            if (flag)
            {
                num = OldNum;
            }
            else
            {
                bool flag2 = OldNum > 30 && OldNum <= 50;
                if (flag2)
                {
                    num = 60 - OldNum;
                }
                else
                {
                    num = 10 + Mathf.FloorToInt((float)(50 - OldNum) / 10f) * 2;
                }
            }
            bool flag3 = num <= 0;
            if (flag3)
            {
                num = 2;
            }
            __result = num * PluginMain.全角色体力上限倍数.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "YunBaoToCoinsNum")]
        public static bool YunBaoToCoinsNum(FormulaData __instance, ref int __result, ref int ShopLv)
        {
            __result = ShopLv * 10 * PluginMain.城中商铺兑换元宝上限.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "GetCanKejuNumMax")]
        public static bool GetCanKejuNumMax(FormulaData __instance, ref int __result)
        {
            __result = int.Parse(Mainload.FamilyData[2]) * PluginMain.科举人数上限.Value;
            return false;
        }

        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "GetChildMax")]
        public static bool GetChildMax(FormulaData __instance, ref int __result, ref string MemberID)
        {
            int num = 9;
            int num2;
            bool flag = int.TryParse(MemberID.Substring(MemberID.Length - 1, 1), out num2);
            if (flag)
            {
                num = num2 % 10;
            }
            bool flag2 = num == 0 || num == 1 || num == 2 || num == 3 || num == 4;
            int num3;
            if (flag2)
            {
                num3 = 2;
            }
            else
            {
                num3 = 1;
            }
            __result = num3 * PluginMain.最大子嗣上限.Value;
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Mainload), "LoadData")]
        public static void UpdatePregnancyAge()
        {
            Mainload.OldShengYu = new List<int> { PluginMain.最小生育年龄.Value, PluginMain.最大生育年龄.Value };
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(H_MinJuPanelA), "OnEnableData")]
        public static void H_MinJuPanelA_OnEnableData(H_MinJuPanelA __instance)
        {
            // 获取 H_MinJuPanelA 实例中的 BuyMunjuCost 字段
            // 通过反射获取私有字段
            var fieldInfo = typeof(H_MinJuPanelA).GetField("BuyMunjuCost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldInfo != null)
            {
                // 获取原始值
                int originalCost = (int)fieldInfo.GetValue(__instance);
                
                // 应用折扣计算
                float discountMultiplier = PluginMain.民居售价折扣.Value / 10f;
                int discountedCost = Mathf.FloorToInt((float)originalCost * discountMultiplier);
                
                // 确保价格不低于1
                discountedCost = Math.Max(1, discountedCost);
                
                // 设置新值
                fieldInfo.SetValue(__instance, discountedCost);
            }
        }
        
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FormulaData), "FengdiGetCoins")]
        public static bool FengdiGetCoins_Prefix(int FengdiIndex, int RenNum, float NongNum, float ShangNum, ref int __result)
        {
            // 重现原始方法的计算逻辑，但应用倍数调整
            string[] splitResult = Mainload.Fengdi_now[FengdiIndex][2].Split(new char[] { '|' });
            int num = int.Parse(splitResult[0]);
            int num2 = int.Parse(splitResult[1]);
            int num3 = int.Parse(splitResult[2]);
            
            // 计算调整后的税收，分别应用相应的倍数
            int num4 = Mathf.CeilToInt(NongNum / 100f * 100000f * ((float)num / 100f) * PluginMain.封地农业税收倍数.Value);
            int num5 = Mathf.CeilToInt((float)RenNum * 50f * ((float)num2 / 100f) * PluginMain.封地人民税收倍数.Value);
            int num6 = Mathf.CeilToInt(ShangNum / 100f * 200000f * ((float)num3 / 100f) * PluginMain.封地商业税收倍数.Value);
            
            // 设置结果并跳过原始方法
            __result = num4 + num5 + num6;
            return false;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FormulaData), "FudiPrice")]
        public static void FudiPrice_Postfix(ref List<int> __result)
        {
            if (__result != null && __result.Count >= 2)
            {
                // 应用府邸销售折扣到item和item2
                float discountMultiplier = PluginMain.府邸销售折扣.Value / 10f;
                __result[0] = Mathf.FloorToInt((float)__result[0] * discountMultiplier);
                __result[1] = Mathf.FloorToInt((float)__result[1] * discountMultiplier);
                
                // 确保价格不低于1
                __result[0] = Math.Max(1, __result[0]);
                __result[1] = Math.Max(1, __result[1]);
            }
        }
        
        public static ConfigEntry<int> 城中可招门客上限倍数;
        public static ConfigEntry<int> 城中可建民居上限倍数;
        public static ConfigEntry<int> 城中可建商铺上限倍数;
        public static ConfigEntry<int> 全角色体力上限倍数;
        public static ConfigEntry<int> 城中商铺兑换元宝上限;
        public static ConfigEntry<int> 科举人数上限;
        public static ConfigEntry<int> 最大子嗣上限;
        public static ConfigEntry<int> 最小生育年龄;
        public static ConfigEntry<int> 最大生育年龄;
        public static ConfigEntry<float> 民居售价折扣;
        public static ConfigEntry<float> 封地农业税收倍数;
        public static ConfigEntry<float> 封地人民税收倍数;
        public static ConfigEntry<float> 封地商业税收倍数;
        public static ConfigEntry<float> 府邸销售折扣;
        

        
    }
}
