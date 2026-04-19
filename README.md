# MatMaker

Una herramienta de escritorio rápida y ligera diseñada para convertir bloques de datos copiados de Excel directamente a archivos `.mat` nativos de MATLAB.

## ¿Para qué sirve?
En el flujo de trabajo de análisis de datos, a menudo la información se recolecta en Excel o CSV. Importar esto manualmente a MATLAB para análisis complejos puede ser tedioso y propenso a errores de formato. MatMaker elimina este paso intermedio: 

1. Escribe el nombre de tu variable (ej. `exp_0126_dis_d1r1`).
2. Pega directamente los datos tabulares.
3. Guarda el archivo `.mat` al instante.

## Características
- **Cero latencia:** Procesa cientos de filas instantáneamente leyendo el portapapeles en crudo (tabuladores de Excel).
- **Validación de variables:** Evita archivos corruptos asegurando que la nomenclatura cumpla las reglas estrictas de MATLAB.
- **Portabilidad Absoluta (Self-Contained):** El ejecutable no requiere conexión a internet, ni instalaciones previas de Python, MATLAB o el runtime de .NET. Funciona en cualquier equipo Windows.

## Descarga y Uso
No es necesario compilar el código. Simplemente dirígete a la sección de **[Releases]** en este repositorio y descarga el archivo `MatMaker.exe`.

## Tecnologías
- **C# / .NET 10 (WinForms):** Interfaz nativa y compilación AOT.
- **MathNet.Numerics:** Manipulación y exportación de matrices estandarizadas.
