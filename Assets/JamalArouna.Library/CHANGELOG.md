# Changelog

## 2.0.0

### Breaking

- Requires Unity 6 (`6000.0`) or newer.
- Split `GameUtilities` into focused utility and extension classes.
- Aligned namespaces with the Runtime and Editor folder structure.
- Renamed ambiguous component and physics APIs.
- Removed the `Inst` and `I` singleton aliases.
- Replaced public mutable `Vector3Mask` fields with properties.
- Removed the incomplete GameObject Picker.
- Removed the DOTween and Odin integration code.

### Changed

- Isolated editor-only diagnostics in `JamalArouna.Library.Editor`.
- Fixed capsule overlap calculations for every capsule axis and non-uniform scale.
- Fixed vector random offsets to use the supplied radius vector.
- Made cooldowns available without Odin and made their duration non-negative.
- Removed generated IDE settings and macOS metadata from source control.
