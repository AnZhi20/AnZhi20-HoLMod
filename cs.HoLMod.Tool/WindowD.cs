using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace cs.HoLMod.Tool
{
    public class WindowD : MonoBehaviour
    {
        // 滚动位置
        private Vector2 _leftScrollPosition = Vector2.zero;
        private Vector2 _rightScrollPosition = Vector2.zero;
        
        // 当前选中的数据类型
        private string _selectedDataType = "";
        
        // 当前加载的数据
        private object _currentData = null;
        
        // 遍历路径（索引列表）
        private List<List<int>> _traversalPath = new List<List<int>>();
        
        // 最大遍历深度
        private const int MAX_DEPTH = 5;
        
        // 数据层级显示模式
        private int _currentDepth = 0;
        
        // GUI样式
        private GUIStyle _selectedButtonStyle;
        private GUIStyle _normalButtonStyle;
        private GUIStyle _depthLabelStyle;
        private GUIStyle _infoLabelStyle;
        private bool _stylesInitialized = false;
        
        // 支持的数据类型列表
        private List<string> _supportedDataTypes = new List<string>
        {
            "Guan_JingCheng",
            "PlayTimeRun",
            "MemberNumWillDead",
            "NuLiNum",
            "VisionIndex",
            "SceneID",
            "CityID_CanAttack",
            "ZuZhangJueCeID_now",
            "BuildTaoZhuangID_SelectNow",
            "GetMoney_Month",
            "SelectNumPer_Shop",
            "SelectNumPerAct",
            "CitySawSetData",
            "CangShuGeData_Now",
            "XiQuHave_Now",
            "ZhiZeData_ZhangMu",
            "VersionID"
        };
        
        void Awake()
        {
            // GUI样式初始化移至OnGUI方法中
        }
        
        private void InitializeGuiStyles()
        {
            _selectedButtonStyle = new GUIStyle(GUI.skin.button);
            _selectedButtonStyle.normal.textColor = Color.blue;
            _selectedButtonStyle.fontStyle = FontStyle.Bold;
            
            _normalButtonStyle = new GUIStyle(GUI.skin.button);
            
            _depthLabelStyle = new GUIStyle(GUI.skin.label);
            _depthLabelStyle.fontStyle = FontStyle.Bold;
            _depthLabelStyle.normal.textColor = Color.yellow;
            
            _infoLabelStyle = new GUIStyle(GUI.skin.label);
            _infoLabelStyle.normal.textColor = Color.gray;
        }
        
        public void OnGUI()
        {
            if (!_stylesInitialized)
            {
                InitializeGuiStyles();
                _stylesInitialized = true;
            }
            GUILayout.BeginHorizontal();
            
            // 第一列：数据类型列表
            GUILayout.BeginVertical(GUILayout.Width(180));
            GUILayout.Label("数据类型列表", _depthLabelStyle);
            GUILayout.Space(5);
            
            _leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition);
            
            foreach (string dataType in _supportedDataTypes)
            {
                GUIStyle buttonStyle = (_selectedDataType == dataType) ? _selectedButtonStyle : _normalButtonStyle;
                if (GUILayout.Button(dataType, buttonStyle))
                {
                    LoadDataType(dataType);
                }
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            
            // 第二列：数据详情和层级浏览
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            if (string.IsNullOrEmpty(_selectedDataType))
            {
                GUILayout.Label("选择左侧数据类型查看详情", _infoLabelStyle);
            }
            else
            {
                GUILayout.Label(string.Format("当前选择: {0}", Main.GetDisplayName(_selectedDataType)), _depthLabelStyle);
                GUILayout.Space(5);
                
                // 显示返回按钮和层级信息
                GUILayout.BeginHorizontal();
                if (_currentDepth > 0 && GUILayout.Button("返回上一级"))
                {
                    NavigateBack();
                }
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("层级: {0}/{1}", _currentDepth + 1, MAX_DEPTH), _infoLabelStyle);
                GUILayout.EndHorizontal();
                
                GUILayout.Space(5);
                
                // 显示数据内容
                _rightScrollPosition = GUILayout.BeginScrollView(_rightScrollPosition, GUILayout.ExpandHeight(true));
                DisplayData(_currentData);
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }
        
        private void LoadDataType(string dataType)
        {
            _selectedDataType = dataType;
            _currentDepth = 0;
            _traversalPath.Clear();
            _traversalPath.Add(new List<int>());
            
            // 获取数据
            _currentData = Main.GetDataByFieldName(dataType);
        }
        
        private void NavigateBack()
        {
            if (_currentDepth > 0)
            {
                _currentDepth--;
                if (_currentDepth < _traversalPath.Count)
                {
                    _traversalPath = _traversalPath.GetRange(0, _currentDepth + 1);
                }
                
                // 更新当前数据
                List<int> indices = _traversalPath[_currentDepth];
                _currentData = Main.GetObjectFromPath(_selectedDataType, indices);
            }
        }
        
        // 根据数据类型显示数据
        private void DisplayData(object data)
        {
            if (data == null)
            {
                GUILayout.Label("没有数据", _infoLabelStyle);
                return;
            }
            
            // 根据数据类型选择显示方法
            Type dataType = data.GetType();
            
            if (dataType.IsArray || data is IList)
            {
                DisplayListData(data as IList);
            }
            else if (dataType.IsClass && dataType != typeof(string))
            {
                DisplayObjectData(data);
            }
            else
            {
                DisplaySimpleData(data);
            }
        }
        
        // 显示列表数据
        private void DisplayListData(IList list)
        {
            GUILayout.Label(string.Format("列表项数量: {0}", list.Count), _infoLabelStyle);
            GUILayout.Space(5);
            
            if (list.Count == 0)
            {
                GUILayout.Label("空列表", _infoLabelStyle);
                return;
            }
            
            // 获取元素类型
            Type elementType = list.GetType().GetElementType();
            if (elementType == null)
            {
                elementType = list[0].GetType();
            }
            
            // 处理不同类型的列表
            if (elementType == typeof(int))
            {
                DisplayIntList(list as List<int>);
            }
            else if (elementType == typeof(float))
            {
                DisplayFloatList(list as List<float>);
            }
            else if (elementType == typeof(string))
            {
                DisplayStringList(list as List<string>);
            }
            else if (elementType.IsPrimitive || elementType == typeof(decimal))
            {
                DisplayPrimitiveList(list);
            }
            else if (elementType.IsClass)
            {
                DisplayObjectList(list);
            }
            else if (elementType.IsArray || elementType.GetInterface("IList") != null)
            {
                DisplayNestedList(list);
            }
            else
            {
                // 默认显示方式
                DisplayGenericList(list);
            }
        }
        
        // 显示嵌套列表
        private void DisplayNestedList(IList list)
        {
            GUILayout.BeginVertical();
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("[{0}]", i), GUILayout.Width(50));
                
                if (item == null)
                {
                    GUILayout.Label("null", _infoLabelStyle);
                }
                else
                {
                    IList nestedList = item as IList;
                    if (nestedList != null)
                    {
                        if (_currentDepth < MAX_DEPTH - 1)
                        {
                            if (GUILayout.Button(string.Format("列表 (长度: {0})", nestedList.Count), GUILayout.ExpandWidth(true)))
                            {
                                NavigateToNestedList(i);
                            }
                        }
                        else
                        {
                            GUILayout.Label(string.Format("列表 (长度: {0})", nestedList.Count));
                        }
                    }
                    else
                    {
                        GUILayout.Label(item.ToString());
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
            GUILayout.EndVertical();
        }
        
        // 显示对象列表
        private void DisplayObjectList(IList list)
        {
            GUILayout.BeginVertical();
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("[{0}]", i), GUILayout.Width(50));
                
                if (item == null)
                {
                    GUILayout.Label("null", _infoLabelStyle);
                }
                else
                {
                    if (_currentDepth < MAX_DEPTH - 1)
                    {
                        string displayText = GetObjectDisplayText(item);
                        if (GUILayout.Button(displayText, GUILayout.ExpandWidth(true)))
                        {
                            NavigateToNestedObject(i);
                        }
                    }
                    else
                    {
                        GUILayout.Label(item.ToString());
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
            GUILayout.EndVertical();
        }
        
        // 显示对象数据
        private void DisplayObjectData(object obj)
        {
            Type objType = obj.GetType();
            GUILayout.Label(string.Format("对象类型: {0}", objType.Name), _depthLabelStyle);
            GUILayout.Space(5);
            
            // 获取公共属性
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // 获取公共字段
            FieldInfo[] fields = objType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            
            if (properties.Length == 0 && fields.Length == 0)
            {
                GUILayout.Label("对象没有可访问的属性或字段", _infoLabelStyle);
                return;
            }
            
            // 显示属性
            if (properties.Length > 0)
            {
                GUILayout.Label("属性:", _depthLabelStyle);
                foreach (PropertyInfo prop in properties)
                {
                    try
                    {
                        object value = prop.GetValue(obj);
                        string valueText = value == null ? "null" : value.ToString();
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(string.Format("{0}: ", prop.Name), GUILayout.Width(150));
                        GUILayout.Label(valueText, GUILayout.ExpandWidth(true));
                        GUILayout.EndHorizontal();
                    }
                    catch
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(string.Format("{0}: ", prop.Name), GUILayout.Width(150));
                        GUILayout.Label("(无法访问)", _infoLabelStyle, GUILayout.ExpandWidth(true));
                        GUILayout.EndHorizontal();
                    }
                }
            }
            
            // 显示字段
            if (fields.Length > 0)
            {
                GUILayout.Space(10);
                GUILayout.Label("字段:", _depthLabelStyle);
                foreach (FieldInfo field in fields)
                {
                    try
                    {
                        object value = field.GetValue(obj);
                        string valueText = value == null ? "null" : value.ToString();
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(string.Format("{0}: ", field.Name), GUILayout.Width(150));
                        GUILayout.Label(valueText, GUILayout.ExpandWidth(true));
                        GUILayout.EndHorizontal();
                    }
                    catch
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(string.Format("{0}: ", field.Name), GUILayout.Width(150));
                        GUILayout.Label("(无法访问)", _infoLabelStyle, GUILayout.ExpandWidth(true));
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
        
        // 显示简单数据
        private void DisplaySimpleData(object data)
        {
            GUILayout.TextArea(data.ToString(), GUILayout.ExpandWidth(true), GUILayout.Height(100));
        }
        
        // 显示整数列表
        private void DisplayIntList(List<int> list)
        {
            StringBuilder content = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                content.AppendFormat("{0}: {1}\n", i, list[i]);
            }
            GUILayout.TextArea(content.ToString(), GUILayout.ExpandWidth(true));
        }
        
        // 显示浮点数列表
        private void DisplayFloatList(List<float> list)
        {
            StringBuilder content = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                content.AppendFormat("{0}: {1}\n", i, list[i]);
            }
            GUILayout.TextArea(content.ToString(), GUILayout.ExpandWidth(true));
        }
        
        // 显示字符串列表
        private void DisplayStringList(List<string> list)
        {
            StringBuilder content = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                content.AppendFormat("{0}: {1}\n", i, list[i]);
            }
            GUILayout.TextArea(content.ToString(), GUILayout.ExpandWidth(true));
        }
        
        // 显示基本类型列表
        private void DisplayPrimitiveList(IList list)
        {
            StringBuilder content = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                content.AppendFormat("{0}: {1}\n", i, list[i]);
            }
            GUILayout.TextArea(content.ToString(), GUILayout.ExpandWidth(true));
        }
        
        // 显示通用列表
        private void DisplayGenericList(IList list)
        {
            GUILayout.BeginVertical();
            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(string.Format("[{0}]", i), GUILayout.Width(50));
                
                if (item == null)
                {
                    GUILayout.Label("null", _infoLabelStyle);
                }
                else
                {
                    GUILayout.Label(item.ToString(), GUILayout.ExpandWidth(true));
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);
            }
            GUILayout.EndVertical();
        }
        
        // 获取对象的显示文本
        private string GetObjectDisplayText(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            
            Type objType = obj.GetType();
            
            // 尝试获取ID或Name属性
            PropertyInfo idProp = objType.GetProperty("ID", BindingFlags.Instance | BindingFlags.Public);
            if (idProp != null)
            {
                object idValue = idProp.GetValue(obj);
                return string.Format("{0} (ID: {1})", objType.Name, idValue);
            }
            
            FieldInfo idField = objType.GetField("ID", BindingFlags.Instance | BindingFlags.Public);
            if (idField != null)
            {
                object idValue = idField.GetValue(obj);
                return string.Format("{0} (ID: {1})", objType.Name, idValue);
            }
            
            PropertyInfo nameProp = objType.GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);
            if (nameProp != null)
            {
                object nameValue = nameProp.GetValue(obj);
                return string.Format("{0}: {1}", objType.Name, nameValue);
            }
            
            FieldInfo nameField = objType.GetField("Name", BindingFlags.Instance | BindingFlags.Public);
            if (nameField != null)
            {
                object nameValue = nameField.GetValue(obj);
                return string.Format("{0}: {1}", objType.Name, nameValue);
            }
            
            // 默认返回类型名称
            return objType.Name;
        }
        
        // 导航到嵌套列表
        private void NavigateToNestedList(int index)
        {
            if (_currentDepth >= MAX_DEPTH - 1)
            {
                return;
            }
            
            // 更新遍历路径
            if (_currentDepth + 1 >= _traversalPath.Count)
            {
                // 创建新的层级路径
                List<int> newPath = new List<int>(_traversalPath[_currentDepth]);
                newPath.Add(index);
                _traversalPath.Add(newPath);
            }
            else
            {
                // 使用已有路径
                _traversalPath[_currentDepth + 1] = new List<int>(_traversalPath[_currentDepth]);
                _traversalPath[_currentDepth + 1].Add(index);
            }
            
            // 获取嵌套列表数据
            object nestedListObj = Main.GetObjectFromPath(_selectedDataType, _traversalPath[_currentDepth + 1]);
            if (nestedListObj != null)
            {
                _currentData = nestedListObj;
                _currentDepth++;
            }
        }
        
        // 导航到嵌套对象
        private void NavigateToNestedObject(int index)
        {
            if (_currentDepth >= MAX_DEPTH - 1)
            {
                return;
            }
            
            // 更新遍历路径
            if (_currentDepth + 1 >= _traversalPath.Count)
            {
                // 创建新的层级路径
                List<int> newPath = new List<int>(_traversalPath[_currentDepth]);
                newPath.Add(index);
                _traversalPath.Add(newPath);
            }
            else
            {
                // 使用已有路径
                _traversalPath[_currentDepth + 1] = new List<int>(_traversalPath[_currentDepth]);
                _traversalPath[_currentDepth + 1].Add(index);
            }
            
            // 获取嵌套对象数据
            object nestedObj = Main.GetObjectFromPath(_selectedDataType, _traversalPath[_currentDepth + 1]);
            if (nestedObj != null)
            {
                _currentData = nestedObj;
                _currentDepth++;
            }
        }
    }
}
