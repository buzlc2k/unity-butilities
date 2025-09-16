using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceButcator
{
    public class RuntimeServiceLocator : IServiceLocator
    {
        private Dictionary<Type, object> services = new();

        public T Get<T>() where T : class
        {
            var type = typeof(T);

            if (services.TryGetValue(type, out var service))
                return service as T;

            return null;
        }

        public IEnumerable<(Type, object)> GetAlls()
        {
            foreach (var s in services)
                yield return (s.Key, s.Value);
        }

        public bool RegisterService<T>(T service) where T : class
        {
            Type type = typeof(T);

            if (!services.TryAdd(type, service))
                return LogUtils.LogError($"ButviceLocator: Service of type {type.FullName} already registered");
            
            return true;
        }

        public bool RegisterService(Type type, object service)
        {
            if (!services.TryAdd(type, service))
                return LogUtils.LogError($"ButviceLocator: Service of type {type.FullName} already registered");
            
            return true;
        }

        public void UnregisterService<T>() where T : class
        {
            var type = typeof(T);
            services.Remove(type);
        }

        public void UnregisterService(Type type)
        {
            services.Remove(type);
        }
    }
}