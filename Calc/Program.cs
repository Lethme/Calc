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
            Shell.Ref();
            while (true)
            {
                Console.Title = "Console";
                Shell.Enter();
                Shell.Execute();
            }
        }
    }
}
