using UnityEngine;

namespace ServiceButcator {
    [DisallowMultipleComponent]
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
