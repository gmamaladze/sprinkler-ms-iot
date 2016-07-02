namespace SprinklerWebApi
{
    public sealed class Rc433MhzSwitch
    {
        private readonly int _gpio;
        private readonly string _device;

        public Rc433MhzSwitch(int gpio, string device)
        {
            _gpio = gpio;
            _device = device;
        }

        public void On()
        {
            Rc433MhzSender.Send(_gpio, _device, 1);
        }

        public void Off()
        {
            Rc433MhzSender.Send(_gpio, _device, 0);
        }
    }
}