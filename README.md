# 3D-ARPG-Demo

## 1. 项目概述

本项目基于 Unity 实现了一个第三人称 3D ARPG 游戏原型，主要用于实践 Unity 客户端开发中的角色控制、动画系统、战斗系统、任务与背包、数据驱动、事件系统、网络同步以及资源热更新等功能。

项目以模块化和数据驱动为主要设计思路，将角色控制、战斗、背包、任务、UI 以及网络同步等功能拆分为相对独立的模块，并使用 ScriptableObject 管理游戏中的静态数据。

项目实现的核心功能包括：

* 第三人称角色移动、跳跃和翻滚；
* 鼠标控制角色朝向以及第三人称 Camera Follow；
* 使用 Animator 管理角色移动、攻击和其他动作状态；
* 基于 ScriptableObject 的物品和任务数据管理；
* 背包系统以及动态生成物品 UI；
* 任务系统以及任务进度显示；
* UI 红点提示和事件通知；
* 使用 Netcode for GameObjects 实现 Host / Client 网络连接；
* 多客户端 Player 动态 Spawn；
* 玩家位置、旋转以及 Animator 状态同步；
* Addressables 远程资源加载；
* 基于 Addressables Content Update 的资源热更新。

---

## 2. 项目运行说明

### 基本操作

* **W / A / S / D**：控制角色移动；
* **鼠标**：控制角色朝向；
* **鼠标左键**：进行普通攻击；
* **I**：打开 / 关闭背包；
* **U**：查看当前任务及任务进度；
* **G**：启动 Host；
* **H**：启动 Client。

网络功能需要分别启动 Host 和 Client 进行测试。

---

## 3. 项目架构

项目采用基于功能模块的组件化架构，并结合 ScriptableObject、事件系统以及状态机实现不同系统之间的解耦。

主要模块包括：

角色与战斗模块：负责玩家移动、动作控制、攻击；
背包模块：基于 ScriptableObject 管理物品数据，并负责运行时背包数据和物品 UI；
任务模块：负责任务数据、任务状态、任务进度以及任务 UI；
UI 模块：负责背包、任务、红点等界面，并通过事件机制响应游戏状态变化；
网络模块：基于 Netcode for GameObjects 实现 Host / Client 连接、Player Spawn 以及角色状态同步；
资源管理模块：使用 Addressables 管理远程资源加载，并实现资源热更新。

各模块之间通过数据对象、事件通知等方式进行通信，减少系统之间的直接依赖，便于后续功能扩展和维护。

## 4. 功能实现

### 4.1 第三人称角色控制

玩家控制系统负责角色的移动、跳跃、翻滚以及角色朝向控制。

角色移动基于 `CharacterController` 实现，并结合摄像机方向计算移动方向，使玩家可以进行第三人称移动。

主要功能包括：

* WASD 第三人称移动；
* 鼠标控制角色朝向；
* 第三人称 Camera Follow；
* 场景碰撞检测。

### 4.2 Animator 与动作状态

项目使用 Unity Animator 管理角色的动作状态。
角色移动时根据当前速度更新 Animator 参数，攻击和其他动作通过 Animator Trigger 或参数进行状态切换。攻击动画通过 Animation Event 通知代码攻击动作结束，使动画播放与实际游戏逻辑保持同步。

### 4.3 ScriptableObject 数据驱动

项目使用 ScriptableObject 管理物品和任务等静态数据，将游戏数据与具体的业务逻辑进行分离。

例如物品数据通过 `ItemData` 保存：

```text
ItemData
├── itemName
├── icon
├── description
└── maxStack
```

背包系统和任务系统通过引用对应的数据对象获取配置，从而避免将物品和任务信息直接硬编码在业务逻辑中，便于后续增加新的物品和任务。

### 4.4 背包系统

<img src="./Docs/Images/inventory.png" width="700">

项目实现了基础背包系统，通过 `InventoryData` 管理玩家当前拥有的物品及数量，并根据运行时数据动态生成 `ItemSlot`。

主要功能包括：

* 添加和删除物品；
* 物品数量管理；
* 动态生成物品 UI；
* 显示物品图标和数量；
* 背包 UI 开关。

整体数据流为：

```text
ItemData
    ↓
InventoryData
    ↓
InventoryItem
    ↓
InventoryPanel
    ↓
ItemSlot
```

### 4.5 任务系统

项目实现了基础任务系统，将任务配置与运行时任务状态进行分离。

`QuestData` 保存任务的静态配置，`QuestInstance` 表示运行时任务状态，`QuestSystem` 负责任务管理和进度更新，`QuestPanel` 负责任务 UI 显示。

主要功能包括：

* 任务数据配置；
* 任务实例管理；
* 任务进度更新；
* 任务列表动态生成；
* 任务 UI 显示。

### 4.6 事件系统与 UI 红点

项目通过事件机制实现 Gameplay 系统与 UI 系统之间的解耦。

例如任务状态发生变化时，`QuestSystem` 通过 `OnQuestUpdated` 通知相关 UI：

```text
QuestSystem
     │
     │ OnQuestUpdated
     ↓
QuestPanel
     │
     └── Refresh()
```

同时通过 `RedDot` 组件实现任务等 UI 的红点提示，使业务系统不需要直接依赖具体的 UI 对象。

### 4.7 网络同步

项目使用 **Netcode for GameObjects 1.7.1** 实现基础多人网络功能。

目前实现了：

* Host / Client 网络连接；
* 多客户端 Player 动态 Spawn；
* 每个客户端控制自己的 Player；
* Player 位置同步；
* Player 旋转同步；
* Animator 状态同步；
* 攻击等 Animator Trigger 同步。

网络模式下通过 `IsOwner` 判断当前 Player 是否属于本地客户端，避免一个客户端控制其他玩家角色。

### 4.8 Addressables 资源加载

项目使用 Unity Addressables 管理需要远程加载的游戏资源。

目前以背包中的物品图标作为测试资源，使用 `AssetReferenceSprite` 保存资源引用，并在运行时通过 `LoadAssetAsync` 异步加载。

基本流程为：

```text
ItemData
    ↓
AssetReferenceSprite
    ↓
Addressables
    ↓
LoadAssetAsync
    ↓
ItemSlot
```

同时对 `AsyncOperationHandle` 进行管理，在资源不再使用时调用 `Addressables.Release` 释放资源。

### 4.9 Addressables 资源热更新

项目使用 Addressables Content Update 实现远程资源热更新。

资源首次构建后，客户端从远程地址加载 Addressable 资源。当远程资源发生修改时，通过 `Content Update` 生成更新内容，客户端再次运行时根据 Catalog 和资源版本信息获取更新后的 Bundle。

整体流程为：

```text
修改远程资源
      ↓
Addressables Content Update
      ↓
生成更新内容
      ↓
客户端检测资源变化
      ↓
下载新的 Bundle
      ↓
加载更新后的资源
```

## 5. 项目总结

本项目以 3D ARPG 为基础，围绕角色控制、任务与背包、UI、网络同步以及资源管理等方向，搭建了一套较为完整的 Unity 客户端功能框架。

在系统设计方面，项目使用 ScriptableObject 对物品和任务等静态数据进行管理，并将运行时数据与 UI 展示进行分离，实现了背包、任务、进度显示等功能。同时结合事件机制实现 Gameplay 系统与 UI 系统之间的解耦，例如通过任务更新事件驱动任务界面和红点状态变化。

在多人功能方面，项目基于 Netcode for GameObjects 实现 Host / Client 网络连接、多客户端 Player 动态 Spawn，以及玩家位置、旋转和 Animator 状态同步，进一步实践了 Unity 客户端中的基础网络同步流程。

此外，项目使用 Addressables 对远程资源进行管理，并通过 Content Update 实现资源热更新，实际验证了在不重新构建整个客户端的情况下更新远程资源的流程。

通过本项目进一步实践了 Unity 客户端开发中的：

角色控制 → 动画系统 → 战斗逻辑 → 数据驱动 → UI 与事件系统 → 网络同步 → 资源管理与热更新

完整开发流程，并对组件化设计、系统解耦以及可扩展的客户端架构进行了实践。

