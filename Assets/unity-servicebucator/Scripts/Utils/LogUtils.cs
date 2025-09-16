#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace ServiceButcator {
    public static class LogUtils
    {
        public static bool LogError(string log)
        {
            Debug.LogError(log);
            return false;
        }

        public static bool LogSuccess(string log)
        {
            Debug.Log(log);
            return true;
        }
    }
}
#endif