using System;
using System.IO.Ports;

namespace OvenTimerAPI
{
    public class Arduino : IDisposable
    {
        bool _disposed = false;
        SerialPort _port;

        public Arduino(string comPort)
        { 
            _port = new SerialPort(comPort, 9600);
            _port.DtrEnable = true;

            _port.Open();
        }

        ~Arduino()
        {
            Dispose(false);
        }

        public string Read() 
        {
            string data = "No data.";

            while(_port.IsOpen)
            {
                if(_port.BytesToRead > 0)
                {
                    // Indicate transmission received
                    _port.Write("1");

                    data = _port.ReadLine();

                    break;
                }
            }

            return data;
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
