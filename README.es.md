# MatMaker

<p align="center">
  <img src="src/MatMaker/Assets/matmaker-mark.png" alt="Logotipo de MatMaker" width="150" />
</p>

<p align="center">
  Crea archivos <code>.mat</code> compatibles con MATLAB a partir de tablas copiadas desde Excel.<br /><br />
  <a href="README.md">English</a> · <a href="README.es.md">Español</a>
</p>

MatMaker es una aplicación de escritorio ligera y multiplataforma para convertir una tabla de 8 columnas en un archivo `.mat` compatible con MATLAB. No requiere tener MATLAB instalado.

## Funciones

- Pega directamente datos separados por tabuladores desde Excel.
- Acepta una fila opcional de encabezados y valida exactamente ocho columnas numéricas.
- Conserva las celdas vacías como `NaN`.
- Guarda una única matriz `double` llamada `Resultados`, igual que los archivos usados en el flujo de Valentia.
- Lee de nuevo el archivo guardado y verifica su variable, dimensiones y valores.
- Cambia la interfaz entre inglés y español.
- Funciona en macOS y Windows mediante lanzamientos autocontenidos.

## Descarga

Obtén el instalador o paquete portable para tu sistema operativo desde el [último lanzamiento](https://github.com/albertobzn9/MatMaker/releases/latest).

Cada lanzamiento incluye un `.dmg` de macOS, un `.zip` portable de macOS, un `.exe` independiente de Windows y un `.zip` portable de Windows.

## Usar la aplicación

1. Escribe el nombre del archivo, sin la extensión `.mat`.
2. Copia las ocho columnas desde Excel y pégalas en el área principal.
3. Selecciona **Validar tabla** para revisar los datos.
4. Selecciona **Guardar archivo .mat…** y elige el destino.

El archivo guardado contiene una única variable de matriz: `Resultados`.

## Ejecutar desde el código

Requisito: [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0).

```bash
git clone https://github.com/albertobzn9/MatMaker.git
cd MatMaker
dotnet run --project src/MatMaker/MatMaker.csproj
```

## Estructura del repositorio

```text
src/MatMaker/       Código de la aplicación Avalonia y recursos gráficos
├── Views/           Marcado de la ventana principal y su código asociado
├── Services/        Análisis, verificación y traducción de la interfaz
└── Assets/          Iconos y logotipo de la aplicación
docs/                Notas sobre arquitectura y formato de archivo
packaging/           Metadatos del paquete para macOS
scripts/             Script local para empaquetar lanzamientos
```

Consulta la [guía de arquitectura](docs/README.md) para conocer el flujo de procesamiento y las garantías de compatibilidad.

## Crear un lanzamiento local

En macOS, crea todos los archivos de distribución con:

```bash
./scripts/package-release.sh 1.2.0
```

El script guarda el `.dmg` de macOS, `.zip` de macOS, `.exe` de Windows y `.zip` de Windows en `artifacts/release/<versión>/`.

## Tecnología

- [.NET 10](https://dotnet.microsoft.com/)
- [Avalonia UI](https://avaloniaui.net/)
- [MathNet.Numerics.Data.Matlab](https://www.nuget.org/packages/MathNet.Numerics.Data.Matlab)

## Licencia

Este repositorio no declara actualmente una licencia. Consulta con la persona propietaria antes de redistribuir o reutilizar el código.
