using System.Collections;
using System.Collections.Generic;
using ToolKits;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

[CustomEditorDrawer]
public class ExampleItemCustomEditor: ACustomEditorDrawer<ExampleItem>
{
    public override void OnGui(Rect rect, ExampleItem obj)
    {
        rect.width = 50;
        obj.A = EditorGUI.TextField(rect, obj.A);
        rect.x += rect.width;
        obj.B = EditorGUI.IntField(rect, obj.B);
        rect.x += rect.width;
        obj.C = EditorGUI.Slider(rect, obj.C, 0f, 10f);
    }
}
