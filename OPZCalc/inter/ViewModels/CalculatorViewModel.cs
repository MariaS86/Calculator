using Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Shapes;

namespace inter.ViewModels
{
    public class CalculatorViewModel : BindableBase
    {
        public CalculatorViewModel()
        {

        }

        private string _result;

        public string Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }

        private string _expression;

        public string Expression
        {
            get { return _expression; }
            set
            {
                SetProperty(ref _expression, value);
                CalculateCommand.RaiseCanExecuteChanged();
                DeleteCharacterCommand.RaiseCanExecuteChanged();
                DeleteOneCharacterCommand.RaiseCanExecuteChanged();
            }
            
        }

        private DelegateCommand<string> _clickCommand;

        public DelegateCommand<string> ClickCommand =>
            _clickCommand ?? (_clickCommand = new DelegateCommand<string>(ExecuteClickCommand));

        private DelegateCommand _calculateCommand;

        public DelegateCommand CalculateCommand =>
            _calculateCommand ?? (_calculateCommand = new DelegateCommand(Calculate1, CanCalculate));
        private DelegateCommand _deleteCharacterCommand;
        public DelegateCommand DeleteCharacterCommand =>
            _deleteCharacterCommand ?? (_deleteCharacterCommand = new DelegateCommand(Del, CanExecuteDeleteCharacter));
        private DelegateCommand _deleteOneCharacterCommand;
        public DelegateCommand DeleteOneCharacterCommand =>
            _deleteOneCharacterCommand ?? (_deleteOneCharacterCommand = new DelegateCommand(DelOne, CanExecuteDeleteCharacter));

    
        //проверка правильности выражения
        private bool CanCalculate()
        {
            var isvalidExpression = Expression != null
                ? Regex.IsMatch(Expression, @"^\d*\,?\d+(\s*[+*/-^]\s*\d*\,?\d)+$")                                                                  //[]     - один символ из указанного в скобках набора;

                : false;
            return isvalidExpression;
        }
       
        //Стирание всех символов
        void Del()
        {
            Expression = string.Empty;
        }
        //Стирание одного символа
        void DelOne()
        {
            Expression = Expression.Remove(Expression.Length-1);
        }

        void ExecuteClickCommand(string param)
        {
            Expression += param;
        }
        private bool CanExecuteDeleteCharacter()
        {
            var isvalidExpression = Expression != null;
            return isvalidExpression;
        }
        //создание потока для записи в файл
        StreamWriter sw; // поток дя записи
        const int NmaxZap = 100; // макс.число записей (количество результатов)
        void Calculate1()
        {
            string expres = Expression;
            var ex = new calc();
            var expr = ex.Calculate(expres);
            string value = expr.ToString();
            Result = value;
            //Реализация памяти через запись результатов в файл
                string[] d = new string[NmaxZap];
                string[] b = new string[NmaxZap];
                int i = 0; // счетчик строк


                d[i] = Result; // добавим строку c результатом
                b[i] = Expression+"="; // добавим строку c выражением

            // Запись в файл: 
            FileInfo fi = new FileInfo("D:\\Maria\\TRPOpractic\\CalculatorMVVM\\Calculate\\OPZCalc\\memory.txt"); // информация о файле 
                if (fi.Exists) //если файл закрыт
                    sw = fi.AppendText(); // открыть поток для добавления
                else
                    sw = fi.CreateText(); // или поток для записи 
            for (int j = 0; j <= i; j++)
                sw.WriteLine(b[j].ToString()); // запись строк results в файл
            for (int n = 0; n <= i; n++)
                    sw.WriteLine(d[n].ToString()); // запись строк results в файл
                sw.Close();
                
            Expression = string.Empty;
        }

        public class calc
        {
            public double Calculate(string input)
            {
                string output = GetExpression(input);
                double result = Counting(output);
                return result;
            }

            //Метод, преобразующий входную строку с выражением в постфиксную запись
            public string GetExpression(string input)
            {
                string output = string.Empty;
                Stack<char> operStack = new Stack<char>();

                for (int i = 0; i < input.Length; i++)
                {

                    if (IsDelimeter(input[i]))
                        continue;


                    if (Char.IsDigit(input[i]))
                    {

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            output += input[i];
                            i++;

                            if (i == input.Length) break;
                        }

                        output += " ";
                        i--;
                    }

                    //Если символ - оператор
                    if (IsOperator(input[i]))
                    {
                        if (input[i] == '(')
                            operStack.Push(input[i]);
                        else if (input[i] == ')')
                        {
                            char s = operStack.Pop();

                            while (s != '(')
                            {
                                output += s.ToString() + ' ';
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            if (operStack.Count > 0)
                                if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                    output += operStack.Pop().ToString() + " ";

                            operStack.Push(char.Parse(input[i].ToString()));

                        }
                    }
                }

                while (operStack.Count > 0)
                    output += operStack.Pop() + " ";

                return output;
            }

            //Метод, вычисляющий значение выражения, уже преобразованного в постфиксную запись
            public double Counting(string input)
            {
                double result = 0;
                Stack<double> temp = new Stack<double>();

                for (int i = 0; i < input.Length; i++)
                {

                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }
                        temp.Push(double.Parse(a));
                        i--;
                    }
                    else if (IsOperator(input[i]))
                    {

                        double a = temp.Pop();
                        double b = temp.Pop();

                        switch (input[i])
                        {
                            case '+': result = b + a; break;
                            case '-': result = b - a; break;
                            case '*': result = b * a; break;
                            case '/': result = b / a; break;
                            case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                        }
                        temp.Push(result);
                    }
                }
                return temp.Peek();
            }

            //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
            public bool IsDelimeter(char c)
            {
                if ((" =".IndexOf(c) != -1))
                    return true;
                return false;
            }
            //Метод возвращает true, если проверяемый символ - оператор
            public bool IsOperator(char с)
            {
                if (("+-/*^()".IndexOf(с) != -1))
                    return true;
                return false;
            }
            //Метод возвращает приоритет оператора
            public byte GetPriority(char s)
            {
                switch (s)
                {
                    case '(': return 0;
                    case ')': return 1;
                    case '+': return 2;
                    case '-': return 3;
                    case '*': return 4;
                    case '/': return 4;
                    case '^': return 5;
                    default: return 6;
                }
            }
        }
    }
}

