using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OPZcalc
{

    class Program
    {
        public static void Main(string[] args)
        {
            char again = 'д';
            while (again == 'д')
            {
                Console.Write("Введите выражение: "); //Предлагаем ввести выражение
                string expres = System.Console.ReadLine();
                string res = expres.Trim('=').Trim('+').Trim('-').Trim('*').Trim('/').Trim('(').Trim(')');
                if (res.Length == 0)
                { Console.WriteLine("Строка только из знаков или пустая"); }
                else
                {
                    if (string.IsNullOrWhiteSpace(expres))
                    {
                        Console.WriteLine("Строка пустая, либо содержит только пробелы");
                    }
                    else
                    {
                        var ex = new calc();
                        var expr = ex.Calculate(expres);
                        Console.WriteLine(expr);
                    }
                }
                Console.WriteLine("Вы хотите продолжить работу с калькулятором? (д/н)");
                again = Convert.ToChar(Console.ReadLine());
            }
        }
    }
}