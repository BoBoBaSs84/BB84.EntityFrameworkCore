# BB84.EntityFrameworkCore

[![CI](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/ci.yml)
[![CD](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/cd.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/cd.yml)
[![CodeQL](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/github-code-scanning/codeql/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/github-code-scanning/codeql)
[![Dependabot](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/dependabot/dependabot-updates/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/actions/workflows/dependabot/dependabot-updates)

[![.NET](https://img.shields.io/badge/net8.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![.NET](https://img.shields.io/badge/net10.0-5C2D91?logo=.NET&labelColor=gray)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![C#](https://img.shields.io/badge/C%23-13.0-239120)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![Issues](https://img.shields.io/github/issues/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/issues)
[![Commit](https://img.shields.io/github/last-commit/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/commit/main)
[![License](https://img.shields.io/github/license/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/blob/main/LICENSE)
[![RepoSize](https://img.shields.io/github/repo-size/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore)
[![Release](https://img.shields.io/github/v/release/BoBoBaSs84/BB84.EntityFrameworkCore)](https://github.com/BoBoBaSs84/BB84.EntityFrameworkCore/releases/latest)

## 🔎 Overview

**BB84.EntityFrameworkCore** is a .NET 8.0 & .NET 10.0 library collection that provides a reusable repository pattern implementation for Entity Framework Core applications. It ships as five focused NuGet packages that can be adopted incrementally.

## 📦 Packages

| Package                                                                                                                | Description                                                       |
| ---------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------- |
| [BB84.EntityFrameworkCore.Entities.Abstractions](src/BB84.EntityFrameworkCore.Entities.Abstractions/README.md)         | Core entity interface definitions — no dependencies               |
| [BB84.EntityFrameworkCore.Entities](src/BB84.EntityFrameworkCore.Entities/README.md)                                   | Default implementations of the entity abstractions                |
| [BB84.EntityFrameworkCore.Repositories.Abstractions](src/BB84.EntityFrameworkCore.Repositories.Abstractions/README.md) | Repository interface definitions                                  |
| [BB84.EntityFrameworkCore.Repositories](src/BB84.EntityFrameworkCore.Repositories/README.md)                           | Default repository implementations and `DatabaseFacadeExtensions` |
| [BB84.EntityFrameworkCore.Repositories.SqlServer](src/BB84.EntityFrameworkCore.Repositories.SqlServer/README.md)       | SQL Server entity configurations, interceptors, and extensions    |

## 💾 Installation

```powershell
dotnet add package BB84.EntityFrameworkCore.Entities.Abstractions
dotnet add package BB84.EntityFrameworkCore.Entities
dotnet add package BB84.EntityFrameworkCore.Repositories.Abstractions
dotnet add package BB84.EntityFrameworkCore.Repositories
dotnet add package BB84.EntityFrameworkCore.Repositories.SqlServer
```

## 🚧 Build and test

```powershell
# Build the solution
dotnet build

# Run all tests
dotnet test

# Run tests
dotnet test
```

## 🏗️ Project structure

```
BB84.EntityFrameworkCore/
├── src/
│   ├── BB84.EntityFrameworkCore.Entities.Abstractions/   # Entity interface definitions
│   ├── BB84.EntityFrameworkCore.Entities/                # Entity implementations
│   ├── BB84.EntityFrameworkCore.Repositories.Abstractions/ # Repository interface definitions
│   ├── BB84.EntityFrameworkCore.Repositories/            # Repository implementations
│   └── BB84.EntityFrameworkCore.Repositories.SqlServer/  # SQL Server configurations and extensions
├── tests/
│   ├── BB84.EntityFrameworkCore.Entities.Tests/          # Entity unit tests
│   └── BB84.EntityFrameworkCore.Repositories.Tests/      # Repository integration tests (requires LocalDB)
└── docs/                                                 # DocFX documentation source
```

## 🤝 Contributing

1. Fork the repository and create a feature branch from `main`.
2. Follow the existing code style (see `.editorconfig`).
3. Add XML documentation for all public APIs.
4. Add unit tests for every new method.
5. Ensure all tests pass across all target frameworks.
6. Open a pull request describing your changes.

## 📖 API Documentation

Complete API reference: <https://bobobass84.github.io/BB84.EntityFrameworkCore>

To build the documentation locally:

```powershell
dotnet tool install -g docfx
docfx docs/docfx.json --serve
```

## ⚖️ License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.
