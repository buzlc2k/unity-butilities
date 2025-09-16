#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

namespace ServiceButcator
{
    public class ServiceRegistingWindow : EditorWindow
    {
        private EditorInstaller editorInstaller = default;

        public static void ShowWindow(Rect position, EditorInstaller editorInstaller)
        {
            var window = GetWindow<ServiceRegistingWindow>("Service Register");
            window.position = new(position.x - 150, position.y, 0, 0);
            window.editorInstaller = editorInstaller;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            IMGUIUtils.LayoutLabel("  Type", Color.white, 12, FontStyle.Bold);

            GUILayout.Space(5);
            TypeField();

            GUILayout.Space(10);
            IMGUIUtils.LayoutLabel("  Object", Color.white, 12, FontStyle.Bold);
            DrawObjectField();

            GUILayout.FlexibleSpace();

            if (IMGUIUtils.LayoutButton("APPLY", FontStyle.Bold, 20, 80, Color.white, Color.yellow))
                Apply();

            GUILayout.EndVertical();

            ResizeWindown();
        }

        #region TypeField
        private string typeSearchQuery = "";
        private string selectedType = "";
        private string[] resultTypes = new string[0];
        private void TypeField()
        {
            var newTypeSearchQuery = EditorGUILayout.TextField(
                typeSearchQuery,
                new GUIStyle(GUI.skin.textField) { fontSize = 13, fixedHeight = 20 }
            );

            GetSearchResults(newTypeSearchQuery);
            TypeDropdown();
        }

        private Vector2 scrollPosition = default;
        private void TypeDropdown()
        {
            if (string.IsNullOrEmpty(typeSearchQuery) || !string.IsNullOrEmpty(selectedType))
                return;

            GUILayout.Space(5);

            var dropdownRect = GUILayoutUtility.GetRect(0, Mathf.Min(resultTypes.Length * 20, 200), GUILayout.ExpandWidth(true));
            var viewRect = new Rect(0, 0, dropdownRect.width - 25, resultTypes.Length * 20);

            GUI.Box(dropdownRect, "", EditorStyles.helpBox);
            scrollPosition = GUI.BeginScrollView(dropdownRect, scrollPosition, viewRect);

            TypeItems(viewRect);

            GUI.EndScrollView();

            HandleOutOfDropDown(dropdownRect);
        }

        private void TypeItems(Rect viewRect)
        {
            for (int itemIndex = 0; itemIndex < resultTypes.Length; itemIndex++)
            {
                var itemRect = new Rect(5, itemIndex * 20, viewRect.width, 20);

                GUI.Label(itemRect, resultTypes[itemIndex]);
                HandleTypeItemInteraction(itemRect, itemIndex);
            }
        }

        private void ResizeWindown()
        {
            int addedTypeItemsHeight = Mathf.Min(resultTypes.Length * 20, 200);

            IMGUIUtils.FixedWindow(this, new(300, 150 + addedTypeItemsHeight));
        }

        private void GetSearchResults(string newTypeSearchQuery)
        {
            if (newTypeSearchQuery == typeSearchQuery)
                return;

            typeSearchQuery = newTypeSearchQuery;
            selectedType = "";
            resultTypes = !string.IsNullOrEmpty(newTypeSearchQuery)
                            ? AppDomain.CurrentDomain
                                .GetAssemblies()
                                .SelectMany(assembly => assembly.GetTypes())
                                .Where(t => t.FullName.ToLower().Contains(typeSearchQuery.ToLower()))
                                .Take(50)
                                .Select(t => $"{t.FullName}")
                                .ToArray()
                            : new string[0];
        }

        private void HandleTypeItemInteraction(Rect itemRect, int itemIndex)
        {
            if (!itemRect.Contains(Event.current.mousePosition)) return;

            switch (Event.current.type)
            {
                case EventType.MouseDown when Event.current.button == 0:
                    SelectType(resultTypes[itemIndex]);
                    break;
                default:
                    EditorGUI.DrawRect(itemRect, new Color(0.3f, 0.5f, 1f, 0.3f));
                    Repaint();
                    break;
            }
        }

        // TODO: Refactor this func
        private void HandleOutOfDropDown(Rect dropdownRect)
        {
            if (Event.current.type == EventType.MouseDown
                && Event.current.button == 0
                && !dropdownRect.Contains(Event.current.mousePosition))
                SelectType("");
        }

        private void SelectType(string typeName)
        {
            typeSearchQuery = typeName.Split('.').Reverse().ToList()[0];
            selectedType = typeName;
            resultTypes = new string[0];

            GUI.FocusControl(null);
            Repaint();
        }
        #endregion

        #region ObjectField
        private UnityEngine.Object selectedObj = default;
        private void DrawObjectField()
        {
            bool disabled = string.IsNullOrEmpty(selectedType);

            EditorGUI.BeginDisabledGroup(disabled);
            {
                selectedObj = EditorGUILayout.ObjectField(
                    selectedObj,
                    disabled ? typeof(UnityEngine.Object) : Type.GetType(selectedType),
                    false,
                    GUILayout.Height(22),
                    GUILayout.ExpandWidth(true)
                );
            }
            EditorGUI.EndDisabledGroup();
        }
        #endregion

        private void Apply()
        {
            if (editorInstaller.Bind(selectedType, selectedObj))
                Close();
        }
        
    }
}
#endif