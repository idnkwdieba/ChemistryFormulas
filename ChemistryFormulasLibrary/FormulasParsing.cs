namespace ChemistryFormulas;

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

        return $"{formula}:1";
    }
}
