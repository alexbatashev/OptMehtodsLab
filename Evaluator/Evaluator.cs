using System;
using System.Collections.Generic;

using MathNet.Symbolics;

namespace Evaluator
{
    public class Evaluator
    {
        public static double Evaluate(double x, string expr)
        {
            var parsed = Infix.ParseOrThrow(expr);
            var values = new Dictionary<String, FloatingPoint>();
            values.Add("x", x);

            var res = MathNet.Symbolics.Evaluate.Evaluate(values, parsed);
            return res.RealValue;
        }
    }
}
