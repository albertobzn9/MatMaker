using MathNet.Numerics.Data.Matlab;
using MathNet.Numerics.LinearAlgebra;

namespace MatMaker;

internal static class MatFileVerifier
{
    public static void ReadAndVerify(Stream stream, Matrix<double> expected, string variableName)
    {
        var matrices = MatlabReader.List(stream)
            .Where(item => !item.Name.StartsWith("__", StringComparison.Ordinal))
            .ToList();

        if (matrices.Count != 1 || matrices[0].Name != variableName)
        {
            throw new InvalidDataException($"La comprobación falló: el archivo debe contener únicamente la variable {variableName}.");
        }

        var actual = MatlabReader.Unpack<double>(matrices[0]);
        if (actual.RowCount != expected.RowCount || actual.ColumnCount != expected.ColumnCount)
        {
            throw new InvalidDataException("La comprobación falló: la matriz guardada no conserva sus dimensiones.");
        }

        for (var row = 0; row < expected.RowCount; row++)
        {
            for (var column = 0; column < expected.ColumnCount; column++)
            {
                var expectedValue = expected[row, column];
                var actualValue = actual[row, column];
                if (double.IsNaN(expectedValue) && double.IsNaN(actualValue))
                {
                    continue;
                }

                if (expectedValue != actualValue)
                {
                    throw new InvalidDataException($"La comprobación falló: cambió el valor de la fila {row + 1}, columna {column + 1}.");
                }
            }
        }
    }
}
