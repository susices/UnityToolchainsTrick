using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AllTrickOverView.Core;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    public static class CustomEditorDrawHelper
    {
        private static Dictionary<Type, ICustomEditorDrawer> CustomEditorDrawerMap;

        public static ICustomEditorDrawer GetCustomEditorDrawer<T>()
        {
            if (CustomEditorDrawerMap==null)
            {
                CustomEditorDrawerMap = new Dictionary<Type, ICustomEditorDrawer>();
                Assembly assembly = Assembly.GetAssembly(typeof(CustomEditorDrawHelper));
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    object[] objects = type.GetCustomAttributes(typeof(CustomPreviewAttribute), true);
                    if (objects.Length==0 || type.IsAbstract)
                    {
                        continue;
                    }
                    
                    ICustomEditorDrawer Idrawer = Activator.CreateInstance(type) as ICustomEditorDrawer;
                    CustomEditorDrawerMap.Add(type, Idrawer);
                }
            }

            if (CustomEditorDrawerMap.TryGetValue(typeof(T), out var drawer))
            {
                return drawer;
            }
            return null;
        }
    }
    
    
    public class TestListView : EditorWindow
    {
        [MenuItem("Test/Old")]
        private static void Init()
        {
            GetWindow<TestListView>();
        }

        private const int s_RowCount = 400;
        private const int s_ColCount = 30;

        private float m_RowHeight = 18f;
        private float m_ColWidth = 52f;
        private Vector2 m_ScrollPosition;

        void OnGUI()
        {
            OnDrawListView();
        }

        private void OnDrawListView()
        {
            m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
            for (int i = 0; i < s_RowCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j < s_ColCount; j++)
                {
                    EditorGUILayout.LabelField((i * 100 + j).ToString(), GUILayout.Width(m_ColWidth));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }


    public class NewTestListView : EditorWindow
{
    [MenuItem("Test/New")]
    private static void Init()
    {
        GetWindow<NewTestListView>();
    }

    private const int s_RowCount = 400;
    private const int s_ColCount = 30;

    private float m_RowHeight = 18f;
    private float m_ColWidth = 52f;
    private Vector2 m_ScrollPosition;

    void OnGUI()
    {
        OnDrawListView2();
    }

    private void OnDrawListView2()
    {
        Rect totalRect = new Rect(0, 0, position.width, position.height);
        Rect contentRect = new Rect(0, 0, s_ColCount * m_ColWidth, s_RowCount * m_RowHeight);
        m_ScrollPosition = GUI.BeginScrollView(totalRect, m_ScrollPosition, contentRect);

        int num;
        int num2;
        GetFirstAndLastRowVisible(out num, out num2, totalRect.height);
        if (num2 >= 0)
        {
            int numVisibleRows = num2 - num + 1;
            IterateVisibleItems(num, numVisibleRows, contentRect.width, totalRect.height);
        }

        GUI.EndScrollView(true);
    }

    /// <summary>
    /// 获取可显示的起始行和结束行
    /// </summary>
    /// <param name="firstRowVisible">起始行</param>
    /// <param name="lastRowVisible">结束行</param>
    /// <param name="viewHeight">视图高度</param>
    private void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible, float viewHeight)
    {
        if (s_RowCount == 0)
        {
            firstRowVisible = lastRowVisible = -1;
        }
        else
        {
            float y = m_ScrollPosition.y;
            float height = viewHeight;
            firstRowVisible = (int)Mathf.Floor(y / m_RowHeight);
            lastRowVisible = firstRowVisible + (int)Mathf.Ceil(height / m_RowHeight);
            firstRowVisible = Mathf.Max(firstRowVisible, 0);
            lastRowVisible = Mathf.Min(lastRowVisible, s_RowCount - 1);
            if (firstRowVisible >= s_RowCount && firstRowVisible > 0)
            {
                m_ScrollPosition.y = 0f;
                GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible, viewHeight);
            }
        }
    }

    /// <summary>
    /// 迭代绘制可显示的项
    /// </summary>
    /// <param name="firstRow">起始行</param>
    /// <param name="numVisibleRows">总可显示行数</param>
    /// <param name="rowWidth">每行的宽度</param>
    /// <param name="viewHeight">视图高度</param>
    private void IterateVisibleItems(int firstRow, int numVisibleRows, float rowWidth, float viewHeight)
    {
        int i = 0;
        while (i < numVisibleRows)
        {
            int num2 = firstRow + i;
            Rect rowRect = new Rect(0f, (float)num2 * m_RowHeight, rowWidth, m_RowHeight);
            float num3 = rowRect.y - m_ScrollPosition.y;
            if (num3 <= viewHeight)
            {
                Rect colRect = new Rect(rowRect);
                colRect.width = m_ColWidth;

                for (int j = 0; j < s_ColCount; j++)
                {
                    EditorGUI.LabelField(colRect, (num2 * 100 + j).ToString());
                    colRect.x += colRect.width;
                }
            }
            i++;
        }
    }
}
    
}




