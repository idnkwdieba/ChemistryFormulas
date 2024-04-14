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
}