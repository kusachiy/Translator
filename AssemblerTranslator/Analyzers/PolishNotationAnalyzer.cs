using AssemblerTranslator.DataTypes.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblerTranslator.Analyzers
{
    public static class PolishNotationAnalyzer
    {
        public static bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

        public static bool IsIntOperator(char с)
        {
            if (("+-/*^()".IndexOf(с) != -1))
                return true;
            return false;
        }
        public static bool IsBoolOperator(char с)
        {
            if (("!&|^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        public static string GetExpression(string input)
        {
            bool waitArg = true;
            int brackets_balance = 0;
            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                if (Char.IsLetter(input[i])) //Если буква
                {
                    if (!waitArg)
                        throw new Exception("Ошибка в синтаксисе условия");
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsIntOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                    waitArg = false;
                }
                //Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    if (!waitArg)
                        throw new Exception("Ошибка в синтаксисе условия");
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsIntOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                    waitArg = false;

                }

                //Если символ - оператор
                if (IsIntOperator(input[i])) //Если оператор
                {

                    if (input[i] == '(') //Если символ - открывающая скобка
                    {
                        brackets_balance++;
                        operStack.Push(input[i]); //Записываем её в стек
                    }
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        brackets_balance--;


                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (waitArg)
                            throw new Exception("Ошибка в синтаксисе условия");
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека
                        waitArg = true;
                    }
                }
            }
            if (waitArg || brackets_balance != 0)
                throw new Exception();
            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";
            
            return output; //Возвращаем выражение в постфиксной записи
        }

        public static string GetBoolExpression(string input)
        {
            bool waitArg = true;
            int brackets_balance = 0;

            input = ToSymbolOperators(input);

            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                if (Char.IsLetter(input[i])) //Если буква
                {
                    if (!waitArg)
                        throw new Exception("Ошибка в синтаксисе условия");
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsBoolOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }
                    waitArg = false;
                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }
                //Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    if (!waitArg)
                        throw new Exception("Ошибка в синтаксисе условия");
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsBoolOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }
                    waitArg = false;
                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsBoolOperator(input[i])) //Если оператор
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                    {
                        operStack.Push(input[i]); //Записываем её в стек
                        brackets_balance++;
                    }
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        brackets_balance--;
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {

                        if (waitArg && input[i] != '!')
                            throw new Exception("Ошибка в синтаксисе условия");
                        waitArg = true;
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetBoolPriority(input[i]) <= GetBoolPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }
            if (waitArg||brackets_balance!=0)
                throw new Exception();
            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output;
        }
      
        public static byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                default: return 6;
            }
        }
        public static byte GetBoolPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '|': return 3;
                case '^': return 4;
                case '&': return 5;
                case '!': return 6;
                default: return 7;
            }
        }

        public static ValueType Calculator(string input, List<BaseVariable> variables,Type type)
        {
            if (type == typeof(int))
                return IntCalculator(input, variables);
            return BoolCalculator(input, variables);
        }

        public static int IntCalculator(string input, List<BaseVariable> variables)
        {
            int result = 0; //Результат
            Stack<int> temp = new Stack<int>(); //Dhtvtyysq стек для решения

            for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (Char.IsLetter(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsIntOperator(input[i])) //Пока не разделитель
                    {
                        a += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }
                    BaseVariable varr = null;
                    if ((varr = variables.FirstOrDefault(v => v.Name == a)) == null)
                        if (varr.Value == null)
                            throw new Exception("Переменная не инициализирована");
                    temp.Push((int)varr.Value); //Записываем в стек
                    i--;
                }
                if (Char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsIntOperator(input[i])) //Пока не разделитель
                    {
                        a += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }
                    temp.Push(int.Parse(a)); //Записываем в стек
                    i--;
                }
                else if (IsIntOperator(input[i])) //Если символ - оператор
                {

                    //Берем два последних значения из стека
                    int a = temp.Pop();
                    int b = temp.Pop();

                    switch (input[i]) //И производим над ними действие, согласно оператору
                    {
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case '*': result = b * a; break;
                        case '/': result = b / a; break;
                        case '^': result = int.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    }
                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }
        
        public static bool BoolCalculator(string input, List<BaseVariable> variables)
        {
            throw new NotImplementedException();
        }

        private static string ToSymbolOperators(string expression)
        {
            expression = expression.Replace("AND", "&").Replace("XOR", "^").Replace("OR", "|").Replace("NOT", "!");
            return expression;
        }

       
    }
}
