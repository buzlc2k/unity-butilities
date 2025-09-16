#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace ServiceButcator
{
    public class EditorInstaller
    {
        protected EditorServiceLocator serviceLocator = default;

        public EditorInstaller()
        {
            if (ServiceLocators.Editor != null)
            {
                serviceLocator = ServiceLocators.Editor as EditorServiceLocator;
                return;
            }

            if (EditorPrefs.HasKey("EditorContainer"))
                serviceLocator = JsonUtility.FromJson<EditorServiceLocator>(EditorPrefs.GetString("EditorContainer"));

            serviceLocator ??= new();
            ServiceLocators.BindEditor(serviceLocator);
        }

        private bool TryValidateService(string typeStr, object obj, out Type type)
        {
            type = null;

            if (string.IsNullOrEmpty(typeStr))
            {
                Debug.LogError("ButviceLocator: Type string cannot be null or empty!");
                return false;
            }

            type = Type.GetType(typeStr);

            if (type == null)
            {
                Debug.LogError($"ButviceLocator: Invalid type string: {typeStr}");
                return false;
            }

            if (obj == null)
            {
                Debug.LogError("ButviceLocator: Object cannot be null!");
                return false;
            }

            if (!type.IsAssignableFrom(obj.GetType()))
            {
                Debug.LogError($"ButviceLocator: Object type {obj.GetType()} is not compatible with {type}");
                return false;
            }

            return true;
        }

        public bool Bind(string typeStr, UnityEngine.Object uObj)
        {
            if (!TryValidateService(typeStr, uObj, out var type))
                return false;

            if(!serviceLocator.RegisterService(type, uObj))
                return false;

            Save();
            return true;
        }

        public void Delete(Type type)
        {
            serviceLocator.UnregisterService(type);
            Save();
        }

        private void Save()
        {
            var jsonString = JsonUtility.ToJson(serviceLocator);
            EditorPrefs.SetString("EditorContainer", jsonString);
        }
    }
}
#endif