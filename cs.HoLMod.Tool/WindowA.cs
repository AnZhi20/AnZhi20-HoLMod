using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace cs.HoLMod.Tool
{
    public class WindowA : MonoBehaviour
    {
        // 滚动位置
        private Vector2 _leftScrollPosition = Vector2.zero;
        private Vector2 _rightScrollPosition = Vector2.zero;
        
        // 当前选中的字段名
        private string _selectedFieldName = "";
        
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
        void Awake()
        {            
            // 初始化移至OnGUI()方法中
        }
        
        private void InitializeGuiStyles()
        {
            _selectedButtonStyle = new GUIStyle(GUI.skin.button);
            _selectedButtonStyle.normal.textColor = Color.yellow;
            _selectedButtonStyle.fontStyle = FontStyle.Bold;

            _normalButtonStyle = new GUIStyle(GUI.skin.button);

            _depthLabelStyle = new GUIStyle(GUI.skin.label);
            _depthLabelStyle.fontStyle = FontStyle.Bold;
            _depthLabelStyle.normal.textColor = Color.cyan;

            _infoLabelStyle = new GUIStyle(GUI.skin.label);
            _infoLabelStyle.normal.textColor = Color.gray;
        }
        
        public void OnGUI()
        {            
            // 确保GUI样式已初始化
            if (!_stylesInitialized)
            {                
                InitializeGuiStyles();
                _stylesInitialized = true;
            }
            
            GUILayout.BeginHorizontal();
            
            // 第一列：Mainload字段列表
            GUILayout.BeginVertical(GUILayout.Width(180));
            GUILayout.Label("MainLoad数据列表", _depthLabelStyle);
            GUILayout.Space(5);
            
            _leftScrollPosition = GUILayout.BeginScrollView(_leftScrollPosition);
            
            if (Main.MainloadFieldNames.Count > 0)
            {
                foreach (string fieldName in Main.MainloadFieldNames)
                {
                    GUIStyle buttonStyle = (_selectedFieldName == fieldName) ? _selectedButtonStyle : _normalButtonStyle;
                    if (GUILayout.Button(fieldName, buttonStyle))
                    {
                        SelectMainloadField(fieldName);
                    }
                }
            }
            else
            {
                GUILayout.Label("没有加载到Mainload字段", _infoLabelStyle);
                GUILayout.Space(10);
                if (GUILayout.Button("重新加载字段"))
                {
                    // 这里应该调用Main.cs中的重新加载方法，但暂时无法直接调用
                    Debug.LogWarning("重新加载字段功能尚未实现");
                }
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            
            // 第二列及以后：数据详情和层级浏览
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            if (string.IsNullOrEmpty(_selectedFieldName))
            {
                GUILayout.Label("选择左侧按钮查看数据详情", _infoLabelStyle);
            }
            else
            {
                GUILayout.Label(string.Format("当前选择: {0}", Main.GetDisplayName(_selectedFieldName)), _depthLabelStyle);
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
                DisplayCurrentLevelData();
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }
        
        private void SelectMainloadField(string fieldName)
        {
            _selectedFieldName = fieldName;
            _currentDepth = 0;
            _traversalPath.Clear();
            _traversalPath.Add(new List<int>());
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
            }
        }
        
        private void DisplayCurrentLevelData()
        {
            try
            {
                object data = GetCurrentLevelData();
                if (data == null)
                {
                    GUILayout.Label("没有数据", _infoLabelStyle);
                    return;
                }
                
                // 根据数据类型显示不同内容
                if (data.GetType().IsArray || data is IList)
                {
                    DisplayListData(data);
                }
                else if (data.GetType().IsClass && data.GetType() != typeof(string))
                {
                    DisplayObjectData(data);
                }
                else
                {
                    DisplayValueData(data);
                }
            }
            catch (System.Exception ex)
            {
                GUILayout.Label("显示数据时出错: " + ex.Message, _infoLabelStyle);
            }
        }
        
        private object GetCurrentLevelData()
        {
            if (string.IsNullOrEmpty(_selectedFieldName) || _currentDepth >= _traversalPath.Count)
            {
                return null;
            }
            
            List<int> indices = _traversalPath[_currentDepth];
            return Main.GetObjectFromPath(_selectedFieldName, indices);
        }
        
        private void DisplayListData(object listData)
        {
            IList list = listData as IList;
            if (list == null)
            {
                GUILayout.Label("无法将数据转换为列表", _infoLabelStyle);
                return;
            }
            
            GUILayout.Label(string.Format("列表项数量: {0}", list.Count), _infoLabelStyle);
            GUILayout.Space(5);
            
            if (list.Count == 0)
            {
                GUILayout.Label("空列表", _infoLabelStyle);
                return;
            }
            
            // 如果是基本类型的列表，直接显示值
            Type elementType = list.GetType().GetElementType();
            if (elementType == null && list.Count > 0)
            {
                elementType = list[0].GetType();
            }
            
            if (elementType != null && (elementType.IsPrimitive || elementType == typeof(string)))
            {
                StringBuilder content = new StringBuilder();
                for (int i = 0; i < list.Count; i++)
                {
                    content.AppendFormat("{0}: {1}\n", i, list[i]);
                }
                GUILayout.TextArea(content.ToString(), GUILayout.ExpandWidth(true));
            }
            else
            {
                // 显示可点击的列表项
                int itemsPerPage = 20; // 每页显示的项数
                int currentPage = 0;
                
                // 简单的分页逻辑
                if (list.Count > itemsPerPage)
                {
                    GUILayout.BeginHorizontal();
                    for (int page = 0; page < list.Count; page += itemsPerPage)
                    {
                        if (GUILayout.Button(string.Format("第{0}页", page / itemsPerPage + 1), GUILayout.Width(80)))
                        {
                            currentPage = page;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                
                // 显示当前页的列表项
                int endIndex = Mathf.Min(currentPage + itemsPerPage, list.Count);
                for (int i = currentPage; i < endIndex; i++)
                {
                    object item = list[i];
                    string displayText = GetItemDisplayText(item, i);
                    
                    GUILayout.BeginHorizontal();
                    if (_currentDepth < MAX_DEPTH - 1 && Main.HasSubData(item))
                    {
                        if (GUILayout.Button(displayText, GUILayout.ExpandWidth(true)))
                        {
                            NavigateToNextLevel(i);
                        }
                    }
                    else
                    {
                        GUILayout.Label(displayText, GUILayout.ExpandWidth(true));
                    }
                    
                    // 显示该项的类型信息
                    GUILayout.Label(string.Format("({0})", item == null ? "null" : item.GetType().Name), _infoLabelStyle, GUILayout.Width(120));
                    GUILayout.EndHorizontal();
                }
            }
        }
        
        private void DisplayObjectData(object objData)
        {
            if (objData == null)
            {
                GUILayout.Label("对象为null", _infoLabelStyle);
                return;
            }
            
            Type objType = objData.GetType();
            GUILayout.Label(string.Format("对象类型: {0}", objType.Name), _infoLabelStyle);
            GUILayout.Space(5);
            
            // 获取所有公共属性
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // 获取所有公共字段
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
                        object value = prop.GetValue(objData);
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
                        object value = field.GetValue(objData);
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
        
        private void DisplayValueData(object valueData)
        {
            GUILayout.TextArea(valueData.ToString(), GUILayout.ExpandWidth(true), GUILayout.Height(100));
        }
        
        private string GetItemDisplayText(object item, int index)
        {
            if (item == null)
            {
                return string.Format("[{0}] null", index);
            }
            
            // 尝试获取有意义的显示文本
            try
            {
                // 检查是否有ID、Name等常见属性
                PropertyInfo idProp = item.GetType().GetProperty("ID", BindingFlags.Instance | BindingFlags.Public);
                if (idProp != null)
                {
                    object idValue = idProp.GetValue(item);
                    return string.Format("[{0}] ID: {1}", index, idValue);
                }
                
                FieldInfo idField = item.GetType().GetField("ID", BindingFlags.Instance | BindingFlags.Public);
                if (idField != null)
                {
                    object idValue = idField.GetValue(item);
                    return string.Format("[{0}] ID: {1}", index, idValue);
                }
                
                PropertyInfo nameProp = item.GetType().GetProperty("Name", BindingFlags.Instance | BindingFlags.Public);
                if (nameProp != null)
                {
                    object nameValue = nameProp.GetValue(item);
                    return string.Format("[{0}] {1}", index, nameValue);
                }
                
                FieldInfo nameField = item.GetType().GetField("Name", BindingFlags.Instance | BindingFlags.Public);
                if (nameField != null)
                {
                    object nameValue = nameField.GetValue(item);
                    return string.Format("[{0}] {1}", index, nameValue);
                }
                
                // 默认显示
                string toString = item.ToString();
                if (toString.Length > 50)
                {
                    toString = toString.Substring(0, 50) + "...";
                }
                return string.Format("[{0}] {1}", index, toString);
            }
            catch
            {
                return string.Format("[{0}] (无法获取显示文本)", index);
            }
        }
        
        private void NavigateToNextLevel(int index)
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
            
            // 进入下一层
            _currentDepth++;
        }
        
        // 获取当前路径的显示名称
        private string GetCurrentPathDisplayName()
        {
            if (string.IsNullOrEmpty(_selectedFieldName) || _currentDepth >= _traversalPath.Count)
            {
                return _selectedFieldName;
            }
            
            return Main.GetDisplayName(Main.BuildPath(_selectedFieldName, _traversalPath[_currentDepth]));
        }
    }
}
