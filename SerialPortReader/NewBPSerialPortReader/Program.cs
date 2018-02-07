using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewBPSerialPortReader
{
    class Program
    {
        static SerialPort newPort;
        static string dataReceived;
        static string Sys = "";
        static string Dys = "";
        static string Pulse = "";

        static void Main(string[] args)
        {
            newPort = new SerialPort("COM10", 2400); //bp: 2400 baud, BMI: 4800 baud Temp:9600


            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);

            try
            {
                newPort.Open();

            }
            catch (IOException err)
            {
                //  Error Handling
            }

            //Console.ReadLine();
        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                dataReceived = newPort.ReadExisting();
                if(dataReceived.Contains("S"))
                {
                    Sys = dataReceived.Substring(2, 3);
                    Dys = dataReceived.Substring(12, 3);
                    Pulse = dataReceived.Substring(17, 3);
                }
                

                //Console.WriteLine(dataReceived);
                //Console.WriteLine(Sys);
                //Console.WriteLine(Dys);
                //Console.WriteLine(Pulse);

            }
            catch (IOException exception)
            {
                //  Error Handling
            }
        }

        public static void resetBP()
        {
            Sys = "";
            Dys = "";
            Pulse = "";
        }
    }
}
