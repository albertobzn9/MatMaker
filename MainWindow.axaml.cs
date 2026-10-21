using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MathNet.Numerics.Data.Matlab;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

namespace MatMaker;

public partial class MainWindow : Window
{
    private const string MatlabVariableName = "Resultados";

    public MainWindow()
    {
        InitializeComponent();
        StatusText.Text = "Pega una tabla de ocho columnas para validarla antes de guardarla.";
    }

    private void ValidateButton_Click(object? sender, RoutedEventArgs eventArgs)
    {
        try
        {
            var result = PastedMatrixParser.Parse(DataTextBox.Text ?? string.Empty);
            ShowValidation(result);
        }
        catch (Exception exception)
        {
            StatusText.Text = exception.Message;
        }
    }

    private async void SaveButton_Click(object? sender, RoutedEventArgs eventArgs)
    {
        try
        {
            var fileName = NormalizeFileName(FileNameTextBox.Text);
            var result = PastedMatrixParser.Parse(DataTextBox.Text ?? string.Empty);

            if (!StorageProvider.CanSave)
            {
                StatusText.Text = "Este sistema no permite elegir dónde guardar el archivo.";
                return;
            }

            var destination = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Guardar archivo MATLAB compatible",
                SuggestedFileName = $"{fileName}.mat",
                DefaultExtension = "mat",
                FileTypeChoices =
                [
                    new FilePickerFileType("Archivo MATLAB") { Patterns = ["*.mat"] }
                ],
                ShowOverwritePrompt = true
            });

            if (destination is null)
            {
                return;
            }

            StatusText.Text = "Guardando y verificando el archivo .mat…";
            await using (var stream = await destination.OpenWriteAsync())
            {
                await Task.Run(() => MatlabWriter.Write(stream, result.Matrix, MatlabVariableName));
            }

            await VerifySavedMatrixAsync(destination, result.Matrix);
            StatusText.Text = $"Guardado y verificado: {destination.Name} ({result.Matrix.RowCount} × 8, double, variable Resultados).";
        }
        catch (Exception exception)
        {
            StatusText.Text = $"No se pudo guardar el archivo: {exception.Message}";
        }
    }

    private void ShowValidation(PasteParseResult result)
    {
        var headerMessage = result.HeaderDiscarded ? " Se descartó la fila de encabezados." : string.Empty;
        var emptyMessage = result.EmptyCellCount > 0
            ? $" {result.EmptyCellCount} celda(s) vacía(s) se guardarán como NaN."
            : string.Empty;
        StatusText.Text = $"Tabla válida: {result.Matrix.RowCount} filas × 8 columnas, tipo double.{headerMessage}{emptyMessage}";
    }

    private static string NormalizeFileName(string? value)
    {
        var name = Path.GetFileNameWithoutExtension((value ?? string.Empty).Trim());
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Escribe un nombre para el archivo antes de guardarlo.");
        }

        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("El nombre del archivo contiene caracteres no permitidos.");
        }

        return name;
    }

    private static async Task VerifySavedMatrixAsync(IStorageFile file, Matrix<double> expected)
    {
        await using var stream = await file.OpenReadAsync();
        await Task.Run(() => MatFileVerifier.ReadAndVerify(stream, expected, MatlabVariableName));
    }
}
