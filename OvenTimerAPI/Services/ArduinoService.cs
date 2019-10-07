using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace OvenTimerAPI.Services
{
    public class ArduinoService : IDisposable
    {
        bool _disposed = false;
        SerialPort _port;

        public ArduinoService(string comPort, Func<string, Task> callBack)
        { 
            InitComPort(comPort);
            ListenForData(callBack);            
        }       

        ~ArduinoService()
        {
            Dispose(false);
        }

        private void InitComPort(string comPort)
        {
            _port = new SerialPort(comPort, 9600);
            _port.DtrEnable = true;
            
            _port.Close();
            _port.Open();
        }

        private void ListenForData(Func<string, Task> callBack) 
        {
            while(_port.IsOpen)
            {
                if(_port.BytesToRead > 0)
                {
                    // Indicate transmission received
                    _port.Write("1");

                    var data = _port.ReadLine();
                    var obj = new object[1];
                    obj[0] = data;

                    callBack(data);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            if (_port != null && _port.IsOpen)
            {
                _port.Close();
            }

            _disposed = true;
        }
    }
}
