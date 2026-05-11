# 依赖注入：MSDI（Godot.DependencyInjection）与 Chickensoft AutoInject

本文说明 Dash 工程中同时使用的两套注入机制、各自职责、`OnResolved` 生命周期，以及继承场景下的行为约定。

---

## 1. 为什么两套都在用？

| 维度 | MSDI（`Microsoft.Extensions.DependencyInjection` + `Godot.DependencyInjection`） | Chickensoft AutoInject |
|------|----------------------------------------------------------------------------------|-------------------------|
| 数据来源 | 全局 `IServiceCollection` 注册后，由 `DependencyInjectionManagerNode` 等从容器解析 | 沿**场景树父链**查找实现了 `IProvide<T>` 的祖先；依赖值在对方调用 `this.Provide()` 后才算就绪 |
| 典型用途 | 日志、`MessagePipe` 发布器、跨场景单例、工具服务等 | **按子树划分的上下文**（如 `PlayerContext`）、可被子树覆盖的提供方、`[Node]` 自动绑定子节点 |
| 代码形态 | `[Inject]`、`ConfigureServices` | `[Meta(typeof(IAutoNode))]`、`[Dependency]`、`this.DependOn<T>()`、`IProvide<T>`、`this.Provide()` |

一句话：**MSDI 管「全局服务」；AutoInject 管「场景树里谁向谁提供什么」**，二者互补，不是二选一。

工程内参考：

- 全局注册：`Dash/Scripts/GameHandler/DIRegistration.cs`（实现 `IServicesConfigurator`，在 `ConfigureServices` 里注册服务）。
- 场景根侧 DI 节点：`Dash/Scripts/GameHandler/DependencyInjectionNode.cs`（继承 `DependencyInjectionManagerNode`）。
- 同时使用两种注入的示例：`Dash/Scripts/Entity/Core/CharacterBase.cs`（`[Inject]` + `[Node]` + `this.Provide()` + `OnResolved`）。

相关包版本以 `Dash.csproj` 为准（如 `Chickensoft.AutoInject`、`Godot.DependencyInjection`）。

---

## 2. AutoInject 在做什么（概念）

- **Provider（提供方）**：节点实现 `IProvide<T>`，在自身依赖准备好后调用 `this.Provide()`，通知 AutoInject「我可以向子孙提供 `T` 了」。
- **Dependent（依赖方）**：用 `[Dependency]` 标记属性，通过 `this.DependOn<T>()` 取值；框架会向上找最近的匹配 `IProvide<T>`。
- **Godot 顺序问题**：`_Ready` 从子到父执行，子节点往往在父还没初始化完时就 Ready。AutoInject 通过订阅 Provider 的初始化，在对方 `Provide()` 之后再完成解析，避免子节点拿不到父节点才创建的数据。

官方说明与算法细节见：[Chickensoft AutoInject](https://github.com/chickensoft-games/AutoInject)（README）。

---

## 3. 什么时候注入？是 `_Ready()` 之前吗？

先把「注入」拆成三种，**时机各不相同**：

### 3.1 `[Node]`（子节点引用绑定，IAutoConnect）

- **触发通知**：`Node.NotificationEnterTree`（进树时）。
- **相对 `_Ready`**：一般在**本节点自己的 `_Ready()` 之前**就已经把 `[Node]` 属性指到场景里对应子节点上了（因为先进树、后整棵子树再进入 Ready 流程）。
- **含义**：这是「在场景里按路径/唯一名**接线**」，不依赖祖先上的 `IProvide<T>`。

### 3.2 `[Dependency]` / `DependOn<T>`（沿父链找 Provider）

- **什么时候开始解析**：依赖方 mixin 在收到 **`NotificationReady`** 时进入 `DependencyResolver` 的流程（与你的 `this.Notify(what)` 转发有关）。
- **相对 `_Ready`**：Godot 约定是**子节点先 `_Ready`，父节点后 `_Ready`**。因此常见情况是：**依赖方自己的 `_Ready()` 已经跑完了**，上面的 Provider 还没 `_Ready()`、也还没调用 `this.Provide()`。
- **什么时候算「注完」**：要等所有需要的祖先 **已经 `Provide()`**。在这之前若去用 `DependOn` 取到的值，可能抛「Provider 未初始化」类异常；所以**不是**「整个基于树的上下文注入」都在 `_Ready` 之前完成。
- **你该在哪用这些值**：放在 **`OnResolved()`**（以及非测试时的 `Setup()`）里；官方描述是：在 `_Ready` / `OnReady` 之后、且（在约定下）第一帧 `_Process` 之前。

### 3.3 `[Inject]`（MSDI / Godot.DependencyInjection）

- 由全局容器在 Godot DI 管线里解析，**与 AutoInject 的 `[Dependency]` 时间轴独立**；不要和「父节点 `Provide`」混成同一拍。

### 3.4 一句话对照

| 机制 | 大致与 `_Ready` 的关系 |
|------|-------------------------|
| `[Node]` | 多在 **`_Ready` 之前**（`EnterTree` 阶段接线） |
| `[Dependency]` 可用 | 多在 **自己 `_Ready` 之后**（要等祖先 `Provide`） |
| `OnResolved` | **一定在自己 `_Ready` 之后**（解析完成后再调） |
| `[Inject]` | 按 Godot.DependencyInjection 实现，**另走一条时间线** |

---

## 4. 生命周期（执行顺序简图）

在约定「Provider 在 `_Ready`（或尽量早）里调用 `this.Provide()`」的前提下，README 中的典型顺序为：

1. 依赖方节点 `_Ready`（在树深处可能先于 Provider）。
2. Provider 节点 `_Ready`（其中调用 `this.Provide()`）。
3. 依赖方的 **`Setup()`**（非测试环境）与 **`OnResolved()`**。
4. 第一帧及之后的 `_Process` 等。

`OnResolved` 的设计意图：**所有带 `[Dependency]` 的依赖都已解析且节点进入 Ready 流程之后**，再写依赖这些值的逻辑（例如订阅 `VisibleOnScreenNotifier2D` 并发布消息），避免空引用或半初始化状态。

若 Provider 同时也是 Dependent，可在自己的 `OnResolved` 里再调用 `this.Provide()`，向子树暴露下一层依赖（仍应尽量保持同步、避免复杂异步链，以免死锁或延迟解析）。

节点退出树时会清理 pending；再次进入树可重新走解析（详见官方文档中关于 `NotificationExitTree` / `RequestReady` 的说明）。

---

## 5. 实现原理（实现层粗述）

- 使用 **Chickensoft Introspection** 生成类型的元数据，**避免运行时反射**扫描属性。
- `DependencyResolver` 在 `NotificationReady` 等时机触发，沿父节点查找 `IProvide<T>`，未 `Provide` 的 Provider 会进入等待，全部就绪后触发 `Setup` / `OnResolved`。
- 必须保留：`public override void _Notification(int what) => this.Notify(what);`，否则 Mixin 收不到 Godot 通知。

`[Node]` 属于 **IAutoConnect**：按路径或唯一名绑定子节点，与 MSDI 无关，但与 `IAutoNode` 同属 AutoInject 生态。

---

## 6. `OnResolved` 与继承

要点如下：

- `OnResolved` **不是** Godot 的 `virtual _Ready` 那种引擎级父子链式回调；框架通过 **`IDependent` 约定**在解析完成时调用**一次** `OnResolved()`。
- 在 C# 中，子类若重新声明 `public void OnResolved()`，对父类同名方法是**隐藏**关系，**不会自动先执行父类再执行子类**。
- **子类未声明 `OnResolved`**：使用父类实现，行为符合直觉。
- **子类声明了 `OnResolved`**：通常只有子类体被调用；父类 `OnResolved` 中的逻辑**不会自动运行**，除非自行抽取为 `protected` 辅助方法并在子类中显式调用该辅助方法（不能直接 `base.OnResolved()`，除非将来改为可复用的 `virtual`/模板方法等明确设计）。

编写建议：公共初始化放进 `protected void OnDependenciesResolvedCore()` 之类，父、子 `OnResolved` 各调一次；或仅在叶子类型实现 `OnResolved`。

---

## 7. 与本项目相关的注意点

1. **`this.Provide()`**：凡作为 Provider 的节点，在提供给子孙的值就绪后必须调用，否则依赖方的 `OnResolved` 可能迟迟不触发或处于未初始化状态。
2. **`[Inject]` 与 `[Dependency]`**：前者由 `Godot.DependencyInjection` 从 MSDI 容器注入；后者由 AutoInject 从场景树解析；同一类可同时使用，职责不同。
3. **分析器**：工程引用 `Chickensoft.AutoInject.Analyzers`，并建议将 `CS9057` 视为错误（与 Introspection 生成器版本匹配相关），见 `Dash.csproj` 配置。

---

## 8. 延伸阅读

- [Chickensoft AutoInject README](https://github.com/chickensoft-games/AutoInject/blob/main/README.md)（Provider / Dependent / `OnResolved` / 算法说明）
- Godot 场景树与 `_Ready` 顺序：[Understanding Tree Order](https://docs.godotengine.org/en/stable/tutorials/scripting/scene_tree.html)（官方文档）

---

*文档随工程实践可继续补充具体场景示例（如某一关卡的根节点如何拆分 Provider）。*
