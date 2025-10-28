using UnityEngine;

namespace ServiceButcator {
    [DisallowMultipleComponent, DefaultExecutionOrder(-50)]
    public abstract class RuntimeInstaller : MonoBehaviour
    {
        protected RuntimeServiceLocator serviceLocator = default;

        protected virtual void Awake()
        {
            InitializeServiceLocator();
            Binds();
        }

        public abstract void InitializeServiceLocator();

        public abstract void Binds();
    }
}
