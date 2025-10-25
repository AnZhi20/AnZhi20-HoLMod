using System;
using System.IO;
using System.Text;
using BepInEx.Logging;
using cs.HoLMod.Tool;
using UnityEngine;

public class WindowMain : MonoBehaviour
{
    private int selectedTab = 0;
    private string[] tabs = { "窗口A", "窗口B", "窗口C", "窗口D", "窗口E" };
    private WindowA windowA;
    private WindowB windowB;
    private WindowC windowC;
    private WindowD windowD;
    private WindowE windowE;
    
    // 日志文件路径
    private string _logFilePath;
    // 日志内容
    private StringBuilder _logContent = new StringBuilder();
    // 日志显示开关
    private bool _showLog = false;
    // 日志滚动位置
    private Vector2 _logScrollPosition = Vector2.zero;
    // 上次显示的日志长度
    private int _lastLogLength = 0;
    // GUI样式
    private GUIStyle _boldLabelStyle;
    private GUIStyle _warningLabelStyle;
    private GUIStyle _errorLabelStyle;
    private bool _stylesInitialized = false;

    // 窗口是否可见
    private bool _isVisible = false;
    
    // 插件日志器
    private ManualLogSource _logger;
    
    public void Initialize(ManualLogSource logger)
    {
        _logger = logger;
        
        // 初始化各个窗口的实例
        windowA = new WindowA();
        windowB = new WindowB();
        windowC = new WindowC();
        windowD = new WindowD();
        windowE = new WindowE();
        
        // 设置日志文件路径
        _logFilePath = Path.Combine(Application.persistentDataPath, "GameCheat.log");
        
        // 清理旧日志
        try
        {
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
            }
            
            // 创建新日志文件
            using (FileStream fs = File.Create(_logFilePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("=== GameCheat 日志开始 ===\n");
                fs.Write(info, 0, info.Length);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("无法创建日志文件: " + ex.Message);
        }
    }
    
    public void ToggleWindow()
    {
        _isVisible = !_isVisible;
        AddLog("窗口已" + (_isVisible ? "显示" : "隐藏"));
    }

    
    private void InitializeGuiStyles()
    {
        _boldLabelStyle = new GUIStyle(GUI.skin.label);
        _boldLabelStyle.fontStyle = FontStyle.Bold;
        _boldLabelStyle.normal.textColor = Color.white;
        
        _warningLabelStyle = new GUIStyle(GUI.skin.label);
        _warningLabelStyle.normal.textColor = Color.yellow;
        
        _errorLabelStyle = new GUIStyle(GUI.skin.label);
        _errorLabelStyle.normal.textColor = Color.red;
    }

    private void OnGUI()
    {
        if (!_isVisible)
            return;
            
        // 初始化GUI样式（只能在OnGUI内部调用）
        if (!_stylesInitialized)
        {
            InitializeGuiStyles();
            _stylesInitialized = true;
        }
        
        // 创建窗口
        GUI.Window(0, new Rect(20, 20, 800, 600), DrawWindow, "HolMod 工具");
    }
    
    private void DrawWindow(int windowID)
    {
        // 绘制标签栏
        selectedTab = GUILayout.Toolbar(selectedTab, tabs, GUILayout.Height(30));

        // 绘制分割线
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
        
        // 绘制日志按钮
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        _showLog = GUILayout.Toggle(_showLog, "显示日志", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        // 根据选中的标签显示对应的窗口内容
        switch (selectedTab)
        {
            case 0:
                windowA.OnGUI();
                break;
            case 1:
                windowB.OnGUI();
                break;
            case 2:
                windowC.OnGUI();
                break;
            case 3:
                windowD.OnGUI();
                break;
            case 4:
                windowE.OnGUI();
                break;
        }
        
        // 绘制日志区域
        if (_showLog)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("日志:", _boldLabelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            _logScrollPosition = GUILayout.BeginScrollView(_logScrollPosition, GUILayout.Height(150));
            GUILayout.TextArea(_logContent.ToString(), GUILayout.ExpandWidth(true));
            GUILayout.EndScrollView();
        }
        
        // 允许拖动窗口
        GUI.DragWindow();
    }

    private void Update()
    {
        // 刷新日志
        if (_showLog)
        {
            RefreshLog();
        }
    }
    
    private void RefreshLog()
    {
        try
        {
            if (File.Exists(_logFilePath))
            {
                string logContent = File.ReadAllText(_logFilePath);
                if (logContent.Length != _lastLogLength)
                {
                    _logContent.Length = 0;
                    _logContent.Append(logContent);
                    _lastLogLength = logContent.Length;
                    
                    // 自动滚动到底部
                    _logScrollPosition.y = float.MaxValue;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("刷新日志失败: " + ex.Message);
        }
    }
    
    // 添加日志
    public void AddLog(string message, LogLevel level = LogLevel.Info)
    {
        try
        {
            string logEntry = string.Format("[{0}] [{1}] {2}\n", 
                DateTime.Now.ToString("HH:mm:ss.fff"),
                level.ToString(),
                message);
            
            // 写入文件
            using (StreamWriter writer = File.AppendText(_logFilePath))
            {
                writer.Write(logEntry);
            }
            
            // 控制台输出
            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log(logEntry);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(logEntry);
                    break;
                case LogLevel.Error:
                    Debug.LogError(logEntry);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("添加日志失败: " + ex.Message);
        }
    }
    
    // 枚举日志级别
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }
}