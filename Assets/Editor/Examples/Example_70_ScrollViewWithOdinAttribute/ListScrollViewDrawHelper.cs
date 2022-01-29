using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    
    public static class ListScrollViewDrawHelper
    {
        public static void DrawScrollList(Rect totalRect, int rowHeight, int rowWidth, CustomDrawerObject customDrawerObject)
        {
            var list = customDrawerObject.Value as IList;
            if (list==null)
            {
                Debug.LogError("DrawScrollList list is null");
                return;
            }

            ScrollListContextData scrollListContextData = customDrawerObject.GetContextData<ScrollListContextData>();
            
            int rawCount = list.Count;
            Rect contentRect = new Rect(totalRect.x, totalRect.y, rowWidth, rowHeight * rawCount);
            scrollListContextData.ScrollPosition = GUI.BeginScrollView(totalRect, scrollListContextData.ScrollPosition, contentRect);

            

            int firstRowVisible;
            int lastRowVisible;
            GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible, totalRect.height, rawCount, rowHeight,
                ref scrollListContextData.ScrollPosition);

            if (lastRowVisible>=0)
            {
                int numVisibleRows = lastRowVisible - firstRowVisible + 1;
                IterateVisibleItems(totalRect, firstRowVisible, numVisibleRows, contentRect.width, totalRect.height,
                    rowHeight, scrollListContextData.ScrollPosition, list, customDrawerObject);
            }
            GUI.EndScrollView(true);
        }

        private static void GetFirstAndLastRowVisible(out int firstRowVisible, out int lastRowVisible, float viewHeight, int rowCount, int rowHeight,ref Vector2 scrollPos)
        {
            if (rowCount==0)
            {
                firstRowVisible = lastRowVisible = -1;
            }
            else
            {
                float y = scrollPos.y;
                float height = viewHeight;
                firstRowVisible = (int)Mathf.Floor(y / rowHeight);
                lastRowVisible = firstRowVisible + (int)Mathf.Ceil(height / rowHeight);
                firstRowVisible = Mathf.Max(firstRowVisible, 0);
                lastRowVisible = Mathf.Min(lastRowVisible, rowCount - 1);
                if (firstRowVisible >= rowCount && firstRowVisible > 0)
                {
                    scrollPos.y = 0f;
                    GetFirstAndLastRowVisible(out firstRowVisible, out lastRowVisible, viewHeight, rowCount,rowHeight,ref scrollPos);
                }
            }
        }

        private static void IterateVisibleItems(Rect totalRect ,int firstRow, int numVisibleRows, float rowWidth, float viewHeight, int rowHeight, 
            Vector2 scrollPos, IList list, CustomDrawerObject customDrawerObject)
        {
            
            var scrollListContextData = customDrawerObject.GetContextData<ScrollListContextData>();
            
            int i = 0;
            while (i<numVisibleRows)
            {
                int drawRow = firstRow + i;
                Rect rowRect = new Rect(totalRect.x, drawRow * rowHeight, rowWidth, rowHeight);
                float num3 = rowRect.y - scrollPos.y;

                if (num3 <= viewHeight)
                {
                    var listItem = list[drawRow];

                    var e = Event.current;
                    switch (e.type)
                    {
                        case EventType.MouseUp:
                            if (!rowRect.Contains(e.mousePosition))
                                break;
                            if (e.button==0)
                            {
                                scrollListContextData.OnScrollItemLeftClick?.Invoke(i);
                            }else if (e.button == 1)
                            {
                                scrollListContextData.OnScrollItemRightClick?.Invoke(i);
                            }
                            break;
                    }
                    if (e.type == EventType.MouseUp && e.button == 1 && rowRect.Contains(e.mousePosition))
                    {
                        e.Use();
                        scrollListContextData.OnScrollItemRightClick?.Invoke(i);
                    }
                    else
                    {
                        CustomEditorDrawHelper.DrawCustomEditor(rowRect, listItem, customDrawerObject);
                    }
                }
                i++;
            }
        }
    }
}

