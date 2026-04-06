# BaiHua.Software 文档中心

欢迎来到 **BaiHua.Software 上位机通用框架**的文档中心。这里包含了完整的架构设计、快速开始指南和开发参考。

---

## 📚 文档导航

### 🚀 升级信息

#### [📄 UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md) - 升级完成总结 ⭐ 必读

**内容**：
- 升级完成情况总结
- 新增 BaiHua.Modules 项目详解
- UI 项目更新说明
- 新旧架构对比
- 后续工作清单

**适用对象**：所有团队成员

**阅读时间**：10-15分钟

---

### 🏗️ 架构设计

#### [📄 Architecture.md](./Architecture.md) - 系统架构设计文档 ⭐ 必读

**内容**：
- 项目概述和技术栈（已更新为 Prism）
- 模块化设计详解（新增）
- 分层架构详解（7层设计）
- 各层职责和模块详解
- 依赖关系和通信机制
- 数据流示例
- 扩展指南和最佳实践

**适用对象**：架构师、技术负责人、新加入团队成员

**阅读时间**：30-45分钟

---

### 🚀 快速上手

#### [📄 QuickStart.md](./QuickStart.md) - 快速参考指南

**内容**：
- 项目文件树
- 编译运行步骤
- 各层职责清单
- 常见开发任务
- 数据流参考
- 编码规范
- 调试技巧

**适用对象**：新手开发者、想快速上手的工程师

**阅读时间**：15-20分钟

---

### 🤝 模块协作

#### [📄 Module-Integration.md](./Module-Integration.md) - 模块间协作指南

**内容**：
- 模块通信规范
- 4种常见交互模式
- 接口设计指南
- 依赖注入配置
- 调试步骤
- 模块交互图

**适用对象**：模块开发者、需要模块间集成的工程师

**阅读时间**：20-25分钟

---

### 🛠️ 开发指南

#### [📄 Development-Guide.md](./Development-Guide.md) - 完整开发指南

**内容**：
- 开发工作流程（4个阶段）
- 命名和代码规范
- 3个完整的开发任务示例
- 单元测试示例
- 发布部署清单

**适用对象**：所有开发者

**阅读时间**：40-50分钟

---

### 🔷 Prism 框架

#### [📄 Prism-Migration-Guide.md](./Prism-Migration-Guide.md) - Prism 迁移指南

**内容**：
- 升级目标和架构对比
- 迁移步骤详解
- 关键代码变更对比
- 模块化架构益处
- 迁移检查清单
- 常见问题

**适用对象**：从 CommunityToolkit.Mvvm 迁移过来的开发者

**阅读时间**：30-40分钟

---

#### [📄 Prism-Module-Development.md](./Prism-Module-Development.md) - Prism 模块开发指南 ⭐ 重要

**内容**：
- 10分钟快速开始
- 5个常用开发模式
- 完整的开发清单
- 常见问题排查
- 完整的示例代码

**适用对象**：需要开发 Prism 模块的开发者

**阅读时间**：20-30分钟

---

## 🎯 快速导航

### 按角色查找文档

**👨‍💼 架构师 / 技术主管**
1. 先读：[UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md) - 了解升级情况
2. 再读：[Architecture.md](./Architecture.md) - 了解新架构
3. 参考：[Prism-Migration-Guide.md](./Prism-Migration-Guide.md) - 了解迁移细节

**👨‍💻 后端开发者**
1. 先读：[UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md) - 快速了解变化
2. 再读：[QuickStart.md](./QuickStart.md) - 快速上手
3. 深入：[Development-Guide.md](./Development-Guide.md) - 完整指南

**🎨 前端 / UI 开发者**
1. 先读：[UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md) - 了解 UI 更新
2. 重点：[Prism-Module-Development.md](./Prism-Module-Development.md) - 模块开发
3. 参考：[Architecture.md](./Architecture.md) 中的 UI 和模块化章节

**🆕 新人入职**
1. 先读：[UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md) - 项目概况
2. 再读：[QuickStart.md](./QuickStart.md) - 快速上手
3. 最后：[Architecture.md](./Architecture.md) - 理解架构
4. 深入：[Prism-Module-Development.md](./Prism-Module-Development.md) - 开发指南

---

## 📖 按主题查找文档

### 我想...

#### 🔍 **了解项目升级情况**
→ 阅读 [UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md)

#### 🏗️ **了解项目整体架构**
→ 阅读 [Architecture.md](./Architecture.md)

#### ⚡ **快速开始开发**
→ 阅读 [QuickStart.md](./QuickStart.md)

#### 🔷 **使用 Prism 开发模块**
→ 阅读 [Prism-Module-Development.md](./Prism-Module-Development.md)

#### 🔗 **理解模块间如何协作**
→ 阅读 [Module-Integration.md](./Module-Integration.md)

#### 🛠️ **学习如何开发新功能**
→ 阅读 [Development-Guide.md](./Development-Guide.md)

#### 📋 **从 CommunityToolkit.Mvvm 迁移到 Prism**
→ 阅读 [Prism-Migration-Guide.md](./Prism-Migration-Guide.md)

---

## 🔑 关键概念速查

### 新架构亮点
```
✅ 模块化设计 - 每个功能独立为模块
✅ Prism 框架 - 业界标准框架
✅ 动态加载 - 支持模块动态加载/卸载
✅ 区域管理 - 支持多个内容区域
```

### 7 层分层架构 + 模块化设计
```
[UI 100]  ← 使用 Prism + 模块化
[BLL 05]
[HAL 02 + Services 04]
[Core 02]
[Infrastructure 03]
[Common 01]

[Modules 06] ← 新项目，包含所有功能模块
```

### 核心原则
- ✅ 接口定义在 Core，实现在各层或模块
- ✅ 通过 Prism DI（DryIOC）解耦
- ✅ 模块通过事件（EventAggregator）通信
- ✅ 不允许越层调用
- ✅ 异步操作使用 Task
- ✅ 统一错误处理和日志记录

---

## 💡 常见问题速答

**Q: 项目最近升级了什么？**
A: 从 CommunityToolkit.Mvvm 升级到 Prism 9.0，采用模块化设计。参考 [UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md)

**Q: 我应该先读哪个文档？**
A: 如果是刚了解这个升级，先读 [UPGRADE-SUMMARY.md](./UPGRADE-SUMMARY.md)；如果要快速开发，读 [Prism-Module-Development.md](./Prism-Module-Development.md)

**Q: 如何创建新的功能模块？**
A: 参考 [Prism-Module-Development.md](./Prism-Module-Development.md) 的"10分钟快速开始"部分

**Q: 旧代码需要全部重写吗？**
A: 不需要。可以渐进式迁移，新功能使用新架构。参考 [Prism-Migration-Guide.md](./Prism-Migration-Guide.md)

**Q: 模块间如何通信？**
A: 使用 Prism 的 EventAggregator（事件）或共享服务接口。参考 [Module-Integration.md](./Module-Integration.md)

---

## 📋 文档清单与状态

| 文档 | 状态 | 上次更新 | 说明 |
|------|------|--------|------|
| UPGRADE-SUMMARY.md | ✅ 完成 | 2024-01-XX | 升级完成总结 |
| Architecture.md | ✅ 完成 | 2024-01-XX | 更新模块化章节 |
| QuickStart.md | ✅ 完成 | 2024-01-XX | 快速参考 |
| Module-Integration.md | ✅ 完成 | 2024-01-XX | 模块协作 |
| Development-Guide.md | ✅ 完成 | 2024-01-XX | 开发指南 |
| Prism-Migration-Guide.md | ✅ 完成 | 2024-01-XX | Prism 迁移指南 |
| Prism-Module-Development.md | ✅ 完成 | 2024-01-XX | 模块开发指南 |
| README.md (本文件) | ✅ 完成 | 2024-01-XX | 文档中心 |

### 📝 待添加文档

- [ ] Database-Design.md - 数据库设计文档
- [ ] API-Docs.md - API 接口文档
- [ ] Deployment-Guide.md - 部署上线指南
- [ ] Troubleshooting.md - 故障排查手册
- [ ] FAQ.md - 常见问题解答
- [ ] Performance-Guide.md - 性能优化指南
- [ ] Security-Guide.md - 安全最佳实践
- [ ] Release-Notes.md - 版本发布说明

---

## 🔗 外部资源

### 官方文档
- [.NET 8 官方文档](https://learn.microsoft.com/en-us/dotnet/)
- [C# 编程指南](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [WPF 官方文档](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)

### Prism 资源
- [Prism 官方网站](https://prismlibrary.com/)
- [Prism GitHub](https://github.com/PrismLibrary/Prism)
- [Prism 文档](https://prismlibrary.com/docs/wpf/introduction.html)

### 开源库文档
- [FreeSql](https://freesql.net/)
- [Serilog](https://serilog.net/)
- [FluentModbus](https://github.com/Apollo3zehn/FluentModbus)

### 最佳实践
- [微软 .NET 设计指南](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)
- [Clean Code Principles](https://www.objectmentor.com/omTeam/clean_code.html)
- [SOLID 原则](https://en.wikipedia.org/wiki/SOLID)

---

## ✨ 提示与技巧

### 💡 高效阅读文档

1. **使用目录快速定位**
   - 每个文档都有详细的目录
   - 使用 Ctrl+F 搜索关键词

2. **利用超链接跳转**
   - 相关文档之间有交叉引用
   - 点击链接快速切换

3. **配合代码阅读**
   - 文档提供了完整代码示例
   - 在 IDE 中打开代码逐行研究

4. **记录学习笔记**
   - 在文档中标记重点
   - 建立自己的知识库

---

## 📈 学习路径建议

### 第 1 天：快速了解
```
早上：
  ├─ 10分钟：阅读 UPGRADE-SUMMARY.md
  ├─ 10分钟：了解项目升级情况
  └─ 15分钟：浏览主窗口功能

下午：
  ├─ 20分钟：阅读 QuickStart.md
  └─ 20分钟：查看代码示例
```

### 第 2-3 天：理解架构
```
  ├─ 30分钟：阅读 Architecture.md（重点：模块化章节）
  ├─ 20分钟：查看依赖关系图
  ├─ 30分钟：阅读 Module-Integration.md
  └─ 30分钟：追踪一个完整的数据流
```

### 第 4-5 天：实践 Prism 开发
```
  ├─ 30分钟：选择一个简单任务
  ├─ 60分钟：阅读 Prism-Module-Development.md
  ├─ 120分钟：完成模块开发任务
  └─ 30分钟：代码审查和优化
```

### 第 6 天：深度理解
```
  ├─ 阅读完整的 Development-Guide.md
  ├─ 阅读 Prism-Migration-Guide.md
  ├─ 学习开发规范和最佳实践
  ├─ 完成单元测试编写
  └─ 参与代码审查
```

---

## 🎉 项目信息

- 📦 项目名称：BaiHua.Software
- 🎯 项目类型：上位机（SCADA/HMI）通用框架（使用 Prism + 模块化设计）
- 🔧 技术框架：.NET 8 + WPF + Prism 9.0
- 📍 文档位置：`BaiHua.Software.UI/docs/`
- 🔗 代码库：https://github.com/by-2020-github/BaiHua.Software

---

**最后更新**：2024-01-XX  
**文档版本**：2.0 (Prism Edition)  
**维护状态**：✅ 活跃维护

**祝你开发愉快！如有任何问题或建议，欢迎提出。** 🚀

