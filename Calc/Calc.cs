using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calc
{
    class Calc
    {


        private string expression;
        delegate double Operation(double a, double b);
        public string Expression
        {
            get
            {
                return expression;
            }

            set
            {
                expression = value;
                expression = expression.Replace(" ", "");
                expression = expression.Replace("--", "+");
                while (expression.Contains("++"))
                {
                    expression = expression.Replace("++", "+");
                }
            }
        }
        public void Launch()
        {
            FindExpressioInBrackets();
            Console.WriteLine("Ответ: " + expression);
            Console.ReadLine();
        }


        private void FindExpressioInBrackets()
        {
            int start = -1;
            bool sqrtIndicator = false;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    if ((i != 0) && (expression[i - 1] == 't'))
                    {
                        sqrtIndicator = true;
                    }
                    else
                    {
                        sqrtIndicator = false;
                    }
                    start = i;
                }
                else if (expression[i] == ')')
                {
                    if (sqrtIndicator)
                    {
                        string inSqrtValue = expression.Substring(start + 1, i - start - 1);
                        expression = expression.Replace("sqrt(" + inSqrtValue + ")", Math.Sqrt(Convert.ToDouble(FindResult(inSqrtValue))).ToString());
                        sqrtIndicator = false;
                        i = -1;
                    }
                    else
                    {
                        string exp = expression.Substring(start + 1, i - start - 1);
                        expression = expression.Replace("(" + exp + ")", FindResult(exp));
                        expression = expression.Replace("--", "+");
                        i = -1;
                    }

                }
                if (!expression.Contains("("))
                {
                    break;
                }
            }
            expression = expression.Replace(expression, FindResult(Expression));
        }


        private string FindResult(string expression)
        {
            List<char> opers = new List<char>();
            List<int> keys = new List<int>();
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '+' || expression[i] == '/' ||
                    expression[i] == '*' || expression[i] == '^')
                {
                    opers.Add(expression[i]);
                    keys.Add(i);
                }
                else if (expression[i] == '-')
                {
                    if (i != 0 && expression[i - 1] != '+' && expression[i - 1] != '/' &&
                    expression[i - 1] != '*' && expression[i - 1] != '^')
                    {
                        opers.Add(expression[i]);
                        keys.Add(i);

                    }
                }
            }
            List<string> s = new List<string> { "", "", "" };
            int size;
            int key = -1;
            int j = -1;
            while (keys.Count != 0)
            {
                if (opers.Contains('^'))
                {
                    key = keys[opers.IndexOf('^')];
                    s = GetResultOfExpression(opers, keys, key, (a, b) => Math.Pow(a, b), expression);
                    expression = expression.Replace(s[0] + "^" + s[1], s[2]);
                    j = keys.IndexOf(key);
                    keys.Remove(key);
                    opers.Remove('^');
                }
                else if (opers.Contains('*'))
                {
                    key = keys[opers.IndexOf('*')];
                    s = GetResultOfExpression(opers, keys, key, (a, b) => a * b, expression);
                    expression = expression.Replace(s[0] + "*" + s[1], s[2]);
                    j = keys.IndexOf(key);
                    keys.Remove(key);
                    opers.Remove('*');
                }
                else if (opers.Contains('/'))
                {
                    key = keys[opers.IndexOf('/')];
                    s = GetResultOfExpression(opers, keys, key, (a, b) => a / b, expression);
                    expression = expression.Replace(s[0] + "/" + s[1], s[2]);
                    j = keys.IndexOf(key);
                    keys.Remove(key);
                    opers.Remove('/');
                }
                else if (opers.Contains('+'))
                {
                    key = keys[opers.IndexOf('+')];
                    s = GetResultOfExpression(opers, keys, key, (a, b) => a + b, expression);
                    expression = expression.Replace(s[0] + "+" + s[1], s[2]);
                    j = keys.IndexOf(key);
                    keys.Remove(key);
                    opers.Remove('+');
                }
                else if (opers.Contains('-'))
                {
                    key = keys[opers.IndexOf('-')];
                    s = GetResultOfExpression(opers, keys, key, (a, b) => a - b, expression);
                    expression = expression.Replace(s[0] + "-" + s[1], s[2]);
                    j = keys.IndexOf(key);
                    keys.Remove(key);
                    opers.Remove('-');
                }
                else
                {
                    return expression;
                }
                size = s[0].Length + s[1].Length + 1 - s[2].Length;
                if (keys.Count > j)
                    for (int i = j; i < keys.Count; i++)
                    {
                        keys[i] = keys[i] - size;
                    }
                expression = expression.Replace("++", "+");
                expression = expression.Replace("--", "-");
            }

            return expression;
        }


        private List<string> GetResultOfExpression(List<char> opers, List<int> keys, int key, Operation func, string expression)
        {
            double a;
            double b;
            bool aNothing = false;
            if (keys.IndexOf(key) == 0)
            {
                if (expression.IndexOf(opers[keys.IndexOf(key)]) == 0)
                {
                    a = 0;
                    aNothing = true;
                }
                else
                    a = Convert.ToDouble(expression.Substring(0, key));
                if (keys.Count > 1)
                    b = Convert.ToDouble(expression.Substring(key + 1, keys[1] - key - 1));
                else
                    b = Convert.ToDouble(expression.Substring(key + 1, expression.Length - key - 1));
            }
            else
            {
                if (opers[keys.IndexOf(key) - 1] != '-')
                    a = Convert.ToDouble(expression.Substring(keys[keys.IndexOf(key) - 1] + 1, key - keys[keys.IndexOf(key) - 1] - 1));
                else
                    a = Convert.ToDouble(expression.Substring(keys[keys.IndexOf(key) - 1], key - keys[keys.IndexOf(key) - 1]));
                if (keys.IndexOf(key) != keys.Count - 1)
                    b = Convert.ToDouble(expression.Substring(key + 1, keys[keys.IndexOf(key) + 1] - key - 1));
                else
                    b = Convert.ToDouble(expression.Substring(key + 1, expression.Length - key - 1));
            }
            double s = func(a, b);
            if ((a < 0 || b < 0) && s > 0)
                return new List<string> { $"{a}", $"{b}", $"+{s}" };
            else if(!aNothing)
                return new List<string> { $"{a}", $"{b}", $"{s}" };
            else
                return new List<string> { "", $"{b}", $"{s}" };
        }


    }
}

