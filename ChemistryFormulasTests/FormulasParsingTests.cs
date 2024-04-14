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

    /// <summary>
    /// ¬озвращает количество химического элемента, равное единице, при передаче формулы
    /// с единственным химическим элементом.
    /// </summary>
    [Fact]
    public void ParseFormula_SingleElement_SingleElementCount()
    {
        Assert.Equal("H:1", FormulasParsing.ParseFormula("H"));
    }
}