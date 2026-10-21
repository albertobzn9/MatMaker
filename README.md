# MatMaker

Una herramienta de escritorio multiplataforma, construida con .NET y Avalonia UI, diseñada para convertir bloques de datos copiados de Excel directamente a archivos `.mat` compatibles con MATLAB.

## ¿Para qué sirve?
En el flujo de trabajo de análisis de datos, a menudo la información se recolecta en Excel o CSV. Importar esto manualmente a MATLAB para análisis complejos puede ser tedioso y propenso a errores de formato. MatMaker elimina este paso intermedio: 

1. Escribe el nombre del archivo (ej. `exp_0126_dis_d1r1`).
2. Pega directamente los datos tabulares.
3. Guarda el archivo `.mat` al instante.

## Características
- **Cero latencia:** Procesa cientos de filas instantáneamente leyendo el portapapeles en crudo (tabuladores de Excel).
- **Compatibilidad Caja Valentia:** Exige exactamente 8 columnas y guarda una única matriz numérica `double` llamada `Resultados`, igual que los archivos MATLAB originales.
- **Validación antes de guardar:** Detecta encabezados, celdas vacías, filas incompletas y números no válidos antes de crear el archivo.
- **Portabilidad:** Avalonia UI permite que la misma aplicación funcione en macOS y Windows, sin requerir MATLAB.

## Descarga y Uso
No es necesario compilar el código. Simplemente dirígete a la sección de **[Releases]** en este repositorio y descarga el archivo `MatMaker.exe`.

## Tecnologías
- **C# / .NET 10 + Avalonia UI:** Interfaz de escritorio multiplataforma.
- **MathNet.Numerics:** Manipulación y exportación de matrices estandarizadas.

## Archivo Paralelo En Drive

El snapshot usado como evidencia tutoral se conserva en:

```text
/Users/ab/Library/CloudStorage/GoogleDrive-jasjabs19@gmail.com/My Drive/workspace/04_doctorado/02_evaluacion-tutoral/01_sources/04_repos_del_proyecto/03_MatMaker-main
```

Es una copia histórica de referencia. El trabajo activo y las correcciones se
hacen únicamente en este repositorio.
