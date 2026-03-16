# 后端逐项检查浏览路径（建议顺序）

> 目的：按“从入口到业务再到数据”的顺序，让你可以逐一核对实现是否符合 PRD。

## 1) 先看启动与全局配置
1. `src/DeclarationManagement.Api/Program.cs`
   - 查看依赖注入、JWT、控制器注册、数据库连接。

## 2) 再看接口入口（Controller）
2. `src/DeclarationManagement.Api/Controllers/AuthController.cs`
3. `src/DeclarationManagement.Api/Controllers/UsersController.cs`
4. `src/DeclarationManagement.Api/Controllers/TasksController.cs`
5. `src/DeclarationManagement.Api/Controllers/DeclarationsController.cs`
6. `src/DeclarationManagement.Api/Controllers/ReviewsController.cs`
7. `src/DeclarationManagement.Api/Controllers/StatisticsController.cs`

> 检查点：Controller 是否只负责收参与返回，不写复杂业务。

## 3) 核心业务服务（Service）
8. `src/DeclarationManagement.Api/Services/AuthService.cs`
9. `src/DeclarationManagement.Api/Services/UserService.cs`
10. `src/DeclarationManagement.Api/Services/TaskService.cs`
11. `src/DeclarationManagement.Api/Services/DeclarationService.cs`
12. `src/DeclarationManagement.Api/Services/ReviewService.cs`
13. `src/DeclarationManagement.Api/Services/StatisticsService.cs`
14. `src/DeclarationManagement.Api/Services/FileStorageService.cs`

> 检查点：
> - 所有数据库操作是否使用 EF Core 异步方法。
> - 审核动作是否同时写审核记录和流程日志。
> - 驳回后是否允许修改并重提。

## 4) 数据传输与映射层
15. `src/DeclarationManagement.Api/DTOs/*.cs`
16. `src/DeclarationManagement.Api/Mapping/MappingProfile.cs`

> 检查点：是否通过 DTO 对外返回，避免直接暴露 Entity。

## 5) 实体与数据库结构
17. `src/DeclarationManagement.Api/Entities/Enums.cs`
18. `src/DeclarationManagement.Api/Entities/*.cs`
19. `src/DeclarationManagement.Api/Data/AppDbContext.cs`

> 检查点：
> - 预审权限与初审权限是否为独立关联表。
> - 枚举命名是否清晰。

## 6) 可选：仓储与公共辅助
20. `src/DeclarationManagement.Api/Repositories/*.cs`
21. `src/DeclarationManagement.Api/Common/*.cs`

---

## 一条命令快速开始浏览
在仓库根目录执行：

```bash
code src/DeclarationManagement.Api/Program.cs \
src/DeclarationManagement.Api/Controllers \
src/DeclarationManagement.Api/Services \
src/DeclarationManagement.Api/DTOs \
src/DeclarationManagement.Api/Entities \
src/DeclarationManagement.Api/Data/AppDbContext.cs
```

如果你不用 VS Code，可以按上面的编号顺序手动打开对应文件夹与文件。
