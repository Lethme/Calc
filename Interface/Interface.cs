using System;
using System.Text.RegularExpressions;
using Expression;

namespace Interface
{
    static public class Interface
    {
        static private class Command
        {
            /// <summary>
            /// Returns string parameters collection from ParamStr string
            /// </summary>
            /// <param name="ParamStr">Строка параметров</param>
            /// <param name="Pattern">Паттерн, по которому из строки выбираются параметры</param>
            static public MatchCollection ParamsCollection(string ParamStr, string Pattern = "([\"].+?[\"]|[^ ]+)+")
            {
                var regex = new Regex(Pattern, RegexOptions.Singleline);
                var Params = regex.Matches(ParamStr);
                return Params;
            }
            /// <summary>
            /// Returns string parameter with ParamNumber index from ParamStr string
            /// </summary>
            /// <param name="ParamStr">Parameters string</param>
            /// <param name="ParamNumber">String parameter index in ParamStr</param>
            static public string GetParam(string ParamStr, int ParamNumber, string Pattern = "([\"].+?[\"]|[^ ]+)+")
            {
                var Params = ParamsCollection(ParamStr, Pattern);
                if (Params.Count - 1 < ParamNumber) return string.Empty;
                else return Params[ParamNumber].ToString();
            }
            /// <summary>
            /// Reads a non-empty string form Console
            /// </summary>
            /// <param name="marker">Specifies a text label which appears on line before reading user input</param>
            static public string Enter(string marker = ">")
            {
                string tempCommand;
                do
                {
                    Console.Write($"{marker} "); tempCommand = Console.ReadLine();
                } while (tempCommand == String.Empty);
                return tempCommand;
            }
        }
        static private class Output
        {
            static public void ClearLine(int Line)
            {
                Console.MoveBufferArea(0, Line, Console.BufferWidth, 1, Console.BufferWidth, Line, ' ', Console.ForegroundColor, Console.BackgroundColor);
                return;
            }
            static public bool Confirmation(string Line = "")
            {
                char symb;
                Console.CursorVisible = false;
                do
                {
                    Console.CursorLeft = 0;
                    Console.Write($"{Line} y/n: ");
                    symb = Console.ReadKey().KeyChar;
                } while (symb != 'y' && symb != 'n');

                Console.CursorVisible = true;

                switch (symb)
                {
                    case 'y': return true;
                    case 'n': return false;
                }

                return false;
            }
        }
        static public class Shell
        {
            static private string command;
            static private string command_base;
            static private int param_count;
            static private string[] param;
            static public string Command { get { return command; } }
            static public string CommandBase { get { return command_base; } }
            static public string[] Param { get { return param; } }
            static public int ParamCount { get { return param_count; } }
            static public class UnitReferences
            {
                static public class Calc
                {
                    static public void FAQ()
                    {
                        int space = 15;
                        Console.WriteLine("\n########   Expressions   ########\n");

                        Console.WriteLine("".PadLeft(space) + "----  Binary operators ----\n");
                        Console.WriteLine("'+': ".PadLeft(space) + "The addition operator + computes the sum of its operands");
                        Console.WriteLine("'-': ".PadLeft(space) + "The subtraction operator - subtracts its right-hand operand from its left-hand operand");
                        Console.WriteLine("'*': ".PadLeft(space) + "The multiplication operator * computes the product of its operands");
                        Console.WriteLine("'/': ".PadLeft(space) + "The division operator / divides its left-hand operand by its right-hand operand");
                        Console.WriteLine("'%': ".PadLeft(space) + "The remainder operator % computes the remainder after dividing its left-hand operand by its right-hand operand");
                        Console.WriteLine("'^': ".PadLeft(space) + "Raises a specified number to the specified power");
                        Console.WriteLine();

                        Console.WriteLine("".PadLeft(space) + "----  Unary operators and functions  ----\n");
                        Console.WriteLine("'-': ".PadLeft(space) + "Negates x");
                        Console.WriteLine("'sqrt()': ".PadLeft(space) + "Returns square root from x");
                        Console.WriteLine("'sqr()': ".PadLeft(space) + "Returns x^2 value");
                        Console.WriteLine("'sin()': ".PadLeft(space) + "Returns the sine of the specified angle");
                        Console.WriteLine("'cos()': ".PadLeft(space) + "Returns the cosine of the specified angle");
                        Console.WriteLine("'tan()': ".PadLeft(space) + "Returns the tangent of the specified angle");
                        Console.WriteLine("'ctg()': ".PadLeft(space) + "Returns the cotangent of the specified angle");
                        Console.WriteLine("'exp()': ".PadLeft(space) + "Returns e^x value");
                        Console.WriteLine("'ln()': ".PadLeft(space) + "Returns the natural logarithm of a specified number");
                        Console.WriteLine("'lg()': ".PadLeft(space) + "Returns base 10 logarithm of a specified number");
                        Console.WriteLine("'log()': ".PadLeft(space) + "Returns base 2 logarithm of a specified number");
                        Console.WriteLine("'pow2()': ".PadLeft(space) + "Returns 2^x value");
                        Console.WriteLine("'pow10()': ".PadLeft(space) + "returns 10^x value");
                        Console.WriteLine("'abs()': ".PadLeft(space) + "Returns the absolute value of a specified number");
                        Console.WriteLine("'round()': ".PadLeft(space) + "Rounds a value to the nearest integer or to the specified number of fractional digits");
                        Console.WriteLine();

                        Console.WriteLine("".PadLeft(space) + "----  Constants and Variables  ----\n");
                        Console.WriteLine("'Pi': ".PadLeft(space) + "Returns the pi constant");
                        Console.WriteLine("'E': ".PadLeft(space) + "Returns the e constant");
                        Console.WriteLine("'Ans': ".PadLeft(space) + "Returns the answer of the last expression");
                        Console.WriteLine();

                        Console.WriteLine("".PadLeft(space) + "Try 'calc -t' command to implement a few test expressions\n");

                        Console.WriteLine("\n########   Expressions   ########\n");

                        Console.WriteLine("\n########   How to use   ########\n");

                        Console.WriteLine("".PadLeft(space) + "'calc <expression>': " + "Evaluates your expression and shows its result value");
                        Console.WriteLine("".PadLeft(space) + "'calc %deg% <true/false>': " + "Calculator will use degrees if deg variable is stated to True and radians otherwise. It's false by default.");

                        Console.WriteLine("\n########   How to use   ########\n");
                    }
                }
            }
            static public void Ref()
            {
                Console.WriteLine("Use 'help' to see all the available commands in current shell!");
                Console.WriteLine("Use 'help <utility>' to see an additional information!\n");
                return;
            }
            static private void GetParams()
            {
                command_base = Interface.Command.GetParam(command, 0);

                var tempCollection = Interface.Command.ParamsCollection(command);
                param_count = tempCollection.Count - 1;

                param = new string[tempCollection.Count - 1];
                for (int i = 1; i < tempCollection.Count; i++)
                {
                    param[i - 1] = tempCollection[i].ToString().Trim('"');
                }
            }
            static private void SetCommand(string Command)
            {
                if (Command != String.Empty)
                {
                    command = Command;
                    GetParams();
                }
            }
            static public void ExecuteString(string Command)
            {
                if (Command != String.Empty)
                {
                    Console.WriteLine($"Executing command: {Command}");
                    SetCommand(Command);
                    Execute();
                }
            }
            static public void ExecuteString(string[] CommandArr)
            {
                foreach (var Command in CommandArr)
                {
                    ExecuteString(Command);
                }
            }
            static public void Enter()
            {
                command = Interface.Command.Enter();
                GetParams();
            }
            static private void faq()
            {
                int stream_size = 30;
                Console.WriteLine("calc \"expression\"".PadLeft(stream_size) + " - Evaluate string math expression");
                Console.WriteLine("rpn \"expression\"".PadLeft(stream_size) + " - Convert classic math expression to Reverse Polish Notation");
                Console.WriteLine();
                Console.WriteLine("help".PadLeft(stream_size) + " - Show all the available commands");
                Console.WriteLine("cls".PadLeft(stream_size) + " - Clear the console output");
                Console.WriteLine("exit".PadLeft(stream_size) + " - Exit");
                Console.WriteLine();
            }
            static public void Execute()
            {
                switch (command_base)
                {
                    case "rpn":
                        {
                            if (param_count == 1)
                            {
                                try
                                {
                                    string result = Expression.Expression.GetRPNExpression(param[0]);
                                    Console.WriteLine($"[Calc] RPN Expression: " + (result == String.Empty ? "Empty" : result) + "\n");
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("[Calc] Invalid expression!\n");
                                }
                            }
                            else if (param_count == 0)
                            {
                                Console.WriteLine("Expression should be stated!");
                                Console.WriteLine("Better use brackets '\"' for typing your expression!\n");
                            }
                            else
                            {
                                Console.WriteLine("Too much arguments!\n");
                            }
                            break;
                        }
                    case "calc":
                        {
                            if (param_count == 2)
                            {
                                if (param[0] == "-r")
                                {
                                    try
                                    {
                                        double result = Expression.Expression.EvaluateRPN(param[1]);
                                        Console.WriteLine($"[Calc] RPN Expression result: {result}\n");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("[Calc] Invalid expression!\n");
                                    }
                                }
                                else
                                {
                                    if (param[1] != "true" && param[1] != "false") break;
                                    switch (param[0])
                                    {
                                        case "%" + nameof(Expression.Expression.Samples.exprpn) + "%":
                                            {
                                                if (Expression.Expression.Samples.SwitchExpectedRPN(Boolean.Parse(param[1])))
                                                    Console.WriteLine("[Calc] Expected RPN expressions will be shown in samples implementation.\n");
                                                else
                                                    Console.WriteLine("[Calc] Expected RPN expressions will be hidden in samples implementation.\n");
                                                break;
                                            }
                                        case "%" + nameof(Expression.Expression.Samples.expres) + "%":
                                            {
                                                if (Expression.Expression.Samples.SwitchExpectedResult(Boolean.Parse(param[1])))
                                                    Console.WriteLine("[Calc] Expected expression results will be shown in samples implementation.\n");
                                                else
                                                    Console.WriteLine("[Calc] Expected expression results will be hidden in samples implementation.\n");
                                                break;
                                            }
                                        case "%" + nameof(Expression.Expression.Samples.actrpn) + "%":
                                            {
                                                if (Expression.Expression.Samples.SwitchActualRPN(Boolean.Parse(param[1])))
                                                    Console.WriteLine("[Calc] Actual RPN expressions will be shown in samples implementation.\n");
                                                else
                                                    Console.WriteLine("[Calc] Actual RPN expressions will be hidden in samples implementation.\n");
                                                break;
                                            }
                                        case "%deg%":
                                            {
                                                Expression.Expression.Deg = Boolean.Parse(param[1]);
                                                if (Expression.Expression.Deg)
                                                    Console.WriteLine("[Calc] Measure unit has been set to Degrees.\n");
                                                else
                                                    Console.WriteLine("[Calc] Measure unit has been set to Radians.\n");
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                            else if (param_count == 1)
                            {
                                switch (param[0])
                                {
                                    case "-t": Expression.Expression.Samples.ImplementSamples(); return;
                                    case "%":
                                        {
                                            Console.WriteLine($"[Calc] Expected RPN expressions display: {nameof(Expression.Expression.Samples.exprpn)} = {Expression.Expression.Samples.exprpn}");
                                            Console.WriteLine($"[Calc] Expected expression results display: {nameof(Expression.Expression.Samples.expres)} = {Expression.Expression.Samples.expres}");
                                            Console.WriteLine($"[Calc] Actual RPN expressions display: {nameof(Expression.Expression.Samples.actrpn)} = {Expression.Expression.Samples.actrpn}");
                                            Console.WriteLine();
                                            return;
                                        }
                                    case "%" + nameof(Expression.Expression.Samples.exprpn) + "%":
                                        {
                                            Console.WriteLine($"[Calc] Expected RPN expressions display: {Expression.Expression.Samples.exprpn}\n");
                                            return;
                                        }
                                    case "%" + nameof(Expression.Expression.Samples.expres) + "%":
                                        {
                                            Console.WriteLine($"[Calc] Expected expression results display: {Expression.Expression.Samples.expres}\n");
                                            return;
                                        }
                                    case "%" + nameof(Expression.Expression.Samples.actrpn) + "%":
                                        {
                                            Console.WriteLine($"[Calc] Actual RPN expressions display: {Expression.Expression.Samples.actrpn}\n");
                                            return;
                                        }
                                    case "%deg%":
                                        {
                                            if (Expression.Expression.Deg)
                                                Console.WriteLine($"[Calc] Calculator is using Degrees!\n");
                                            else
                                                Console.WriteLine($"[Calc] Calculator is using Radians!\n");
                                            return;
                                        }
                                }

                                try
                                {
                                    double result = Expression.Expression.Parse(param[0]);
                                    Console.WriteLine($"[Calc] Expression result: {result}\n");
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("[Calc] Invalid expression!\n");
                                }
                            }
                            else if (param_count == 0)
                            {
                                Console.WriteLine("Expression should be stated!");
                                Console.WriteLine("Better use brackets '\"' for typing your expression!\n");
                            }
                            else
                            {
                                Console.WriteLine("Too much arguments!\n");
                            }
                            break;
                        }
                    case "help":
                        {
                            if (param_count == 1)
                            {
                                switch(param[0])
                                {
                                    case "calc":
                                        {
                                            Shell.UnitReferences.Calc.FAQ();
                                            return;
                                        }
                                }
                            }

                            Interface.Shell.faq();
                            return;
                        }
                    case "cls":
                        {
                            Console.Clear();
                            Interface.Shell.Ref();
                            return;
                        }
                    case "exit":
                        {
                            if (Interface.Output.Confirmation("Are you sure you want to exit?"))
                            {
                                Console.Clear();
                                System.Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine('\n');
                                return;
                            }
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong command!\n");
                            return;
                        }
                }
            }
        }
    }
}
