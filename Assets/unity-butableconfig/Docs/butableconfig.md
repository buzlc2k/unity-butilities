# UNITY-BUTBLECONFIG

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/Unity-100000?style=flat&logo=unity&logoColor=white" alt="Unity">
</p>

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
  - [1. Create a Record Class](#1-create-a-record-class)
  - [2. Create a ConfigTable](#2-create-a-configtable)
  - [3. Create an Asset in Unity](#3-create-an-asset-in-unity)
  - [4. Access Records in Code](#4-access-records-in-code)
- [Examples](#examples)

## Overview

**ButbleConfig** is a powerful Unity package designed to manage table-like data efficiently. It provides a clean, editor-friendly way to handle configuration data with fast lookup capabilities and compile-time safety.

## Features

- 📋 **Record-based System**: Define data entries as serializable classes
- 🔑 **Key Fields**: Mark fields as searchable keys using `[RecordKeyField]` attribute
- 📝 **ScriptableObject Integration**: Store and edit data directly in Unity Editor
- ⚡ **Auto-sorting**: Automatically sorts records by keys when added
- 🔍 **Fast Lookups**: Utilizes Binary Search for optimal performance
- 🛠️ **Code Intelligence**: IDE analyzer provides compile-time warnings and validation
- 🎯 **Type Safety**: Strong typing prevents runtime errors

## Getting Started

### Prerequisites

- **Language**: C#
- **Engine**: Unity 2019.4 or later
- **Package**: [NuGet For Unity](https://github.com/GlitchEnzo/NuGetForUnity) (optional)

### Installation

1. **Download Package**
   - Download `butbleconfig.unitypackage` from the **Package** folder

2. **Import to Unity**
   - Drag & drop into your Project window, or
   - Use `Assets > Import Package > Custom Package...`

3. **Setup Analyzer** (Optional but Recommended)
   
   Choose one of the following options from the **Analyzer** folder:
   
   - **DLL Method**: Drag the prebuilt analyzer DLL into your Unity project
   - **NuGet Method**: Use the `.nupkg` file with NuGet For Unity for version management

## Usage

### 1. Create a Record Class

Define your data structure as a serializable class. Use `[RecordKeyField]` to mark searchable fields, with `Priority` controlling comparison order (lower numbers = higher priority).

```cs
using ButbleConfig;

[System.Serializable]
public class ItemRecord
{
    [RecordKeyField(0)] 
    public int itemId;    
    public string itemName;
    public int price;
    public string description;
    
    // Optional: Custom constructor
    public ItemRecord(int itemId, string itemName, int price)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.price = price;
    }
}
```

### 2. Create a ConfigTable

Create a ScriptableObject class that inherits from `ConfigTable<T>`:

```cs
using ButbleConfig;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
public class ItemConfig : ConfigTable<ItemRecord> 
{
    // Optional: Add custom methods for specific queries
    public ItemRecord GetWeapon(int weaponId)
    {
        return Get(weaponId);
    }
}
```

### 3. Create an Asset in Unity

1. **Create the Asset**:
   - Right-click in Project Window
   - Navigate to `Create > Configs > ItemConfig`
   - Name your asset (e.g., "GameItemConfig")

2. **Add Data**:
   - Select the created asset
   - In the Inspector, expand the Records list
   - Add new entries and fill in the data
   - Records will automatically sort by key fields

### 4. Access Records in Code

```cs
using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private ItemConfig itemConfig;
    
    void Start()
    {
        // Get all records
        List<ItemRecord> allItems = itemConfig.GetAllRecord();
        Debug.Log($"Total items loaded: {allItems.Count}");
        
        // Get specific record by composite key (itemId, subId)
        ItemRecord sword = itemConfig.Get(1);
        if (sword != null)
        {
            Debug.Log($"Found item: {sword.itemName} - Price: {sword.price}");
        }
    }
}
```

---
