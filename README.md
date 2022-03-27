# bReader
[![](https://img.shields.io/github/v/release/artiga033/bReader)](https://github.com/artiga033/bReader/releases)
[![](https://img.shields.io/badge/-Blazor-%23780C93?logo=blazor)](https://blazor.net)
[![](https://img.shields.io/badge/-6.0-%234169AA?logo=.NET)](https://dot.net)

A RSS Reader built with ASP.NET Core Blazor in fluent design style.

ASP.NET Core Blazor 构建的 RSS 阅读器，采用 Fluent Design 设计。

# Requirement 运行需求

- .NET 6 Runtime (Server version)
- ~~Browser with WebAssembly Support(Wasm Version)~~

# Deployment 部署

## Blazor Server

### Run 运行

Download from [release](https://github.com/artiga033/bReader/releases), make sure you have .NET 6 installed, and run the program.

从[release](https://github.com/artiga033/bReader/releases)下载，确保已安装.NET 6 运行时，运行程序。

### Configuration 配置

Blazor Server is using a .NET Generic Host hosting its service with inner Kestrel. So you may just configure the `appsettngs.json` like any other ASP.NET Core App. Check [this on MSDN](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-6.0) for detail.

Blazor Server 使用.NET 泛型主机的 Kestrel 服务器提供服务，可以像配置任何此类 ASP.NET Core 应用程序一样配置它的`appsettings.json`。参考 MSDN 的[这篇文章](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/servers/kestrel/endpoints?view=aspnetcore-6.0)。

## Blazor WebAssembly (不建议使用)
**⚠Warning: Currently Blazor Wasm is not fully servable, as restricted by the CORS strategy. It won't work for most rss sources who don't use a open CORS strategy.**

**⚠警告: 受CORS策略限制，目前Blazor Wasm无法完全提供服务.在没有“开放的” CORS策略RSS源上无法工作。**

Blazor Webassembly is a set of static html.You may serve it with any web server you like.

Blazor Webassembly 是静态html。可以使用Web服务器直接部署。
