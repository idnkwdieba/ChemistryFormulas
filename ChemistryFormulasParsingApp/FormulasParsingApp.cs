namespace ChemistryFormulas;

/// <summary>
/// Консольное приложение, осуществляющее парсинг химических формул.
/// </summary>
internal class FormulasParsingApp
{
    /// <summary>
    /// Точка входа программы.
    /// </summary>
    /// <param name="args">Передаваемые аргументы.</param>
    static void Main(string[] args)
    {
        Console.WriteLine($"H20 => {FormulasParsing.ParseFormula("H2O")}");
        Console.WriteLine($"Cu(OH)2 => {FormulasParsing.ParseFormula("Cu(OH)2")}");
        Console.WriteLine($"C2H5OH => {FormulasParsing.ParseFormula("C2H5OH")}");
    }
}


