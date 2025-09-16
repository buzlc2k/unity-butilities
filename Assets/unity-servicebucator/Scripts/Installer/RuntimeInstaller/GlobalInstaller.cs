using UnityEngine;

namespace ServiceButcator {
    public class GlobalInstaller : RuntimeInstaller
    {
        public override void InitializeServiceLocator()
        {
            serviceLocator = new();
            ServiceLocators.BindGlobal(serviceLocator);
        }

        public override void Binds()
        {
            // etc ...
        }
    }
}
