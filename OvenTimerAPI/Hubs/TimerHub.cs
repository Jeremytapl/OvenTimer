using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using OvenTimerAPI.Services;
using System;
using System.Threading.Tasks;

namespace OvenTimerAPI
{
    public class TimerHub : Hub
    {
        private IConfiguration _config;

        public delegate Task SendMessageDelegate(string message);

        public TimerHub(IConfiguration configuration)
        {
            _config = configuration;
        }
        
        public async Task StartListener()
        {
            Func<string, Task> sendMessageCallback = SendMessage;

            try
            {
                string comPort = _config["ComPort"];                

                using (var arduino = new ArduinoService(comPort, sendMessageCallback))
                { }
            }
            catch (Exception e)
            {                
                await SendMessage(e.Message);
            }            
        }

        public async Task SendMessage(string message) 
        {
            await Clients.All.SendAsync("ReceiveInput", message);
        }
    }
}
