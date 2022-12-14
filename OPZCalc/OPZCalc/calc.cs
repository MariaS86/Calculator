namespace OPZcalc
{
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