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
            return "";
        }

        _formula = formula;
        _length = formula.Length;

        var parseResult = new Dictionary<string, int>();

        var result = new StringBuilder();

        var remainingCommaCount = 0;

        while (_length > 0)
        {
            foreach (var chemicalElem in GetChemicalElemData())
            {
                // Если такой элемент уже существует в словаре.
                if (!parseResult.TryAdd(chemicalElem.Key, chemicalElem.Value))
                {
                    // Увеличиваем счетчик таких элементов.
                    parseResult[chemicalElem.Key] += chemicalElem.Value;
                }
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

        var isParenthesesBlockClosed =
            _formula[0] == '('
                ? false
                : true;

        while (chemicalElemEndIndex < _length)
        {
            if (chemicalElemEndIndex != 0
                && (char.IsUpper(_formula[chemicalElemEndIndex]) || _formula[chemicalElemEndIndex] == '('))
            {
                if (isParenthesesBlockClosed)
                {
                    break;
                }
            }

            if (char.IsDigit(_formula[chemicalElemEndIndex]))
            {
                if (isParenthesesBlockClosed)
                {
                    chemicalElemEndIndex++;
                    break;
                }
            }

            if (_formula[chemicalElemEndIndex] == ')')
            {
                isParenthesesBlockClosed = true;
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
    private static string GetCurrentChemicalFormulaPiece()
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
    /// Возвращает химические элементы и их количество.
    /// </summary>
    /// <returns>Данные о химических элементах.</returns>
    private static Dictionary<string, int> GetChemicalElemData()
    {
        var chemicalFormulaPiece = GetCurrentChemicalFormulaPiece();

        var numberIndex = GetNumberIndex(chemicalFormulaPiece);
        var length = chemicalFormulaPiece.Length;

        var chemicalElems = new Dictionary<string, int>();

        var chemicalElemsMultiplier =
            numberIndex == length
                ? 1
                : Convert.ToInt32(chemicalFormulaPiece[^1].ToString());

        var isParenthesesBlockPresent =
            chemicalFormulaPiece[0] == '('
                ? true
                : false;

        if (!isParenthesesBlockPresent)
        {
            chemicalElems.Add(
                numberIndex == length
                    ? chemicalFormulaPiece
                    : chemicalFormulaPiece.Substring(0, numberIndex),
                chemicalElemsMultiplier);

            return chemicalElems;
        }

        var originalFormula = _formula;

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
            chemicalFormulaPiece = GetCurrentChemicalFormulaPiece();

            numberIndex = GetNumberIndex(chemicalFormulaPiece);
            length = chemicalFormulaPiece.Length;

            if (numberIndex == length)
            {
                chemicalElems.Add(chemicalFormulaPiece, chemicalElemsMultiplier);
            }
            else
            {
                chemicalElems.Add(
                    chemicalFormulaPiece.Substring(0, numberIndex),
                    Convert.ToInt32(chemicalFormulaPiece[^1].ToString())
                        * chemicalElemsMultiplier);
            }
        }

        _formula = originalFormula;
        _length = originalFormula.Length;

        return chemicalElems;
    }
}
