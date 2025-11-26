using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using YuanAPI;

namespace cs.HoLMod.AddItem;

[BepInPlugin(MODGUID, MODNAME, VERSION)]
[BepInDependency(YuanAPIPlugin.MODGUID)]
public class AddItem : BaseUnityPlugin
{
    public const string MODGUID = "cs.HoLMod.AddItem.AnZhi20";
    public const string MODNAME = "HoLMod.AddItem";
    public const string VERSION = "4.0.0";
    
    internal new static ManualLogSource Logger;
    internal static string LocaleNamespace = "AddItem";
    
    private static IAddItemModel _model;
    private static IAddItemView _view;
    private static AddItemController _controller;
    
    private void Awake()
    {
        Logger = base.Logger;
        
        CheckConfigVersion();
        LoadLocalizations();
    }
    
    private void Start()
    {
        Localization.OnLanguageChanged += ItemData.RefreshText;
        ItemData.RefreshText(Localization.GetLocale(Mainload.SetData[4]));
        ItemData.RefreshProp();
        InitializeMVC();
        
        Logger.LogInfo("物品添加器初始化完毕");
    }

    private void CheckConfigVersion()
    {
        var versionConfig = Config.Bind(new ConfigDefinition("内部配置", "已加载版本"), VERSION,
            new ConfigDescription("用于跟踪插件版本，请勿手动修改"));

        if (versionConfig.Value == VERSION) 
            return;
        
        Logger.LogInfo($"检测到插件版本更新至 {VERSION}，配置更新");
        versionConfig.Value = VERSION;
    }

    private static void LoadLocalizations()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var modPath = Path.GetDirectoryName(executingAssembly.Location);
        Localization.LoadFromPath(modPath);
    }

    private static void InitializeMVC()
    {
        // 初始化Model
        _model = new AddItemModule();

        // 初始化View
        var obj = new GameObject("AddItemUI");
        obj.AddComponent<IMGUIAddItemView>();
        // 添加悬浮窗组件
        obj.AddComponent<cs.HoLMod.AddItem.Views.IFloatingView>();
        DontDestroyOnLoad(obj);
        _view = obj.GetComponent<IMGUIAddItemView>();
        _view.Initialize(_model);

        // 初始化Controller
        _controller = new AddItemController(_model,  _view);
    }
}

