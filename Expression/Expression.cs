using System;
using Collections;

namespace Expression
{
    static public class Expression
    {
        static public class Samples
        {
            static public bool exprpn { get; private set; } = false;
            static public bool expres { get; private set; } = true;
            static public bool actrpn { get; private set; } = false;
            static public bool SwitchExpectedRPN(bool state)
            {
                exprpn = state;
                return exprpn;
            }
            static public bool SwitchExpectedResult(bool state)
            {
                expres = state;
                return expres;
            }
            static public bool SwitchActualRPN(bool state)
            {
                actrpn = state;
                return actrpn;
            }
            private struct Sample
            {
                public string Expression { get; }
                public double ExpectedResult { get; }
                public string ExpectedRPN { get; }
                public Sample(string Expression, string ExpectedRPN = "", double ExpectedResult = Double.NaN)
                {
                    this.Expression = Expression;
                    this.ExpectedResult = ExpectedResult;
                    this.ExpectedRPN = ExpectedRPN;
                }
            }
            static private Sample[] _samples = new Sample[]
            {
                new Sample("(16^(1/4))^(5/(64/(2^7)))", "16 1 4 / ^ 5 64 2 7 ^ / / ^", 1024),
                new Sample("((3*15)-(7+7*4))/(1,25*8^(1/3))", "3 15 * 7 7 4 * + - 1,25 8 1 3 / ^ * /", 4),
                new Sample("5*(-3)", "5 3 $ *", -15),
                new Sample("-10*7", "10 $ 7 *", -70),
                new Sample("2^(-1)", "2 1 $ ^", 0.5),
                new Sample("-5*(5^2/11*(-3)-1)^2", "5 $ 5 2 ^ 11 / 3 $ * 1 - 2 ^ *", -305.6198347),
                new Sample("sqrt(40-(-9))+(-2/5*(-(-5))+423,233^2/15^3,33333)", "40 9 $ - # 11 22 3 / ^ 22,222 / 44,3321 $ - 6 1 3 / ^ + +", 26.520817722),
                new Sample("sqrt(40-(-9)) + (11^(22/3)/22,222)-(-44,3321)+6^(1/3)", "", 1950331.26665),
                new Sample("sqrt(sqr(23,33))-4/sqr(sqrt(sqr((-16)/(-4))))", "", 23.08),
                new Sample("sqrt(85^2/(21^(-3))-11*11/(-55-(-55+33*123/443)))", "85 2 ^ 21 3 $ ^ / 11 11 * 55 $ 55 $ 33 123 * 443 / + - / - #", 8179.89842272),
                new Sample("sqr(sin(-0,5)) + sqr(cos(-0,5))", "0,5 $ S & 0,5 $ C & +", 1),
                new Sample("sin(-0,5)^2 + cos(-0,5)^2", "0,5 $ S 2 ^ 0,5 $ C 2 ^ +", 1),
                new Sample("1/sin(cos(sqrt(1-(-2,34))*(1,3+5,85^2)-2,71^(15/0,9455))/3,76534)", "1 1 2,34 $ - # 1,3 5,85 2 ^ + * 2,71 15 0,9455 / ^ - C 3,76534 / S /", 3.8099727),
                new Sample("ln(abs(ctg(sqrt(10,54^pi)^0,5)/tan(exp(pi)*0,99^(-5)/0,25)))", "10,54 p ^ # 0,5 ^ c p E 0,99 5 $ ^ 0,25 / * t / A l", 5.46338)
            };
            static public void ImplementSamples()
            {
                int spaces_from_left = 30;
                foreach (var item in _samples)
                {
                    try
                    {
                        Console.WriteLine("[Calc]" + "Current expression: ".PadLeft(spaces_from_left) + item.Expression);
                        if (exprpn) Console.WriteLine("[Calc]" + "Expected RPN expression: ".PadLeft(spaces_from_left) + item.ExpectedRPN);
                        if (actrpn)
                        {
                            string resultRPN = GetRPNExpression(item.Expression);
                            Console.WriteLine("[Calc]" + "Actual RPN expression: ".PadLeft(spaces_from_left) + resultRPN);
                        }
                        if (expres) Console.WriteLine("[Calc]" + "Expected expression result: ".PadLeft(spaces_from_left) + item.ExpectedResult);
                        double result = Parse(item.Expression);
                        Console.WriteLine("[Calc]" + "Actual expression result: ".PadLeft(spaces_from_left) + result);
                        Console.WriteLine();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[Calc] Invalid expression!\n");
                    }
                }
            }
        }
        static private class Enums
        {
            public enum UnaryOperators
            {
                Minus = '$',
                SquareRoot = '#',
                Square = '&',
                Sin = 'S',
                Cos = 'C',
                Tan = 't',
                Ctg = 'c',
                Ln = 'l',
                Lg = 'g',
                Log = 'd',
                Exp = 'E',
                Pow2 = 'P',
                Pow10 = 'D',
                Abs = 'A',
                Round = 'R',
                Fact = 'F'
            }
            public enum ConstantOperators
            {
                Pi = 'p',
                E = 'e',
                Ans = 'a'
            }
            public enum BinaryOperators
            {
                Plus = '+',
                Minus = '-',
                Mul = '*',
                Div = '/',
                Pow = '^',
                Mod = '%',
                LeftBracket = '(',
                RightBracket = ')'
            }
            public enum Delimeters
            {
                Space = ' ',
                Equal = '='
            }
        }
        static private class Service
        {
            static public int Factorial(int Number)
            {
                if (Number < 0) throw new ArgumentException();

                int result = Number;
                while (Number != 1) result *= --Number;

                return result;
            }
            static public bool IsDelimeter(char Operator)
            {
                if (Enum.IsDefined(typeof(Enums.Delimeters), (int)Operator)) return true;
                return false;
            }
            static public bool IsBinaryOperator(char Operator)
            {
                if (Enum.IsDefined(typeof(Enums.BinaryOperators), (int)Operator)) return true;
                return false;
            }
            static public bool IsUnaryOperator(char Operator)
            {
                if (Enum.IsDefined(typeof(Enums.UnaryOperators), (int)Operator)) return true;
                return false;
            }
            static public bool IsConstantOperator(char Operator)
            {
                if (Enum.IsDefined(typeof(Enums.ConstantOperators), (int)Operator)) return true;
                return false;
            }
            static public byte GetOperatorPriority(char Operator)
            {
                switch (Operator)
                {
                    case (char)Enums.BinaryOperators.LeftBracket: return 0;
                    case (char)Enums.BinaryOperators.RightBracket: return 1;
                    case (char)Enums.BinaryOperators.Plus: return 2;
                    case (char)Enums.BinaryOperators.Minus: return 3;
                    case (char)Enums.BinaryOperators.Mul: return 4;
                    case (char)Enums.BinaryOperators.Div: return 4;
                    case (char)Enums.BinaryOperators.Mod: return 4;
                    case (char)Enums.BinaryOperators.Pow: return 5;
                    default: return 6;
                }
            }
        }
        static public double Ans { get; private set; } = Double.NaN;
        static public bool Deg { get; private set; } = false;
        static public string LastExpression { get; private set; } = String.Empty;
        static public bool SwitchRadDeg()
        {
            if (Deg) Deg = false;
            else Deg = true;
            return Deg;
        }
        static public string GetRPNExpression(string Expression)
        {
            string RPNExpression = string.Empty;
            Stack<char> OperatorStack = new Stack<char>();

            for (int i = 0; i < Expression.Length; i++)
            {
                if (Service.IsDelimeter(Expression[i]))
                    continue;

                if (Char.IsDigit(Expression[i]))
                {
                    while (!Service.IsDelimeter(Expression[i]) && !Service.IsBinaryOperator(Expression[i]))
                    {
                        RPNExpression += Expression[i++];
                        if (i == Expression.Length) break;
                    }
                    RPNExpression += ((char)Enums.Delimeters.Space).ToString();
                    i--;
                }
                if (Char.IsLetter(Expression[i]))
                {
                    string Function = String.Empty;
                    while (Char.IsLetter(Expression[i]) || Char.IsDigit(Expression[i]))
                    {
                        Function += Expression[i];
                        if (i + 1 == Expression.Length) break;
                        i++;
                    }

                    switch (Function.ToLower())
                    {
                        case "sqrt": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.SquareRoot); break; }
                        case "sqr": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Square); break; }
                        case "sin": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Sin); break; }
                        case "cos": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Cos); break; }
                        case "tan": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Tan); break; }
                        case "ctg": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Ctg); break; }
                        case "exp": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Exp); break; }
                        case "ln": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Ln); break; }
                        case "lg": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Lg); break; }
                        case "log": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Log); break; }
                        case "pow2": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Pow2); break; }
                        case "pow10": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Pow10); break; }
                        case "abs": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Abs); break; }
                        case "round": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Round); break; }
                        case "fact": { if (Expression[i] != (char)Enums.BinaryOperators.LeftBracket) throw new ArgumentException(); OperatorStack.Push((char)Enums.UnaryOperators.Fact); break; }
                        case "pi": { OperatorStack.Push((char)Enums.ConstantOperators.Pi); break; }
                        case "e": { OperatorStack.Push((char)Enums.ConstantOperators.E); break; }
                        case "ans": { OperatorStack.Push((char)Enums.ConstantOperators.Ans); break; }
                        default: { throw new ArgumentException($"{Function} is not a function!"); }
                    }
                }
                if (Service.IsBinaryOperator(Expression[i]))
                {
                    if (Expression[i] == (char)Enums.BinaryOperators.LeftBracket)
                        OperatorStack.Push(Expression[i]);
                    else if (Expression[i] == (char)Enums.BinaryOperators.RightBracket)
                    {
                        char Operator = OperatorStack.Pop();

                        while (Operator != (char)Enums.BinaryOperators.LeftBracket)
                        {
                            RPNExpression += Operator + ((char)Enums.Delimeters.Space).ToString();
                            Operator = OperatorStack.Pop();
                        }
                    }
                    else
                    {
                        if (OperatorStack.Count > 0)
                            if (Service.GetOperatorPriority(Expression[i]) <= Service.GetOperatorPriority(OperatorStack.Peek()))
                                RPNExpression += OperatorStack.Pop() + ((char)Enums.Delimeters.Space).ToString();

                        if (Expression[i] == (char)Enums.BinaryOperators.Minus)
                        {
                            var j = i;
                            while (j > 0 && !Char.IsDigit(Expression[j]) && !(Expression[j] == (char)Enums.BinaryOperators.LeftBracket))
                            {
                                j--;
                            }
                            if (Expression[j] == (char)Enums.BinaryOperators.LeftBracket || !Char.IsDigit(Expression[j]) || j == -1) OperatorStack.Push((char)Enums.UnaryOperators.Minus);
                            else OperatorStack.Push((char)Enums.BinaryOperators.Minus);
                        }
                        else
                        {
                            OperatorStack.Push(Expression[i]);
                        }
                    }
                }
            }

            while (OperatorStack.Count > 0)
                RPNExpression += OperatorStack.Pop() + ((char)Enums.Delimeters.Space).ToString();

            return RPNExpression;
        }
        static public double EvaluateRPNExpression(string RPNExpression)
        {
            double result = 0;
            Stack<double> Numbers = new Stack<double>();

            for (int i = 0; i < RPNExpression.Length; i++)
            {
                if (Char.IsDigit(RPNExpression[i]))
                {
                    string Number = string.Empty;

                    while (!Service.IsDelimeter(RPNExpression[i]) && !Service.IsBinaryOperator(RPNExpression[i]))
                    {
                        Number += RPNExpression[i++];
                        if (i == RPNExpression.Length) break;
                    }
                    Numbers.Push(double.Parse(Number));
                    i--;
                }
                else if (Service.IsConstantOperator(RPNExpression[i]))
                {
                    switch (RPNExpression[i])
                    {
                        case (char)Enums.ConstantOperators.Pi: { Numbers.Push(Math.PI); break; }
                        case (char)Enums.ConstantOperators.E: { Numbers.Push(Math.E); break; }
                        case (char)Enums.ConstantOperators.Ans: { if (Double.IsNaN(Ans) || Double.IsInfinity(Ans)) throw new ArgumentException(); Numbers.Push(Ans); break; }
                    }
                }
                else if (Service.IsUnaryOperator(RPNExpression[i]))
                {
                    double Operand = Numbers.Pop();

                    switch (RPNExpression[i])
                    {
                        case (char)Enums.UnaryOperators.Minus: result = -(Operand); break;
                        case (char)Enums.UnaryOperators.SquareRoot: { if (Operand < 0) throw new ArgumentOutOfRangeException(); result = Math.Sqrt(Operand); break; }
                        case (char)Enums.UnaryOperators.Square: { result = Operand * Operand; break; }
                        case (char)Enums.UnaryOperators.Sin: { if (Deg) result = Math.Sin(Operand * (Math.PI / 180)); else result = Math.Sin(Operand); break; }
                        case (char)Enums.UnaryOperators.Cos: { if (Deg) result = Math.Cos(Operand * (Math.PI / 180)); else result = Math.Cos(Operand); break; }
                        case (char)Enums.UnaryOperators.Tan: { if (Deg) result = Math.Tan(Operand * (Math.PI / 180)); else result = Math.Tan(Operand); break; }
                        case (char)Enums.UnaryOperators.Ctg: { if (Deg) result = 1 / Math.Tan(Operand * (Math.PI / 180)); else result = 1 / Math.Tan(Operand); break; }
                        case (char)Enums.UnaryOperators.Exp: { result = Math.Exp(Operand); break; }
                        case (char)Enums.UnaryOperators.Ln: { if (Operand <= 0) throw new ArgumentException(); result = Math.Log(Operand); break; }
                        case (char)Enums.UnaryOperators.Lg: { if (Operand <= 0) throw new ArgumentException(); result = Math.Log10(Operand); break; }
                        case (char)Enums.UnaryOperators.Log: { if (Operand <= 0) throw new ArgumentException(); result = Math.Log(Operand) / Math.Log(2); break; }
                        case (char)Enums.UnaryOperators.Fact: { if (Operand - Math.Round(Operand) != 0) throw new ArgumentException(); result = Service.Factorial((int)Operand); break; }
                        case (char)Enums.UnaryOperators.Pow2: { result = Math.Pow(2, Operand); break; }
                        case (char)Enums.UnaryOperators.Pow10: { result = Math.Pow(10, Operand); break; }
                        case (char)Enums.UnaryOperators.Abs: { result = Math.Abs(Operand); break; }
                        case (char)Enums.UnaryOperators.Round: { result = Math.Round(Operand); break; }
                    }
                    if (Double.IsNaN(result)) throw new ArgumentException();
                    Numbers.Push(result);
                }
                else if (Service.IsBinaryOperator(RPNExpression[i]))
                {
                    double RightOperand = Numbers.Pop();
                    double LeftOperand = Numbers.Pop();

                    switch (RPNExpression[i])
                    {
                        case (char)Enums.BinaryOperators.Plus: result = LeftOperand + RightOperand; break;
                        case (char)Enums.BinaryOperators.Minus: result = LeftOperand - RightOperand; break;
                        case (char)Enums.BinaryOperators.Mul: result = LeftOperand * RightOperand; break;
                        case (char)Enums.BinaryOperators.Div:
                            {
                                if (RightOperand == 0) throw new DivideByZeroException();
                                result = LeftOperand / RightOperand;
                                break;
                            }
                        case (char)Enums.BinaryOperators.Mod:
                            {
                                if (RightOperand == 0) throw new DivideByZeroException();
                                result = LeftOperand % RightOperand;
                                break;
                            }
                        case (char)Enums.BinaryOperators.Pow: result = Math.Pow(LeftOperand, RightOperand); break;
                    }
                    if (Double.IsNaN(result)) throw new ArgumentException();
                    Numbers.Push(result);
                }
            }
            if (Double.IsNaN(Numbers.Peek()) || Double.IsInfinity(Numbers.Peek())) throw new ArgumentOutOfRangeException("Expression result is NaN!");
            if (Numbers.Count != 1) throw new ArgumentOutOfRangeException();
            return Numbers.Pop();
        }
        static public double Parse(string Expression)
        {
            string RPNExpression = GetRPNExpression(Expression);
            double result = EvaluateRPNExpression(RPNExpression);
            LastExpression = Expression;
            Ans = result;
            return result;
        }
    }
}