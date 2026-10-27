using Avalonia;

namespace MatMaker;

internal enum AppLanguage
{
    English,
    Spanish
}

internal enum UiText
{
    Subtitle,
    FileName,
    FileNamePlaceholder,
    FileNameHint,
    MatlabVariable,
    SameAsOriginal,
    PasteTable,
    PasteHint,
    DataPlaceholder,
    InitialStatus,
    Validate,
    Save,
    CannotSavePicker,
    SavePickerTitle,
    MatFile,
    Saving,
    SavedAndVerified,
    SaveFailed,
    ValidTable,
    HeaderDiscarded,
    EmptyCells,
    NoData,
    MixedFirstRow,
    HeadersOnly,
    WrongColumnCount,
    InvalidCell,
    MissingFileName,
    InvalidFileName,
    VerificationVariable,
    VerificationDimensions,
    VerificationValue
}

internal sealed class AppLocalizer
{
    private static readonly AppLocalizer Fallback = new();

    private static readonly IReadOnlyDictionary<UiText, string> English = new Dictionary<UiText, string>
    {
        [UiText.Subtitle] = "Convert a table pasted from Excel into a compatible MATLAB file.",
        [UiText.FileName] = "File name",
        [UiText.FileNamePlaceholder] = "Example: exp_0526_dis_d4r4",
        [UiText.FileNameHint] = "It will be saved with the .mat extension; this name identifies the rat during automation.",
        [UiText.MatlabVariable] = "MATLAB variable",
        [UiText.SameAsOriginal] = "Same as the original .mat files",
        [UiText.PasteTable] = "Paste the Excel table",
        [UiText.PasteHint] = "One header row is accepted. The table must have exactly eight numeric columns.",
        [UiText.DataPlaceholder] = "Copy the eight columns from Excel and paste them here.",
        [UiText.InitialStatus] = "Paste an eight-column table and validate it before saving.",
        [UiText.Validate] = "Validate table",
        [UiText.Save] = "Save .mat file…",
        [UiText.CannotSavePicker] = "This system cannot choose where to save the file.",
        [UiText.SavePickerTitle] = "Save compatible MATLAB file",
        [UiText.MatFile] = "MATLAB file",
        [UiText.Saving] = "Saving and verifying the .mat file…",
        [UiText.SavedAndVerified] = "Saved and verified: {0} ({1} × 8, double, variable Resultados).",
        [UiText.SaveFailed] = "Could not save the file: {0}",
        [UiText.ValidTable] = "Valid table: {0} rows × 8 columns, double.",
        [UiText.HeaderDiscarded] = " The header row was discarded.",
        [UiText.EmptyCells] = " {0} empty cell(s) will be saved as NaN.",
        [UiText.NoData] = "There is no data. Copy the table from Excel and paste it here.",
        [UiText.MixedFirstRow] = "The first row mixes text and numbers. Fix the table or paste a complete header followed by the data.",
        [UiText.HeadersOnly] = "The table contains only headers; data rows are missing.",
        [UiText.WrongColumnCount] = "Row {0} has {1} columns. The automation requires exactly 8.",
        [UiText.InvalidCell] = "The cell in row {0}, column {1} does not contain a valid number: “{2}”.",
        [UiText.MissingFileName] = "Enter a file name before saving.",
        [UiText.InvalidFileName] = "The file name contains unsupported characters.",
        [UiText.VerificationVariable] = "Verification failed: the file must contain only the Resultados variable.",
        [UiText.VerificationDimensions] = "Verification failed: the saved matrix does not preserve its dimensions.",
        [UiText.VerificationValue] = "Verification failed: the value in row {0}, column {1} changed."
    };

    private static readonly IReadOnlyDictionary<UiText, string> Spanish = new Dictionary<UiText, string>
    {
        [UiText.Subtitle] = "Convierte una tabla pegada desde Excel a un archivo MATLAB compatible.",
        [UiText.FileName] = "Nombre del archivo",
        [UiText.FileNamePlaceholder] = "Ejemplo: exp_0526_dis_d4r4",
        [UiText.FileNameHint] = "Se guardará con extensión .mat y ese nombre identificará a la rata en la automatización.",
        [UiText.MatlabVariable] = "Variable MATLAB",
        [UiText.SameAsOriginal] = "Igual que los .mat originales",
        [UiText.PasteTable] = "Pega la tabla de Excel",
        [UiText.PasteHint] = "Se acepta una fila de encabezados. La tabla debe tener exactamente ocho columnas numéricas.",
        [UiText.DataPlaceholder] = "Copia las ocho columnas desde Excel y pégalas aquí.",
        [UiText.InitialStatus] = "Pega una tabla de ocho columnas para validarla antes de guardarla.",
        [UiText.Validate] = "Validar tabla",
        [UiText.Save] = "Guardar archivo .mat…",
        [UiText.CannotSavePicker] = "Este sistema no permite elegir dónde guardar el archivo.",
        [UiText.SavePickerTitle] = "Guardar archivo MATLAB compatible",
        [UiText.MatFile] = "Archivo MATLAB",
        [UiText.Saving] = "Guardando y verificando el archivo .mat…",
        [UiText.SavedAndVerified] = "Guardado y verificado: {0} ({1} × 8, double, variable Resultados).",
        [UiText.SaveFailed] = "No se pudo guardar el archivo: {0}",
        [UiText.ValidTable] = "Tabla válida: {0} filas × 8 columnas, tipo double.",
        [UiText.HeaderDiscarded] = " Se descartó la fila de encabezados.",
        [UiText.EmptyCells] = " {0} celda(s) vacía(s) se guardarán como NaN.",
        [UiText.NoData] = "No hay datos. Copia la tabla desde Excel y pégala aquí.",
        [UiText.MixedFirstRow] = "La primera fila mezcla texto y números. Corrige la tabla o pega un encabezado completo seguido de los datos.",
        [UiText.HeadersOnly] = "La tabla sólo contiene encabezados; faltan las filas de datos.",
        [UiText.WrongColumnCount] = "La fila {0} tiene {1} columnas. La automatización requiere exactamente 8.",
        [UiText.InvalidCell] = "La celda de fila {0}, columna {1} no contiene un número válido: “{2}”.",
        [UiText.MissingFileName] = "Escribe un nombre para el archivo antes de guardarlo.",
        [UiText.InvalidFileName] = "El nombre del archivo contiene caracteres no permitidos.",
        [UiText.VerificationVariable] = "La comprobación falló: el archivo debe contener únicamente la variable Resultados.",
        [UiText.VerificationDimensions] = "La comprobación falló: la matriz guardada no conserva sus dimensiones.",
        [UiText.VerificationValue] = "La comprobación falló: cambió el valor de la fila {0}, columna {1}."
    };

    public AppLanguage CurrentLanguage { get; private set; } = AppLanguage.English;

    public event EventHandler? LanguageChanged;

    public static AppLocalizer Current => (Application.Current as App)?.Localizer ?? Fallback;

    public string Get(UiText key) => (CurrentLanguage == AppLanguage.English ? English : Spanish)[key];

    public string Format(UiText key, params object[] values) => string.Format(Get(key), values);

    public void SetLanguage(AppLanguage language)
    {
        if (CurrentLanguage == language)
        {
            return;
        }

        CurrentLanguage = language;
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }
}
