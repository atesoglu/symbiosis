using System.Threading;
using System.Threading.Tasks;
using Application.Events.Base;
using Application.Services;
using Application.Services.EventDispatcher;

namespace Application.IntegrationTests.Services
{
    public class EmptyEventDispatcherService : IEventDispatcherService
    {
        public async Task Dispatch(EventBase @event, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}