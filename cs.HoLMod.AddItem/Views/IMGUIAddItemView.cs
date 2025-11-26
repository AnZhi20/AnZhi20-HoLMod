using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using YuanAPI;

namespace cs.HoLMod.AddItem;

public class IMGUIAddItemView : MonoBehaviour, IAddItemView
{
    // 窗口设置
    private Rect _windowRect;
    private Vector2 _scrollPosition; // 滚动位置
    private float _scaleFactor = 1.0f; // 分辨率缩放因子
    
    #region 实现 IAddItemView
    
    // 事件
    public event Action OnFilterChanged;
    public event Action OnCountInputChanged;
    public event Action OnNameInputChanged;
    public event Action OnAddButton;
    
    public bool ShowMenu { get; set; }
    
    // 页面状态
    public MenuTab PanelTab { get; set; } = MenuTab.Items;
    
    // 货币界面
    public CurrencyClass SelectedCurrency { get; set; } = CurrencyClass.Coins;
    
    // 物品页面相关
    public string SearchText { get; set; } = "";
    public PropClass? SelectedPropClass { get; set; }
    public int? SelectedPropId { get; set; }
    public int? HoveredPropId { get; set; }
    
    // 话本相关
    public int? SelectedBookId { get; set; }

    // 马匹相关
    public int? SelectedHorseId { get; set; }
    
    // 地图相关
    public MapTab SelectedMap { get; set; } = MapTab.Mansion;
    public int SelectedJunId { get; set; }
    public int SelectedXianId { get; set; }
    public string SelectedArea { get; set; } = "16";
    
    // 控制栏
    public string CountInput
    {
        get;
        set
        {
            if (field == value) 
                return;
            field = value;
            OnCountInputChanged?.Invoke();
        }
    } = "1";
    public string NameInput
    {
        get;
        set
        {
            if (field == value) 
                return;
            field = value;
            OnNameInputChanged?.Invoke();
        }
    } = "";

    #endregion

    private Localization.LocalizationInstance _i18N;
    private Localization.LocalizationInstance _vStr;
    
    private IAddItemModel _model;

    public void Initialize(IAddItemModel model)
    {
        _i18N = Localization.CreateInstance(@namespace: AddItem.LocaleNamespace);
        _vStr = Localization.CreateInstance(@namespace: Localization.VanillaNamespace);
        
        _model = model;
        
        UpdateResolutionSettings();
    }


    
    private void Update()
    {
        // 按F2键切换窗口显示
        if (Input.GetKeyDown(KeyCode.F2))
        {
            ShowMenu = !ShowMenu;

            if (ShowMenu)
            {
                UpdateResolutionSettings();
                // 关闭地图面板，避免误操作
                Mainload.isMapPanelOpen = false;
            }
            else
            {
                // 关闭窗口时清除悬浮状态
                HoveredPropId = null;
            }
            
            AddItem.Logger.LogInfo($"物品添加器窗口已{(ShowMenu?"打开":"关闭")}" );
        }
        
        // 阻止游戏输入当窗口显示时（游戏会继续运行，但不允许操作游戏界面）
        if (!ShowMenu) 
            return;
        
        if (Input.mouseScrollDelta.y != 0 || // 阻止鼠标滚轮
            Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) && // 阻止鼠标点击
            Input.anyKeyDown && !Input.GetKeyDown(KeyCode.F2)) // 阻止键盘输入（保留F2键用于关闭窗口）
        {
            Input.ResetInputAxes();
        }
        

    }
    

    
   // 绘制物品按钮并直接处理鼠标悬浮检测
        private void DrawItemButton(int propId)
        {
            string itemName = _vStr.t($"Text_AllProp.{propId}");
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(itemName), GUI.skin.button);
            
            // 处理按钮点击
            if (GUI.Button(buttonRect, itemName))
            {
                SelectedPropId = propId;
            }
            
            // 直接在GUI渲染时检测鼠标悬浮，这能正确处理滚动视图中的坐标
            Event currentEvent = Event.current;
            if (currentEvent != null && (currentEvent.type == EventType.MouseMove || currentEvent.type == EventType.Repaint))
            {
                // 检查鼠标是否在当前按钮上
                if (buttonRect.Contains(currentEvent.mousePosition))
                {
                    HoveredPropId = propId;
                }
            }
        }
    
    private static float _defaultWidth = 800f;
    private static float _defaultHeight = 1000f;
    // 分辨率更新处理方法
    private void UpdateResolutionSettings()
    {
        // 获取当前屏幕分辨率
        var currentWidth = (float)Screen.width;
        var currentHeight = (float)Screen.height;

        const float refW = 1920f;
        const float refH = 1080f;
        
        _scaleFactor = Mathf.Min(currentWidth / refW, currentHeight / refH);
        _windowRect = new Rect(150, 50, _defaultWidth, _defaultHeight);

        AddItem.Logger.LogInfo($"当前分辨率: {currentWidth}x{currentHeight}，缩放因子: {_scaleFactor}");
    }
    
    private void OnGUI()
    {
        if (!ShowMenu) return;
        
        // 在每次GUI渲染周期开始时重置HoveredPropId，确保鼠标不在任何按钮上时悬浮窗会隐藏
        if (Event.current.type == EventType.Repaint)
        {
            HoveredPropId = null;
        }
        
        // 保存窗口背景色并设置为半透明
        var originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);

        {
            // 显示一个半透明的背景遮罩，防止操作游戏界面
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            GUI.color = new Color(0, 0, 0, 0.1f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.EndGroup();

            // 应用缩放因子
            var guiMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
                new Vector3(_scaleFactor, _scaleFactor, 1f));

            // 创建窗口（逻辑尺寸）
            _windowRect = GUI.Window(0, _windowRect, DrawWindow, "", GUI.skin.window);
            
            // 恢复原始矩阵
            GUI.matrix = guiMatrix;
        }
        
        // 恢复原始背景色
        GUI.backgroundColor = originalBackgroundColor;
    }
    
    private void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        // 固定标题置于最上方
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        GUILayout.Label($"{_i18N.t("Mod.Name")} v{AddItem.VERSION} by:{_i18N.t("Mod.Author")}");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        
        // 菜单页面标签
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        Enum.GetNames(typeof(MenuTab)).ForEach((key, index) =>
        {
            if (GUILayout.Button($"{_i18N.t("MenuTab." + key)}", GUILayout.ExpandWidth(true)))
                PanelTab = (MenuTab)index;
        });
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        
        // 根据当前模式显示不同内容
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true),GUILayout.ExpandHeight(true));
        switch (PanelTab)
        {
            case MenuTab.Currency:
                DrawAddCurrency();
                break;
            case MenuTab.Items:
                DrawAddItem();
                break;
            case MenuTab.Stories:
                DrawAddStories();
                break;
            case MenuTab.Horses:
                DrawAddHorses();
                break;
            case MenuTab.Map:
                DrawAddMaps();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        GUILayout.EndVertical();
        
        // 底部
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        
        GUILayout.Space(10f);
        DrawAddButton();
        GUILayout.Space(10f);
        
        // 使用说明
        {
            GUILayout.BeginVertical();
            
            GUILayout.Label(_i18N.t("Info.Description"), GUI.skin.box);
            for (var i = 1; i <= 6; i++)
                GUILayout.Label(_i18N.t($"Description.{i}"));
            
            GUILayout.EndVertical();
        }
        GUILayout.EndVertical();
        
        GUILayout.EndVertical();
        
        // 允许拖动窗口
        GUI.DragWindow(new Rect(0, 0, _windowRect.width, _windowRect.height));
    }

    private void DrawAddCurrency()
    {
        // 货币类型选择
        GUILayout.Label(_i18N.t("Info.Category"), GUILayout.Width(160f));
        
        GUILayout.BeginVertical();
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition,
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_i18N.t("CurrencyClass.Coins"), GUILayout.Width(160f)))
        {
            SelectedCurrency = CurrencyClass.Coins;
        }
        if (GUILayout.Button(_i18N.t("CurrencyClass.Gold"), GUILayout.Width(160f)))
        {
            SelectedCurrency = CurrencyClass.Gold;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        
        GUILayout.Space(10f);
        
        // 当前货币数量显示
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.Label(_i18N.t("Info.CurrencyNow.Coins",  FormulaData.GetCoinsNum()),
            GUILayout.ExpandWidth(true));
        GUILayout.Label(_i18N.t("Info.CurrencyNow.Gold", args: Mainload.CGNum[1]), 
            GUILayout.ExpandWidth(true));
        GUILayout.EndVertical();
    }

    private void DrawAddItem()
    {
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_i18N.t("Info.Search"), GUILayout.Width(160f));
            var newSearchText = GUILayout.TextField(SearchText, GUILayout.ExpandWidth(true));
            if (newSearchText != SearchText)
            {
                SearchText = newSearchText;
                OnFilterChanged?.Invoke();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10f);
        
        // 物品分类按钮
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_i18N.t("Info.Category")+
                            (SelectedPropClass != null ? _i18N.t($"PropClass.{SelectedPropClass.ToString()}") : ""),
                GUILayout.Width(160f));
            if (GUILayout.Button(_i18N.t("Button.Clear"), GUILayout.Width(120f)))
            {
                SearchText = "";
                SelectedPropClass = null;
                OnFilterChanged?.Invoke();
            }
            GUILayout.EndHorizontal();

            // 分类按钮布局 - 分多行显示
            Enum.GetNames(typeof(PropClass)).ForEach((cate, index) =>
            {
                if (index == 0 || index % 5 == 2) 
                    GUILayout.BeginHorizontal();

                if (GUILayout.Button(_i18N.t($"PropClass.{cate}"), 
                     GUILayout.Width(index < 2 ? 285f : 140f)))
                {
                    SelectedPropClass = (PropClass)Enum.Parse(typeof(PropClass), cate);
                    OnFilterChanged?.Invoke();
                }
                
                if (index % 5 == 1) 
                    GUILayout.EndHorizontal();
                
                if (index == 1)
                    GUILayout.Space(10f);
            });
        }
        
        GUILayout.Space(10f);
         
        // 物品列表滚动栏
        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            // 创建物品列表
            if (_model.FilteredProps.Count > 0)
            {
                _model.FilteredProps.ForEach(propId =>
                {
                    DrawItemButton(propId);
                });
            }
            else
            {
                GUILayout.Label(_i18N.t("Error.NoProp"), GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }

    private void DrawAddStories()
    {
        // 话本模式
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, 
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
        // 创建话本列表，根据语言显示对应文本
        ItemData.StoriesList.ForEach((book, index) =>
        {
            if (GUILayout.Button(book, GUILayout.ExpandWidth(true)))
            {
                SelectedBookId = index;
            }
        });
            
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawAddHorses()
    {
        // 马匹模式
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, 
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
        // 创建马匹列表，根据语言显示对应文本
        ItemData.HorsesList.ForEach((horse, index) =>
        {
            if (GUILayout.Button(horse, GUILayout.ExpandWidth(true)))
            {
                SelectedHorseId = index;
            }
        });
            
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    [Flags]
    private enum MapPartFlag
    {
        None = 1,
        Jun = 1<<1,
        Xian = 1<<2,
        Area = 1<<3,
        Name = 1<<4,
    }

    private Dictionary<MapTab, MapPartFlag> _mapPartFlags = new()
    {
        { MapTab.Mansion, MapPartFlag.Jun | MapPartFlag.Xian | MapPartFlag.Name },
        { MapTab.Farm , MapPartFlag.Jun | MapPartFlag.Xian | MapPartFlag.Area | MapPartFlag.Name },
        { MapTab.Fief , MapPartFlag.Jun},
        { MapTab.Clan, MapPartFlag.Jun | MapPartFlag.Xian },
        { MapTab.Cemetery, MapPartFlag.Jun | MapPartFlag.Xian | MapPartFlag.Area | MapPartFlag.Name },
    };

    private List<string> _mapArea = ["4", "9", "16", "25"];
    
    private void DrawAddMaps()
    {
        // 类型选项
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        Enum.GetNames(typeof(MapTab)).ForEach((key, index) =>
        {
            // 隐藏未做完的功能
            if(key == "Family") 
                return;
            if (GUILayout.Button(_i18N.t("MapTab." + key), GUILayout.ExpandWidth(true)))
                SelectedMap = (MapTab)index;
        });
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f);
        
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, 
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        // 郡
        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Jun) == MapPartFlag.Jun)
        {
            GUILayout.Label(_i18N.t("Info.Jun"), GUILayout.Width(160f));
            
            var perLineBtn = 6;
            var maxBtn = ItemData.JunList.Count;
            ItemData.JunList.ForEach((junName, index) =>
            {
                if(index % perLineBtn == 0)
                    GUILayout.BeginHorizontal();
                
                if (GUILayout.Button(junName, GUILayout.Width(100f)))
                {
                    SelectedJunId = index;
                }
                
                if(++index % perLineBtn == 0 || index == maxBtn)
                    GUILayout.EndHorizontal();
            });
        }
        
        // 县
        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Xian) == MapPartFlag.Xian)
        {
            GUILayout.Space(10f);
            GUILayout.Label(_i18N.t("Info.Xian"), GUILayout.Width(160f));

            var perLineBtn = 6;
            var maxBtn = ItemData.XianList[SelectedJunId].Count;
            // 防错补丁
            if (SelectedXianId >= maxBtn)
                SelectedXianId = 0;
            ItemData.XianList[SelectedJunId].ForEach((xianName, index) =>
            {
                if(index % perLineBtn == 0)
                    GUILayout.BeginHorizontal();
                    
                if (GUILayout.Button(xianName, GUILayout.Width(100f)))
                {
                    SelectedXianId = index;
                }
                
                if(++index % perLineBtn == 0 || index == maxBtn)
                    GUILayout.EndHorizontal();
            });
        }
        
        // 面积
        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Area) == MapPartFlag.Area)
        {
            GUILayout.Space(10f);
            GUILayout.Label(_i18N.t("Info.Area"), GUILayout.Width(160f));
            
            GUILayout.BeginHorizontal();
            _mapArea.ForEach(area =>
            {
                if (GUILayout.Button(area, GUILayout.Width(100f)))
                {
                    SelectedArea = area;
                }
            });
            GUILayout.EndHorizontal();
        }
            
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private string GetSelectMap()
    {
        var str = _i18N.t("MapTab." + Enum.GetName(typeof(MapTab), SelectedMap));
        
        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Jun) == MapPartFlag.Jun)
        {
            str += " - ";
            str += ItemData.JunList[SelectedJunId];
        }

        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Xian) == MapPartFlag.Xian)
        {
            str += " - ";
            str += ItemData.XianList[SelectedJunId][SelectedXianId];
        }

        if ((_mapPartFlags[SelectedMap] & MapPartFlag.Area) == MapPartFlag.Area)
        {
            str += " - ";
            str += SelectedArea;
        }

        return str;
    }

    private void DrawAddButton()
    {
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        {
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

            var text = PanelTab switch
            {
                MenuTab.Currency => _i18N.t($"CurrencyClass.{SelectedCurrency.ToString()}"),
                MenuTab.Items => SelectedPropId != null ? _vStr.t($"Text_AllProp.{SelectedPropId}") : "",
                MenuTab.Stories => SelectedBookId != null ? ItemData.StoriesList[(int)SelectedBookId] : "",
                MenuTab.Horses => SelectedHorseId != null ? ItemData.HorsesList[(int)SelectedHorseId] : "",
                MenuTab.Map => GetSelectMap(),
                _ => ""
            };
            GUILayout.Label($"{_i18N.t("Info.Selected")}{text}", GUILayout.ExpandWidth(true));

            GUILayout.Space(6f);

            // 数量输入框
            GUILayout.BeginHorizontal();
            switch (PanelTab)
            {
                case MenuTab.Currency:
                case MenuTab.Items:
                case MenuTab.Stories:
                    GUILayout.Label(_i18N.t("Info.Count"), GUILayout.Width(100f));
                    CountInput = GUILayout.TextField(CountInput, GUILayout.ExpandWidth(true));
                    break;
                case MenuTab.Horses:
                case MenuTab.Map:
                    if ((_mapPartFlags[SelectedMap] & MapPartFlag.Name) == MapPartFlag.Name)
                    {
                        GUILayout.Label(_i18N.t("Info.Name"), GUILayout.Width(100f));
                        NameInput = GUILayout.TextField(NameInput, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.Label(_i18N.t("Info.Name"), GUILayout.Width(100f));
                        GUILayout.TextField(_i18N.t("Info.NoName"), GUILayout.ExpandWidth(true));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(6f);
            
            GUILayout.BeginHorizontal();
            DrawQuickAddRow();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }

        // 添加按钮
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        {
            GUILayout.Space(15f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(_i18N.t("Button.Add"),
                    GUILayout.Width(180f), GUILayout.Height(80f)))
            {
                OnAddButton?.Invoke();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
            
        GUILayout.EndHorizontal();
    }
    
    private static (string label, int value)[] _kQuickAdds =
    [
        ("1", 1),
        ("1K", 1000),
        ("1M", 1_000_000),
        ("1B", 1_000_000_000),
        ("Max", int.MaxValue)
    ];

    private void DrawQuickAddRow()
    {
        foreach (var (label, value) in _kQuickAdds)
        {
            if (GUILayout.Button(label, GUILayout.Width(80f), GUILayout.Height(36f)))
            {
                CountInput = value.ToString();
            }
            GUILayout.Space(6f);
        }
    }
}
