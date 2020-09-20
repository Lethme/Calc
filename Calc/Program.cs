using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Interface.Interface;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Calc";

            Shell.Ref();
            Shell.ExecuteString(args);

            while (true)
            {
                Shell.Enter();
                Shell.Execute();
            }
        }
    }
}
