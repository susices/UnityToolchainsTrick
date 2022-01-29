using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities.Editor;
using ToolKits;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[CustomEditorDrawer(typeof(ExampleItemA))]
public class ExampleItemACustomEditor: ACustomEditorDrawer
{
    public override void OnGui(Rect rect,object objValue, CustomDrawerObject customDrawerObject)
    {
        var obj = objValue as ExampleItemA;
        if (obj==null)
        {
            Debug.LogError("ExampleItemACustomEditor can not get value");
            return;
        }

        var ScrollListContextData = customDrawerObject.GetContextData<ScrollListContextData>();
        var drawRect = rect;
        drawRect.width = 50;
        EditorGUI.LabelField(rect, nameof(obj.A));
        drawRect.x += drawRect.width;
        
        obj.A = EditorGUI.TextField(drawRect, obj.A);
        drawRect.x += drawRect.width;
        EditorGUI.LabelField(drawRect, nameof(obj.B));
        drawRect.x += drawRect.width;
        obj.B = EditorGUI.IntField(drawRect, obj.B);
        drawRect.x += drawRect.width;
        EditorGUI.LabelField(drawRect, nameof(obj.C));
        drawRect.x += drawRect.width;
        drawRect.width = 150;
        obj.C = EditorGUI.Slider(drawRect, obj.C, 0f, 10f);
        drawRect.x += rect.width;
        drawRect.width = 50;
        // if (ScrollListContextData.HasClick && rect.Contains(ScrollListContextData.ClickedMousePos))
        // {
        //     SirenixEditorGUI.DrawBorders(new Rect(rect), 1, Color.blue);
        //     return;
        // }
        
        // if (ScrollListContextData.HasClick && rect.Contains(ScrollListContextData.ClickedMousePos))
        // {
        //     ScrollListContextData.HasClick = false;
        //     var genericMenu = new GenericMenu();
        //     genericMenu.AddItem(new GUIContent("功能1"), false, () =>
        //     {
        //         (customDrawerObject.Value as List<ExampleBaseItem>).Remove(objValue as ExampleBaseItem);
        //     });
        //     genericMenu.AddItem(new GUIContent("功能合集/功能2"), false, () => { Debug.Log("功能2"); });
        //     genericMenu.AddItem(new GUIContent("功能合集/功能3"), false, () => { Debug.Log("功能3"); });
        //     genericMenu.AddSeparator("功能合集/");
        //     genericMenu.AddItem(new GUIContent("功能合集/功能4"), false, () => { Debug.Log("功能4"); });
        //     genericMenu.ShowAsContext();
        // }
    }
}
