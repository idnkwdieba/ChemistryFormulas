namespace ChemistryFormulas.Tests;

/// <summary>
/// �������� �������� ���������� ������.
/// </summary>
public class FormulasParsingTests
{
    /// <summary>
    /// ���������� ������ ������ ��� �������� ������ ������.
    /// </summary>
    [Fact]
    public void ParseFormula_EmptyString_EmptyString()
    {
        Assert.Empty(FormulasParsing.ParseFormula(string.Empty));
    }

    /// <summary>
    /// ���������� ���������� ����������� ��������, ������ �������, ��� �������� �������
    /// � ������������ ���������� ���������.
    /// </summary>
    [Fact]
    public void ParseFormula_SingleElement_SingleElementCount()
    {
        Assert.Equal("H:1", FormulasParsing.ParseFormula("H"));
    }

    /// <summary>
    /// ���������� ���������� ����������� ��������, ������ ����, ��� �������� �������
    /// � ����� ���������� ���������, ���������� �������� ����� ����.
    /// </summary>
    [Fact]
    public void ParseFormula_DoubleElement_ElementCountEqualsTwo()
    {
        Assert.Equal("H:2", FormulasParsing.ParseFormula("H2"));
    }

    /// <summary>
    /// ���������� ���������� ���������� ���������� ��������� � �������,
    /// � ������� ���� ��� ������ ���������� ��������.
    /// </summary>
    [Fact]
    public void ParseFormula_MultipleElements_CorrectElemetsCount()
    {
        Assert.Equal("H:2,O:1", FormulasParsing.ParseFormula("H2O"));
    }

    /// <summary>
    /// ���������� ���������� ���������� ���������� ��������� � �������,
    /// � ������� ���� � ��� �� ���������� ������� ����������� ��������� ���.
    /// </summary>
    [Fact]
    public void ParseFormula_ElementsMultipleOccurrences_CorrectElemetsCount()
    {
        Assert.Equal("C:2,H:6,O:1", FormulasParsing.ParseFormula("C2H5OH"));
    }
}