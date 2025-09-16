using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceButcator
{
    public static class ServiceLocators
    {
        private static IServiceLocator m_global;
#if UNITY_EDITOR
        private static IServiceLocator m_editor;
#endif
        private static Dictionary<Scene, IServiceLocator> m_scenes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            m_global = null;
            m_scenes = new Dictionary<Scene, IServiceLocator>();
        }

        public static IServiceLocator Global { get => m_global;}
#if UNITY_EDITOR
        public static IServiceLocator Editor { get => m_editor;}
#endif
        public static IServiceLocator SceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;

            if (!m_scenes.TryGetValue(scene, out IServiceLocator container))
                Debug.LogWarning("ButviceLocator: Doesn't exist Scene container, check binding Scene ServiceLocator again");

            return container;
        }

        internal static void BindGlobal(IServiceLocator global)
        {
            if (m_global == global)
                Debug.LogWarning("ButviceLocator: ServiceLocator.ConfigureAsGlobal: Already configured as global");

            else if (m_global != null)
                Debug.LogError("ButviceLocator: ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global");

            else
                m_global = global;
        }
#if UNITY_EDITOR  
        internal static void BindEditor(IServiceLocator editor)
        {
            if (m_editor == editor)
                Debug.LogWarning("ServiceLocator.ConfigureAsEditor: Already configured as editor");
                
            else if (m_editor != null)
                Debug.LogError("ServiceLocator.ConfigureAsEditor: Another ServiceLocator is already configured");
                
            else
                m_editor = editor;
        }
#endif
        internal static void BindScene(Scene scene, IServiceLocator container)
        {
            if (m_scenes.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured for this scene");
                return;
            }

            m_scenes.Add(scene, container);
        }
    }
}