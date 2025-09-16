#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace ServiceButcator {
    public static class IMGUIUtils
    {
        // public static void 

        public static void LayoutLabel(string text, Color color, int fontSize, FontStyle fontStyle)
        {
            GUIStyle labelStyle = new(GUI.skin.label)
            {
                fontStyle = fontStyle,
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
            };

            labelStyle.normal.textColor = color;
            labelStyle.wordWrap = true;

            GUILayoutOption[] labelOptions = new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
            };

            GUILayout.Label(text, labelStyle, labelOptions);
        }

        public static bool LayoutButton(string text, FontStyle fontStyle, float height, float width, Color normalColor, Color hoverColor)
        {
            GUIStyle buttonStyle = new(GUI.skin.button)
            {
                fontStyle = fontStyle,
                alignment = TextAnchor.MiddleCenter
            };
            buttonStyle.normal.textColor = normalColor;
            buttonStyle.hover.textColor = hoverColor;

            GUILayoutOption[] buttonOptions = new GUILayoutOption[]
            {
                GUILayout.Height(height),
                GUILayout.MinWidth(width),
            };

            return GUILayout.Button(text, buttonStyle, buttonOptions);
        }

        public static void FixedWindow(EditorWindow window, Vector2 fixedSize)
        {
            window.minSize = fixedSize;
            window.maxSize = new(fixedSize.x + 1, fixedSize.y + 1);
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
                pix[i] = col;

            Texture2D result = new(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
#endif