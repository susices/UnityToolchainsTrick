using System.Collections;
using System.Collections.Generic;
using ToolKits;
using UnityEditor;
using UnityEngine;

namespace ToolKits
{
    [CustomEditorDrawer(typeof(ExampleItemB))]
    public class ExampleItemBCustomEditor : ACustomEditorDrawer
    {
        public override void OnGui(Rect rect, object objValue, CustomDrawerObject customDrawerObject)
        {
            var obj = objValue as ExampleItemB;
            if (obj==null)
            {
                Debug.LogError("ExampleItemBCustomEditor can not get value");
                return;
            }
            rect.width = 50;
            EditorGUI.LabelField(rect, nameof(obj.D));
            rect.x += rect.width;
            obj.D = EditorGUI.TextField(rect, obj.D);
        }
    }
}

