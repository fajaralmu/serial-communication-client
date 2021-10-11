using System;
using System.IO.Ports;
using System.Threading;

namespace SerialCommunication
{
    class CommandPayload
    {
        readonly Commands name;
        readonly byte[] arguments;

        public int Size { get => 2 + arguments.Length; }
        public CommandPayload(Commands name, params byte[] arguments)
        {
            this.name = name;
            this.arguments = arguments;
        }

        public byte[] Extract()
        {
            byte[] result = new byte[Size];
            result[0] = (byte)name;
            result[1] = (byte)arguments.Length;
            for (int i = 2; i < Size; i++)
            {
                result[i] = arguments[i - 2];
            }
            return result;
        }
    }
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
            CommandPayload ledOn = new CommandPayload(Commands.LED_ON, 13, 0, 3);
            CommandPayload ledOff = new CommandPayload(Commands.LED_OFF, 13);
            CommandPayload ledBlink = new CommandPayload(Commands.LED_BLINK, 13, 1, 10);
           
            writeCommand(ledOn);
            Thread.Sleep(6000);

            writeCommand(ledOff);
            Thread.Sleep(6000);

            writeCommand(ledOn);
            Thread.Sleep(6000);

            writeCommand(ledOff);
            Thread.Sleep(6000);

            writeCommand(ledBlink);
            Thread.Sleep(10000);

            writeCommand(ledOff);
            Thread.Sleep(10000);
            
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