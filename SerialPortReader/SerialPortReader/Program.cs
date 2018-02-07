using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace Serial_Test
{
    class Program
    {
        static SerialPort newPort;
        static string dataReceived;

        
        static void Main(string[] args)
        {
            newPort = new SerialPort("COM5", 4800); //bp: 2400 baud, BMI: 4800 baud Temp:9600


            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);


            try
            {
                newPort.Open();
                newPort.WriteLine("80$"); // 80$ turns off automatic measure 81% turns on automatic measure
            }
            catch (IOException err)
            {
                Console.WriteLine("[ERR] " + err);
            }

            if (newPort.IsOpen)
            {
                if (args.Length > 0)
                {
                    if (args[0] == "11")
                    {
                        startMeasurement();
                    }
                    if (args[0] == "10")
                    {
                        stopMeasurement();
                    }
                    if (args[0] == "81")
                    {
                        autoMeasure();
                    }
                }
            }
           
            //Console.ReadLine();
            newPort.Close();

            Console.WriteLine("End of Program");
        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string value = "";
                dataReceived = newPort.ReadLine();
                Console.Write(dataReceived);
                value += dataReceived;
                while (value.Length > 10)
                {
                    Environment.Exit(0);
                }
                //if (dataReceived.Length == 3)
                //{
                //    Environment.Exit(0);
                //}
            }
            catch (IOException exception)
            {
                Console.WriteLine("[ERR] " + exception);
            }
        }

        public static void startMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer
            //  Call Writeline 60$
            newPort.WriteLine("11$");   // start measurement command
            //Console.WriteLine("Measurement Started");
            Console.ReadLine();
        }

        public static void stopMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer
            newPort.WriteLine("10$");   // stop measurement command
            //Console.WriteLine("Measurement Stopped");
            newPort.Close();
        }

        public static void autoMeasure()
        {
            Thread.Sleep(500);
            newPort.WriteLine("81$");
            Console.ReadLine();
        }
    }
}
