using System;
using System.Collections.Generic;
using MathNet.Symbolics;

namespace OptMethodsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Expression: ");
            var expr = Console.ReadLine();
            var parsed = Infix.ParseOrThrow(expr);
            var res = Evaluate.Evaluate(new Dictionary<String, FloatingPoint>(), parsed);
            Console.WriteLine(res);
        }
    }
}