namespace ChemistryFormulas;

using System.Text;

/// <summary>
/// Парсинг химических формул.
/// </summary>
public class FormulasParsing
{
    private static string _formula;
    private static int _length;

    /// <summary>
    /// Возвращает результат парсинга химической формулы.
    /// </summary>
    /// <param name="formula">Химическая формула для парсинга.</param>
    /// <returns>Результат парсинга.</returns>
    public static string ParseFormula(string formula)
    {
        _formula = formula;

        if (string.IsNullOrEmpty(_formula))
        {
            return "";
        }

        _length = _formula.Length;
        var numberIndex = GetNumberIndex();

        return
            new StringBuilder()
                .Append(_formula.Substring(0, numberIndex))
                .Append(':')
                .Append(
                    numberIndex == _length
                        ? '1'
                        : _formula.Substring(numberIndex))
                .ToString();
    }

    /// <summary>
    /// Возвращает индекс следующего числа в формуле.
    /// </summary>
    /// <returns>Индекс следующего числа в формуле.</returns>
    private static int GetNumberIndex()
    {
        int numberIndex = 0;

        while (numberIndex < _length)
        {
            if (char.IsDigit(_formula[numberIndex]))
            {
                break;
            }

            numberIndex++;
        }

        return numberIndex;
    }
}
