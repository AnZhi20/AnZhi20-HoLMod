using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YuanAPI;

namespace cs.HoLMod.AddItem;

public class AddItemModule : IAddItemModel
{
    // 随机数生成器
    System.Random Random = new System.Random();

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

    /// <summary>
    /// 添加马匹
    /// </summary>
    /// <param name="UIId">UI ID</param>
    public void AddHorse(int UIId)
    {
        if  (int.Parse(Mainload.FamilyData[6]) > 0)
        {
            Mainload.Horse_Have.Add(new List<string>
			{
				$"{UIId}",                          // 马匹的UIID，对应官方的预制件名称
				"1",                                // 马匹的年龄，默认添加的均为1
				(18 + TrueRandom.GetRanom(8)).ToString(),   // 索引2，不知道用来干嘛的，先使用随机整数来赋值，至少还没出现Bug
				"100",                              // 马匹的力量，默认添加的均为100
				"100",                              // 马匹的速度，默认添加的均为100
				"100",                              // 马匹的智商，默认添加的均为100
				"null"                              // 马匹的主人，默认添加的均为null，没有主人的时候为null
			});
            Mainload.FamilyData[6] = (int.Parse(Mainload.FamilyData[6]) - 1).ToString();
            Mainload.HorseData_Enter = "null";
            ShowInfo(_i18N.t($"Tip.AddHorse.Succeed"));

            // 遍历所有马匹，将之前版本错误添加的马匹重新正确设置
            for (int i = 0; i < Mainload.Horse_Have.Count; i++)
            {
                if (Mainload.Horse_Have[i][6] == null)
                {
                    Mainload.Horse_Have[i][6] = "null";
                }
            }
        }
        else
        {
            ShowWarning(_i18N.t($"Tip.AddHorse.Failed"));

            // 遍历所有马匹，将之前版本错误添加的马匹重新正确设置
            for (int i = 0; i < Mainload.Horse_Have.Count; i++)
            {
                if (Mainload.Horse_Have[i][6] == null)
                {
                    Mainload.Horse_Have[i][6] = "null";
                }
            }
        }
        
    }

    #region 地图添加
    
    // 公共接口方法
    public void AddMansion(int junId, int xianId, string name)
    {
        if (!ValidateJunUnlocked(junId)) return;
        if (CheckLocationOccupied(junId, xianId)) return;
        
        var mansionData = new List<string>
        {
            $"{junId}|{xianId}",                            // 0 位置
            name,                                           // 1 名称
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
            
            ShowInfo(_i18N.t("Tip.Farm.AddSucceed", GetLocationName(junId, xianId)));
            
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
            ShowInfo(_i18N.t("Tip.Fief.AlreadyUnlocked", ItemData.JunList[junId]));
        }
        else
        {
            Mainload.Fengdi_now[junId][0] = "1";
            ShowInfo(_i18N.t("Tip.Fief.UnlockSucceed", ItemData.JunList[junId]));
        }
    }

    public void AddClan(int junId, int xianId, string name)
    {
        ShowWarning(_i18N.t("Tip.Clan.Developing"));

		/*
        // 判断所处位置是否为已覆灭的世家遗址
        bool IsClanRuins = false;
        int ClanIndex  = Mainload.ShiJia_Now.Count;  // 世家索引默认为新世家索引
        for (int i = 0; i < Mainload.ShiJia_Now.Count; i++)
        {
            if (Mainload.ShiJia_Now[i][5] == $"{junId}|{xianId}")
            {
                IsClanRuins = true;
                ClanIndex = i;	// 如果为已覆灭的世家遗址，更新世家索引记录
                break;
            }
        }

        // 执行主要的Add逻辑
        string ClanName = name;
        if (IsClanRuins)
        {
            NewShijia_InOldShijia(ClanIndex, ClanName);
            ShowInfo(_i18N.t("Tip.Clan.AddSucceed_A", GetLocationName(junId, xianId)));
        }
        else
        {
            NewShijiaData(junId, xianId, true, ClanName);
            ShowInfo(_i18N.t("Tip.Clan.AddSucceed_B", GetLocationName(junId, xianId)));
        }
		*/
    }

    public void AddCemetery(int junId, int xianId, string area, string name)
    {
        AddFarm(junId, xianId, area, name);
        if (name != "null")
		{
			int num = int.Parse(Mainload.SceneID.Split(new char[]
			{
				'|'
			})[1]);
			int i = int.Parse(Mainload.SceneID.Split(new char[]
			{
				'|'
			})[2]);
			if (i < Mainload.Mudi_now[num].Count)
			{
				Mainload.Mudi_now[num][i] = new List<string>
				{
					Mainload.NongZ_now[num][i][4],
					name,
					Mainload.NongZ_now[num][i][5],
					"0",
					Mainload.Time_now[0].ToString() + "|" + Mainload.Time_now[1].ToString(),
					"0",
					"0",
					"0",
					"0",
					"0"
				};
			}
			else
			{
				int num2 = 0;
				while (i >= Mainload.Mudi_now[num].Count)
				{
					Mainload.Mudi_now[num].Add(new List<string>());
					num2++;
				}
				Mainload.Mudi_now[num][i] = new List<string>
				{
					Mainload.NongZ_now[num][i][4],
					name,
					Mainload.NongZ_now[num][i][5],
					"0",
					Mainload.Time_now[0].ToString() + "|" + Mainload.Time_now[1].ToString(),
					"0",
					"0",
					"0",
					"0",
					"0"
				};
			}
			Mainload.NongZ_now[num][i][0] = "-2";
			Mainload.NongZ_now[num][i][8] = "2";
			if (num == 0)
			{
				Mainload.NewCreateMudi.Add(Mainload.Mudi_now[num][i][0] + "|" + i.ToString());
			}
			for (int j = 0; j < Mainload.NongzHaveData.Count; j++)
			{
				if (Mainload.NongzHaveData[j] == num.ToString() + "|" + i.ToString())
				{
					Mainload.NongzHaveData.RemoveAt(j);
					break;
				}
			}
			Mainload.SceneID = "L|@|$".Replace("@", num.ToString()).Replace("$", i.ToString());
			Mainload.isFNongZPanelOpen = false;
			Mainload.isKuaiToSceneInit = true;
			return;
		}
		Mainload.Tip_Show.Add(new List<string>
		{
			"1",
			AllText.Text_TipShow[66][Mainload.SetData[4]]
		});
        ShowInfo(_i18N.t("Tip.FarmToCemetery", GetLocationName(junId, xianId)));
    }
    
    // 在旧址处崛起新世家
    public static void NewShijia_InOldShijia(int ShijiaIndex, string ClanName)
	{
		if (TrueRandom.GetRanom(150) < 2)
		{
			int num = TrueRandom.GetRanom(25);
			if (num >= 5 && num <= 20)
			{
				num = 1;
			}
			else if (num >= 21 && num <= 24)
			{
				num = 2;
			}
			int shijiaLv = TrueRandom.GetRanom(20) + 10;
			Mainload.ShiJia_Now[ShijiaIndex] = new List<string>
			{
				"0",
				ClanName,       // 将原本官方的RandName.GetXingShiOnly()随机姓氏, 替换为ClanName输入姓氏
				shijiaLv.ToString(),
				"0",
				"0",
				Mainload.ShiJia_Now[ShijiaIndex][5],
				num.ToString(),
				"0",
				"null",
				"0",
				TrueRandom.GetRanom(InitFudiBuild.AllFudidata.Count).ToString(),
				FormulaData.JunNum_Shijia(shijiaLv, num.ToString()),
				"100"
			};
			Mainload.NewCreateShijia.Add(Mainload.ShiJia_Now[ShijiaIndex][5] + "|" + ShijiaIndex.ToString());
			for (int i = 0; i < Mainload.ShiJia_Now.Count; i++)
			{
				if (i != ShijiaIndex)
				{
					string[] array = Mainload.ShiJia_Now[i][8].Split(new char[]
					{
						'|'
					});
					string text = "null";
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].Split(new char[]
						{
							'@'
						})[0] != ShijiaIndex.ToString())
						{
							if (text == "null")
							{
								text = array[j];
							}
							else
							{
								text = text + "|" + array[j];
							}
						}
					}
					Mainload.ShiJia_Now[i][8] = text;
				}
			}
			string[] array2 = Mainload.ShiJia_king[5].Split(new char[]
			{
				'|'
			});
			string text2 = "null";
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k].Split(new char[]
				{
					'@'
				})[0] != ShijiaIndex.ToString())
				{
					if (text2 == "null")
					{
						text2 = array2[k];
					}
					else
					{
						text2 = text2 + "|" + array2[k];
					}
				}
			}
			Mainload.ShiJia_king[5] = text2;
			Mainload.Event_now.Add(new List<string>
			{
				"20",
				"182|2|null|27@" + ShijiaIndex.ToString() + "|0"
			});
		}
	}

    // 新的世家数据
    public static void NewShijiaData(int CityID, int XianID, bool isInit, string ClanName)
	{
        if (Mainload.ShijiaOutXianPoint[CityID].Contains(XianID))
		{
			bool flag = false;
			int shijiaLv;
			if (isInit)
			{
				shijiaLv = TrueRandom.GetRanom(3) + 10 + 5 * Mainload.ShiJia_Now.Count;
			}
			else
			{
				for (int i = 0; i < Mainload.ShiJia_Now.Count; i++)
				{
					if (Mainload.ShiJia_Now[i][5] == CityID.ToString() + "|" + XianID.ToString())
					{
						flag = true;
						break;
					}
				}
				shijiaLv = TrueRandom.GetRanom(3) + 15 + Mainload.ShiJia_Now.Count;
			}
			if (!flag)
			{
				int num = TrueRandom.GetRanom(25);
				if (num >= 5 && num <= 20)
				{
					num = 1;
				}
				else if (num >= 21 && num <= 24)
				{
					num = 2;
				}
				if (Mainload.ShiJia_Now.Count < 2)
				{
					num = 1;
				}
				else if (Mainload.ShiJia_Now.Count == 2)
				{
					num = 2;
				}
				else if (Mainload.ShiJia_Now.Count == 3)
				{
					num = 1;
				}
				else if (Mainload.ShiJia_Now.Count == 4)
				{
					num = 2;
				}
				Mainload.ShiJia_Now.Add(new List<string>
				{
					"0",
					ClanName,       // 将原本官方的RandName.GetXingShiOnly()随机姓氏, 替换为ClanName输入姓氏
					shijiaLv.ToString(),
					"0",
					"0",
					CityID.ToString() + "|" + XianID.ToString(),
					num.ToString(),
					"0",
					"null",
					"0",
					TrueRandom.GetRanom(InitFudiBuild.AllFudidata.Count).ToString(),
					FormulaData.JunNum_Shijia(shijiaLv, num.ToString()),
					"100"
				});
				Mainload.Member_other.Add(new List<List<string>>());
				Mainload.Member_Other_qu.Add(new List<List<string>>());
				FormulaData.New_Member_OneShijia(Mainload.ShiJia_Now.Count - 1);
				if (!isInit)
				{
					Mainload.NewCreateShijia.Add(string.Concat(new string[]
					{
						CityID.ToString(),
						"|",
						XianID.ToString(),
						"|",
						(Mainload.ShiJia_Now.Count - 1).ToString()
					}));
				}
				FormulaData.CreatNewNongZOutFengdi(Mainload.ShiJia_Now.Count - 1, CityID, XianID);
				if (!isInit)
				{
					Mainload.NewCreateNongZ.Add(string.Concat(new string[]
					{
						CityID.ToString(),
						"|",
						XianID.ToString(),
						"|",
						(Mainload.NongZ_now[0].Count - 1).ToString()
					}));
					return;
				}
			}
		}
		else
		{
			bool flag2 = false;
			if (!isInit)
			{
				for (int j = 0; j < Mainload.NongZ_now[0].Count; j++)
				{
					if (Mainload.NongZ_now[0][j][4] == CityID.ToString() + "|" + XianID.ToString())
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				List<int> list = new List<int>();
				if (Mainload.ShiJia_Now.Count > 2)
				{
					list = new List<int>
					{
						Mainload.ShiJia_Now.Count - 3,
						Mainload.ShiJia_Now.Count - 2,
						Mainload.ShiJia_Now.Count - 1
					};
				}
				else if (Mainload.ShiJia_Now.Count > 1)
				{
					list = new List<int>
					{
						Mainload.ShiJia_Now.Count - 2,
						Mainload.ShiJia_Now.Count - 1
					};
				}
				else if (Mainload.ShiJia_Now.Count > 0)
				{
					list = new List<int>
					{
						Mainload.ShiJia_Now.Count - 1
					};
				}
				if (list.Count > 0)
				{
					int ranom = TrueRandom.GetRanom(list.Count);
					if (Mainload.ShiJia_Now[ranom][0] == "0")
					{
						FormulaData.CreatNewNongZOutFengdi(ranom, CityID, XianID);
						if (!isInit)
						{
							Mainload.NewCreateNongZ.Add(string.Concat(new string[]
							{
								CityID.ToString(),
								"|",
								XianID.ToString(),
								"|",
								(Mainload.NongZ_now[0].Count - 1).ToString()
							}));
							return;
						}
					}
				}
				else
				{
					FormulaData.CreatNewNongZOutFengdi(-2, CityID, XianID);
					if (!isInit)
					{
						Mainload.NewCreateNongZ.Add(string.Concat(new string[]
						{
							CityID.ToString(),
							"|",
							XianID.ToString(),
							"|",
							(Mainload.NongZ_now[0].Count - 1).ToString()
						}));
					}
				}
			}
		}
	
        /*
		Mainload.ShiJia_Now.Add(new List<string>
		{
			"0",
			ClanName,       // 将原本官方的RandName.GetXingShiOnly()随机姓氏, 替换为ClanName输入姓氏
			"25",
			"0",
			"0",
			CityID.ToString() + "|" + XianID.ToString(),
			"4",
			"0",
			"null",
			"0",
			TrueRandom.GetRanom(InitFudiBuild.AllFudidata.Count).ToString(),
			FormulaData.JunNum_Shijia(25, "1"),
			"100"
		});
		Mainload.Member_other.Add(new List<List<string>>());
		Mainload.Member_Other_qu.Add(new List<List<string>>());
		FormulaData.CreatNewNongZOutFengdi(Mainload.ShiJia_Now.Count - 1, CityID, XianID);
        */
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