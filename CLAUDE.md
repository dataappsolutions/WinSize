# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

WinSize is a Windows-only .NET 8 utility for resizing open application windows. It exposes two interfaces:
- **WinSizeCore** — console app (direct-mode and interactive)
- **WinSizeUI** — Windows Forms GUI with preset display sizes

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
```

There are no test projects.

## Publish (minimal, framework-dependent single file)

```bash
dotnet publish WinSizeCore/WinSize.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

dotnet publish WinSizeUI/WinSizeUI.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

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
└── WinSizeShared.cs  — Root copy of shared logic (duplicate of WinSizeCore/WinSizeShared.cs)
```

**Key design points:**

- `WinSizeShared.cs` contains all Win32 P/Invoke (`user32.dll`: `SetWindowPos`, `EnumWindows`, `ShowWindow`, etc.) and the three public methods used by both UIs: `GetOpenWindows()`, `TryResizeWindow()`, `FindWindowByExactTitle()`.
- `WinSizeUI` references the `WinSizeCore` project to share the `WinSizeShared` namespace — there is no separate shared library project.
- The root `WinSizeShared.cs` is a duplicate of `WinSizeCore/WinSizeShared.cs`; edits to shared logic must be made in `WinSizeCore/WinSizeShared.cs` (the one actually compiled into the referenced project).
- `TryResizeWindow` restores minimized/maximized windows (`ShowWindow` with `SW_RESTORE`) before calling `SetWindowPos`.
- `WindowInfo` is the data model: `{ Handle, Title, ProcessName }`.
