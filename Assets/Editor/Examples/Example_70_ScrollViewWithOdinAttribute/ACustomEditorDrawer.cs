using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKits
{
    public abstract class ACustomEditorDrawer<T>: ICustomEditorDrawer
    {
        public abstract void OnGui(Rect rect, T obj);
        
    }

    public interface ICustomEditorDrawer
    {
        
    }
}

