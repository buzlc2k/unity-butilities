using UnityEngine;

namespace ServiceButcator {
    public abstract class SceneInstaller : RuntimeInstaller
    {
        public override void InitializeServiceLocator()
        {
            serviceLocator = new();
            ServiceLocators.BindScene(gameObject.scene, serviceLocator);
        }
    }
}
