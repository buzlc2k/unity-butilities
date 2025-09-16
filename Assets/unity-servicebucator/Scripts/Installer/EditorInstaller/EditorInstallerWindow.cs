#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ServiceButcator {
    public class EditorInstallerWindow : EditorWindow
    {
        private EditorInstaller editorInstaller = default;

        [MenuItem("Window/ServiceButcator/EditorInstallerWindow")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(EditorInstallerWindow));
        }

        private void OnEnable()
        {
            editorInstaller ??= new();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            IMGUIUtils.LayoutLabel("Registered Services", Color.white, 20, FontStyle.Bold);

            GUILayout.Space(10);
            RegisteredServices();

            GUILayout.Space(5);
            Buttons();

            GUILayout.EndVertical();
        }

        private Vector2 scrollPosition = default;
        private Rect scrollRect = default;
        private Rect viewRect = default;
        private int selectedIndex = -1;
        private void RegisteredServices()
        {
            var registeredServices = ServiceLocators.Editor.GetAlls().ToList();

            scrollRect = GUILayoutUtility.GetRect(0, Mathf.Clamp(registeredServices.Count * 105 + 5, 30, 320), GUILayout.ExpandWidth(true));
            viewRect = new Rect(0, 0, scrollRect.width - 20, registeredServices.Count * 105 + 5);

            GUI.Box(scrollRect, "", EditorStyles.helpBox);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, viewRect, false, true);

            if (registeredServices.Count <= 0)
                NoServiceLabel();
            else
                for (int i = 0; i < registeredServices.Count; i++)
                    RegisteredServiceItem(i, registeredServices[i].Item1, registeredServices[i].Item2 as SerializableUnityObject);

            GUI.EndScrollView();

            HandleOutOfScroll();
        }

        private void NoServiceLabel()
        {
            var labelRect = new Rect(5, 5, viewRect.width, 20);
            GUI.Label(labelRect, "Container is empty !", new GUIStyle(EditorStyles.label)
            {
                fontSize = 11,
                fontStyle = FontStyle.Italic,
                normal = { textColor = Color.white }
            });
        }

        private void RegisteredServiceItem(int index, Type type, UnityEngine.Object service)
        {
            var itemRect = new Rect(5, index * 105 + 5, viewRect.width, 100);

            if (selectedIndex == index)
                EditorGUI.DrawRect(itemRect, new Color(0.3f, 0.5f, 1f, 0.3f));

            GUI.Box(itemRect, GUIContent.none, EditorStyles.helpBox);

            GUI.BeginGroup(itemRect);

            var headerRect = new Rect(5, 5, itemRect.width - 10, 20);
            GUI.Label(headerRect, $"     Element {index}", new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 13,
                normal = { textColor = Color.white }
            });

            var typeLabelRect = new Rect(5, 30, 50, 20);
            GUI.Label(typeLabelRect, "Type:", EditorStyles.label);

            var typeFieldRect = new Rect(60, 30, itemRect.width - 70, 20);
            GUI.Label(typeFieldRect, type != null ? type.Name : "None", EditorStyles.textField);

            var objLabelRect = new Rect(5, 60, 50, 20);
            GUI.Label(objLabelRect, "Object:", EditorStyles.label);

            var objFieldRect = new Rect(60, 60, itemRect.width - 70, 20);
            EditorGUI.ObjectField(objFieldRect, service, typeof(UnityEngine.Object), false);

            GUI.EndGroup();

            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && itemRect.Contains(Event.current.mousePosition))
                SetSelectedSevice(index);
        }

        private void Buttons()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (IMGUIUtils.LayoutButton("+", FontStyle.Bold, 20, 50, Color.white, Color.cyan))
                OpenRegistingWindown();

            if (IMGUIUtils.LayoutButton("-", FontStyle.Bold, 20, 50, Color.white, Color.red))
                DeleteRegisteredService();

            GUILayout.EndHorizontal();
        }

        private void OpenRegistingWindown()
        {
            ServiceRegistingWindow.ShowWindow(new Rect(position.x + position.width / 2, position.y + position.height / 16, 0, 0), editorInstaller);
        }

        private void DeleteRegisteredService()
        {
            var registeredServices = ServiceLocators.Editor.GetAlls().ToList();

            if (registeredServices.Count <= 0)
            {
                Debug.LogError($"ButviceLocator: It doessn't have any service to delete");
                return;
            }

            if (selectedIndex == -1)
                selectedIndex = registeredServices.Count - 1;

            editorInstaller.Delete(registeredServices[selectedIndex].Item1);
        }

        private void HandleOutOfScroll()
        {
            if (Event.current.type == EventType.MouseDown
                && Event.current.button == 0
                && !scrollRect.Contains(Event.current.mousePosition))

                SetSelectedSevice();
        }

        private void SetSelectedSevice(int selectedServiceIndex = -1)
        {
            selectedIndex = selectedServiceIndex;
            Repaint();
        }
    }
}
#endif
