using System;
using System.Collections.Generic;
using UnityEngine;
using YuanAPI;

namespace cs.HoLMod.AddItem;

public class AddItemController
{
    private IAddItemModel _model;
    private IAddItemView _view;

    private Localization.LocalizationInstance _i18N;
    
    internal AddItemController(IAddItemModel model, IAddItemView view)
    {
        _model = model;
        _view = view;

        _i18N = Localization.CreateInstance(@namespace: AddItem.LocaleNamespace);
        
        BindView();
    }
    
    private void BindView()
    {
        _view.OnFilterChanged += FilteredProps;
        _view.OnCountInputChanged += CheckCountInput;
        _view.OnAddButton += CallAddButton;
    }

    private void FilteredProps()
    {
        var selectClass = _view.SelectedPropClass == null ? -1 : (int)_view.SelectedPropClass;
        _model.FilterItems(selectClass, _view.SearchText);
    }

    private void CheckCountInput()
    {
        if (!int.TryParse(_view.CountInput, out var count))
        {
            _view.CountInput = "1";
            return;
        }
        
        switch (_view.PanelTab)
        {
            case MenuTab.Currency:
                count = _view.SelectedCurrency switch
                {
                    CurrencyClass.Coins => Mathf.Clamp(count, 1, 1000000000),   // 10亿
                    CurrencyClass.Gold => Mathf.Clamp(count, 1, 1000000),       // 100万
                    _ => throw new ArgumentOutOfRangeException(),
                };
                _view.CountInput = count.ToString();
                break;
            case MenuTab.Items:
                var storage = int.Parse(Mainload.FamilyData[5]);
                storage = storage < 1 ? 1 : storage;
                _view.CountInput = Mathf.Clamp(count, 1, storage).ToString();
                break;
            case MenuTab.Stories:
                _view.CountInput = "1";
                break;
            case MenuTab.Map:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CallAddButton()
    {
        switch (_view.PanelTab)
        {
            case MenuTab.Currency:
                CheckCountInput();
                WhenAddCurrency();
                break;
            case MenuTab.Items:
                CheckCountInput();
                WhenAddItem();
                break;
            case MenuTab.Stories:
                CheckCountInput();
                WhenAddStories();
                break;
            case MenuTab.Map:
                WhenAddMaps();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    #region 添加按键处理
    
    private void WhenAddCurrency()
    {
        switch (_view.SelectedCurrency)
        {
            case CurrencyClass.Coins:
                _model.AddCoins(int.Parse(_view.CountInput));
                break;
            case CurrencyClass.Gold:
                _model.AddGold(int.Parse(_view.CountInput));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    private void WhenAddItem()
    {
        if (_view.SelectedPropId == null)
        {
            MsgTool.TipMsg(_i18N.t("Tip.SelectedNone"));
            return;
        }
        _model.AddProp((int)_view.SelectedPropId, int.Parse(_view.CountInput));
    }
    private void WhenAddStories()
    {
        if (_view.SelectedBookId == null)
        {
            MsgTool.TipMsg(_i18N.t("Tip.SelectedNone"));
            return;
        }
        _model.AddStoriesBook((int)_view.SelectedBookId);
    }

    private void WhenAddMaps()
    {
        var junId = _view.SelectedJunId;
        var xianId = _view.SelectedXianId;
        var area = _view.SelectedArea;
        switch (_view.SelectedMap)
        {
            case MapTab.Mansion:
                _model.AddMansion(junId, xianId, GetName());
                break;
            case MapTab.Farm:
                if (!CheckArea(area))
                {
                    MsgTool.TipMsg("无效面积");
                    break;
                }
                _model.AddFarm(junId, xianId, area, GetName());
                break;
            case MapTab.Fief:
                _model.AddFief(junId);
                break;
            case MapTab.Family:
                _model.AddFamily(junId, xianId);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private List<string> _mapArea = ["4", "9", "16", "25"];
    private bool CheckArea(string area)
    {
        return _mapArea.Contains(area);
    }

    private string GetName()
    {
        var name = _view.NameInput;
        if (!string.IsNullOrWhiteSpace(name)) 
            return name;
        
        switch (_view.SelectedMap)
        {
            case MapTab.Mansion:
                name = RandName.GetFudiName();
                break;
            case MapTab.Farm:
                name = RandName.GetNongZName();
                break;
            case MapTab.Fief:
            case MapTab.Family:
            default:
                throw new ArgumentOutOfRangeException();
        }
        return name;
    }
    
    #endregion
}