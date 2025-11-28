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
    private Texture2D _backgroundTexture; // 添加背景纹理变量
    private Texture2D _instructionTexture; // 使用说明背景纹理
    
    // 按钮纹理
    private Texture2D _biaoQianATexture; // 选中状态标签背景
    private Texture2D _biaoQianBTexture; // 未选中状态标签背景
    private Texture2D _btfTexture; // 普通按钮背景
    private Texture2D _btgTexture; // List按钮背景
    
    // 字体资源
    private Font _mediumFont; // SourceHanSansSC-Medium-2.otf
    private Font _boldFont; // SourceHanSansSC-Bold-2.otf
    private Texture2D _startBTATexture; // StartBTA按钮背景
    
    // 检测字符串是否包含中文字符
    private bool ContainsChinese(string text)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]");
    }
    
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
    float topY ;
    float bottomY ;
    
    // 话本相关
    public int? SelectedBookId { get; set; }
    public int? HoveredStoryId { get; set; }

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
        
        // DLL所在路径
        string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllDirectory = System.IO.Path.GetDirectoryName(assemblyPath);

        // 加载背景纹理
        string texturePath = System.IO.Path.Combine(dllDirectory, "Sprites", "PanelC.png");
        
        if (System.IO.File.Exists(texturePath))
        {
            _backgroundTexture = new Texture2D(2, 2);
            byte[] fileData = System.IO.File.ReadAllBytes(texturePath);
            _backgroundTexture.LoadImage(fileData);
        }
        
        // 加载StartBTA纹理用于使用说明背景
        string startBTAPath = System.IO.Path.Combine(dllDirectory, "Sprites", "StartBTA.png");
        if (System.IO.File.Exists(startBTAPath))
        {
            _startBTATexture = new Texture2D(2, 2);
            byte[] fileData = System.IO.File.ReadAllBytes(startBTAPath);
            _startBTATexture.LoadImage(fileData);
        }
        
        // 加载按钮纹理
        LoadButtonTexture(ref _biaoQianATexture, dllDirectory, "BiaoQianA.png");
        LoadButtonTexture(ref _biaoQianBTexture, dllDirectory, "BiaoQianB.png");
        LoadButtonTexture(ref _btfTexture, dllDirectory, "BTF.png");
        LoadButtonTexture(ref _btgTexture, dllDirectory, "BTG.png");
        
        // 加载字体资源
        LoadFont(ref _mediumFont, dllDirectory, System.IO.Path.Combine("Fonts", "SourceHanSansSC-Medium-2.otf"));
        LoadFont(ref _boldFont, dllDirectory, System.IO.Path.Combine("Fonts", "SourceHanSansSC-Bold-2.otf"));
        
        UpdateResolutionSettings();
    }
    
    // 辅助方法：加载按钮纹理
    private void LoadButtonTexture(ref Texture2D texture, string dllDirectory, string textureName)
    {
        string texturePath = System.IO.Path.Combine(dllDirectory, "Sprites", textureName);
        
        if (System.IO.File.Exists(texturePath))
        {
            texture = new Texture2D(2, 2);
            byte[] fileData = System.IO.File.ReadAllBytes(texturePath);
            texture.LoadImage(fileData);
        }
    }
    
    // 辅助方法：加载字体资源
    private void LoadFont(ref Font font, string dllDirectory, string fontPath)
    {
        string fullFontPath = System.IO.Path.Combine(dllDirectory, fontPath);
        
        if (System.IO.File.Exists(fullFontPath))
        {
            // 使用Font.CreateDynamicFontFromOSFont加载字体，确保字体被标记为动态字体
            font = Font.CreateDynamicFontFromOSFont(fullFontPath, 20);
            
            // 确保材质使用GUI/Text Shader
            if (font.material != null)
            {
                font.material.shader = Shader.Find("GUI/Text Shader");
            }
        }
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
                HoveredStoryId = null;
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
            
        // 创建按钮样式
        GUIStyle btgButtonStyle = new GUIStyle(GUI.skin.button);
        if (_btgTexture != null)
        {
            btgButtonStyle.normal.background = _btgTexture;
        }
        btgButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        btgButtonStyle.alignment = TextAnchor.MiddleCenter;
        
        Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(itemName), btgButtonStyle);
        
        // 按钮点击
        if (GUI.Button(buttonRect, itemName, btgButtonStyle))
        {
            SelectedPropId = propId;
            HoveredPropId = null;
        }
        
        // 检测鼠标悬浮
        Event currentEvent = Event.current;
        if (currentEvent != null && (currentEvent.type == EventType.MouseMove || currentEvent.type == EventType.Repaint) )
        {
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
        
        // 在每次GUI渲染周期开始时重置悬浮状态，确保鼠标不在任何按钮上时悬浮窗会隐藏
        if (Event.current.type == EventType.Repaint)
        {
            HoveredPropId = null;
            HoveredStoryId = null;
        }
        
        // 保存原始GUI设置
        var originalBackgroundColor = GUI.backgroundColor;
        var originalWindowStyle = GUI.skin.window;
        
        // 创建透明窗口样式
        GUIStyle transparentWindowStyle = new GUIStyle(GUI.skin.window);
        transparentWindowStyle.normal.background = null;
        transparentWindowStyle.border = new RectOffset(0, 0, 0, 0);
        transparentWindowStyle.padding = new RectOffset(0, 0, 0, 0);
        GUI.skin.window = transparentWindowStyle;
        
        // 设置为透明背景
        GUI.backgroundColor = Color.clear;
        
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

            // 绘制背景纹理，并添加边距，上下预留40f，左右预留20f
            if (_backgroundTexture != null)
            {
                // 计算带边距的绘制区域
                Rect drawRect = new Rect(
                    _windowRect.x - 50f,  // 左边距50f
                    _windowRect.y - 40f,  // 上边距40f
                    _windowRect.width + 100f,  // 左右各50f，总共100f
                    _windowRect.height + 80f  // 上下各40f，总共80f
                );
                GUI.DrawTexture(drawRect, _backgroundTexture, ScaleMode.StretchToFill);
            }
            else
            {
                // 如果纹理加载失败，使用默认背景色作为备选
                GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 0.95f);
            }
            
            // 创建窗口
            _windowRect = GUI.Window(0, _windowRect, DrawWindow, "");
            
            // 恢复原始矩阵
            GUI.matrix = guiMatrix;
        }
        
        // 恢复原始GUI设置
        GUI.backgroundColor = originalBackgroundColor;
        GUI.skin.window = originalWindowStyle;
    }
    
    private void DrawWindow(int windowID)
    {
        // 创建全局的bold字体样式 - 根据是否包含中文设置不同大小
        GUIStyle boldLabelStyle = new GUIStyle(GUI.skin.label);
        if (_boldFont != null)
        {
            boldLabelStyle.font = _boldFont;
            // 大多数标签使用这个样式，根据是否包含中文设置字体大小
              string sampleText = _i18N.t("Info.Category");
              boldLabelStyle.fontSize = ContainsChinese(sampleText) ? 18 : 16;
        }
        else
        {
            boldLabelStyle.fontSize = 18;
        }
        
        GUIStyle boldButtonStyle = new GUIStyle(GUI.skin.button);
        if (_boldFont != null)
        {
            boldButtonStyle.font = _boldFont;
            // 按钮文字也根据是否包含中文设置字体大小
              string sampleText = _i18N.t("Button.Clear");
              boldButtonStyle.fontSize = ContainsChinese(sampleText) ? 18 : 16;
        }
        else
        {
            boldButtonStyle.fontSize = 18;
        }
        // 确保按钮文本居中对齐
        boldButtonStyle.alignment = TextAnchor.MiddleCenter;
        
        // 应用全局样式（除了已经特殊设置的标题样式）
        GUI.skin.label = boldLabelStyle;
        GUI.skin.button = boldButtonStyle;
        
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        // 固定标题置于最上方
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        
        // 创建标题样式 - 使用medium字体，根据是否包含中文设置不同大小
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        
        string titleText = $"{_i18N.t("Mod.Name")}";
        string subheadingText = $"{_i18N.t("Mod.VersionText")}{AddItem.VERSION}{_i18N.t("Mod.AuthorText")}{_i18N.t("Mod.Author")}";
        if (_mediumFont != null)
        {
            titleStyle.font = _mediumFont;
            // 根据是否包含中文设置字体大小
            titleStyle.fontSize = ContainsChinese(titleText) ? 30 : 28;
            // 设置标题居中对齐和字体颜色（RGB:161,80,80）
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = new Color(0.6314f, 0.3137f, 0.3137f, 1.0f); // RGB:161,80,80
        }
        else
        {
            titleStyle.fontSize = 30;
            // 设置标题居中对齐和字体颜色（RGB:161,80,80）
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = new Color(0.6314f, 0.3137f, 0.3137f, 1.0f); // RGB:161,80,80
        }
        
        GUILayout.Label(titleText, titleStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        // 副标题换行并靠右对齐
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();
        // 为副标题创建右对齐样式
        GUIStyle subheadingStyle = new GUIStyle(titleStyle);
        subheadingStyle.fontSize = ContainsChinese(subheadingText) ? 24 : 22;
        subheadingStyle.normal.textColor = new Color(0.6314f, 0.3137f, 0.3137f, 1.0f); // RGB:161,80,80
        GUILayout.Label(subheadingText, subheadingStyle);
        GUILayout.EndHorizontal();
        GUILayout.Space(30f);
        
        // 菜单页面标签
        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        Enum.GetNames(typeof(MenuTab)).ForEach((key, index) =>
        {
            // 创建按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            // 设置按钮背景纹理
            buttonStyle.normal.background = (_biaoQianBTexture != null) ? _biaoQianBTexture : null;
            // 如果是当前选中的标签，使用选中状态的背景
            if ((MenuTab)index == PanelTab && _biaoQianATexture != null)
            {
                buttonStyle.normal.background = _biaoQianATexture;
            }
            // 设置文字颜色和居中
            buttonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUILayout.Button($"{_i18N.t("MenuTab." + key)}", buttonStyle, GUILayout.ExpandWidth(true)))
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
            
            // 创建无边框样式并使用StartBTA.png作为背景
            GUIStyle instructionStyle = new GUIStyle();
            if (_startBTATexture != null)
            {
                instructionStyle.normal.background = _startBTATexture;
            }
            instructionStyle.border = new RectOffset(0, 0, 0, 0);
            instructionStyle.margin = new RectOffset(0, 0, 0, 0);
            instructionStyle.padding = new RectOffset(10, 10, 10, 10);
            instructionStyle.alignment = TextAnchor.MiddleLeft; // 靠左对齐
            
            // 使用_mediumFont字体并根据是否包含中文设置不同大小（中文24，非中文20）
            instructionStyle.font = _mediumFont;
            string descriptionText = _i18N.t("Info.Description");
            instructionStyle.fontSize = ContainsChinese(descriptionText) ? 24 : 20;
            
            // 创建内容样式
            GUIStyle contentStyle = new GUIStyle();
            contentStyle.normal.textColor = new Color(0.6314f, 0.3137f, 0.3137f, 1.0f); // RGB:161,80,80
            contentStyle.font = _mediumFont;
            // 内容字体大小：中文20，非中文18
            contentStyle.fontSize = 20;
            // 注意：循环内会根据具体内容再次调整大小
            
            // 绘制使用说明标题和内容，设置字体颜色（RGB:161,80,80）
            instructionStyle.normal.textColor = new Color(0.6314f, 0.3137f, 0.3137f, 1.0f); // RGB:161,80,80
            GUILayout.Label(_i18N.t("Info.Description"), instructionStyle);
            
            // 绘制使用说明内容
            for (var i = 1; i <= 6; i++) {
                string contentText = _i18N.t($"Description.{i}");
                // 根据内容是否包含中文设置字体大小（中文20，非中文18）
                contentStyle.fontSize = ContainsChinese(contentText) ? 20 : 18;
                GUILayout.Label(contentText, contentStyle);
            }
            
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
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        // 使用_mediumFont字体并根据是否包含中文设置不同大小
        if (_mediumFont != null)
        {
            labelStyle.font = _mediumFont;
            string categoryText = _i18N.t("Info.Category");
            labelStyle.fontSize = ContainsChinese(categoryText) ? 18 : 16;
        }
        GUILayout.Label(_i18N.t("Info.Category"), labelStyle, GUILayout.Width(160f));
        
        // 创建按钮样式，使用BTF.png作为背景
        GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
        if (_btfTexture != null)
        {
            btfButtonStyle.normal.background = _btfTexture;
            btfButtonStyle.active.background = _btfTexture;
            btfButtonStyle.focused.background = _btfTexture;
            btfButtonStyle.hover.background = _btfTexture;
        }
        btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        btfButtonStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.BeginVertical();
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition,
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(_i18N.t("CurrencyClass.Coins"), btfButtonStyle, GUILayout.Width(160f)))
        {
            SelectedCurrency = CurrencyClass.Coins;
        }
        if (GUILayout.Button(_i18N.t("CurrencyClass.Gold"), btfButtonStyle, GUILayout.Width(160f)))
        {
            SelectedCurrency = CurrencyClass.Gold;
        }
        GUILayout.EndHorizontal();
        
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        
        GUILayout.Space(10f);
        
        // 当前货币数量显示
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUIStyle currencyLabelStyle = new GUIStyle();
        currencyLabelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        // 使用_mediumFont字体并根据是否包含中文设置不同大小
        if (_mediumFont != null)
        {
            currencyLabelStyle.font = _mediumFont;
            string currencyText = _i18N.t("Info.CurrencyNow.Coins");
            currencyLabelStyle.fontSize = ContainsChinese(currencyText) ? 18 : 16;
        }
        GUILayout.Label(_i18N.t("Info.CurrencyNow.Coins",  FormulaData.GetCoinsNum()), currencyLabelStyle,
            GUILayout.ExpandWidth(true));
        GUILayout.Label(_i18N.t("Info.CurrencyNow.Gold", args: Mainload.CGNum[1]), currencyLabelStyle,
            GUILayout.ExpandWidth(true));
        GUILayout.EndVertical();
    }

    private void DrawAddItem()
    {
        {
            GUILayout.BeginHorizontal();
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            // 使用_mediumFont字体并根据是否包含中文设置不同大小
                if (_mediumFont != null)
                {
                    labelStyle.font = _mediumFont;
                    string searchText = _i18N.t("Info.Search");
                    labelStyle.fontSize = ContainsChinese(searchText) ? 18 : 16;
                }
                GUILayout.Label(_i18N.t("Info.Search"), labelStyle, GUILayout.Width(160f));
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
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            // 使用_mediumFont字体并根据是否包含中文设置不同大小
            if (_mediumFont != null)
            {
                labelStyle.font = _mediumFont;
                string categoryText = _i18N.t("Info.Category");
                labelStyle.fontSize = ContainsChinese(categoryText) ? 18 : 16;
            }
            GUILayout.Label(_i18N.t("Info.Category")+
                            (SelectedPropClass != null ? _i18N.t($"PropClass.{SelectedPropClass.ToString()}") : ""),
                labelStyle, GUILayout.Width(160f));
            // 创建按钮样式
            GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
            if (_btfTexture != null)
            {
                btfButtonStyle.normal.background = _btfTexture;
            }
            btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            btfButtonStyle.alignment = TextAnchor.MiddleCenter;
            // 使用_mediumFont字体并根据是否包含中文设置不同大小
            string buttonText = _i18N.t("Button.Clear");
            if (_mediumFont != null)
            {
                btfButtonStyle.font = _mediumFont;
                btfButtonStyle.fontSize = ContainsChinese(buttonText) ? 18 : 16;
            }
            
            if (GUILayout.Button(_i18N.t("Button.Clear"), btfButtonStyle, GUILayout.Width(120f)))
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

                // 创建按钮样式
                GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
                if (_btfTexture != null)
                {
                    btfButtonStyle.normal.background = _btfTexture;
                }
                btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                btfButtonStyle.alignment = TextAnchor.MiddleCenter;
                
                if (GUILayout.Button(_i18N.t($"PropClass.{cate}"), btfButtonStyle,
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

        // 记录滚动视图开始位置
        float beforeScrollY = GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height;
        
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
            
            // 计算滚动视图的实际可见区域
            topY = beforeScrollY;
            bottomY = GUILayoutUtility.GetLastRect().y + GUILayoutUtility.GetLastRect().height;
        }
    }

    private void DrawAddStories()
    {
        // 话本模式
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, 
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            
        // 创建使用BTG纹理的按钮样式
        GUIStyle btgButtonStyle = new GUIStyle(GUI.skin.button);
        if (_btgTexture != null)
        {
            btgButtonStyle.normal.background = _btgTexture;
            btgButtonStyle.hover.background = _btgTexture;
            btgButtonStyle.active.background = _btgTexture;
        }
        btgButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        btgButtonStyle.alignment = TextAnchor.MiddleCenter;
        
        // 创建话本列表，根据语言显示对应文本
        ItemData.StoriesList.ForEach((book, index) =>
        {
            string bookName = book;
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(bookName), btgButtonStyle);
            
            // 处理按钮点击
            if (GUI.Button(buttonRect, bookName, btgButtonStyle))
            {
                SelectedBookId = index;
                HoveredStoryId = null;
            }
            
            // 直接在GUI渲染时检测鼠标悬浮，这能正确处理滚动视图中的坐标
            Event currentEvent = Event.current;
            if (currentEvent != null && (currentEvent.type == EventType.MouseMove || currentEvent.type == EventType.Repaint))
            {
                if (buttonRect.Contains(currentEvent.mousePosition))
                {
                    HoveredStoryId = index;
                }
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
            
        // 创建使用BTG纹理的按钮样式
        GUIStyle btgButtonStyle = new GUIStyle(GUI.skin.button);
        if (_btgTexture != null)
        {
            btgButtonStyle.normal.background = _btgTexture;
            btgButtonStyle.hover.background = _btgTexture;
            btgButtonStyle.active.background = _btgTexture;
        }
        btgButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
        btgButtonStyle.alignment = TextAnchor.MiddleCenter;
        
        // 创建马匹列表，根据语言显示对应文本
        ItemData.HorsesList.ForEach((horse, index) =>
        {
            if (GUILayout.Button(horse, btgButtonStyle, GUILayout.ExpandWidth(true)))
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
            if(key == "Clan") 
                return;
            
            // 创建按钮样式
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            // 设置按钮背景纹理
            buttonStyle.normal.background = (_biaoQianBTexture != null) ? _biaoQianBTexture : null;
            // 如果是当前选中的标签，使用选中状态的背景
            if ((MapTab)index == SelectedMap && _biaoQianATexture != null)
            {
                buttonStyle.normal.background = _biaoQianATexture;
            }
            // 设置文字颜色和居中
            buttonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUILayout.Button(_i18N.t("MapTab." + key), buttonStyle, GUILayout.ExpandWidth(true)))
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
            GUIStyle labelStyle = new GUIStyle();
              labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
              // 使用_mediumFont字体并根据是否包含中文设置不同大小
              if (_mediumFont != null)
              {
                  labelStyle.font = _mediumFont;
                  string junText = _i18N.t("Info.Jun");
                  labelStyle.fontSize = ContainsChinese(junText) ? 18 : 16;
              }
              GUILayout.Label(_i18N.t("Info.Jun"), labelStyle, GUILayout.Width(160f));
            
            var perLineBtn = 5; // 每行5个按钮
            var maxBtn = ItemData.JunList.Count;
            ItemData.JunList.ForEach((junName, index) =>
            {
                if(index % perLineBtn == 0)
                    GUILayout.BeginHorizontal();
                
                // 创建按钮样式
                GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
                if (_btfTexture != null)
                {
                    btfButtonStyle.normal.background = _btfTexture;
                }
                btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                btfButtonStyle.alignment = TextAnchor.MiddleCenter;
                
                if (GUILayout.Button(junName, btfButtonStyle, GUILayout.Width(150f)))   // 改为150f
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
            GUIStyle labelStyle = new GUIStyle();
              labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
              // 使用_mediumFont字体并根据是否包含中文设置不同大小
              if (_mediumFont != null)
              {
                  labelStyle.font = _mediumFont;
                  string xianText = _i18N.t("Info.Xian");
                  labelStyle.fontSize = ContainsChinese(xianText) ? 18 : 16;
              }
              GUILayout.Label(_i18N.t("Info.Xian"), labelStyle, GUILayout.Width(160f));

            var perLineBtn = 5; // 每行5个按钮
            var maxBtn = ItemData.XianList[SelectedJunId].Count;
            // 防错补丁
            if (SelectedXianId >= maxBtn)
                SelectedXianId = 0;
            ItemData.XianList[SelectedJunId].ForEach((xianName, index) =>
            {
                if(index % perLineBtn == 0)
                    GUILayout.BeginHorizontal();
                    
                // 创建按钮样式
                GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
                if (_btfTexture != null)
                {
                    btfButtonStyle.normal.background = _btfTexture;
                }
                btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                btfButtonStyle.alignment = TextAnchor.MiddleCenter;
                
                if (GUILayout.Button(xianName, btfButtonStyle, GUILayout.Width(150f)))   // 改为150f
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
            GUIStyle labelStyle = new GUIStyle();
              labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
              // 使用_mediumFont字体并根据是否包含中文设置不同大小
              if (_mediumFont != null)
              {
                  labelStyle.font = _mediumFont;
                  string areaText = _i18N.t("Info.Area");
                  labelStyle.fontSize = ContainsChinese(areaText) ? 18 : 16;
              }
              GUILayout.Label(_i18N.t("Info.Area"), labelStyle, GUILayout.Width(160f));
            
            GUILayout.BeginHorizontal();
            _mapArea.ForEach(area =>
            {
                // 创建按钮样式
                GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
                if (_btfTexture != null)
                {
                    btfButtonStyle.normal.background = _btfTexture;
                }
                btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                btfButtonStyle.alignment = TextAnchor.MiddleCenter;
                
                if (GUILayout.Button(area, btfButtonStyle, GUILayout.Width(100f)))
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
            GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                // 使用_mediumFont字体并根据是否包含中文设置不同大小
                if (_mediumFont != null)
                {
                    labelStyle.font = _mediumFont;
                    string selectedText = _i18N.t("Info.Selected");
                    labelStyle.fontSize = ContainsChinese(selectedText) ? 18 : 16;
                }
                GUILayout.Label($"{_i18N.t("Info.Selected")}{text}", labelStyle, GUILayout.ExpandWidth(true));

            GUILayout.Space(6f);

            // 数量输入框
            GUILayout.BeginHorizontal();
            switch (PanelTab)
            {
                case MenuTab.Currency:
                case MenuTab.Items:
                case MenuTab.Stories:
                    GUIStyle countLabelStyle = new GUIStyle();
                    countLabelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                    // 使用_mediumFont字体并根据是否包含中文设置不同大小
                    if (_mediumFont != null)
                    {
                        countLabelStyle.font = _mediumFont;
                        string countText = _i18N.t("Info.Count");
                        countLabelStyle.fontSize = ContainsChinese(countText) ? 18 : 16;
                    }
                    GUILayout.Label(_i18N.t("Info.Count"), countLabelStyle, GUILayout.Width(100f));
                    CountInput = GUILayout.TextField(CountInput, GUILayout.ExpandWidth(true));
                    break;
                case MenuTab.Horses:
                case MenuTab.Map:
                    GUIStyle nameLabelStyle = new GUIStyle();
                    nameLabelStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
                    if ((_mapPartFlags[SelectedMap] & MapPartFlag.Name) == MapPartFlag.Name)
                    {
                        GUILayout.Label(_i18N.t("Info.Name"), nameLabelStyle, GUILayout.Width(100f));
                        NameInput = GUILayout.TextField(NameInput, GUILayout.ExpandWidth(true));
                    }
                    else
                    {
                        GUILayout.Label(_i18N.t("Info.Name"), nameLabelStyle, GUILayout.Width(100f));
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
            // 创建按钮样式
            GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
            if (_btfTexture != null)
            {
                btfButtonStyle.normal.background = _btfTexture;
            }
            btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            btfButtonStyle.alignment = TextAnchor.MiddleCenter;
            // 使用_mediumFont字体并设置大小为40
            if (_mediumFont != null)
            {
                btfButtonStyle.font = _mediumFont;
                btfButtonStyle.fontSize = 40;
            }
            
            if (GUILayout.Button(_i18N.t("Button.Add"), btfButtonStyle,
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
            // 创建按钮样式
            GUIStyle btfButtonStyle = new GUIStyle(GUI.skin.button);
            if (_btfTexture != null)
            {
                btfButtonStyle.normal.background = _btfTexture;
            }
            btfButtonStyle.normal.textColor = new Color(82/255f, 60/255f, 50/255f, 1.0f); // RGB:82,60,50
            btfButtonStyle.alignment = TextAnchor.MiddleCenter;
            
            if (GUILayout.Button(label, btfButtonStyle, GUILayout.Width(80f), GUILayout.Height(36f)))
            {
                CountInput = value.ToString();
            }
            GUILayout.Space(6f);
        }
    }
}
