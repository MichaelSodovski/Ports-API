using PortsApi.Models;
using System;

namespace PortsApi
{
    public abstract class BaseLogic : IDisposable
    {
        protected TestContext DB = new TestContext();
        public void Dispose()
        {
            DB.Dispose();
        }
    }
}
