# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

WinSize is a Windows-only .NET 8 utility for resizing open application windows. It exposes multiple interfaces:
- **WinSizeCore** — console app (direct-mode and interactive)
- **WinSizeUI** — Windows Forms GUI with preset display sizes
- **WinSizeUI3** — WinUI 3 GUI (Windows App SDK)
- **WinSizeService** — system tray app + ASP.NET Core localhost REST API
- **WinSizeWebUI** — vanilla JS web frontend (content-only, served by WinSizeService)

## Build & Run

```bash
# Build everything
dotnet build WinSize.sln

# Run console app (interactive)
dotnet run --project WinSizeCore/WinSize.csproj

# Run console app (direct mode: title width height)
dotnet run --project WinSizeCore/WinSize.csproj -- "Untitled - Notepad" 800 600

# Run WinForms UI
dotnet run --project WinSizeUI/WinSizeUI.csproj

# Run WinUI 3 UI (must target x64 or x86 explicitly)
dotnet run --project WinSizeUI3/WinSizeUI3.csproj -r win-x64

# Run web service (tray app + REST API, web UI at http://localhost:5250)
dotnet run --project WinSizeService/WinSizeService.csproj
```

## Tests

```bash
dotnet test WinSizeTests/WinSizeTests.csproj
```

WinSizeTests uses xunit and references WinSizeUI (net8.0-windows).

## Publish (minimal, framework-dependent single file)

```bash
dotnet publish WinSizeCore/WinSize.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

dotnet publish WinSizeUI/WinSizeUI.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

# WinSizeService publishes as a folder (wwwroot static files must remain alongside the executable)
dotnet publish WinSizeService/WinSizeService.csproj -c Release -r win-x64 --self-contained false
```

WinSizeUI3 uses Windows App SDK publish profiles (`win-x64.pubxml`, etc.) — publish via Visual Studio or the profile directly.

## Architecture

```
WinSize.sln
├── WinSizeCore/      — Console app (.NET 8)
│   ├── WinSize.csproj
│   ├── Program.cs         — WindowManager class; interactive + direct modes
│   └── WinSizeShared.cs   — Win32 interop + window management logic
├── WinSizeUI/        — Windows Forms app (.NET 8-windows)
│   ├── WinSizeUI.csproj   — References WinSizeCore project
│   ├── Form1.cs           — Main form: window list, resize controls, display presets
│   └── Program.cs
├── WinSizeUI3/       — WinUI 3 app (net8.0-windows10.0.19041.0, Windows App SDK 2.x)
│   ├── WinSizeUI3.csproj  — References WinSizeCore project; unpackaged (WindowsPackageType=None)
│   ├── App.xaml.cs
│   └── MainWindow.xaml.cs
├── WinSizeTests/     — xunit test project (.NET 8-windows, references WinSizeUI)
├── WinSizeService/   — Tray app + ASP.NET Core REST API (Microsoft.NET.Sdk, net8.0-windows)
│   ├── WinSizeService.csproj  — References WinSizeCore + WinSizeWebUI; FrameworkReference AspNetCore.App
│   └── Program.cs             — [STAThread] Main; NotifyIcon tray; WebApplication on background thread
├── WinSizeWebUI/     — Vanilla JS frontend (content-only project)
│   ├── WinSizeWebUI.csproj    — No SDK output; wwwroot/** copied to WinSizeService output via Content Include
│   └── wwwroot/
│       └── index.html         — Single-file UI: window list, preset sizes, resize + center controls
└── WinSizeShared.cs  — Root copy of shared logic (duplicate of WinSizeCore/WinSizeShared.cs)
```

**Key design points:**

- `WinSizeShared.cs` contains all Win32 P/Invoke (`user32.dll`: `SetWindowPos`, `EnumWindows`, `ShowWindow`, etc.) and the three public methods used by all UIs: `GetOpenWindows()`, `TryResizeWindow()`, `FindWindowByExactTitle()`.
- `WinSizeUI`, `WinSizeUI3`, and `WinSizeService` each reference the `WinSizeCore` project to share the `WinSizeShared` namespace — there is no separate shared library project.
- The root `WinSizeShared.cs` is a duplicate of `WinSizeCore/WinSizeShared.cs`; edits to shared logic must be made in `WinSizeCore/WinSizeShared.cs` (the one actually compiled into the referenced project).
- `TryResizeWindow` restores minimized/maximized windows (`ShowWindow` with `SW_RESTORE`) before calling `SetWindowPos`.
- `WindowInfo` is the data model: `{ Handle, Title, ProcessName }`.
- `WinSizeService` uses `Microsoft.NET.Sdk` (not `.Web`) + `<FrameworkReference Include="Microsoft.AspNetCore.App" />` to combine ASP.NET Core with WinForms (`UseWindowsForms=true`, `OutputType=WinExe`).
- `WinSizeService` REST API: `GET /api/windows` returns `{ handle, title, processName }`; `POST /api/resize` accepts `{ handle, width, height, center }`. Listens on `http://localhost:5250`.
- Body binding in the resize endpoint uses `ReadFromJsonAsync` with `PropertyNameCaseInsensitive = true` (automatic minimal API body binding is unreliable without the web SDK).
- `WinSizeWebUI` static files are pulled into `WinSizeService`'s output via `<Content Include="..\WinSizeWebUI\wwwroot\**" Link="wwwroot\..." CopyToOutputDirectory="PreserveNewest" />` in `WinSizeService.csproj`. The content root is set to `AppContext.BaseDirectory` at startup.
