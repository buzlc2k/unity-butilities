#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceButcator
{
    [Serializable]
    public class EditorServiceLocator : IServiceLocator
    {
        [SerializeField]
        private SerializableDictionary<SerializableType, SerializableUnityObject> services = new();

        public T Get<T>() where T : class
        {
            var type = SerializableType.Of<T>();

            if (services.TryGetValue(type, out var service))
                return (UnityEngine.Object)service as T;

            return null;
        }

        public IEnumerable<(Type, object)> GetAlls()
        {
            foreach (var s in services)
                yield return (s.Key, s.Value);
        }

        public bool RegisterService<T>(T service) where T : class
        {
            var serializedType = SerializableType.Of<T>();

            if (service is UnityEngine.Object serviceUObj)
            {
                if (!services.TryAdd(serializedType, new(serviceUObj)))
                    return LogUtils.LogError($"ButviceLocator: Service of type {serializedType} already registered");

                return LogUtils.LogSuccess("ButviceLocator: Register successfully");
            }
            else
                return LogUtils.LogError($"ButviceLocator: Cannot register non-UnityEngine.Object service '{serializedType}' in Editor build. Only UnityEngine.Object types are supported.");
        }

        public bool RegisterService(Type type, object service)
        {
            var serializedType = SerializableType.Of(type);

            if (service is UnityEngine.Object serviceUObj)
            {
                if (!services.TryAdd(serializedType, new(serviceUObj)))
                    return LogUtils.LogError($"ButviceLocator: Service of type {serializedType} already registered");

                return LogUtils.LogSuccess("ButviceLocator: Register successfully");
            }
            else
                return LogUtils.LogError($"ButviceLocator: Cannot register non-UnityEngine.Object service '{serializedType}' in Editor build. Only UnityEngine.Object types are supported.");
        }

        public void UnregisterService<T>() where T : class
        {
            var type = SerializableType.Of<T>();
            services.Remove(type);
        }

        public void UnregisterService(Type type)
        {
            var serializedType = SerializableType.Of(type);
            services.Remove(serializedType);
        }
    }
}
#endif