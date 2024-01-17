using System.Reflection.Emit;

namespace calcAPI
{
    public class calculatorLogic
    {
        public static string hardExpression(ref string expression)
        {
            if (!expression.Contains('(') && !expression.Contains(')'))
            {
                return easyExpression(expression);
            }
            else
            {
                List<int> openingIndexes = new List<int>();
                int openingIndex = 0;
                List<int> closingIndexes = new List<int>();
                int closingIndex = 0;
                for (int i = 0; i < expression.Length; i++)
                {
                    if (expression[i] == '(')
                    {
                        openingIndexes.Insert(openingIndex++, i);
                    }
                    else if (expression[i] == ')')
                    {
                        closingIndexes.Insert(closingIndex++, i);
                    }
                }
                if (openingIndexes.Count!=closingIndexes.Count)
                {
                    return "Количество открывающих скобок не совпадает с количеством закрывающих";
                }
                for (int i = 0; i < openingIndex; i++)
                {
                    string sub = expression.Substring(openingIndexes[openingIndex - i - 1]);
                    string easySubExpression = sub.Substring(1, sub.IndexOf(')') - 1);
                    string firstPart = expression.Substring(0, openingIndexes[openingIndex - i - 1]);
                    string secondPart = expression.Substring(easySubExpression.Length + firstPart.Length + 2);
                    string expressionSolution = easyExpression(easySubExpression);
                    if (expressionSolution == "Неправильно использованы символы операций")
                    {
                        return expressionSolution;
                    }
                    else
                    {
                        expression = firstPart + expressionSolution + secondPart;
                    }
                }
                return easyExpression(expression);
            }
        }
        static string easyExpression(string expression)

        {
            int startOfDigit = 0;
            List<double> digits = new List<double>();
            List<int> indexes = new List<int>();
            Operation[] operations = new Operation[10];
            int indexOfDigit = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if ((expression[i] <= 57 && expression[i] >= 48) || (expression[i] == '.'))
                {
                    if (i == expression.Length - 1)
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit + 1)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit - 1].operand2 = indexOfDigit;
                        indexOfDigit++;
                    }
                }
                else
                {
                    if (expression[i] == '+')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('+', indexOfDigit, indexOfDigit, 2);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (expression[i] == '-')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('-', indexOfDigit, indexOfDigit, 2);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (expression[i] == '*')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('*', indexOfDigit, indexOfDigit, 1);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (expression[i] == '/')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('/', indexOfDigit, indexOfDigit, 1);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (expression[i] == '^')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('^', indexOfDigit, indexOfDigit, 0);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (expression[i] == 'k')
                    {
                        try
                        {
                            digits.Insert(indexOfDigit, double.Parse(expression.Substring(startOfDigit, i - startOfDigit)));
                        }
                        catch
                        {
                            return "Неправильно использованы символы операций";
                        }
                        operations[indexOfDigit] = new Operation('k', indexOfDigit, indexOfDigit, 0);
                        indexOfDigit++;
                        startOfDigit = i + 1;
                        if (i == expression.Length - 1)
                        {
                            return "Неправильно использованы символы операций";
                        }
                    }
                    if (indexOfDigit > 0)
                        operations[indexOfDigit - 1].operand2 = indexOfDigit;
                }

            }
            operations = operations.Where(c => c != null).ToArray();
            ShakerSort(operations);
            List<List<int>> tiedOperands = new List<List<int>>();
            int tiedIndex = 0;
            for (int i = 0; i < operations.Length; i++)
            {
                if (operations[i].op == '+')
                {
                    double digit = digits[operations[i].operand1] + digits[operations[i].operand2];
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
                if (operations[i].op == '-')
                {
                    double digit = digits[operations[i].operand1] - digits[operations[i].operand2];
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
                if (operations[i].op == '*')
                {
                    double digit = digits[operations[i].operand1] * digits[operations[i].operand2];
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
                if (operations[i].op == '/')
                {
                    double digit = digits[operations[i].operand1] / digits[operations[i].operand2];
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
                if (operations[i].op == '^')
                {
                    double digit = Math.Pow(digits[operations[i].operand1], digits[operations[i].operand2]);
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
                if (operations[i].op == 'k')
                {
                    double digit = Math.Pow(digits[operations[i].operand2], 1 / digits[operations[i].operand1]);
                    operationMaker(ref operations, ref tiedOperands, ref tiedIndex, ref digits, digit, i);
                }
            }
            return digits[0].ToString();
        }

        static void Swap(ref Operation e1, ref Operation e2)
        {
            var temp = e1;
            e1 = e2;
            e2 = temp;
        }
        static Operation[] ShakerSort(Operation[] array)
        {
            for (var i = 0; i < array.Length / 2; i++)
            {
                var swapFlag = false;
                //проход слева направо
                for (var j = i; j < array.Length - i - 1; j++)
                {
                    if (array[j].importance > array[j + 1].importance)
                    {
                        Swap(ref array[j], ref array[j + 1]);
                        swapFlag = true;
                    }
                }

                //проход справа налево
                for (var j = array.Length - 2 - i; j > i; j--)
                {
                    if (array[j - 1].importance > array[j].importance)
                    {
                        Swap(ref array[j - 1], ref array[j]);
                        swapFlag = true;
                    }
                }

                //если обменов не было выходим
                if (!swapFlag)
                {
                    break;
                }
            }

            return array;
        }
        static void operationMaker(ref Operation[] operations, ref List<List<int>> tiedOperands, ref int tiedIndex, ref List<double> digits, double digit, int i)
        {
            bool tied = false;
            for (int j = 0; j < tiedOperands.Count; j++)
            {
                if (tiedOperands[j].Contains(operations[i].operand1) && !tiedOperands[j].Contains(operations[i].operand2))
                {
                    tiedOperands[j].Add(operations[i].operand2);
                    tied = true;
                }
                if (!tiedOperands[j].Contains(operations[i].operand1) && tiedOperands[j].Contains(operations[i].operand2))
                {
                    tiedOperands[j].Add(operations[i].operand1);
                    tied = true;
                }
            }
            if (!tied)
            {
                tiedOperands.Add(new List<int>());
                tiedOperands[tiedIndex].Add(operations[i].operand1);
                tiedOperands[tiedIndex].Add(operations[i].operand2);
                tiedIndex++;
            }
            for (int k = 0; k < tiedOperands.Count; k++)
            {
                if (tiedOperands[k].Contains(operations[i].operand2) || tiedOperands[k].Contains(operations[i].operand1))
                {
                    foreach (int tiedOperation in tiedOperands[k])
                    {
                        digits[tiedOperation] = digit;
                    }
                }
            }
        }
        public class Operation
        {
            public char op;
            public int operand1, operand2, importance, order;
            public Operation(char op, int operand1, int order, int importance)
            {
                this.op = op;
                this.operand1 = operand1;
                this.order = order;
                this.importance = importance;
            }
        }
        public static string oneStepExpression(string strOperand1, string strOperand2, string operation)
        {
            foreach(char c in strOperand1)
            {
                if (!((c <= 57 && c >= 48) || c == '.'))
                {
                    return "Первый операнд содержит запрещенные символы";
                }
            }
            foreach (char c in strOperand2)
            {
                if (!((c <= 57 && c >= 48) || c == '.'))
                {
                    return "Второй операнд содержит запрещенные символы";
                }
            }
            if (operation!="+" && operation != "-" && operation != "*" && operation != "/" && operation != "^" && operation != "k")
            {
                return "Операция указана неверно";
            }
            double operand1 = double.Parse(strOperand1);
            double operand2 = double.Parse(strOperand2);
            string rezult = "";
            if (operation == "+")
            {
                double sum = operand1 + operand2;
                rezult = sum.ToString();
            }
            if (operation == "-")
            {
                double razn = operand1 - operand2;
                rezult = razn.ToString();
            }
            if (operation == "*")
            {
                double proizv = operand1 * operand2;
                rezult = proizv.ToString();
            }
            if (operation == "/")
            {
                double chastnoe = operand1 / operand2;
                rezult = chastnoe.ToString();
            }
            if (operation == "^")
            {
                double stepen = Math.Pow(operand1, operand2);
                rezult = stepen.ToString();
            }
            if (operation == "k")
            {
                double koren = Math.Pow(operand2, 1 / operand1);
                rezult = koren.ToString();
            }
            return rezult;
        }
    }
}
