using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using MathNet.Numerics.Data.Matlab;
using MathNet.Numerics.LinearAlgebra;
using System.IO;

namespace MatMaker;

public partial class MainWindow : Window
{
    private const string MatlabVariableName = "Resultados";
    private readonly AppLocalizer _localizer;
    private bool _applyingLanguage;

    public MainWindow()
    {
        InitializeComponent();
        _localizer = AppLocalizer.Current;
        _localizer.LanguageChanged += Localizer_LanguageChanged;
        Closed += (_, _) => _localizer.LanguageChanged -= Localizer_LanguageChanged;
        ApplyLocalizedText();
        StatusText.Text = _localizer.Get(UiText.InitialStatus);
    }

    private void Localizer_LanguageChanged(object? sender, EventArgs eventArgs) => ApplyLocalizedText();

    private void ApplyLocalizedText()
    {
        SubtitleText.Text = _localizer.Get(UiText.Subtitle);
        FileNameLabelText.Text = _localizer.Get(UiText.FileName);
        FileNameTextBox.PlaceholderText = _localizer.Get(UiText.FileNamePlaceholder);
        FileNameHintText.Text = _localizer.Get(UiText.FileNameHint);
        MatlabVariableLabelText.Text = _localizer.Get(UiText.MatlabVariable);
        SameAsOriginalText.Text = _localizer.Get(UiText.SameAsOriginal);
        PasteTableLabelText.Text = _localizer.Get(UiText.PasteTable);
        PasteHintText.Text = _localizer.Get(UiText.PasteHint);
        DataTextBox.PlaceholderText = _localizer.Get(UiText.DataPlaceholder);
        ValidateButton.Content = _localizer.Get(UiText.Validate);
        SaveButton.Content = _localizer.Get(UiText.Save);

        _applyingLanguage = true;
        LanguageToggleButton.IsChecked = _localizer.CurrentLanguage == AppLanguage.English;
        _applyingLanguage = false;
        UpdateLanguageToggleAppearance();
    }

    private void LanguageToggleButton_IsCheckedChanged(object? sender, RoutedEventArgs eventArgs)
    {
        if (!_applyingLanguage)
        {
            _localizer.SetLanguage(LanguageToggleButton.IsChecked == true
                ? AppLanguage.English
                : AppLanguage.Spanish);
        }
    }

    private void UpdateLanguageToggleAppearance()
    {
        var englishSelected = _localizer.CurrentLanguage == AppLanguage.English;
        EnglishLanguageText.FontWeight = englishSelected ? FontWeight.Bold : FontWeight.Normal;
        SpanishLanguageText.FontWeight = englishSelected ? FontWeight.Normal : FontWeight.Bold;
        EnglishLanguageText.Foreground = new SolidColorBrush(Color.Parse(englishSelected ? "#0F6CBD" : "#6B7280"));
        SpanishLanguageText.Foreground = new SolidColorBrush(Color.Parse(englishSelected ? "#6B7280" : "#0F6CBD"));
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
                StatusText.Text = _localizer.Get(UiText.CannotSavePicker);
                return;
            }

            var destination = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = _localizer.Get(UiText.SavePickerTitle),
                SuggestedFileName = $"{fileName}.mat",
                DefaultExtension = "mat",
                FileTypeChoices =
                [
                    new FilePickerFileType(_localizer.Get(UiText.MatFile)) { Patterns = ["*.mat"] }
                ],
                ShowOverwritePrompt = true
            });

            if (destination is null)
            {
                return;
            }

            StatusText.Text = _localizer.Get(UiText.Saving);
            await using (var stream = await destination.OpenWriteAsync())
            {
                await Task.Run(() => MatlabWriter.Write(stream, result.Matrix, MatlabVariableName));
            }

            await VerifySavedMatrixAsync(destination, result.Matrix);
            StatusText.Text = _localizer.Format(
                UiText.SavedAndVerified,
                destination.Name,
                result.Matrix.RowCount);
        }
        catch (Exception exception)
        {
            StatusText.Text = _localizer.Format(UiText.SaveFailed, exception.Message);
        }
    }

    private void ShowValidation(PasteParseResult result)
    {
        var headerMessage = result.HeaderDiscarded ? _localizer.Get(UiText.HeaderDiscarded) : string.Empty;
        var emptyMessage = result.EmptyCellCount > 0
            ? _localizer.Format(UiText.EmptyCells, result.EmptyCellCount)
            : string.Empty;
        StatusText.Text = $"{_localizer.Format(UiText.ValidTable, result.Matrix.RowCount)}{headerMessage}{emptyMessage}";
    }

    private static string NormalizeFileName(string? value)
    {
        var name = Path.GetFileNameWithoutExtension((value ?? string.Empty).Trim());
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(AppLocalizer.Current.Get(UiText.MissingFileName));
        }

        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException(AppLocalizer.Current.Get(UiText.InvalidFileName));
        }

        return name;
    }

    private static async Task VerifySavedMatrixAsync(IStorageFile file, Matrix<double> expected)
    {
        await using var stream = await file.OpenReadAsync();
        await Task.Run(() => MatFileVerifier.ReadAndVerify(stream, expected, MatlabVariableName));
    }
}
