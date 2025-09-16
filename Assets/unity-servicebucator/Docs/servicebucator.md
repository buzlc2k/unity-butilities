# UNITY-SERVICEBUTCATOR

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/Unity-100000?style=flat&logo=unity&logoColor=white" alt="Unity">
</p>

---

## Table of Contents

- [Overview](#overview)
- [Getting Started](#getting-started)
- [Usage](#usage)

---

# Unity-ServiceButcator

Unity-ServiceButcator is a **Service Locator package for Unity** designed to simplify service registration and access.  
It works seamlessly in both **Edit Mode** and **Play Mode**, making it ideal for modular, scalable, and maintainable projects.

---

## Why Unity-ServiceButcator?

This package enhances your Unity workflow by providing:

- 🚀 **Multi-Environment Support** – Works in **runtime**, **scene**, and **editor** modes, enabling flexible modular architectures.  
- 🎛️ **Easy Setup** – Intuitive editor windows and clear APIs for quick configuration.  
- 🧩 **Maintainable & Scalable** – Centralized service registration makes large projects easier to manage and extend.

---

## Getting Started

### Prerequisites

- **Language**: C#  
- **Engine**: Unity

---

### Installation

You can install Unity-ServiceButcator in one of two ways:

1. **Direct Copy**  
   - Download the repository and copy the `Unity-ServiceButcator` folder into your Unity project’s `Assets/` directory.  

2. **Unity Package Import**  
   - Download the `unity-servicebucator.unitypackage` release from the repository and import it into Unity using  
     `Assets → Import Package → Custom Package...`

---

## Usage

Unity-ServiceButcator supports **three installation patterns**, depending on your needs:

### 1. Editor-Only Services

- Open **Window → ServiceButcator → EditorInstallerWindow** to access the editor UI.  
- Register and manage your services directly through the editor.  
- **Note:** In Edit Mode, only `UnityEngine.Object` assets that exist on disk (not scene objects) can be registered.

---

### 2. Scene-Based Services

Create a class that inherits from `SceneInstaller`, and override the `Binds()` method to register your services at scene startup:

```csharp
using ServiceButcator;

public class ExampleSceneInstaller : SceneInstaller
{
    [SerializeField] private AService sevice;

    protected override void Binds()
    {
        serviceLocator.RegisterService<IMyService>(sevice);
    }
}
```

---

### 3. Global Services
Similar to `Scene-Based Services`, but with support for `DontDestroyOnLoad`, allowing services to persist across multiple scenes. Perfect for global systems like AudioManager, SaveDataService, or Analytics, ...


