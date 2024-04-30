namespace ChemistryFormulas;

using System.Collections.Generic;
using System.Text;

/// <summary>
/// Парсинг химических формул.
/// </summary>
public class FormulasParsing
{
    /// <summary>
    /// Химическая формула.
    /// </summary>
    private static string _formula;

    /// <summary>
    /// Длина строки химической формулы.
    /// </summary>
    private static int _length;

    /// <summary>
    /// Возвращает результат парсинга химической формулы.
    /// </summary>
    /// <param name="formula">Химическая формула для парсинга.</param>
    /// <returns>Результат парсинга.</returns>
    public static string ParseFormula(string formula)
    {
        if (string.IsNullOrEmpty(formula))
        {
            return string.Empty;
        }

        _formula = formula;
        _length = formula.Length;

        var chemicalElems = GetChemicalElems();

        return GetChemicalElemsString(chemicalElems);
    }

    /// <summary>
    /// Возвращает строку с перечислением химических элементов.
    /// </summary>
    /// <param name="chemicalElems">Химические элементы.</param>
    /// <returns>Строку с перечислением химических элементов.</returns>
    private static string GetChemicalElemsString(Dictionary<string, int> chemicalElems)
    {
        var result = new StringBuilder();
        var remainingCommaCount = chemicalElems.Count - 1;

        foreach (var elem in chemicalElems)
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
    /// Возвращает химические элементы и их количество.
    /// </summary>
    /// <returns>Химические элементы и их количество.</returns>
    private static Dictionary<string, int> GetChemicalElems()
    {
        var result = new Dictionary<string, int>();

        while (_length > 0)
        {
            foreach (var chemicalElem in GetChemicalElemsData())
            {
                // Если такой элемент уже существует в словаре.
                if (!result.TryAdd(chemicalElem.Key, chemicalElem.Value))
                {
                    // Увеличиваем счетчик таких элементов.
                    result[chemicalElem.Key] += chemicalElem.Value;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Возвращает индекс следующего химического элемента в формуле.
    /// </summary>
    /// <returns>Индекс следующего химического элемента в формуле.</returns>
    private static int GetChemicalElemEndIndex()
    {
        var chemicalElemEndIndex = 0;
        var currentSymbol = _formula[0];
        var isParenthesesBlockClosed = currentSymbol != '(';

        while (chemicalElemEndIndex < _length)
        {
            currentSymbol = _formula[chemicalElemEndIndex];

            if (IsNewFormulaBlockStarted())
            {
                break;
            }

            if (IsFormulaBlockEndedWithDigit())
            {
                chemicalElemEndIndex++;
                break;
            }

            if (IsParenthesesClosed())
            {
                isParenthesesBlockClosed = true;
            }

            chemicalElemEndIndex++;
        }

        return chemicalElemEndIndex;

        bool IsNewFormulaBlockStarted()
        {
            return chemicalElemEndIndex != 0 && isParenthesesBlockClosed
                && (currentSymbol == '(' || char.IsUpper(currentSymbol));
        }

        bool IsFormulaBlockEndedWithDigit()
        {
            return char.IsDigit(currentSymbol) && isParenthesesBlockClosed;
        }

        bool IsParenthesesClosed()
        {
            return currentSymbol == ')';
        }
    }

    /// <summary>
    /// Возвращает индекс числа в химическом элементе.
    /// </summary>
    /// <param name="chemicalElemString">Строка с химическим элементом.</param>
    /// <returns>Индекс числа в химическом элементе.</returns>
    private static int GetNumberIndex(string chemicalElemString)
    {
        return chemicalElemString
            .ToArray()
            .Select((symbol, index) => new { index, symbol })
                .Where(elem => char.IsDigit(elem.symbol))
            .Select(elem => elem.index)
            .FirstOrDefault(chemicalElemString.Length);
    }

    /// <summary>
    /// Возвращает текущий химический элемент, усекает строку химической формулы.
    /// </summary>
    /// <returns>Текущий химический элемент.</returns>
    private static string GetCurrentChemicalFormulaPiece()
    {
        var chemicalElemEndIndex = GetChemicalElemEndIndex();

        var curChemicalElem = chemicalElemEndIndex == _length
            ? _formula
            : _formula.Substring(0, chemicalElemEndIndex);

        _formula = chemicalElemEndIndex >= _length
            ? string.Empty
            : _formula.Substring(chemicalElemEndIndex);

        _length = _formula.Length;

        return curChemicalElem;
    }

    /// <summary>
    /// Возвращает химические элементы и их количество.
    /// </summary>
    /// <returns>Данные о химических элементах.</returns>
    private static Dictionary<string, int> GetChemicalElemsData()
    {
        var chemicalFormulaPiece = GetCurrentChemicalFormulaPiece();

        if (chemicalFormulaPiece[0].Equals('('))
        {
            return GetChemicalElemsParenthesesData(ref chemicalFormulaPiece);
        }

        return GetChemicalElemData(chemicalFormulaPiece);
    }

    /// <summary>
    /// Возвращает данные о химическом элементе.
    /// </summary>
    /// <param name="chemicalElem">Часть химической формулы с элементом.</param>
    /// <returns>Данные о химическом элементе.</returns>
    private static Dictionary<string, int> GetChemicalElemData(string chemicalElem)
    {
        var numberIndex = GetNumberIndex(chemicalElem);
        var chemicalElems = new Dictionary<string, int>();

        if (numberIndex == chemicalElem.Length)
        {
            chemicalElems.Add(chemicalElem, 1);

            return chemicalElems;
        }

        chemicalElems.Add(
            chemicalElem.Substring(0, numberIndex),
            Convert.ToInt32(chemicalElem[^1].ToString()));

        return chemicalElems;
    }

    /// <summary>
    /// Возвращает химические элементы в скобках и их количество.
    /// </summary>
    /// <param name="chemicalFormulaPiece">Химическая формула.</param>
    /// <returns>Данные о химических элементах.</returns>
    private static Dictionary<string, int> GetChemicalElemsParenthesesData(
        ref string chemicalFormulaPiece)
    {
        var chemicalElems = new Dictionary<string, int>();

        var chemicalElemsMultiplier =
            GetNumberIndex(chemicalFormulaPiece) == chemicalFormulaPiece.Length
                ? 1
                : Convert.ToInt32(chemicalFormulaPiece[^1].ToString());

        var originalFormula = ReplaceMainFormula(chemicalFormulaPiece);

        while (_length > 0)
        {
            foreach (var elem in GetChemicalElemsData())
            {
                chemicalElems.Add(elem.Key, elem.Value * chemicalElemsMultiplier);
            }
        }

        _formula = originalFormula;
        _length = originalFormula.Length;

        return chemicalElems;
    }

    /// <summary>
    /// Заменяет основную химическую формулу на выражение в скобках.
    /// </summary>
    /// <param name="newFormula">Заменяющее выражение.</param>
    /// <returns>Исходную формулу.</returns>
    private static string ReplaceMainFormula(string newFormula)
    {
        var result = _formula;

        _formula = newFormula.Substring(
            1,
            newFormula.Length -
                GetNumberIndex(newFormula) == newFormula.Length
                    ? 1
                    : 2);
        _length = _formula.Length;

        return result;
    }
}