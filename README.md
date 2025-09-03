# Eshop - ASP.NET Core 电商系统

## 项目概述

Eshop 是一个基于 ASP.NET Core 和 Entity Framework Core 构建的电商系统，包含完整的前后台管理功能。系统采用经典的三层架构，实现了用户认证、商品管理、购物车、订单处理等核心电商功能。

## 功能特性

### 前台功能
- **用户管理**
  - 用户注册与登录
  - 个人中心（待完善）
  - 安全退出

- **商品浏览**
  - 商品列表分页展示
  - 商品搜索
  - 商品详情查看

- **购物车**
  - 添加商品到购物车
  - 修改购物车商品数量
  - 从购物车移除商品
  - 购物车商品搜索

- **订单管理**
  - 创建订单
  - 查看订单列表
  - 订单详情
  - 订单状态跟踪（待付款、待发货、待收货、已完成）
  - 模拟支付功能
  - 确认收货

### 后台管理
- **数据看板**
  - 商品总数统计
  - 订单总数统计
  - 用户总数统计
  - 销售总额统计

- **商品管理**
  - 商品列表（分页+搜索）
  - 添加新商品
  - 编辑商品信息
  - 删除商品

- **订单管理**
  - 订单列表（支持按状态筛选）
  - 订单详情查看
  - 修改订单状态
  - 删除订单

- **用户管理**
  - 用户列表（分页+搜索）
  - 添加新用户
  - 编辑用户信息
  - 删除用户
  - 设置/取消管理员权限

## 技术栈

- **后端**
  - .NET 8.0
  - ASP.NET Core MVC
  - Entity Framework Core 8.0
  - SQL Server 2022

- **前端**
  - Razor 视图引擎
  - Bootstrap 5
  - jQuery 3.x
  - AJAX（部分功能）

- **安全**
  - 密码加盐哈希存储
  - Session 管理用户状态
  - 防伪令牌（Anti-forgery token）
  - 权限过滤器（登录验证、管理员验证）

## 项目结构

```
Eshop/
│
├── Controllers/           # 控制器
│   ├── Admin/            # 后台管理控制器
│   └── *.cs              # 前台控制器
├── Data/                  # 数据相关
│   └── DbInitializer.cs  # 数据库初始化
│
├── Filters/               # 过滤器
│   ├── AdminFilter.cs    # 管理员权限验证
│   └── LoginFilter.cs    # 登录验证
│
├── Migrations/           # EF Core 数据库迁移文件
│
├── Models/               # 数据模型
│   ├── Admin/            # 后台视图模型
│   └── *.cs              # 实体类
│
├── Utils/                # 工具类
│   └── PasswordHelper.cs # 密码处理
│
├── Views/                # 视图文件
│   ├── Admin/            # 后台管理视图
│   ├── Cart/             # 购物车视图
│   ├── Home/             # 首页视图
│   ├── Orders/           # 订单视图
│   ├── Products/         # 商品视图
│   └── Users/            # 用户视图
│
├── Filters/              # 过滤器
│   ├── AdminFilter.cs    # 管理员权限过滤器
│   └── LoginFilter.cs    # 登录验证过滤器
│
├── Migrations/           # 数据库迁移文件
├── wwwroot/              # 静态资源文件
└── Program.cs            # 应用程序入口
```

## 技术栈

- **后端**
  - .NET 8.0
  - ASP.NET Core MVC
  - Entity Framework Core 8.0
  - SQL Server 2022

- **前端**
  - Razor 视图引擎
  - Bootstrap 5
  - jQuery 3.x
  - AJAX（部分功能）

- **开发工具**
  - Visual Studio 2022 或 VS Code
  - SQL Server Management Studio

## 数据库设计

系统包含以下主要数据表：
- `Products` - 商品信息
- `Users` - 用户信息
- `Carts` - 购物车
- `CartItems` - 购物车项
- `Orders` - 订单
- `OrderItems` - 订单项

## 依赖项

- Microsoft.EntityFrameworkCore.SqlServer (9.0.8)
- Microsoft.EntityFrameworkCore.Tools (9.0.8)
- X.PagedList (10.5.9)
- X.PagedList.Mvc.Core (10.5.9)

## 快速开始

### 环境要求

- .NET 8.0 SDK 
- SQL Server 2022
- Visual Studio 2022 或 VS Code

### 安装步骤

1. 克隆仓库
   ```bash
   git clone https://github.com/yourusername/Eshop.git
   cd Eshop
   ```

2. 配置数据库连接字符串
   在 `appsettings.json` 中修改数据库连接字符串：
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EshopDb;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. 应用数据库迁移
   ```bash
   dotnet ef database update
   ```

4. 运行项目
   ```bash
   dotnet run
   ```

5. 访问应用
   - 前台：https://localhost:5001
   - 后台：https://localhost:5001/Admin/Dashboard

## 默认账号

- **管理员账号**
  - 用户名：admin
  - 密码：Admin1

- **普通用户**
  - 可通过注册页面自行注册

## 贡献指南

1. Fork 项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

## 致谢

- [Bootstrap](https://getbootstrap.com/)
- [jQuery](https://jquery.com/)
- [Font Awesome](https://fontawesome.com/)

---
