# MatMaker

<p align="center">
  <img src="src/MatMaker/Assets/matmaker-mark.png" alt="MatMaker logo" width="150" />
</p>

<p align="center">
  Create MATLAB-compatible <code>.mat</code> files from tables copied from Excel.<br />
  <a href="README.md">English</a> · <a href="README.es.md">Español</a>
</p>

MatMaker is a lightweight cross-platform desktop app for turning an 8-column table into a compatible MATLAB `.mat` file. It does not require MATLAB to be installed.

## Features

- Paste tab-separated data directly from Excel.
- Accept an optional header row and validate exactly eight numeric columns.
- Preserve empty cells as `NaN`.
- Save one `double` matrix named `Resultados`, matching the files used by the Valentia workflow.
- Read the saved file back and verify its variable name, dimensions, and values.
- Switch the interface between English and Spanish.
- Run on macOS and Windows from self-contained releases.

## Download

Get the installer or portable package for your operating system from the [latest release](https://github.com/albertobzn9/MatMaker/releases/latest).

Each release provides a macOS `.dmg`, a macOS portable `.zip`, a standalone Windows `.exe`, and a Windows portable `.zip`.

## Use the app

1. Enter the file name, without the `.mat` extension.
2. Copy the eight columns from Excel and paste them into the main text area.
3. Select **Validate table** to review the data.
4. Select **Save .mat file…** and choose a destination.

The saved file contains only one matrix variable: `Resultados`.

## Run from source

Requirements: [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
git clone https://github.com/albertobzn9/MatMaker.git
cd MatMaker
dotnet run --project src/MatMaker/MatMaker.csproj
```

## Repository layout

```text
src/MatMaker/       Avalonia application source and application assets
├── Views/           Main window markup and its code-behind
├── Services/        Parsing, verification, and interface localization
└── Assets/          Application icons and logo
docs/                Architecture and file-format notes
packaging/           macOS bundle metadata
scripts/             Local release-packaging script
```

See [the architecture guide](docs/README.md) for the processing flow and compatibility guarantees.

## Build a release locally

On macOS, create all distribution files with:

```bash
./scripts/package-release.sh 1.2.0
```

The script writes the macOS `.dmg`, macOS `.zip`, Windows `.exe`, and Windows `.zip` to `artifacts/release/<version>/`.

## Technology

- [.NET 10](https://dotnet.microsoft.com/)
- [Avalonia UI](https://avaloniaui.net/)
- [MathNet.Numerics.Data.Matlab](https://www.nuget.org/packages/MathNet.Numerics.Data.Matlab)

## License

This repository currently does not declare a license. Contact the repository owner before redistributing or reusing the code.
