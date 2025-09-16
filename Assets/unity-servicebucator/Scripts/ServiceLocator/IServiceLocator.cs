using System;
using System.Collections.Generic;
using UnityEngine;

namespace ServiceButcator
{
    public interface IServiceLocator
    {
        public T Get<T>() where T : class;
        public IEnumerable<(Type, object)> GetAlls();
        public bool RegisterService<T>(T service) where T : class;
        public bool RegisterService(Type type, object service);
        public void UnregisterService<T>() where T : class;
        public void UnregisterService(Type type);
    }
}