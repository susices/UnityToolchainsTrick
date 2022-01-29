using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolKits
{
    public abstract class ACustomEditorDrawer
    {
        public abstract void OnGui(Rect rect,object obj, CustomDrawerObject customDrawerObject);
    }
}

