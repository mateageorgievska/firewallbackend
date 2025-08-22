using Models;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using PCES.EventSourcing.Aggregates;
using PCES.EventSourcing.Events;
using PCES.EventSourcing.Commands;

namespace Aggregates
{
    [EventStream(InEventStreamName = "status",
             ResultEventStreamName = "process",
             UniqueIdentifierName = nameof(FirewallRequestDto.Id),
             StreamPrefix = "status")]
    public class StatusAggregate : AggregateRoot<FirewallRequestDto>
    {
        public StatusAggregate(string uid, string streamPrefix, bool synchronizedEvents,
            DaprClient daprClient,
            ILogger<StatusAggregate> logger,
            IHttpContextAccessor httpContext,
            DaprEventStore eventStore,
            CommandsFactory commandsFactory)
        : base(uid, streamPrefix, synchronizedEvents, daprClient, logger, httpContext, eventStore, commandsFactory)
        {
        }

        public override Task<FirewallRequestDto?> GetInitialState(string uid)
        {
            return null;
        }

        public override Task<EventData> UpdateState(EventData @event)
        {
            return null;
        }

    }
}
