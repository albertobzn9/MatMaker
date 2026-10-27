using MathNet.Numerics.LinearAlgebra;
using System.Globalization;

namespace MatMaker;

internal sealed record PasteParseResult(Matrix<double> Matrix, bool HeaderDiscarded, int EmptyCellCount);

internal static class PastedMatrixParser
{
    private const int RequiredColumnCount = 8;
    private static readonly CultureInfo CommaDecimalCulture = CultureInfo.GetCultureInfo("es-ES");

    public static PasteParseResult Parse(string rawText)
    {
        var lines = rawText.Replace("\r\n", "\n").Replace('\r', '\n')
            .Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        if (lines.Count == 0)
        {
            throw new ArgumentException(AppLocalizer.Current.Get(UiText.NoData));
        }

        var firstRow = SplitRow(lines[0]);
        var headerDiscarded = IsHeader(firstRow);
        if (!headerDiscarded && firstRow.Any(cell => !string.IsNullOrWhiteSpace(cell) && !TryParseFiniteNumber(cell, out _)))
        {
            throw new ArgumentException(AppLocalizer.Current.Get(UiText.MixedFirstRow));
        }

        var firstDataLine = headerDiscarded ? 1 : 0;
        if (firstDataLine == lines.Count)
        {
            throw new ArgumentException(AppLocalizer.Current.Get(UiText.HeadersOnly));
        }

        var rows = new List<double[]>();
        var emptyCellCount = 0;
        for (var lineIndex = firstDataLine; lineIndex < lines.Count; lineIndex++)
        {
            var cells = SplitRow(lines[lineIndex]);
            if (cells.Length != RequiredColumnCount)
            {
                throw new ArgumentException(AppLocalizer.Current.Format(UiText.WrongColumnCount, lineIndex + 1, cells.Length));
            }

            var row = new double[RequiredColumnCount];
            for (var column = 0; column < RequiredColumnCount; column++)
            {
                var cell = cells[column].Trim();
                if (cell.Length == 0)
                {
                    row[column] = double.NaN;
                    emptyCellCount++;
                    continue;
                }

                if (!TryParseFiniteNumber(cell, out var value))
                {
                    throw new ArgumentException(AppLocalizer.Current.Format(UiText.InvalidCell, lineIndex + 1, column + 1, cell));
                }

                row[column] = value;
            }

            rows.Add(row);
        }

        return new PasteParseResult(
            Matrix<double>.Build.DenseOfRowArrays(rows),
            headerDiscarded,
            emptyCellCount);
    }

    private static string[] SplitRow(string row) => row.Split('\t', StringSplitOptions.None);

    private static bool IsHeader(IReadOnlyList<string> cells) =>
        cells.Count == RequiredColumnCount &&
        cells.All(cell => string.IsNullOrWhiteSpace(cell) || !TryParseFiniteNumber(cell.Trim(), out _));

    private static bool TryParseFiniteNumber(string text, out double value)
    {
        var styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
        var success = double.TryParse(text, styles, CultureInfo.InvariantCulture, out value) ||
                      double.TryParse(text, styles, CommaDecimalCulture, out value);
        return success && double.IsFinite(value);
    }
}
