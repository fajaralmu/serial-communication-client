using System;
using System.IO.Ports;
using System.Threading;
using serial_communication_client.Commands;

namespace serial_communication_client
{
   
    public class ProgramSerialWrite
    {
        static SerialPort serial;
        public static void Main(string[] args)
        {
            serial = new SerialPort();
            serial.BaudRate = 9600;
            serial.PortName = "COM7";
            serial.DataReceived += DataReceived;

            serial.Open();
            Console.WriteLine("=======Serial opened========");
            CommandLedPayload ledOn = new CommandLedPayload(CommandName.LED_ON, 13, 1);
            CommandLedPayload ledOff = new CommandLedPayload(CommandName.LED_OFF, 13, 1);
            CommandLedPayload ledBlink = new CommandLedPayload(CommandName.LED_BLINK, 13, 0,  1);
           
            for (int i = 0; i < 500; i++)
            {
                Console.WriteLine($"----------------------- { i } ----------------------");
                CommandLedPayload cmd = i % 2 == 0 ? ledOn : ledOff;
                writeCommand(cmd);
                Thread.Sleep(cmd.DurationMs);
            }
            Console.WriteLine("************* END  COMMAND ***************");
            // end //

            Console.ReadLine();

            serial.Close();
        }

        static void writeCommand(CommandPayload command)
        {
            serial.Write(command.Extract(), 0, command.Size);
        }

        private static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine(">> " + serial.ReadLine());
        }
    }
}