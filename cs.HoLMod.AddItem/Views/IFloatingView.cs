using UnityEngine;
using YuanAPI;

namespace cs.HoLMod.AddItem.Views;

public class IFloatingView : MonoBehaviour
{
    // 物品悬浮窗相关字段
    private int? _selectedPropId;
    public static Vector2 _mousePosition;
    private bool _isVisible;
    private Rect _windowRect = new Rect(0, 0, 200, 300);
    
    // 话本悬浮窗相关字段
    private int? _selectedStoryId;
    private bool _isStoryVisible;
    private Rect _storyWindowRect = new Rect(0, 0, 300, 200);
    private bool _showStoryToggleButton = false;
    private Rect _storyToggleButtonRect = new Rect(10, 10, 30, 30);

    private Localization.LocalizationInstance _i18N;
    private Localization.LocalizationInstance _vStr;
    private bool _isInitialized;
    private Texture2D _backgroundTexture; // 窗口背景纹理
    private Texture2D _contentTexture; // 内容区域背景纹理
    private Texture2D _lineTexture; // 分割线纹理
    private Texture2D _currencyTexture; // 货币图标纹理（铜钱图标）
    private Font _boldFont; // 粗体字体
    private Font _mediumFont; // 中等粗细字体
    private GUIStyle _itemNameStyle; // 物品名称样式
    private GUIStyle _contentTextStyle; // 内容文本样式
    
    // 检测字符串是否包含中文字符
    private bool ContainsChinese(string text)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]");
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (_isInitialized)
            return;

        _i18N = Localization.CreateInstance(@namespace: AddItem.LocaleNamespace);
        _vStr = Localization.CreateInstance(@namespace: Localization.VanillaNamespace);

        // 加载字体 - 从DLL所在路径的Fonts文件夹加载
        string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllDirectory = System.IO.Path.GetDirectoryName(assemblyPath);
        string boldFontPath = System.IO.Path.Combine(dllDirectory, "Fonts", "SourceHanSansSC-Bold-2.otf");
        string mediumFontPath = System.IO.Path.Combine(dllDirectory, "Fonts", "SourceHanSansSC-Medium-2.otf");

        // 加载字体文件并确保它们是动态字体
        if (System.IO.File.Exists(boldFontPath))
        {
            // 使用Font.CreateDynamicFontFromOSFont加载字体，这会确保字体被标记为动态字体
            Font dynamicFont = Font.CreateDynamicFontFromOSFont(boldFontPath, 20);
            _boldFont = dynamicFont;
            
            // 确保材质使用GUI/Text Shader
            if (_boldFont.material != null)
            {
                _boldFont.material.shader = Shader.Find("GUI/Text Shader");
            }
        }
        if (System.IO.File.Exists(mediumFontPath))
        {
            // 使用Font.CreateDynamicFontFromOSFont加载字体
            Font dynamicFont = Font.CreateDynamicFontFromOSFont(mediumFontPath, 18);
            _mediumFont = dynamicFont;
            
            // 确保材质使用GUI/Text Shader
            if (_mediumFont.material != null)
            {
                _mediumFont.material.shader = Shader.Find("GUI/Text Shader");
            }
        }

        // 创建字体样式
        _itemNameStyle = new GUIStyle();
        if (_boldFont != null)
        {
            _itemNameStyle.font = _boldFont;
        }
        _itemNameStyle.normal.textColor = new Color(161/255f, 80/255f, 80/255f); // 设置为棕红色
        _itemNameStyle.alignment = TextAnchor.MiddleCenter;

        _contentTextStyle = new GUIStyle();
        if (_mediumFont != null)
        {
            _contentTextStyle.font = _mediumFont;
        }
        _contentTextStyle.normal.textColor = new Color(96/255f, 66/255f, 57/255f);
        _contentTextStyle.alignment = TextAnchor.MiddleLeft;

        // 加载背景纹理 - 从DLL所在路径的Sprites文件夹加载
        string texturePath = System.IO.Path.Combine(dllDirectory, "Sprites", "PanelC.png");

        if (System.IO.File.Exists(texturePath))
        {
            _backgroundTexture = new Texture2D(2, 2);
            byte[] fileData = System.IO.File.ReadAllBytes(texturePath);
            _backgroundTexture.LoadImage(fileData);
        }

        // 加载内容区域背景纹理
        string contentTexturePath = System.IO.Path.Combine(dllDirectory, "Sprites", "KuangC.png");
        if (System.IO.File.Exists(contentTexturePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(contentTexturePath);
            _contentTexture = new Texture2D(2, 2);
            _contentTexture.LoadImage(fileData);
        }
        
        // 加载分割线纹理
        string lineTexturePath = System.IO.Path.Combine(dllDirectory, "Sprites", "lineA.png");
        if (System.IO.File.Exists(lineTexturePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(lineTexturePath);
            _lineTexture = new Texture2D(2, 2);
            _lineTexture.LoadImage(fileData);
        }
        
        // 加载铜钱图标纹理
        string currencyTexturePath = System.IO.Path.Combine(dllDirectory, "Sprites", "TongQian.png");
        if (System.IO.File.Exists(currencyTexturePath))
        {
            byte[] fileData = System.IO.File.ReadAllBytes(currencyTexturePath);
            _currencyTexture = new Texture2D(2, 2);
            _currencyTexture.LoadImage(fileData);
        }

        _isInitialized = true;
    }
    
    private void Update()
    {
        UpdateFloatingView();
    }
    
    /// <summary>
    /// 更新悬浮窗状态和数据
    /// </summary>
    private void UpdateFloatingView()
    {
        Initialize(); // 确保初始化
        
        // 检查当前是否在Items标签页且菜单显示
        var addItemView = FindObjectOfType<IMGUIAddItemView>();
        if (addItemView == null || !addItemView.ShowMenu)
        {
            _isVisible = false;
            _isStoryVisible = false;
            _showStoryToggleButton = false;
            return;
        }
        
        // 物品悬浮窗逻辑
        if (addItemView.PanelTab == MenuTab.Items)
        {
            _showStoryToggleButton = false;
            // 获取鼠标悬浮的物品ID
            _selectedPropId = addItemView.HoveredPropId;
            
            // 更新鼠标位置
            IFloatingView._mousePosition = Input.mousePosition;
            IFloatingView._mousePosition.y = Screen.height - IFloatingView._mousePosition.y;
            
            // 如果鼠标悬浮在物品上，显示悬浮窗
            _isVisible = _selectedPropId.HasValue;
            _isStoryVisible = false;
            
            if (_isVisible)
            {
                // 设置悬浮窗位置在鼠标旁边
                _windowRect.x = IFloatingView._mousePosition.x + 20;
                _windowRect.y = IFloatingView._mousePosition.y - 20;
                
                // 确保悬浮窗不超出屏幕
                _windowRect.x = Mathf.Clamp(_windowRect.x, 0, Screen.width - _windowRect.width);
                _windowRect.y = Mathf.Clamp(_windowRect.y, 0, Screen.height - _windowRect.height);
            }
        }
        // 话本悬浮窗逻辑
        else if (addItemView.PanelTab == MenuTab.Stories)
        {
            _isVisible = false;
            _showStoryToggleButton = true;
            // 获取鼠标悬浮的话本ID
            _selectedStoryId = addItemView.HoveredStoryId;
            
            // 更新鼠标位置
            IFloatingView._mousePosition = Input.mousePosition;
            IFloatingView._mousePosition.y = Screen.height - IFloatingView._mousePosition.y;
            
            // 更新话本悬浮窗位置和可见性
            if (_selectedStoryId.HasValue)
            {
                _storyWindowRect.x = IFloatingView._mousePosition.x + 20;
                _storyWindowRect.y = IFloatingView._mousePosition.y - 20;
                
                // 确保悬浮窗不超出屏幕
                _storyWindowRect.x = Mathf.Clamp(_storyWindowRect.x, 0, Screen.width - _storyWindowRect.width);
                _storyWindowRect.y = Mathf.Clamp(_storyWindowRect.y, 0, Screen.height - _storyWindowRect.height);
                
                // 当有话本ID时自动设置可见性为true（通过切换按钮控制）
                _isStoryVisible = true;
            }
        }
        else
        {
            _isVisible = false;
            _isStoryVisible = false;
            _showStoryToggleButton = false;
        }
    }
    
    private void OnGUI()
    {
        // 绘制物品悬浮窗
        if (_isVisible && _selectedPropId.HasValue)
        {
            // 保存当前的GUI设置
            Color originalBackgroundColor = GUI.backgroundColor;
            Color originalContentColor = GUI.contentColor;
            GUIStyle originalWindowStyle = GUI.skin.window;
            
            // 创建透明窗口样式
            GUIStyle transparentWindowStyle = new GUIStyle(GUI.skin.window);
            transparentWindowStyle.normal.background = null;
            transparentWindowStyle.border = new RectOffset(0, 0, 0, 0);
            transparentWindowStyle.padding = new RectOffset(0, 0, 0, 0);
            GUI.skin.window = transparentWindowStyle;
            
            // 设置为透明背景和白色内容
            GUI.backgroundColor = Color.clear;
            GUI.contentColor = Color.white;
            
            // 绘制自定义背景
            if (_backgroundTexture != null)
            {
                GUI.DrawTexture(_windowRect, _backgroundTexture, ScaleMode.StretchToFill);
            }
            else
            {
                // 如果纹理加载失败，使用默认背景色作为备选
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                GUI.Box(_windowRect, "");
                GUI.backgroundColor = Color.clear;
            }
            
            // 使用透明窗口样式创建窗口，保留拖动功能
            _windowRect = GUI.Window(12345, _windowRect, DrawFloatingWindow, "");
            
            // 恢复原始GUI设置
            GUI.backgroundColor = originalBackgroundColor;
            GUI.contentColor = originalContentColor;
            GUI.skin.window = originalWindowStyle;
        }
        
        // 绘制话本悬浮窗
        if (_isStoryVisible && _selectedStoryId.HasValue)
        {
            // 保存当前的GUI设置
            Color originalBackgroundColor = GUI.backgroundColor;
            Color originalContentColor = GUI.contentColor;
            GUIStyle originalWindowStyle = GUI.skin.window;
            
            // 创建透明窗口样式
            GUIStyle transparentWindowStyle = new GUIStyle(GUI.skin.window);
            transparentWindowStyle.normal.background = null;
            transparentWindowStyle.border = new RectOffset(0, 0, 0, 0);
            transparentWindowStyle.padding = new RectOffset(0, 0, 0, 0);
            GUI.skin.window = transparentWindowStyle;
            
            // 设置为透明背景和白色内容
            GUI.backgroundColor = Color.clear;
            GUI.contentColor = Color.white;
            
            // 绘制自定义背景
            if (_backgroundTexture != null)
            {
                GUI.DrawTexture(_storyWindowRect, _backgroundTexture, ScaleMode.StretchToFill);
            }
            else
            {
                // 如果纹理加载失败，使用默认背景色作为备选
                GUI.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                GUI.Box(_storyWindowRect, "");
                GUI.backgroundColor = Color.clear;
            }
            
            // 使用透明窗口样式创建窗口，保留拖动功能
            _storyWindowRect = GUI.Window(12346, _storyWindowRect, DrawStoryFloatingWindow, "");
            
            // 恢复原始GUI设置
            GUI.backgroundColor = originalBackgroundColor;
            GUI.contentColor = originalContentColor;
            GUI.skin.window = originalWindowStyle;
        }
    }
    
    private void DrawFloatingWindow(int windowID)
    {
        // 绘制内容区域背景
        if (_contentTexture != null)
        {
            GUI.DrawTexture(new Rect(5, 5, _windowRect.width - 10, _windowRect.height - 10), _contentTexture, ScaleMode.StretchToFill);
        }
        
        GUILayout.BeginArea(new Rect(10, 10, _windowRect.width - 20, _windowRect.height - 20));

        if (!_selectedPropId.HasValue)
            return;

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        // 通过空行调整布局
        GUILayout.Space(10f);

        // 物品名称
        string itemName = _vStr.t($"Text_AllProp.{_selectedPropId}");
        // 根据是否包含中文设置字体大小
        GUIStyle nameStyle = new GUIStyle(_itemNameStyle);
        nameStyle.fontSize = ContainsChinese(itemName) ? 20 : 16;
        GUILayout.Label(itemName, nameStyle);
        
        // 绘制分割线
        if (_lineTexture != null)
        {
            GUILayout.Space(5f);
            // 创建无边框样式并设置颜色和居中对齐
            GUIStyle lineStyle = new GUIStyle();
            lineStyle.normal.background = null;
            lineStyle.border = new RectOffset(0, 0, 0, 0);
            lineStyle.margin = new RectOffset(0, 0, 0, 0);
            lineStyle.padding = new RectOffset(0, 0, 0, 0);
            lineStyle.alignment = TextAnchor.MiddleCenter;
            // 设置颜色
            GUI.color = new Color(226/255f, 141/255f, 120/255f);
            GUILayout.Box(_lineTexture, lineStyle, GUILayout.Width(180), GUILayout.Height(20));
            // 恢复颜色
            GUI.color = Color.white;
            GUILayout.Space(5f);
        }
        else
        {
            GUILayout.Space(10f);
        }

        // 物品属性
        if (Mainload.AllPropdata != null)
        {
            string[] props = Mainload.AllPropdata[_selectedPropId.Value][2].Split('|');

            // 绘制属性信息
            if (int.Parse(props[0]) > 0)
            {
                string text = _vStr.t("Text_UIA.1123").Replace("@", props[0]);
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }
            if (int.Parse(props[1]) > 0)
            {
                string text = _vStr.t("Text_UIA.1124").Replace("@", props[1]);
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }
            if (int.Parse(props[2]) > 0)
            {
                string text = _vStr.t("Text_UIA.1125").Replace("@", props[2]);
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }
            if (int.Parse(props[3]) > 0)
            {
                string text = _vStr.t("Text_UIA.1126").Replace("@", props[3]);
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }

            // 绘制带符号的属性
            DrawSignedProperty(props[4], _vStr.t("Text_UIA.1127"));
            DrawSignedProperty(props[5], _vStr.t("Text_UIA.1128"));
            DrawSignedProperty(props[6], _vStr.t("Text_UIA.1129"));
            DrawSignedProperty(props[7], _vStr.t("Text_UIA.1130"));

            // 其他特殊属性
            if (int.Parse(props[8]) > 0)
            {
                string text = _vStr.t("Text_UIA.1131");
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }
            if (int.Parse(props[9]) > 0)
            {
                string text = _vStr.t("Text_UIA.1075").Replace("@", "+" + props[9]);
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
            }

            // 技能属性
            for (int i = 10; i <= 15; i++)
            {
                if (int.Parse(props[i]) > 0)
                {
                    string text = _vStr.t($"Text_AllMemberSkill.{i - 9}") + ": +" + props[i];
                    GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                    textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                    GUILayout.Label(text, textStyle);
                }
            }

            // 特殊物品说明
            DrawSpecialItemDescription(_selectedPropId.Value);
            
            // 插入间隔进行排版
            GUILayout.Space(20f);

            // 物品类型
            int propClassId = int.Parse(Mainload.AllPropdata[_selectedPropId.Value][1]);
            string className = "(" + _vStr.t($"Text_AllPropClass.{propClassId}") + ")";
            GUIStyle classNameStyle = new GUIStyle(_contentTextStyle);
            classNameStyle.fontSize = ContainsChinese(className) ? 18 : 16;
            GUILayout.Label(className, classNameStyle);

            // 插入间隔进行排版
            GUILayout.Space(20f);

            // 物品价格
            float price = float.Parse(Mainload.AllPropdata[_selectedPropId.Value][0]);
            string priceValue = FormulaData.NumShow(Mathf.CeilToInt(price));
            
            GUILayout.BeginHorizontal();
            GUIStyle priceStyle = new GUIStyle(_contentTextStyle);
            priceStyle.fontSize = ContainsChinese(_i18N.t("FloatingView.Price")) ? 18 : 16;
            
            // 价格标签
            GUIStyle labelStyle = new GUIStyle(priceStyle);
            labelStyle.margin.right = 0;
            GUILayout.Label(_i18N.t("FloatingView.Price") + ":", labelStyle, GUILayout.ExpandWidth(false));
            
            // 铜钱图标
            if (_currencyTexture != null)
            {
                GUIStyle iconStyle = new GUIStyle();
                iconStyle.margin.left = 0;
                GUILayout.Label(_currencyTexture, iconStyle, GUILayout.Width(20), GUILayout.Height(20));
            }
            
            // 价格数字
            GUILayout.Label(priceValue, priceStyle);
            
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();

        // 允许窗口拖动
        GUI.DragWindow(new Rect(0, 0, float.MaxValue, 20));
    }
    
    // 绘制话本悬浮窗内容
    private void DrawStoryFloatingWindow(int windowID)
    {
        // 绘制内容区域背景
        if (_contentTexture != null)
        {
            GUI.DrawTexture(new Rect(5, 5, _storyWindowRect.width - 10, _storyWindowRect.height - 10), _contentTexture, ScaleMode.StretchToFill);
        }
        
        GUILayout.BeginArea(new Rect(10, 10, _storyWindowRect.width - 20, _storyWindowRect.height - 20));

        if (!_selectedStoryId.HasValue)
            return;

        GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        // 通过空行调整布局
        GUILayout.Space(10f);

        // 话本名称
        string storyName = GetStoryName(_selectedStoryId.Value);
        // 根据是否包含中文设置字体大小
        GUIStyle nameStyle = new GUIStyle(_itemNameStyle);
        nameStyle.fontSize = ContainsChinese(storyName) ? 20 : 16;
        GUILayout.Label(storyName, nameStyle);
        
        // 绘制分割线
        if (_lineTexture != null)
        {
            GUILayout.Space(5f);
            // 创建无边框样式并设置颜色和居中对齐
            GUIStyle lineStyle = new GUIStyle();
            lineStyle.normal.background = null;
            lineStyle.border = new RectOffset(0, 0, 0, 0);
            lineStyle.margin = new RectOffset(0, 0, 0, 0);
            lineStyle.padding = new RectOffset(0, 0, 0, 0);
            lineStyle.alignment = TextAnchor.MiddleCenter;
            // 设置颜色
            GUI.color = new Color(226/255f, 141/255f, 120/255f);
            GUILayout.Box(_lineTexture, lineStyle, GUILayout.Width(280), GUILayout.Height(20));
            // 恢复颜色
            GUI.color = Color.white;
            GUILayout.Space(5f);
        }
        else
        {
            GUILayout.Space(10f);
        }

        // 话本描述
        string storyDesc = GetStoryDescription(_selectedStoryId.Value);
        GUIStyle descStyle = new GUIStyle(_contentTextStyle);
        descStyle.fontSize = ContainsChinese(storyDesc) ? 18 : 16;
        descStyle.wordWrap = true;
        GUILayout.Label(storyDesc, descStyle);

        GUILayout.EndVertical();
        GUILayout.EndArea();

        // 允许窗口拖动
        GUI.DragWindow(new Rect(0, 0, float.MaxValue, 20));
    }
    
    // 获取话本名称
    private string GetStoryName(int storyId)
    {
        try
        {
            if (Mainload.XiQuID_Enter > storyId)
            {
                string[] storyInfo = AllText.Text_AllXiQu[Mainload.XiQuID_Enter][storyId].Split('|');
                if (storyInfo.Length > 0)
                {
                    return storyInfo[0];
                }
            }
        }
        catch {}
        return _vStr.t($"Text_AllXiQu.{storyId}").Split('|')[0];
    }
    
    // 获取话本描述
    private string GetStoryDescription(int storyId)
    {
        try
        {
            if (Mainload.XiQuID_Enter > storyId)
            {
                string[] storyInfo = AllText.Text_AllXiQu[Mainload.XiQuID_Enter][storyId].Split('|');
                if (storyInfo.Length > 1)
                {
                    return storyInfo[1];
                }
            }
        }
        catch {}
        return _vStr.t($"Text_AllXiQu.{storyId}").Split('|')[1];
    }
    
    /// <summary>
    /// 绘制带符号的属性（正负值都显示）
    /// </summary>
    private void DrawSignedProperty(string value, string label)
    {
        int val = int.Parse(value);
        if (val != 0)
        {
            string sign = val > 0 ? "+" : "";
            string text = label.Replace("@", sign + value);
            GUIStyle textStyle = new GUIStyle(_contentTextStyle);
            textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
            GUILayout.Label(text, textStyle);
        }
    }

    /// <summary>
    /// 绘制特殊物品的说明
    /// </summary>
    private void DrawSpecialItemDescription(int propId)
    {
        switch (propId.ToString())
        {
            case "75":
            {
                string text = _vStr.t("Text_UIA.1258");
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
                break;
            }
            case "282":
            {
                string text = _vStr.t("Text_UIA.1259");
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
                break;
            }
            case "283":
            {
                string text = _vStr.t("Text_UIA.1260");
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
                break;
            }
            case "285":
            {
                string text = _vStr.t("Text_UIA.1808");
                GUIStyle textStyle = new GUIStyle(_contentTextStyle);
                textStyle.fontSize = ContainsChinese(text) ? 18 : 16;
                GUILayout.Label(text, textStyle);
                break;
            }
        }
    }
    
    /// <summary>
    /// 设置悬浮窗可见性
    /// </summary>
    public void SetVisible(bool visible)
    {
        _isVisible = visible;
    }
    
    /// <summary>
    /// 设置选中的物品ID
    /// </summary>
    public void SetSelectedPropId(int? propId)
    {
        _selectedPropId = propId;
    }
}