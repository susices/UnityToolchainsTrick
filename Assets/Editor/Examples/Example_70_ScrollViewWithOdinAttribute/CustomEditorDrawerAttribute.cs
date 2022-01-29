using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKits
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomEditorDrawerAttribute : Attribute
    {
        public Type DrawType;
        public CustomEditorDrawerAttribute(Type type)
        {
            DrawType = type;
        }
    }
}

