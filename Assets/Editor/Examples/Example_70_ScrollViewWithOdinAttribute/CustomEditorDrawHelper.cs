using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AllTrickOverView.Core;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ToolKits
{
    public static class CustomEditorDrawHelper
    {
        private static Dictionary<Type, ACustomEditorDrawer> CustomEditorDrawerMap = null;

        public static ACustomEditorDrawer GetCustomEditorDrawer(Type drawType)
        {
            if (CustomEditorDrawerMap==null)
            {
                CustomEditorDrawerMap = new Dictionary<Type, ACustomEditorDrawer>();
                Assembly assembly = Assembly.GetAssembly(typeof(CustomEditorDrawHelper));
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    object[] objects = type.GetCustomAttributes(typeof(CustomEditorDrawerAttribute), true);
                    if (objects.Length==0 || type.IsAbstract)
                    {
                        continue;
                    }

                    var attr = objects[0] as CustomEditorDrawerAttribute;
                    if (attr==null)
                    {
                        Debug.LogError($"get CustomEditorDrawerAttribute failed!");
                        continue;
                    }

                    ACustomEditorDrawer aDrawer = Activator.CreateInstance(type) as ACustomEditorDrawer;
                    if (aDrawer == null)
                    {
                        Debug.LogError($"{type} is not ACustomEditorDrawer");
                        continue;
                    }
                    CustomEditorDrawerMap.Add(attr.DrawType, aDrawer);
                }
            }

            if (CustomEditorDrawerMap.TryGetValue(drawType, out var drawer))
            {
                return drawer;
            }
            return null;
        }

        public static void DrawCustomEditor(Rect rect,object obj, CustomDrawerObject customDrawerObject)
        {
            GetCustomEditorDrawer(obj.GetType()).OnGui(rect, obj, customDrawerObject);
        }
    }
    
    public class AbstractListView : EditorWindow
{
    [MenuItem("Test/New")]
    private static void Init()
    {
        GetWindow<AbstractListView>();
    }

    
    private List<ExampleBaseItem> itemList;
    public SerializedObject _serializedObject;
    public CustomDrawerObject itemListDrawerObject;

    private void OnEnable()
    {
        if (itemListDrawerObject==null)
        {
            
            if (itemList==null)
            {
                itemList = new List<ExampleBaseItem>();
                for (int i = 0; i < 1000; i++)
                {
                    itemList.Add(new ExampleItemA());
                    itemList.Add(new ExampleItemB());
                }
            }
            itemListDrawerObject = CustomDrawerObject.Create(itemList);
        }
        else
        {
            itemList = itemListDrawerObject.Value as List<ExampleBaseItem>;
        }
        
        itemListDrawerObject.GetContextData<ScrollListContextData>().OnScrollItemRightClick+=(OnScrollListRightClick);
    }

    void OnGUI()
    {
        
        Rect rect = EditorGUILayout.GetControlRect(false, 500, GUILayout.Width(500));
        ListScrollViewDrawHelper.DrawScrollList(rect, 18,480, itemListDrawerObject);
    }

    private void OnScrollListRightClick(int index)
    {
        var list = itemListDrawerObject.Value as List<ExampleBaseItem>;
        if (list==null)
        {
            return;
        }
        var genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("删除"), false, () =>
        {   
            list.RemoveAt(index);
        });
        genericMenu.AddItem(new GUIContent("插入"), false, () =>
        {   
            list.Insert(index, new ExampleItemA());
        });
        genericMenu.AddItem(new GUIContent("功能合集/功能2"), false, () => { Debug.Log("功能2"); });
        genericMenu.AddItem(new GUIContent("功能合集/功能3"), false, () => { Debug.Log("功能3"); });
        genericMenu.AddSeparator("功能合集/");
        genericMenu.AddItem(new GUIContent("功能合集/功能4"), false, () => { Debug.Log("功能4"); });
        genericMenu.ShowAsContext();
    }
}
    
    public class CustomDrawerObject : SerializedScriptableObject
    {
        public object Value;
        private Dictionary<Type, object> m_contextData = new Dictionary<Type, object>();
        
        public static CustomDrawerObject Create (object obj)
        {
            var drawerObject = CreateInstance<CustomDrawerObject>();
            drawerObject.Value = obj;
            return drawerObject;
        }

        public static void Initialize(object rowObject, CustomDrawerObject customDrawerObject)
        {
            if (customDrawerObject==null)
            {
                customDrawerObject = CreateInstance<CustomDrawerObject>();
                customDrawerObject.Value = rowObject;
            }
            else
            {
                rowObject = customDrawerObject.Value;
            }
        }

        public T GetContextData<T>() where T : SerializedScriptableObject
        {
            object contextData;
            if (m_contextData.TryGetValue(typeof(T), out contextData))
            {
                return contextData as T;
            }
            
            contextData = CreateInstance<T>();
            m_contextData.Add(typeof(T), contextData);
            return contextData as T;
        }
    }
    
    public class ScrollListContextData : SerializedScriptableObject
    {
        public Vector2 ScrollPosition;

        public bool HasClick = false;
        public Vector2 ClickedMousePos;

        public Action<int> OnScrollItemLeftClick;

        public Action<int> OnScrollItemRightClick;
    }
}




