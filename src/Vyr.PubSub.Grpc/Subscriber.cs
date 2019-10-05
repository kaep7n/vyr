using Google.Protobuf.WellKnownTypes;
using PubSub;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vyr.Core;
using Vyr.Skills;
using static PubSub.BrokerService;

namespace Vyr.Playground.Grpc
{
    public class Subscriber : DataflowSkill
    {
        private readonly BrokerServiceClient pubSubClient;
        private Subscription subscription;

        public Subscriber(BrokerServiceClient pubSubClient)
        {
            if (pubSubClient is null)
            {
                throw new ArgumentNullException(nameof(pubSubClient));
            }

            this.pubSubClient = pubSubClient;
        }

        public override async Task EnableAsync()
        {
            await base.EnableAsync();
            await this.SubscribeAsync();
            await this.AttachAsync();
        }

        public override async Task DisableAsync()
        {
            await this.UnsubscribeAsync();
            await base.DisableAsync();
        }

        protected override async Task ProcessAsync(Core.IMessage message)
        {
            var grpcMessage = new Message();
            grpcMessage.Topic = message.Topic;
            grpcMessage.CreatedAtUtc = Timestamp.FromDateTime(message.CreatedAtUtc);

            await this.pubSubClient.PublishAsync(grpcMessage);
        }

        private async Task SubscribeAsync(params string[] topics)
        {
            this.subscription = new Subscription
            {
                ClientId = new Id()
            };

            foreach (var topic in topics)
            {
                this.subscription.Topics.Add(topic);
            }

            await this.pubSubClient.SubscribeAsync(this.subscription);
        }

        private async Task UnsubscribeAsync()
        {
            await this.pubSubClient.UnsubscribeAsync(this.subscription);
        }

        private async Task AttachAsync()
        {
            using var call = this.pubSubClient.Attach(this.subscription);

            var responseStream = call.ResponseStream;

            var cts = new CancellationTokenSource();

            while (await responseStream.MoveNext(cts.Token))
            {
                var grpcMessage = responseStream.Current;
            }
        }
    }
}
