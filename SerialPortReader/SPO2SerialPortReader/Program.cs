using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace SPO2SerialPortReader
{
    class Program
    {
        static SerialPort newPort;
        static string dataReceived;
        static readonly string SPO2COMPort_Time = System.IO.File.ReadAllText(@"C:\AIT\Logs\ConfigSPO2.ini");
        static string SPO2COMPort = string.Empty;
        static string SPO2WaitTime = string.Empty;

        //static Int32 int1 = 0;
        //static Int32 int2 = 0;
        //static Int32 int3 = 0;

        static void Main(string[] args)
        {

            int index = SPO2COMPort_Time.IndexOf(',');

            if (index > 0) //have ,
            {
                string[] ValueSplit = SPO2COMPort_Time.Split(',');
                SPO2COMPort = ValueSplit[0];
                SPO2WaitTime = ValueSplit[1];
            }

            newPort = new SerialPort(SPO2COMPort, 19200); //bp: 2400 baud, BMI: 4800 baud Temp:9600
            newPort.DataBits = 8;
            newPort.StopBits = StopBits.One;
            newPort.Parity = Parity.Odd;

            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);

            try
            {
                newPort.Open();
            }
            catch (IOException)
            {
                Console.WriteLine("COM Port not in use");
            }

            if (newPort.IsOpen)
            {


            }
            Thread.Sleep(5000);

            ////Comment if call from app as exe////
            Console.ReadLine();


            newPort.Close();

            //Console.WriteLine("End of Program");
        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int count = newPort.BytesToRead;
                byte[] data = new byte[count];
                newPort.Read(data, 0, count);
                //Console.Write(BitConverter.ToString(data));

                string decValue = BitConverter.ToString(data);
                if (count == 2)
                {
                    string[] decValueSplit = decValue.Split('-');
                    int value1 = Convert.ToInt32(decValueSplit[0], 16);
                    int value2 = Convert.ToInt32(decValueSplit[1], 16);
                    Console.WriteLine(value1 + " " + value2);


                    //To call from app as Exe, to stop when data received
                    //Environment.Exit(0);

                    ////foreach (string val in decValueSplit)
                    ////{
                    ////    int value = Convert.ToInt32(val, 16);
                    ////    Console.WriteLine(value);
                    ////}
                }
                Array.Clear(data, 0, data.Length);
            }
            catch (IOException exception)
            {
                //Console.WriteLine("Exception: " + exception);
            }

            //try
            //{
            //    dataReceived = newPort.ReadLine();
            //    Console.WriteLine(dataReceived);
            //}
            //catch (IOException exception)
            //{
            //    //  Error handling
            //    Console.WriteLine(exception.ToString());
            //}
           
        }

    }
}
