using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace WebApplication35
{
    [HubName("operations")]
    public class OperationHub : Hub
    {
        // Keep track of the list of pending operations to be completed
        private static ConcurrentDictionary<int, CancellationTokenSource> _pendingOperations = new ConcurrentDictionary<int, CancellationTokenSource>();
        private static int _operationId;

        public int BeginOperation()
        {
            // Get a unique id for this operation
            int operationId = Interlocked.Increment(ref _operationId);
            var cts = new CancellationTokenSource();

            // Add it to the list
            _pendingOperations.TryAdd(operationId, cts);

            DoSomeLongRunningTask(operationId, cts);

            return operationId;
        }

        public void CancelOperation(int operationId)
        {
            CancellationTokenSource cts;
            if (_pendingOperations.TryGetValue(operationId, out cts))
            {
                cts.Cancel();
            }
        }

        private void DoSomeLongRunningTask(int operationId, CancellationTokenSource cts)
        {
            // Kick off the long running operation in the background
            Task.Run(async () =>
            {
                // When the cts trips we're going to call the client back and tell them the operation is cancelled
                CancellationTokenRegistration registration = cts.Token.Register(() => Clients.Caller.cancelled(operationId));

                // Pretend to something long running
                await Task.Delay(5000);

                // Don't call complete if we're cancelled
                if (!cts.IsCancellationRequested)
                {
                    Clients.Caller.complete(operationId);
                }

                // Don't leak registrations
                registration.Dispose();

                // Remove it from the list of pending operations
                _pendingOperations.TryRemove(operationId, out cts);

                // Might not need this but lets not forget to dispose anything
                cts.Dispose();
            });
        }
    }
}