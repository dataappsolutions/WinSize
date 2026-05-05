# WinSize

WinSize is a small .NET 8 utility for resizing open Windows application windows.

It includes:
- **WinSizeCore**: a console app that can resize a window by exact title or run in interactive mode.
- **WinSizeUI**: a Windows Forms app for selecting and resizing windows with a GUI.
- **WinSizeUI3**: a WinUI 3 (Windows App SDK) app offering the same functionality with a modern Fluent Design interface.

## Features

- Enumerates visible, titled desktop windows.
- Displays window title, process name, and process ID.
- Resizes windows to a user-provided width and height.
- Restores minimized/maximized windows before resizing.
- Optional center-alignment of the resized window on screen.
- Common display size presets (SVGA through 4K UHD) available in both GUI apps.

## Project structure

- `WinSizeCore/` - Console app (`net8.0`)
- `WinSizeUI/` - Windows Forms app (`net8.0-windows`)
- `WinSizeUI3/` - WinUI 3 app (`net8.0-windows10.0.19041.0`, Windows App SDK 2.0)
- `WinSizeShared.cs` - Shared Win32 interop + window management logic

## Requirements

- Windows OS (uses `user32.dll` APIs)
- .NET 8 SDK
- **WinSizeUI3 only**: [Windows App Runtime 2.x](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/downloads) must be installed on the target machine

## Build

From the repository root:

```bash
dotnet build WinSize.sln
```

## Publish with minimal deployed files (framework-dependent, no bundled .NET runtime)

If you want the smallest output and do **not** want to include .NET 8 runtime files, publish as a **framework-dependent single-file** app.

### WinSizeCore (console)

Bash:

```bash
dotnet publish WinSizeCore/WinSize.csproj \
  -c Release \
  -r win-x64 \
  --self-contained false \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true
```

PowerShell:

```powershell
dotnet publish WinSizeCore/WinSize.csproj `
  -c Release `
  -r win-x64 `
  --self-contained false `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true
```

Output folder:

```text
WinSizeCore/bin/Release/net8.0/win-x64/publish/
```

### WinSizeUI (WinForms)

Bash:

```bash
dotnet publish WinSizeUI/WinSizeUI.csproj \
  -c Release \
  -r win-x64 \
  --self-contained false \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true
```

PowerShell:

```powershell
dotnet publish WinSizeUI/WinSizeUI.csproj `
  -c Release `
  -r win-x64 `
  --self-contained false `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true
```

Output folder:

```text
WinSizeUI/bin/Release/net8.0-windows/win-x64/publish/
```

> Note: Framework-dependent deployments require .NET 8 runtime to already be installed on the target machine.

### WinSizeUI3 (WinUI 3)

WinSizeUI3 cannot be published as a single file due to native Windows App SDK components. It publishes as a folder:

PowerShell:

```powershell
dotnet publish WinSizeUI3/WinSizeUI3.csproj `
  -c Release `
  -r win-x64 `
  --self-contained false `
  -p:PublishTrimmed=false `
  -p:PublishReadyToRun=false `
  -p:PublishProfile='""'
```

Output folder:

```text
WinSizeUI3/bin/Release/net8.0-windows10.0.19041.0/win-x64/publish/
```

> Note: The target machine must have [Windows App Runtime 2.x](https://learn.microsoft.com/en-us/windows/apps/windows-app-sdk/downloads) installed.

## Run (Console)

Interactive mode:

```bash
dotnet run --project WinSizeCore/WinSize.csproj
```

Direct resize mode:

```bash
dotnet run --project WinSizeCore/WinSize.csproj -- "Untitled - Notepad" 800 600
```

## Run (WinForms UI)

```bash
dotnet run --project WinSizeUI/WinSizeUI.csproj
```

## Run (WinUI 3)

```bash
dotnet run --project WinSizeUI3/WinSizeUI3.csproj -r win-x64
```

> Note: A runtime identifier (`-r win-x64`) is required — WinUI 3 does not support AnyCPU.

## Notes

- Window title matching in direct console mode is an **exact** match.
- If a resize fails, the app reports the Win32 error code.
