namespace ChemistryFormulas.Tests;

/// <summary>
/// Проверка парсинга химических формул.
/// </summary>
public class FormulasParsingTests
{
    /// <summary>
    /// Возвращает пустую строку при передаче пустой строки.
    /// </summary>
    [Fact]
    public void ParseFormula_EmptyString_EmptyString()
    {
        Assert.Empty(FormulasParsing.ParseFormula(string.Empty));
    }

    /// <summary>
    /// Возвращает количество химического элемента, равное единице, при передаче формулы
    /// с единственным химическим элементом.
    /// </summary>
    [Fact]
    public void ParseFormula_SingleElement_SingleElementCount()
    {
        Assert.Equal("H:1", FormulasParsing.ParseFormula("H"));
    }

    /// <summary>
    /// Возвращает количество химического элемента, равное двум, при передаче формулы
    /// с одним химическим элементом, количество которого равно двум.
    /// </summary>
    [Fact]
    public void ParseFormula_DoubleElement_ElementCountEqualsTwo()
    {
        Assert.Equal("H:2", FormulasParsing.ParseFormula("H2"));
    }

    /// <summary>
    /// Возвращает корректное количество химических элементов в формуле,
    /// в которой есть два разных химических элемента.
    /// </summary>
    [Fact]
    public void ParseFormula_MultipleElements_CorrectElemetsCount()
    {
        Assert.Equal("H:2,O:1", FormulasParsing.ParseFormula("H2O"));
    }

    /// <summary>
    /// Возвращает корректное количество химических элементов в формуле,
    /// в которой один и тот же химический элемент встречается несколько раз.
    /// </summary>
    [Fact]
    public void ParseFormula_ElementsMultipleOccurrences_CorrectElemetsCount()
    {
        Assert.Equal("C:2,H:6,O:1", FormulasParsing.ParseFormula("C2H5OH"));
    }
}