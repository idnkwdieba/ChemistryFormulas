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
    /// Типы символов.
    /// </summary>
    enum SymbolType
    {
        OpenParenthesis, CloseParenthesis, Digit, Upper, Other
    }

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

        var chemicalElems = new Dictionary<string, int>();
        var result = new StringBuilder();

        while (_length > 0)
        {
            GetChemicalElemData().ToList().ForEach
            (
                elem =>
                {
                    // Если такой элемент уже существует в словаре.
                    if (!chemicalElems.TryAdd(elem.Key, elem.Value))
                    {
                        // Увеличиваем счетчик таких элементов.
                        chemicalElems[elem.Key] += elem.Value;
                    }
                }
            );
        }

        var remainingCommaCount = chemicalElems.Count - 1;

        chemicalElems.ToList().ForEach
        (
            elem =>
            {
                result.Append($"{elem.Key}:{elem.Value}");

                if (remainingCommaCount > 0)
                {
                    result.Append(',');
                    remainingCommaCount--;
                }
            }
        );

        return result.ToString();
    }

    /// <summary>
    /// Возвращает тип символа.
    /// </summary>
    /// <param name="symbol">Символ для определения типа.</param>
    /// <returns>Тип символа.</returns>
    private static SymbolType GetSymbolType(char symbol)
    {
        if (symbol.Equals('('))
        {
            return SymbolType.OpenParenthesis;
        }

        if (symbol.Equals(')'))
        {
            return SymbolType.CloseParenthesis;
        }

        if (char.IsDigit(symbol))
        {
            return SymbolType.Digit;
        }

        if (char.IsUpper(symbol))
        {
            return SymbolType.Upper;
        }

        return SymbolType.Other;
    }

    /// <summary>
    /// Возвращает индекс следующего химического элемента в формуле.
    /// </summary>
    /// <returns>Индекс следующего химического элемента в формуле.</returns>
    private static int GetChemicalElemEndIndex()
    {
        var chemicalElemEndIndex = 0;
        var currentSymbol = _formula[0];
        var breakFlag = false;

        var isParenthesesBlockClosed = !currentSymbol.Equals('(');

        while (chemicalElemEndIndex < _length)
        {
            currentSymbol = _formula[chemicalElemEndIndex];

            switch (GetSymbolType(currentSymbol))
            {
                case SymbolType.OpenParenthesis:
                case SymbolType.Upper:
                    breakFlag = chemicalElemEndIndex != 0 && isParenthesesBlockClosed;
                    break;
                case SymbolType.Digit:
                    breakFlag = isParenthesesBlockClosed;
                    chemicalElemEndIndex += Convert.ToInt32(breakFlag);
                    break;
                case SymbolType.CloseParenthesis:
                    isParenthesesBlockClosed = true;
                    break;
                default:
                    break;
            }

            if (breakFlag)
            {
                break;
            }

            chemicalElemEndIndex++;
        }

        return chemicalElemEndIndex;
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

        string curChemicalElem;

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
    /// Возвращает химические элементы и их количество.
    /// </summary>
    /// <returns>Данные о химических элементах.</returns>
    private static Dictionary<string, int> GetChemicalElemData()
    {
        var chemicalFormulaPiece = GetCurrentChemicalFormulaPiece();

        // Если химические элементы обернуты в скобки.
        if (chemicalFormulaPiece[0] == '(')
        {
            return GetChemicalElemsParenthesesData(ref chemicalFormulaPiece);
        }

        var numberIndex = GetNumberIndex(chemicalFormulaPiece);
        var chemicalElems = new Dictionary<string, int>();

        // Если после химического элемента не следует число.
        if (numberIndex == chemicalFormulaPiece.Length)
        {
            chemicalElems.Add(chemicalFormulaPiece, 1);

            return chemicalElems;
        }

        chemicalElems.Add(
            chemicalFormulaPiece.Substring(0, numberIndex),
            Convert.ToInt32(chemicalFormulaPiece[^1].ToString()));

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
        var length = chemicalFormulaPiece.Length;
        var originalFormula = _formula;
        var numberIndex = GetNumberIndex(chemicalFormulaPiece);
        var chemicalElems = new Dictionary<string, int>();

        var chemicalElemsMultiplier =
            numberIndex == length
                ? 1
                : Convert.ToInt32(chemicalFormulaPiece[^1].ToString());

        // Временная замена химической формулы для работы с выражением в скобках.
        _formula = chemicalFormulaPiece.Substring(
            1,
            chemicalFormulaPiece.Length -
                numberIndex == length
                    ? 1
                    : 2);
        _length = _formula.Length;

        while (_length > 0)
        {
            foreach (var elem in GetChemicalElemData())
            {
                chemicalElems.Add(elem.Key, elem.Value * chemicalElemsMultiplier);
            }
        }

        _formula = originalFormula;
        _length = originalFormula.Length;

        return chemicalElems;
    }
}
