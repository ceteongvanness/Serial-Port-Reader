using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewBMISerialPortReader
{
    class Program
    {
        static SerialPort newPort;
        static string dataReceived;
        static string Height = "";
        static string Weight = "";


        static void Main(string[] args)
        {
            newPort = new SerialPort("COM9", 9600); //bp: 2400 baud, BMI: 4800 baud Temp:9600
            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);


            try
            {
                newPort.Open();
                ////newPort.WriteLine("60$"); // 80$ turns off automatic measure 81% turns on automatic measure
            }
            catch (IOException err)
            {
                //  Error Handling 
            }

            if (newPort.IsOpen)
            {
                //  Call function here - startMeasurement(), stopMeasurement()
                startMeasurement();
                
            }
            Console.ReadLine();



        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Data Received");
            try
            {
                dataReceived += newPort.ReadLine();
                Console.WriteLine("DataReceived : " + dataReceived);

                //Console.WriteLine("L: " + dataReceived.Length);
                //Weight = dataReceived.Substring(2, 5);
                //Height = dataReceived.Substring(10, 5);

                ////Thread.Sleep(5500); // 300 minimum for data transfer
                ////newPort.WriteLine("11$");   // start measurement command
                ////Console.WriteLine("Cmd sent");
            }
            catch (IOException exception)
            {
                //  Error handling
                Console.WriteLine(exception.ToString());
            }
        }

        public static void startMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer
            newPort.Write("11$");   // start measurement command
        }

        public static void stopMeasurement()
        {
            Thread.Sleep(500); // 300 minimum for data transfer
            newPort.WriteLine("10$");   // stop measurement command
            newPort.Close();
        }

    }
}
