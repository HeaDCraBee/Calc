using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение");
            Console.WriteLine("Доступные символы: (), ^, *, /, sqrt(n), +, -");
            Calc calc = new Calc
            {
                Expression = Console.ReadLine()
            };
            calc.Launch();
        }
    }
}
