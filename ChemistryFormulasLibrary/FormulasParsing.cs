namespace ChemistryFormulas;

using System.Collections.Generic;
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

        var parseResult = new Dictionary<string, int>();

        var curChemicalElem = new Dictionary<string, int>();
        string curChemicalElemKey;

        var result = new StringBuilder();

        var remainingCommaCount = 0;

        if (string.IsNullOrEmpty(_formula))
        {
            return "";
        }

        _length = _formula.Length;

        while (_length > 0)
        {
            curChemicalElem = GetChemicalElemData();
            curChemicalElemKey = curChemicalElem.Keys.First();

            // Если такой элемент уже существует в словаре.
            if (!parseResult.TryAdd(curChemicalElemKey, curChemicalElem[curChemicalElemKey]))
            {
                // Увеличиваем счетчик таких элементов.
                parseResult[curChemicalElemKey] += curChemicalElem[curChemicalElemKey];
            }
        }

        remainingCommaCount = parseResult.Count - 1;

        foreach (var elem in parseResult)
        {
            result.Append($"{elem.Key}:{elem.Value}");

            if (remainingCommaCount > 0)
            {
                result.Append(',');
                remainingCommaCount--;
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Возвращает индекс следующего химического элемента в формуле.
    /// </summary>
    /// <returns>Индекс следующего химического элемента в формуле.</returns>
    private static int GetChemicalElemEndIndex()
    {
        var chemicalElemEndIndex = 0;

        while (chemicalElemEndIndex < _length)
        {
            if (chemicalElemEndIndex != 0 && char.IsUpper(_formula[chemicalElemEndIndex]))
            {
                break;
            }

            if (char.IsDigit(_formula[chemicalElemEndIndex]))
            {
                chemicalElemEndIndex++;
                break;
            }

            chemicalElemEndIndex++;
        }

        return chemicalElemEndIndex;
    }

    /// <summary>
    /// Возвращает индекс числа в химическом элементе.
    /// </summary>
    /// <returns>Индекс числа в химическом элементе.</returns>
    private static int GetNumberIndex(string chemicalElemString)
    {
        var numberIndex = 0;
        var length = chemicalElemString.Length;

        while (numberIndex < length)
        {
            if (char.IsDigit(chemicalElemString[numberIndex]))
            {
                break;
            }

            numberIndex++;
        }

        return numberIndex;
    }

    /// <summary>
    /// Возвращает текущий химический элемент, усекает строку химической формулы.
    /// </summary>
    /// <returns>Текущий химический элемент.</returns>
    private static string GetCurrentElem()
    {
        var chemicalElemEndIndex = GetChemicalElemEndIndex();

        string curChemicalElem;
        string curChemicalElemCount;

        curChemicalElem =
            chemicalElemEndIndex == _length
                ? _formula
                : _formula.Substring(0, chemicalElemEndIndex);

        _formula =
            chemicalElemEndIndex >= _length
                ? string.Empty
                : _formula.Substring(chemicalElemEndIndex);

        _length = _formula.Length;

        return curChemicalElem;
    }

    /// <summary>
    /// Возвращает данные о текущем химическом элементе.
    /// </summary>
    /// <returns>Данные о химическом элементе.</returns>
    private static Dictionary<string, int> GetChemicalElemData()
    {
        var chemicalElem = GetCurrentElem();

        var numberIndex = GetNumberIndex(chemicalElem);
        var length = chemicalElem.Length;

        var elem = new Dictionary<string, int>();

        if (numberIndex == length)
        {
            elem.Add(chemicalElem, 1);
        }
        else
        {
            elem.Add(chemicalElem.Substring(0, numberIndex), Convert.ToInt32(chemicalElem[^1].ToString()));
        }

        return elem;
    }
}
