using Grpc.Core;
using PubSub;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static PubSub.BrokerService;

namespace Vyr.Playground.Grpc
{
    public class BrokerServer : BrokerServiceBase
    {
        private readonly BufferBlock<Message> buffer = new BufferBlock<Message>();

        private readonly ConcurrentDictionary<string, Subscription> subscriptions = new ConcurrentDictionary<string, Subscription>();

        public override Task<Subscription> Subscribe(Subscription request, ServerCallContext context)
        {
            this.subscriptions.TryAdd(request.ClientId, request);

            return Task.FromResult(request);
        }

        public override async Task Attach(Subscription request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            while (this.subscriptions.TryGetValue(request.ClientId, out var subscription))
            {
                var message = await this.buffer.ReceiveAsync();

                if (subscription.Topics.Contains(message.Topic))
                {
                    await responseStream.WriteAsync(message);
                }
            }
        }

        public override Task<Subscription> Unsubscribe(Subscription request, ServerCallContext context)
        {
            return base.Unsubscribe(request, context);
        }

        public async override Task<Message> Publish(Message request, ServerCallContext context)
        {
            await this.buffer.SendAsync(request);

            return request;
        }
    }
}
