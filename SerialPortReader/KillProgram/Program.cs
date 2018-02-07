using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KillProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "1") //kills Height Weight program
                {
                    foreach (var process in Process.GetProcessesByName("SerialPortReader"))
                    {
                        process.Kill();
                    }
                }
                if (args[0] == "2") //kills Blood Pressure program
                {
                    foreach (var process in Process.GetProcessesByName("BPSerialPortReader"))
                    {
                        process.Kill();
                    }
                }
                if (args[0] == "3") //kills Temperature program
                {
                    foreach (var process in Process.GetProcessesByName("TempPortReader"))
                    {
                        process.Kill();
                    }
                }
                if (args[0] == "4") //kills SPO2
                {
                    foreach (var process in Process.GetProcessesByName("SPO2SerialPortReader"))
                    {
                        process.Kill();
                    }
                }
            }
        }
    }
}
