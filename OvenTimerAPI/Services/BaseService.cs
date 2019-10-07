using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OvenTimerAPI.Services
{
    public abstract class BaseService : IDisposable
    {
        protected bool _disposed = false;
        protected TimerContext _ctx;

        public BaseService(TimerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Timer context may not be null");
            }

            _ctx = context;
        }

        ~BaseService()
        {
            Dispose(false);
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

            if (disposing && _ctx != null)
            {
                _ctx.Dispose();
            }

            _disposed = true;
        }
    }
}