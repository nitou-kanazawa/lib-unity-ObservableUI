# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**ObservableUI** is a Unity package that extends R3 (Reactive Extensions for Unity) with reactive UI bindings for Unity's uGUI and TextMeshPro components. It provides bidirectional data binding, observable events, and reactive component wrappers for building reactive user interfaces in Unity.

- **Package Name**: `jp.nitou.observableui`
- **Version**: 1.0.0
- **Unity Version**: 6000.0+ (Unity 6.2+)
- **Current Branch**: `feature/switch-to-r3`
- **Main Branch**: `main`

## Architecture

### Assembly Structure

The project is organized into two main assemblies:

1. **ObservableUI** (Runtime) - `Assets/ObservableUI/Core/`
   - Namespace: `Nitou.ObservableUI` (components), `R3` (extensions)
   - Contains reactive components, interfaces, and extension methods
   - References: R3.Unity, R3.Unity.TextMeshPro, Unity UI modules

2. **ObservableUI.Editor** (Editor-only) - `Assets/ObservableUI/Editor/`
   - Namespace: `Nitou.ObservableUI.Editor`
   - Editor tools and custom inspectors
   - Has internal access to Core assembly

### Core Components

The framework consists of three main layers:

1. **Reactive Components** (`Assets/ObservableUI/Core/Components/`)
   - `ReactiveInputField<T>`: Base class for type-safe reactive input fields
   - `IntReactiveInputField`, `FloatReactiveInputField`: Concrete implementations
   - `Vector2ReactiveInputField`, `Vector3ReactiveInputField`: Multi-field inputs (incomplete)
   - `ReactiveEnumDropdown<TEnum>`: Reactive dropdown for enum selection

2. **Interfaces** (`Assets/ObservableUI/Core/Interface/`)
   - `IReactivePropertyHolder<T>`: Base contract for components with reactive properties
   - `IReactiveInputField<T>`: Interface for reactive input fields
   - `IReactiveInputFieldStepper<T>`: Interface for stepper-like controls
   - `IInputValidator<T>`: Input validation contract

3. **Extension Methods** (`Assets/ObservableUI/Core/Extensions/`)
   - Provide reactive bindings for Unity UI components
   - Naming convention:
     - `SubscribeToXXX`: One-way binding (observable → UI)
     - `BindToXXX`: Two-way binding (reactive property ↔ UI)
     - `OnXXXAsObservable()`: Observable event sources

### Key Dependencies

- **R3 (Reactive Extensions)**: Installed via git URL `https://github.com/Cysharp/R3.git?path=src/R3.Unity/Assets/R3.Unity`
- **TextMeshPro**: Primary text component (built into Unity 6+)
- **Unity UI (uGUI)**: Core UI framework

## Development Commands

### Building the Project

This is a Unity project. Build through Unity Editor:
1. Open project in Unity 6000.0.2f6 or later
2. File → Build Settings
3. Select platform and build

### Running Tests

Currently no test assembly is defined. To add tests:
- Create test assemblies in `Assets/ObservableUI/Tests/Runtime/` and `Assets/ObservableUI/Tests/Editor/`
- Reference `com.unity.test-framework`

### Opening in IDE

The project includes support for:
- JetBrains Rider (via `com.unity.ide.rider`)
- Visual Studio (via `com.unity.ide.visualstudio`)

Open the `ObservableUI.sln` solution file in your IDE.

## Design Patterns

### Reactive Component Pattern

All reactive components follow this lifecycle using R3:

```csharp
// Awake: Initialize reactive property from view
void Awake() {
    _reactiveProperty = new ReactiveProperty<T>(GetInitialValue());
    _reactiveProperty.Subscribe(SetToView).AddTo(this);
}

// OnDestroy: Clean up subscriptions
void OnDestroy() {
    _reactiveProperty?.Dispose();
}
```

Note: R3 uses `Disposable.Combine()` instead of `StableCompositeDisposable.Create()`

### Template Method Pattern

`ReactiveInputField<T>` uses template method pattern:
- Abstract methods: `TryParseFromView()`, `SetToView()`, `ObserveEndEditEvent()`
- Concrete implementations provide type-specific parsing and display logic

### Extension Method Conventions

- **One-way binding**: Observable → UI component (Subscribe pattern)
- **Two-way binding**: ReactiveProperty ↔ UI component (Bind pattern)
- Use `AddTo(Component)` for automatic subscription cleanup

## Important Code Conventions

1. **Using R3 instead of UniRx**: The codebase has migrated from UniRx to R3
2. **Extension methods are in `R3` namespace**, not `UniRx` namespace
3. **Reactive components use `where T : struct`** constraint for value types
4. **Parse failures reset input fields** to previous valid value
5. **All subscriptions use `AddTo(this)`** to prevent memory leaks
6. **Editor validation** via `OnValidate()` for auto-assigning component references
7. **Japanese comments** are common in the codebase
8. **Use `Disposable.Combine()`** for combining disposables, not `StableCompositeDisposable.Create()`

## Known Incomplete Features

- `Vector2ReactiveInputField.TryParseFromView()`: Throws `NotImplementedException`
- `Vector3ReactiveInputField.TryParseFromView()`: Throws `NotImplementedException`
- `Assets/ObservableUI/Localization/`: Empty directory
- `Assets/ObservableUI/Core/Utilities/`: Empty directory

## File Structure

```
Assets/ObservableUI/
├── Core/                          # Runtime assembly
│   ├── Components/
│   │   ├── InputField/           # Reactive input field variants
│   │   └── Dropdown/             # Reactive dropdown components
│   ├── Interface/                # Core interfaces
│   ├── Extensions/               # Extension methods for UI bindings
│   ├── Utilities/                # (Empty)
│   └── Localization/             # (Empty)
├── Editor/                        # Editor assembly
└── package.json                   # Unity package manifest

Assets/Develop/                    # Development/testing code
Assets/Scenes/                     # Example scenes
```

## Git Workflow

- Main development branch: `main`
- Feature branches: `feature/*`
- Recent work focused on Unity 6.2 compatibility and R3 reactive library migration (branch: `feature/switch-to-r3`)

## Extension Method Examples

### One-Way Binding (Observable → UI)
```csharp
observable.SubscribeToText(textComponent);
observable.SubscribeToSlider(slider);
```

### Two-Way Binding (ReactiveProperty ↔ UI)
```csharp
reactiveProperty.BindToInputField(inputField, parse, format);
reactiveProperty.BindToSlider(slider);
reactiveProperty.BindToToggle(toggle);
```

### Observable Events
```csharp
inputField.OnEndEditAsObservable().Subscribe(value => ...);
button.OnDoubleClickAsObservable(interval).Subscribe(_ => ...);
button.OnLongPressAsObservable(threshold).Subscribe(_ => ...);
```