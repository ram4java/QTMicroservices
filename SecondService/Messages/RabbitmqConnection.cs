using RabbitMQ.Client;

namespace SecondService.Messages
{
    public class RabbitmqConnection : IRabbitmqConnection, IDisposable
    {
        private IConnection? _conn;
        private bool disposedValue;

        public IConnection connection => (IConnection) InitializeConnection(); //throw new NotImplementedException();

        private async Task<IConnection> InitializeConnection()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            return await factory.CreateConnectionAsync();
            //_conn = connection;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _conn?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~RabbitmqConnection()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
