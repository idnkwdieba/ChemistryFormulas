namespace ChemistryFormulas.Tests;

/// <summary>
/// ѕроверка парсинга химических формул.
/// </summary>
public class FormulasParsingTests
{
    /// <summary>
    /// ¬озвращает пустую строку при передаче пустой строки.
    /// </summary>
    [Fact]
    public void ParseFormula_EmptyString_EmptyString()
    {
        Assert.Empty(FormulasParsing.ParseFormula(string.Empty));
    }
}