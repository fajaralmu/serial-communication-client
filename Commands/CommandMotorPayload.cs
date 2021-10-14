namespace serial_communication_client.Commands
{
    public class CommandMotorPayload : CommandPayload
    {
        public int Angle => _angle;
        private int _angle;
        public CommandMotorPayload(byte pin, byte angle) : base(CommandName.MOVE_SERVO, pin,0,0,angle)
        {
            _angle = angle;
        }
    }

}