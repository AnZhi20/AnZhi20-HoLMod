using System;
using System.Collections.Generic;
using System.Linq;
using YuanAPI;

namespace cs.HoLMod.AddItem;

public class AddItemModule : IAddItemModel
{
    # region IAddItemModel 实现
    
    public event Action OnFilteredPropsChanged;
    public List<int> FilteredProps
    {
        get;
        private set
        {
            field = value;
            OnFilteredPropsChanged?.Invoke();
        }
    }
    
    # endregion

    #region 私有字段

    private Localization.LocalizationInstance _i18N;
    private Localization.LocalizationInstance _vStr;
    
    #endregion

    internal AddItemModule()
    {
        _i18N = Localization.CreateInstance(@namespace: AddItem.LocaleNamespace);
        _vStr = Localization.CreateInstance(@namespace: Localization.VanillaNamespace);

        FilteredProps = ItemData.AllProps;
    }
    
    public void FilterItems(int propClass = -1 , string search = "")
    {
        var list = propClass == -1 ? ItemData.AllProps : ItemData.ClassifiedProps[propClass];
        if (string.IsNullOrEmpty(search))
        {
            FilteredProps = list;
            return;
        }
        search = search.ToLower();
        FilteredProps = list.Where(index => _vStr.t($"Text_AllProp.{index}")
            .ToLower().Contains(search)).ToList();
    }

    public void AddCoins(int count)
    {
        FormulaData.ChangeCoins(count);
        ShowInfo(_i18N.t("Tip.AddCoins.Succeed", count));
    }

    public void AddGold(int count)
    {
        var gold = int.Parse(Mainload.CGNum[1]);
        Mainload.CGNum[1] = (gold + count).ToString();
        ShowInfo(_i18N.t("Tip.AddGold.Succeed", count));
    }
    
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="propId">ID</param>
    /// <param name="propCount">数量</param>
    public void AddProp(int propId, int propCount)
    {
        if (PropTool.AddProp(propId, propCount))
        {
            ShowInfo(_i18N.t($"Tip.AddItem.Succeed", 
                args: [_vStr.t($"Text_AllProp.{propId}"), propCount]));
        }
        else
        {
            ShowWarning(_i18N.t($"Tip.AddItem.Failed", 
                args: [_vStr.t($"Text_AllProp.{propId}"), propCount]));
        }
    }
    
    /// <summary>
    /// 添加话本
    /// </summary>
    /// <param name="bookId">话本ID</param>
    public void AddStoriesBook(int bookId)
    {
        // 验证话本ID是否有效
        if (bookId<0 || bookId >= ItemData.StoriesList.Count)
        {
            ShowWarning(_i18N.t($"Tip.AddStories.None", bookId));
            return;
        }
        
        if (!CheckIfBookExists(bookId))
        {
            Mainload.XiQuHave_Now.Add([
                bookId.ToString(),  // 话本ID
                "",                 // 位置信息（留空）
                "100"
            ]);
            ShowInfo(_i18N.t($"Tip.AddStories.Succeed", args:ItemData.StoriesList[bookId]));
        }
        else
        {
            ShowWarning(_i18N.t($"Tip.AddStories.Failed", args:ItemData.StoriesList[bookId]));
        }
    }
    
    /// <summary>
    /// 检查话本是否已存在
    /// </summary>
    /// <param name="bookId">话本ID</param>
    /// <returns>是否存在</returns>
    private static bool CheckIfBookExists(int bookId)
    {
        if (Mainload.XiQuHave_Now == null || Mainload.XiQuHave_Now.Count == 0)
            return false;

        var bookIdStr = bookId.ToString();
        return Mainload.XiQuHave_Now.Any(book => book[0] == bookIdStr);
    }

    #region 地图添加
    
    // 公共接口方法
    public void AddMansion(int junId, int xianId, string name)
    {
        if (!ValidateJunUnlocked(junId)) return;
        if (CheckLocationOccupied(junId, xianId)) return;
        
        var mansionData = new List<string>
        {
            $"{junId}|{xianId}", // 0 位置
            name,        // 1 名称
            "0", "0", "0", "0", "0", "0", "0", "0", "0",    // 2-10
            "1", "0", "0", "0", "0", "0", "0", "0",         // 11-18
            "0", "0", "0", "0", "0", "0", "0", "0",         // 19-26
            "0", "0", "0", "0", "0", "0", "0", "0",         // 27-34
            "0", "0", "25", "0", "0"                        // 35-39
        };
        
        try
        {
            Mainload.Fudi_now.Add(mansionData);
            Mainload.NewCreateShijia.Add($"{junId}|{xianId}|-1");
            
            ShowInfo($"已在({GetLocationName(junId, xianId)})添加府邸");
            
            Mainload.SceneID = "M|" + (Mainload.Fudi_now.Count - 1);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to add Mansion: {ex.Message}", ex);
        }
    }

    public void AddFarm(int junId, int xianId, string area, string name)
    {
        if (!ValidateJunUnlocked(junId)) return;
        if (CheckLocationOccupied(junId, xianId)) return;
        
        var locationKey = $"{junId}|{xianId}";
        var farmData = new List<string>
        {
            "-1", "null", 
            GetFertilityString(), "1",
            locationKey, area, 
            name, "0", "0", "null",
            (TrueRandom.GetRanom(80) + 20).ToString(),
            "0", "0", "0", "null", "null", "null",
            Mainload.Time_now[0].ToString(),
            "null", "null", "null", "null", "null", "null", 
            "0|0|0",
            (Mainload.Time_now[0] - 1).ToString(),
            "5|3|2"
        };
        
        var index = -1;
        for (var i = 0; i < Mainload.NongZ_now[0].Count; i++)
        {
            if (Mainload.NongZ_now[0][i][0] == "-1" || Mainload.NongZ_now[0][i][4] != locationKey) 
                continue;
            index = i;
            break;
        }

        if (index == -1)
        {
            Mainload.NongZ_now[0].Add(farmData);
            index = Mainload.NongZ_now[0].Count - 1;
        }
        else
        {
            Mainload.NongZ_now[0][index] = farmData;
        }
        
        try
        {
            Mainload.NongZ_now[0].Add(farmData);
            SaveData.SaveBuild("Z", "0", index.ToString(), true);
            Mainload.ZhuangTou_now[0].Add(new List<List<string>>());
            Mainload.NongzHaveData.Add("0" + "|" + index);
            
            ShowInfo($"已在({GetLocationName(junId, xianId)})添加农庄");
            
            Mainload.SceneID = "Z|0|" + index;
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to add Farm: {ex.Message}", ex);
        }
        return;
        
        string GetFertilityString()
        {
            var iArea = int.Parse(area);
            var list = new List<string>();
            for (var i = 0; i < 25; i++)
            {
                list.Add(i<iArea?"1":"0");
            }
            return string.Join("|", list);
        }
    }

    public void AddFief(int junId)
    {
        if (!ValidateJunUnlocked(junId)) return;
        
        if (Mainload.Fengdi_now[++junId][0] == "1")
        {
            ShowInfo($"{ItemData.JunList[junId]}的封地已解锁，无需解锁");
        }
        else
        {
            Mainload.Fengdi_now[junId][0] = "1";
            ShowInfo($"{ItemData.JunList[junId]}的封地已解锁成功");
        }
    }

    public void AddFamily(int junId, int xianId)
    {
        ShowWarning("添加世家功能正在开发中");
    }
    
    private static bool ValidateJunUnlocked(int junId)
    {
        var controlForce = Mainload.CityData_now[junId][0][0].Split('|')[0];
        if (controlForce == "-1") 
            return true;
        
        ShowWarning($"{ItemData.JunList[junId]}未解锁或郡城叛军未清剿，无法添加");
        return false;
    }

    private static bool CheckLocationOccupied(int junId, int xianId)
    {
        var locationKey = $"{junId}|{xianId}";
        if (Mainload.Fudi_now.Any(mansion => mansion[0] == locationKey) ||
            Mainload.NongZ_now[0].Any(farm => farm[0] == "-1" && farm[4] == locationKey) )
        {
            ShowWarning($"{GetLocationName(junId, xianId)}已有其他建筑，添加失败");
            return true;
        }
        return false;
    }
    
    private static string GetLocationName(int junId, int xianId)
    {
        return $"{ItemData.JunList[junId]}-{ItemData.XianList[junId][xianId]}";
    }

    private static void ShowWarning(string message)
    {
        MsgTool.TipMsg(message, TipLv.Warning);
        AddItem.Logger.LogWarning(message);
    }

    private static void ShowInfo(string message)
    {
        MsgTool.TipMsg(message);
        AddItem.Logger.LogInfo(message);
    }

    #endregion
}