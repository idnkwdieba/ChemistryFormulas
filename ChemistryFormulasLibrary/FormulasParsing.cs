namespace ChemistryFormulas;

using System;
using System.Text;

/// <summary>
/// Парсинг химических формул.
/// </summary>
public class FormulasParsing
{
    /// <summary>
    /// Возвращает результат парсинга химической формулы.
    /// </summary>
    /// <param name="formula">Химическая формула для парсинга.</param>
    /// <returns>Результат парсинга.</returns>
    public static string ParseFormula(string formula)
    {
        if (string.IsNullOrEmpty(formula))
        {
            return "";
        }

        var length = formula.Length;
        var numberIndex = 0;

        for (numberIndex = 0; numberIndex < length; numberIndex++)
        {
            if (char.IsDigit(formula[numberIndex]))
            {
                break;
            }
        }

        return
            new StringBuilder()
                .Append(formula.Substring(0, numberIndex))
                .Append(':')
                .Append(
                    numberIndex == length
                        ? '1'
                        : formula.Substring(numberIndex))
                .ToString();
    }
}
