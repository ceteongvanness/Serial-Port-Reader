using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace BPSerialPortReader
{
    class Program
    {
        //static bool sent = false;
        static SerialPort newPort;
        static string dataReceived;
        static string[] collection;
        static string value;

        static void Main(string[] args)
        {
            newPort = new SerialPort("COM1", 2400); //bp: 2400 baud, BMI: 4800 baud Temp:9600


            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);

            try
            {
                newPort.Open();

            }
            catch (IOException err)
            {
                Console.WriteLine("[ERR01] " + err);
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
                }
            }
            //if (collection != null)
            //{
            //    Console.WriteLine("Sys: " + collection[2] + " Dia: " + collection[4] + " Pr: " + collection[5]);
            //}
            //Console.ReadLine();
            
            if(value != null)
            {
                collection = value.Split(',');
                while (collection.Length > 6)
                {
                    Environment.Exit(0);
                }
            }

            newPort.Close();
            Console.WriteLine("End of Program");
        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                
                dataReceived = newPort.ReadExisting();
                Console.Write(dataReceived);
                value += dataReceived;
                collection = value.Split(',');
                while (collection.Length > 6)
                {
                    Environment.Exit(0);
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine("[ERR02] " + exception);
            }
        }

        public static void startMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer

            //Console.WriteLine("Measurement Started");
            //try
            //{
            //    string emptyLine = Reader.ReadLine(50000);
            //}
            //catch (TimeoutException)
            //{
            //    Console.WriteLine("");
            //} 
            Console.ReadLine();
        }

        public static void stopMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer
           
            //Console.WriteLine("Measurement Stopped");
            newPort.Close();
        }
    }
}
