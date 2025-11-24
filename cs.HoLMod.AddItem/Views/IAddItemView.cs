using System;

namespace cs.HoLMod.AddItem;

public interface IAddItemView
{
    event Action OnFilterChanged;
    event Action OnCountInputChanged;
    event Action OnNameInputChanged;
    event Action OnAddButton;
    
    bool ShowMenu { get; set; }
    
    // 页面状态
    MenuTab PanelTab { get; set; }
    
    // 货币界面
    CurrencyClass SelectedCurrency { get; set; }
    
    // 物品页面相关
    string SearchText { get; set; }
    PropClass? SelectedPropClass { get; set; }
    int? SelectedPropId { get; set; }
    
    // 话本相关
    int? SelectedBookId { get; set; }
    
    // 马匹相关
    int? SelectedHorseId { get; set; }
    
    // 地图相关
    MapTab SelectedMap { get; set; }
    int SelectedJunId { get; set; }
    int SelectedXianId { get; set; }
    string SelectedArea { get; set; }
    
    // 控制栏
    string CountInput { get; set; }
    string NameInput { get; set; }

    void Initialize(IAddItemModel model);
}