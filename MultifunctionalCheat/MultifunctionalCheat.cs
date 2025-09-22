using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace MultifunctionalCheat
{
    
    [BepInPlugin("cs.HoLMod.MultifunctionalCheat.AnZhi20", "HoLMod.MultifunctionalCheat", "1.1.0")]
    public class PluginMain : BaseUnityPlugin
    {
        // 当前插件版本
        private const string CURRENT_VERSION = "1.1.0"; // 与BepInPlugin属性中定义的版本保持一致
        
        private void Awake()
        {
            Logger.LogInfo("多功能修改器已加载！");
            
            // 配置文件路径
            string configFilePath = Path.Combine(Paths.ConfigPath, "cs.HoLMod.MultifunctionalCheat.AnZhi20.cfg");
            
            try
            {
                // 检查是否存在配置文件
                if (File.Exists(configFilePath))
                {
                    // 读取配置文件中的版本信息
                    string loadedVersion = "";
                    using (StreamReader reader = new StreamReader(configFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.StartsWith("## 已加载版本（Loaded Version） = "))
                            {
                                loadedVersion = line.Substring(33).Trim();
                                break;
                            }
                        }
                    }
                    
                    // 检查是否需要更新配置
                    bool isVersionUpdated = loadedVersion != CURRENT_VERSION;
                    
                    // 如果版本更新，删除配置文件
                    if (isVersionUpdated)
                    {
                        Logger.LogInfo($"检测到插件版本更新至 {CURRENT_VERSION}，正在删除旧的配置文件...");
                        File.Delete(configFilePath);
                        Logger.LogInfo("旧配置文件已成功删除。");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("读取或删除配置文件时出错: " + ex.Message);
            }
            
            // 配置游戏倍率（无论是否删除配置文件都会执行，确保配置项存在）
            PluginMain.城中可招门客上限倍数 = base.Config.Bind<int>("倍率调整", "城中可招门客上限倍数", 1, "城中可招募门客的数量=家族等级*2*城中可招门客上限倍数，填1为不修改");
            PluginMain.城中可建民居上限倍数 = base.Config.Bind<int>("倍率调整", "城中可建民居上限倍数", 1, "所有城中可购买民居数量总和=家族等级*城中可建民居上限倍数，填1为不修改");
            PluginMain.城中可建商铺上限倍数 = base.Config.Bind<int>("倍率调整", "城中可建商铺上限倍数", 1, "所有城中可建造商铺数量总和=家族等级*城中可建商铺上限倍数，填1为不修改");
            PluginMain.全角色体力上限倍数 = base.Config.Bind<int>("倍率调整", "全角色体力上限倍数", 1, "全角色体力上限=默认上限*全角色体力上限倍数，填1为不修改");
            PluginMain.城中商铺兑换元宝上限 = base.Config.Bind<int>("倍率调整", "城中商铺兑换元宝上限倍数", 1, "所有城中每个商铺可兑换元宝上限=钱庄等级*10*城中商铺兑换元宝上限倍数，填1为不修改");
            PluginMain.科举人数上限 = base.Config.Bind<int>("倍率调整", "科举人数上限倍数", 1, "科举可选人数上限=家族等级*科举人数上限倍数，填1为不修改");
            PluginMain.最大子嗣上限 = base.Config.Bind<int>("倍率调整", "最大子嗣上限倍数", 1, "每个女性可以生的子嗣上限=1（或2）*最大子嗣上限倍数，填1为不修改");
            PluginMain.最小生育年龄 = base.Config.Bind<int>("数值调整", "最小生育年龄", 18, "女性NPC生育的最小年龄，影响幽会偷情和随机怀孕事件");
            PluginMain.最大生育年龄 = base.Config.Bind<int>("数值调整", "最大生育年龄", 50, "女性NPC生育的最大年龄，影响幽会偷情和随机怀孕事件");

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

        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(H_MinJuPanelA), "OnEnableData")]
        public static void H_MinJuPanelA_OnEnableData(H_MinJuPanelA __instance)
        {
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

        
        public static ConfigEntry<int> 城中可招门客上限倍数;
        public static ConfigEntry<int> 城中可建民居上限倍数;
        public static ConfigEntry<int> 城中可建商铺上限倍数;
        public static ConfigEntry<int> 全角色体力上限倍数;
        public static ConfigEntry<int> 城中商铺兑换元宝上限;
        public static ConfigEntry<int> 科举人数上限;
        public static ConfigEntry<int> 最大子嗣上限;
        public static ConfigEntry<int> 最小生育年龄;
        public static ConfigEntry<int> 最大生育年龄;
    }
}
