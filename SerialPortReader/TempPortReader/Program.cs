using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text.RegularExpressions;

namespace TempPortReader
{
    class Program
    {
        static SerialPort newPort;
        static string dataReceived;

        static void Main(string[] args)
        {
            newPort = new SerialPort("COM10", 9600); //bp: 2400 baud, BMI: 4800 baud Temp:9600


            newPort.DataReceived += new SerialDataReceivedEventHandler(portDataReceived);

            try
            {
                newPort.Open();

            }
            catch (IOException err)
            {
                Console.WriteLine("-");
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
                Console.ReadLine();
            }
            newPort.Close();
            //Console.WriteLine("End of Program");
        }

        public static void portDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //try
            //{
            //    dataReceived = newPort.ReadExisting();
            //    Console.WriteLine(dataReceived);

            //    string value = dataReceived;
            //    string[] collection = value.Split(':');

            //    Console.WriteLine("Temperature:" + collection[3]);
            //    //Environment.Exit(0);

            //}
            //catch (IOException exception)
            //{
            //    Console.WriteLine("N/A");
            //    Console.WriteLine("\n Message ---", exception.Message);

            //}
            SerialPortReader reader = (SerialPortReader)sender;
            int indexStart = -1;
            int indexEnd = -1;

            switch (e.EventType)
            {
                case SerialData.Chars:
                    string text = reader.getSerialPort().ReadExisting();
                    standingThermometerDataBuffer = standingThermometerDataBuffer + text;
                    try { 
                        indexStart = standingThermometerDataBuffer.IndexOf("W");
                        indexEnd = standingThermometerDataBuffer.IndexOf("\n\r");
                        List<string> strArray = new List <string>();
                        while (indexStart != -1 && indexEnd != -1)
                        {
                            string slpitData = standingThermometerDataBuffer.Substring(indexStart, indexStart + indexEnd + 2);
                            standingThermometerDataBuffer = standingThermometerDataBuffer.Substring(indexStart + indexEnd + 2);
                            strArray.Add(slpitData);
                            indexStart = standingThermometerDataBuffer.IndexOf("W");
                            indexEnd = standingThermometerDataBuffer.IndexOf("\n\r");
                        }
                        string[] temperatureInfo = null;
                        string[] barcodeInfo = null;
                        foreach (string str in strArray)
                        {
                            string strInfo = str.Trim();
                            strInfo = strInfo.Replace("\r","");
                            strInfo = strInfo.Replace("\n","");
                            strInfo = strInfo.Replace("\0", "");
                            if (strInfo.Length > 0)
                            {
                                string[] info = strInfo.Split(":");
                                if (info[0].ToLower().Equals("w") && temperatureInfo == null)
                                {
                                    temperatureInfo = info;
                                    break;
                                }
                                else if (info[0].ToLower().Equals("m"))
                                {
                                    barcodeInfo = info;
                                }
                            }
                            if (barcodeInfo != null && temperatureInfo != null)
                            {
                                break;
                            }

                            if (temperatureInfo != null)
                            {
                                string[] info = temperatureInfo;
                                try
                                {
                                    string weight = "", temp = "";
                                    if (info[1].Equals(SerialPortManager.StandingThermometerNoValue) == false)
                                    {
                                        float weightF = 0;
                                        if (!float.TryParse(info[1], out weightF))
                                            weight = "";
                                        else
                                            weight = weightF.ToString("0.0");
                                    }
                                    if(temp.Length > 0 || weight.Length > 0){
                                        float temperatureF = 0;
                                        float weightF = 0;

                                        if(!float.TryParse(temp, out temperatureF))
                                            temperatureF = 0;
                                        if(!float.TryParse(weight, out weightF))
                                            weightF =0;
                                        if(weightF > 0)
                                            weightF += TaidiiExe.Properties.Settings.Default.WERIGJING_SCALE;
                                        if(weightF < 0)
                                            weightF = 0;
                                        if(temperatureF > 30){
                                            // raise event
                                            New_TWH_DataReceivedEventArgs tempArgs = new New_TWH_DataReceivedEventArgs(temperatureF.ToString(), weightF.ToString(),"");
                                            if(SerialPortDataManager.New_TWH_EventHandler != null){
                                                SerialPortManager.New_TWH_EventHandle(tempArgs);
                                            }
                                        }
                                    }

                                }
                                catch(Exception e){}
                            }
                            if(barcodeInfo != null){
                                string[] info = barcodeInfo;
                                try{
                                    string barcode = info[1];
                                    if(barcode[barcode.Length -1] == '#'){
                                        barcode = barcode.Substring(0, barcode.Length - 1);
                                    }
                                    if(SerialPortManager.NewBarCodeEventHandler != null){
                                        SerialPort
                                    }
                                }
                            }
                    }

                   
            }
          
            

        }

        public static void startMeasurement()
        {
           // Thread.Sleep(500);

           // Console.WriteLine("Measurement Started");;
            Console.ReadLine();
        }

        public static void stopMeasurement()
        {
            //Thread.Sleep(500);

           // Console.WriteLine("Measurement Stopped");
            newPort.Close();
        }
    }
}
