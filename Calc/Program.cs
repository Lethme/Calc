using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interface;

namespace Calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Interface.Interface.Shell.Ref();
            while (true)
            {
                Console.Title = "Console";
                Interface.Interface.Shell.Enter();
                Interface.Interface.Shell.Execute();
            }
        }
    }
}
