# MatMaker architecture

This document describes the current application flow and the compatibility rules used to create MATLAB files.

## Application flow

1. `Program.cs` starts the Avalonia desktop application.
2. `App.axaml.cs` creates `MainWindow` and owns the shared language state.
3. The user pastes tab-separated data and optionally supplies a file name.
4. `PastedMatrixParser` validates and converts the table into a `double` matrix.
5. `MainWindow` writes the matrix as `Resultados` in a `.mat` file.
6. `MatFileVerifier` reads the file back and checks its variable name, dimensions, and every value.

## Source map

| Location | Responsibility |
| --- | --- |
| `src/MatMaker/Views/` | Avalonia window markup and user interaction. |
| `src/MatMaker/Services/AppLocalizer.cs` | English and Spanish interface text plus shared language state. |
| `src/MatMaker/Services/PastedMatrixParser.cs` | Parses and validates the pasted Excel table. |
| `src/MatMaker/Services/MatFileVerifier.cs` | Validates the persisted `.mat` file after saving. |
| `src/MatMaker/Assets/` | Application icons and logo. |
| `packaging/macos/Info.plist` | macOS application-bundle metadata. |
| `scripts/package-release.sh` | Creates self-contained macOS and Windows packages on macOS. |

## MATLAB compatibility contract

Every file created by MatMaker follows these rules:

- It contains exactly one user matrix variable, named `Resultados`.
- The matrix uses MATLAB-compatible `double` values.
- The matrix has exactly eight columns.
- Empty pasted cells become `NaN` values.
- An optional complete header row is discarded before saving.
- The app reads the output back before reporting success, so the variable name, dimensions, and values must match the data that was pasted.

## Text input rules

The app expects tab-separated rows, as copied from Excel. It accepts decimal points and decimal commas. A partially textual first row is rejected because it is neither a complete header nor valid numeric data.

## Change guidelines

- Keep the compatibility contract unchanged unless all downstream applications that read `Resultados` are updated deliberately.
- Add all user-facing strings to `AppLocalizer` in both languages.
- Do not bypass the read-back verification when saving a `.mat` file.
