# Jamal Arouna Unity Library

A focused collection of reusable runtime utilities, extensions, components, and
editor diagnostics for Unity 6.

## Installation

In Unity, open `Window > Package Manager`, choose `Add package from Git URL`,
and enter:

```text
https://github.com/Jamal2911/JamalArouna-Unity-Library.git?path=Assets/JamalArouna.Library
```

## Structure

```text
JamalArouna.Library/
├── Runtime/
│   ├── Async/
│   ├── Collections/
│   ├── Components/
│   ├── Diagnostics/
│   ├── Execution/
│   ├── Extensions/
│   ├── Math/
│   ├── Physics/
│   ├── StateMachine/
│   └── Systems/
└── Editor/
    └── Diagnostics/
```

The folder structure and namespaces match. Runtime code is compiled into
`JamalArouna.Library`; editor-only code is isolated in
`JamalArouna.Library.Editor`.

## Main APIs

- `JamalArouna.Library.Async.AwaitableUtility`
- `JamalArouna.Library.Collections.CollectionExtensions`
- `JamalArouna.Library.Components`
- `JamalArouna.Library.Diagnostics.JLog`
- `JamalArouna.Library.Execution`
- `JamalArouna.Library.Extensions`
- `JamalArouna.Library.Math`
- `JamalArouna.Library.Physics`
- `JamalArouna.Library.StateMachine`
- `JamalArouna.Library.Systems.LockSystem<TLock>`

## Version 2 migration

Version 2 intentionally removes compatibility aliases. Replace the former
`JamalArouna.Library.Utilities` imports with the focused namespaces above.
`GameUtilities` was removed and its useful operations moved to dedicated
utility and extension classes.

The unfinished GameObject Picker and its reflection-based global key hook were
removed. DOTween and Odin are no longer optional compile-time dependencies.

See [CHANGELOG.md](Assets/JamalArouna.Library/CHANGELOG.md) for the detailed
breaking-change summary.

## License

Copyright © Jamal Arouna.
